using Microsoft.Extensions.Logging;
using SandTablePlatform.Models;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace SandTablePlatform.Service
{
    public class SandTableService
    {
        private SandTableOption config;
        private object socket_lock = new object();
        private ILogger<SandTableService> logger;
        private List<WebSocketPackage> webSockets = new List<WebSocketPackage>();

        byte[] revieveBuffer = new byte[1024 * 10];
        byte[] sendBuffer = new byte[1024 * 10];
        IPEndPoint endPoint;
        Socket socket;
        SocketAsyncEventArgs recieveEvent;
        SocketAsyncEventArgs sendEvent;
        private AutoResetEvent ResetEvent = new AutoResetEvent(false);
        public SandTableService(IOptions<SandTableOption> options, ILogger<SandTableService> logger) {
            this.config = options.Value;
            this.logger = logger;
        }
        public void Start(CancellationToken token)
        {
            ListenToSandTable(token);
        }
        /// <summary>
        /// 监听沙盘变化
        /// </summary>
        /// <returns></returns>
        private void ListenToSandTable(CancellationToken token)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress iPAddress = IPAddress.Parse(config.HostName);
            endPoint = new IPEndPoint(iPAddress, config.Port);
            //socket.Bind(endPoint);
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.DisconnectReuseSocket = true;
            args.RemoteEndPoint = endPoint;
            args.Completed += IO_Completed;
            bool connnect = socket.ConnectAsync(args);
            if (!connnect) {
                StartReceiveData(args);
            }
            while (!token.IsCancellationRequested) {
                Thread.Sleep(2000);
            }
            CloseSocket();

        }
        protected void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                case SocketAsyncOperation.Connect:
                    
                    StartReceiveData(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }
        public int Send(byte[] buffer)
        {
            ResetEvent.WaitOne(3000);
            try
            {
                return SendAsync(buffer).Result;
            }
            finally {
                ResetEvent.Set();
            }
        }
        public int Send(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            return Send(data);
        }
        /// <summary>
        /// 数据发送，给沙盘发送数据
        /// </summary>
        /// <returns></returns>
        public Task<int> SendAsync(byte[] buffer)
        {
            ResetEvent.WaitOne(3000);
            if (!socket.Connected) {
                throw new Exception("socket已断开连接");
            }
            //CancellationTokenSource source = new CancellationTokenSource();
            TaskCompletionSource<int> source = new TaskCompletionSource<int>();
            sendEvent.UserToken = source;
            sendEvent.SetBuffer(buffer, 0, buffer.Length);
            bool send = socket.SendAsync(sendEvent); 
            if (!send) {
                ProcessSend(sendEvent);
            }
            return source.Task.ContinueWith(task=> {
                ResetEvent.Set();
                return task.Result;
            });
        }
        /// <summary>
        /// 数据发送异步方法
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<int> SendAsync(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            return await SendAsync(data);
        }
        public void StartReceiveData(SocketAsyncEventArgs e)
        {
            if (e != recieveEvent) {
                recieveEvent = new SocketAsyncEventArgs();
                recieveEvent.DisconnectReuseSocket = true;
                recieveEvent.RemoteEndPoint = endPoint;
                recieveEvent.Completed += IO_Completed;
                recieveEvent.SetBuffer(revieveBuffer, 0, revieveBuffer.Length);
                sendEvent = new SocketAsyncEventArgs();
                sendEvent.DisconnectReuseSocket = true;
                sendEvent.RemoteEndPoint = endPoint;
                sendEvent.Completed += IO_Completed;
                sendEvent.SetBuffer(sendBuffer, 0, revieveBuffer.Length);
            }
            bool recieve = socket.ReceiveAsync(recieveEvent);
            if (!recieve) {
                ProcessReceive(recieveEvent);
            }
        }
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                ReleseSource(e.UserToken,true);
                CloseSocket();
            }
            else {
                ReleseSource(e.UserToken);
            }
        }
        private void ReleseSource(object token,bool error= false) {
            if (token is TaskCompletionSource<int>)
            {
                TaskCompletionSource<int> source = token as TaskCompletionSource<int>;
                if (error)
                {
                    source.SetException(new Exception("socket 已关闭"));
                }
                else {
                    source.SetResult(1);
                }
            }
        }
        /// <summary>
        /// 数据接收，接收到沙盘发送过来的数据
        /// </summary>
        /// <returns></returns>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                byte[] data = e.Buffer.Take(e.BytesTransferred).ToArray();
                string text = Encoding.UTF8.GetString(data);
                logger.LogInformation("接收到数据{0}",text);
                StartReceiveData(e);
                WebSocketData<string> respon = new WebSocketData<string>("msg",text);
                string json = JsonConvert.SerializeObject(respon);
                Broadcast(json);
                //m_maxNumberConnectedClients.Release();
            }
            else
            {
                CloseSocket();
            }
        }
        /// <summary>
        /// 订阅沙盘，当沙盘有消息过来时，通过WebSocket通知前端
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public void Subscribe(WebSocket socket, CancellationTokenSource token)
        {
            lock (socket_lock)
            {
                webSockets.Add(new WebSocketPackage(socket, token));
            }

        }
        /// <summary>
        /// 广播消息，广播给所有WebSocket
        /// </summary>
        /// <returns></returns>
        private void Broadcast(string msg)
        {
            WebSocketPackage[] sockets = null;
            lock (socket_lock)
            {
                sockets = webSockets.ToArray();
            }
            byte[] data = Encoding.UTF8.GetBytes(msg);
            Parallel.ForEach(sockets, async (pk, state, l) =>
            {
                try
                {
                    if (pk.Socket.State == WebSocketState.CloseReceived) {
                        CloseWebSocket(pk);
                        return;
                    }
                    await pk.Socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    CloseWebSocket(pk);
                }
            });

        }
        private void CloseWebSocket(WebSocketPackage pk) {
            if (pk != null && pk.Socket.State != WebSocketState.Closed)
            {
                pk.Socket.CloseAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
            }
            pk.Token.Cancel();
            lock (socket_lock)
            {
                webSockets.Remove(pk);
            }
        }
        private void CloseSocket()
        {
            if (socket != null&&socket.Connected)
            { 
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
        private void ResetSocket()
        {
            if (socket != null && socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}

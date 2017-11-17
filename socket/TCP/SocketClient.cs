using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
namespace LandMark.Common.TCP
{
    public class SocketClient
    {
        private int m_numConnections;   // the maximum number of connections the sample is designed to handle simultaneously 
        private int m_receiveBufferSize;// buffer size to use for each socket I/O operation 
        const int opsToPreAlloc = 2;    // read, write (don't alloc buffer space for accepts)
        Socket clientSocket;            // the socket used to listen for incoming connection requests
                                        // pool of reusable SocketAsyncEventArgs objects for write, read and accept socket operations
        SocketAsyncEventArgsPool m_readWritePool;
        Semaphore m_maxNumberConnectedClients=new Semaphore(1,1);
        public SocketClient(int numConnections, int receiveBufferSize)
        {
            m_numConnections = numConnections;
            m_receiveBufferSize = receiveBufferSize;
            m_readWritePool = new SocketAsyncEventArgsPool(numConnections);
        }
        public void Init()
        {
            SocketAsyncEventArgs readWriteEventArg;
            for (int i = 0; i < m_numConnections; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.UserToken = new AsyncUserToken();
                m_readWritePool.Push(readWriteEventArg);
            }
        }

        public void Connect(IPEndPoint localEndPoint)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //SocketAsyncEventArgs connectEventArg = new SocketAsyncEventArgs();
            //connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            //connectEventArg.RemoteEndPoint = localEndPoint;
            //string sendStr = "send to server : hello,ni hao";
            //byte[] sendBytes = Encoding.UTF8.GetBytes(sendStr);
            //connectEventArg.SetBuffer(sendBytes, 0, sendBytes.Length);
            //StartConnect(connectEventArg);
            clientSocket.Connect(localEndPoint);
        }
        public void Send(byte[] msg, IPEndPoint localEndPoint)
        {

            m_maxNumberConnectedClients.WaitOne();
            SocketAsyncEventArgs connectEventArg = new SocketAsyncEventArgs();
            connectEventArg.RemoteEndPoint = localEndPoint;
            connectEventArg.SetBuffer(msg, 0, msg.Length);
            connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            bool willRaiseEvent = clientSocket.SendAsync(connectEventArg);
            if (!willRaiseEvent)
            {
                //ProcessConnect(connectEventArg);
            }
        }
        public void StartConnect(SocketAsyncEventArgs connectEventArg)
        {            
            connectEventArg.DisconnectReuseSocket=true;
            bool willRaiseEvent = clientSocket.ConnectAsync(connectEventArg);
            if (!willRaiseEvent)
            {
                //ProcessConnect(connectEventArg);
            }
        }
        void IO_Completed(object sender, SocketAsyncEventArgs e)
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
                    ProcessSend(e);
                    break;
                case SocketAsyncOperation.SendTo:
                    ProcessSend(e);
                    break;
                case SocketAsyncOperation.ReceiveFrom:
                    ProcessReceive(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }

        }
        private void StartSend()
        {

        }
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                string recStr = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
                Console.WriteLine(recStr);
                m_maxNumberConnectedClients.Release();
            }
            else
            {
                CloseClientSocket(e);
            }
        }
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                bool willRaiseEvent = clientSocket.ReceiveAsync(e);
                 
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            try
            {
               clientSocket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception ex) {

            }
            //clientSocket.Close();
            m_readWritePool.Push(e);
        }

    }
}

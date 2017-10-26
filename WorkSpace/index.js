(function(win,$){
    var main = $.fn.main = function () {
        selector = this;
        _init();
    },
    selector,
    loginForm,
    $vTable = "#vTable";

    var _init = function () {
        //$('#scoreBoard1').ScoreBoard({number: "13.45", time: 300 });

        _checkLogin();
        _getPageInfo();
        _initEven();
        _initValidate();
    }

    var _initEven = function () {
        selector.on('click', "#BtLogin", _login);
        selector.on('click', "#btCancel", function () {
            unlogin(function (resp) {
                _loginError();
            })
        });


    }

    var _initValidate = function () {
        loginForm = $('#loginForm').validate({
            onclick: false,
            onfocusout: false,
            onkeyup:false,
            rules: {
                user: {
                    required: true,
                    //rangelength: [6, 12]
                },
                pwd: {
                    required: true,
                    //rangelength: [6, 12]
                    remote: {
                        url: "/Handles/ASHX/Common_Handlers.ashx",     //后台处理程序
                        type: "post",               //数据发送方式
                        dataType: "json",           //接受数据格式   
                        data: {},
                        success: function (resp) {
                                $("#loginLoading").css("display", "none");
                                if (resp.state == 0) {
                                    setPower(resp.Desc);
                                    _loginSuccess(resp.Msg, resp.F_GroupID, resp.Desc);
                                    return true;
                                    //alert(resp.Msg);
                                }//登录失败
                                else {
                                    loginForm.settings.messages.pwd.remote = resp.Msg;
                                    var errors = {};
                                    //在user上显示异常信息，密码上方显示可能会被掩盖住
                                    errors["user"] = resp.Msg;
                                    loginForm.showErrors(errors);
                                    //alert(resp.Msg);
                                }
                        },
                        error: function (xhr) {
                            $("#loginLoading").css("display", "none");
                            var errors = {};
                            errors["user"] = xhr.responseText;
                            loginForm.showErrors(errors);
                                            //bs_alert('发生了错误\r\n' + xhr.responseText);
                        }
                    }
                }
            },
            messages: {
                user: {
                    required: "这是必填字段",
                    rangelength: "长度要在6-12之间"
                },
                pwd: {
                    required: "这是必填字段",
                    rangelength: "长度要在6-12之间",
                    remote:"登录失败"
                }
            }
        });
    }

    var _checkLogin = function () {
        isLogin(_loginSuccess, _loginError);
    }

    var _getUserData = function () {
        var publickey = _getRsaPublicKey();
        var username = $("#user").val();
        var passwd = $("#pwd").val();
        setMaxDigits(129);
        var key = new RSAKeyPair(publickey.Exponent, "", publickey.Modulus);
        var passwdScuri = encryptedString(key, passwd);
        var usernameScuri = encryptedString(key, username);
        return {
            Action: "Login",
            username: usernameScuri,
            passwd: passwdScuri
        }
    }

    var _getPageInfo = function () {
        var ajaxOption = getAjaxOption();
        $.extend(true, ajaxOption, {
            url: "/Handles/ASHX/GroupIndex_Handlers.ashx",
            data: { Action: "Get_ViewInfo" },
            success: function (data) {
                var result = JSON.parse(data);
                _setScoreBoard(result.realtimetotalenergy, result.TotalRunTime);
                _setValue(result);
                
            }
        });
        $.ajax(ajaxOption);
    }

    //设置记分牌
    var _setScoreBoard=function(energy,runtime)
    {
        var energyStr = energy.toString();
        var runtimeStr = runtime.toString();
        while (energyStr.length < 6)
        {
            energyStr = "0" + energyStr;
        }
        while (runtimeStr.length < 6)
        {
            runtimeStr = "0" + runtimeStr;
        }
        $('#scoreBoard1').ScoreBoard({ number: energyStr, time: 300 });
        $('#scoreBoard2').ScoreBoard({ number: runtimeStr, time: 300 });
    }

    var _setValue=function(info)
    {
        var tbs = $($vTable).find("b[class*='tbValue']");
        tbs.each(function (index,el) {
            var name = el.dataset.name;
            el.innerHTML = info[name];
        });
    }

    var _login = function () {
        var sendata = _getUserData();
        $.extend(true, loginForm.settings.rules.pwd.remote.data, sendata);
        $("#loginLoading").css("display", "block");
        setTimeout(function () {
            loginForm.form();
        }, 300);
    }

    var _loginSuccess=function(username,groupid,power)
    {
        $("#subnavContent").html(side.toIndexHtml(groupid));

        //显示登录后的信息
        $("#beforeLogin").css("display", "none");
        $("#minLogin").css("display", "none");
        $("#btCancel").css("display", "inline-block");
        $("#userInfo").html('<i class="ico ico-person"></i>' + username);
        $("#userInfo").css("display", "inline-block");
        //登录界面隐藏
        $('.btn_menulogin').removeClass('active');
        $('.banner-wrap>.login').hide();
        $('.banner-wrap>.banner').show();
        //清空输入框
        $("#user").val("");
        $("#pwd").val("");

        //显示导航栏
        $("#subnav").css("display", "block");
        $("#minsubnav").css("display", "block");
    }

    var _loginError=function(){
        $("#minLogin").css("display", "inline-block");
        $("#beforeLogin").css("display", "inline-block");
        $("#btCancel").css("display", "none");
        $("#userInfo").css("display", "none");
        //隐藏导航栏
        $("#subnav").css("display", "none");
        $("#minsubnav").css("display", "none");
    }

    var _getRsaPublicKey = function () {
        var publickey = "";
        $.ajax({
            url: "/Handles/ASHX/Common_Handlers.ashx",
            async: false,
            method: 'POST',
            dataType: 'json',
            data: {
                Action: "getRasPublicKey",
            },
            success: function (resp) {
                publickey = resp;
            }
        });
        return publickey;
    }

    

})(this, jQuery)

$(function () {
    $("body").main();
});
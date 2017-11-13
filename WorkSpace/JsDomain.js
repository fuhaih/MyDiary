/// <reference path="../ChangNing/Login.shtml" />
/*
全局javascript
*/
//给不支持forEach的浏览器数组添加forEach函数
if (!Array.prototype.forEach) {

    Array.prototype.forEach = function forEach(callback, thisArg) {

        var T, k;

        if (this == null) {
            throw new TypeError("this is null or not defined");
        }
        var O = Object(this);
        var len = O.length >>> 0;
        if (typeof callback !== "function") {
            throw new TypeError(callback + " is not a function");
        }
        if (arguments.length > 1) {
            T = thisArg;
        }
        k = 0;

        while (k < len) {

            var kValue;
            if (k in O) {

                kValue = O[k];
                callback.call(T, kValue, k, O);
            }
            k++;
        }
    };
}
Array.prototype.orderBy= function (name) {
    for (var i = 0; i < this.length; i++)
    {
        for (var j = 0; j < this.length - 1; j++)
        {
            if (this[j][name] < this[j + 1][name])
            {
                var max = this[j + 1];
                this[j + 1] = this[j];
                this[j] = max;
            }
        }
    }
}
Array.prototype.where = function (callback)
{
    var casesh = [];
    this.forEach(function (el,index) {
        if (callback(el, index))
        {
            casesh.push(el);
        }
    });
    return casesh;
}
Array.prototype.first = function () {
    if (this.length > 0) {
        return this[0];
    } else {
        return null;
    }
}
Array.prototype.groupBy = function (name) {
    var result = {};
    this.forEach(function (el, index) {
        if (!result[el[name]])
        {
            result[el[name]] = [];
        }
        result[el[name]].push(el);
    });
    return result;
}
Array.prototype.max=function(name){
    var max = this[0][name];
    for (var i = 0; i < this.length; i++)
    {
        var value = this[i][name];
        if (max < value)
        {
            max = value;
        }
    }
    return max;
}
Array.prototype.sum = function (name) {
    var sum =0;
    for (var i = 0; i < this.length; i++) {
        sum += this[i][name];
    }
    return sum;
}
Array.prototype.select = function (callback) {
    var result = [];
    this.forEach(function (el, index) {
        result.push(callback(el,index));
    });
    return result;
}
String.prototype.trim = function () { //删除左右两端的空格 
    return this.replace(/(^\s*)|(\s*$)/g, ""); 
} 
$(function () {
    //$("body").mCustomScrollbar({
    //    theme: "minimal-dark"
    //});
    initialChart();
   
})

var isLogin = function (Login,unLogin) {
    var option = getAjaxOption();
    $.extend(true, option, {
        url: "/Handles/ASHX/Common_Handlers.ashx",
        data: { Action: "checkLogin" },
        dataType: "json",
        async: false,
        success: function (result) {
            if (result != null && result != "") {
                if (result.state != 0) {
                    unLogin();
                } else {
                    Login(result.data, result.F_GroupID, result.Desc);
                }
            }
        },
    })
    $.ajax(option);
}

var setPower = function (power)
{
    $(".power" + power).css("display","none");
}

var unlogin = function (unlogin) {
    $.ajax({
        url: "/Handles/ASHX/Common_Handlers.ashx",
        method: 'POST',
        dataType: 'text',
        data: {
            Action: "UnLogin",
        },
        success: function (resp) {
            unlogin(resp);
        }
    });
}

$(window).resize(function () {
    initialChart();
});

function initialChart()
{
    if ($(".Chart").length > 0) {
        $(".Chart").each(function (index, el) {
            var myChart = echarts.init(el);

            // 指定图表的配置项和数据
            var option = {
                title: {
                    text: 'chart示例'
                },
                tooltip: {},
                legend: {
                    data: ['销量']
                },
                xAxis: {
                    data: ["衬衫", "羊毛衫", "雪纺衫", "裤子", "高跟鞋", "袜子"]
                },
                yAxis: {},
                series: [{
                    itemStyle: {
                        normal: {
                            color:"#1daf92"
                        }
                    },
                    name: '销量',
                    type: 'bar',
                    data: [5, 20, 36, 10, 10, 20]
                }]
            };

            // 使用刚指定的配置项和数据显示图表。
            myChart.setOption(option);
        })
    }
}

var ajaxOption = 
{
    type: 'post',
    url: '/Handles/ASHX/GroupIndex_Handlers.ashx',
    async: true,
    dataType: 'text',
    data: {},
    success: function (result) {
    },
    error: function (xhr) {
        
        if (xhr.responseText.indexOf("<!DOCTYPE html>") >= 0 || xhr.responseText.indexOf("<html>") >= 0) {
            var match = /<b> 异常详细信息: <\/b>(.*)<br>/.exec(xhr.responseText);
            alert(match[1]);
        } else {
            alert(xhr.responseText);
        }
        if (this.loader) {
            this.loader.forEach(function (el, index) {
                el.addLoaderFail();
            });
        }
        //bs_alert('发生了错误\r\n' + xhr.responseText);
    }
}

function getAjaxOption() {
    $.extend(true, result = {}, ajaxOption);
    return result;
}
/*-----根据table的columns格式来获取rows的内容并返回新生成的table---*/
function converToTable(tb,array)
{
    var array = tb.Rows;
    tb.Rows = [];
    var setrow = function (columns, obj) {
        var row = {};
        for (var index in columns) {
            row[columns[index].ColumnName] = obj[columns[index].ColumnName];
        }
        return row;
    }
    for (var index in array) {
        var obj = array[index];
        tb.Rows.push(setrow(tb.Columns, obj));
    }
    return tb;
}

function addLoader() {
    $("body").children('div[class*="page-loading-overlay"]').removeClass("loaded");
}

function removeLoader() {
    $("body").children('div[class*="page-loading-overlay"]').addClass("loaded");
}

function downloadCsv(table) {
    table = converToTable(table);
    var columns = [];
    table.Columns.forEach(function (element, index) {
        columns.push(element.Caption);
    });
    var rows = [];
    table.Rows.forEach(function (el, index) {
        var row = [];
        table.Columns.forEach(function (col,index) {
            row.push(el[col.ColumnName]);
        });
        rows.push(row);
    });

    var csv = columns.join(',') + "\n";
    rows.forEach(function (element, index) {
        csv += index < rows.length ? element.join(',') + "\n" : element.join(',');
    });
    var blob = new Blob(["\ufeff" + csv], { type: "text/plain;charset=utf8" });
    saveAs(blob, table.TableName + ".csv");
}

var _getTime = function () {
    var startTime;
    var endTime;
    var li = $("#timeParam").find("li[class*='active']");
    var cycle = li.data("cycle");
    switch (cycle) {
        case "day": {
            var dayinput = li.find("input[class*='dayinput']");
            var day = dayinput.val();
            startTime = new Date(Date.parse(day.replace(/-/g, "/")));;
            endTime = new Date(Date.parse(day.replace(/-/g, "/")));
            endTime.addDays(1);
        } break;
        case "month": {
            var yearselect = li.find("select[class*='yearselect']");
            var year = yearselect.val();

            var monthselect = li.find("select[class*='monthselect']");
            var month = monthselect.val();
            var startStr = year + "-" + month + "-01";
            startTime = new Date(Date.parse(startStr.replace(/-/g, "/")));;
            endTime = new Date(Date.parse(startStr.replace(/-/g, "/")));
            endTime.addMonths(1);

        } break;
        case "session": {
            var yearselect = li.find("select[class*='yearselect']");
            var year = yearselect.val();

            var sessionselect = li.find("select[class*='sessionselect']");
            var session = sessionselect.val();
            var startStr = year + "-" + (parseInt(session) - 1) * 3 + "-01";
            startTime = new Date(Date.parse(startStr.replace(/-/g, "/")));;
            endTime = new Date(Date.parse(startStr.replace(/-/g, "/")));
            endTime.addMonths(3);
        } break;
        case "year": {
            var yearselect = li.find("select[class*='yearselect']");
            var year = yearselect.val();
            startStr = year + "-01-01";
            startTime = new Date(Date.parse(startStr.replace(/-/g, "/")));;
            endTime = new Date(Date.parse(startStr.replace(/-/g, "/")));
            endTime.addYears(1);

        } break;
    }

    return {
        cycle:cycle,
        startTime: startTime.Format("yyyy/MM/dd"),
        endTime: endTime.Format("yyyy/MM/dd")
    };
}

var getTime = function () {
    var startTime;
    var endTime;
    var li = $("#timeParam").find("li[class*='active']");
    var cycle = li.data("cycle");
    switch (cycle) {
        case "day": {
            var dayinput = li.find("input[class*='time']");
            var day = dayinput.val();
            startTime = new Date(Date.parse(day.replace(/-/g, "/")));;
            endTime = new Date(Date.parse(day.replace(/-/g, "/")));
            endTime.addDays(1);
        } break;
        case "month": {
            var monthinput = li.find("input[class*='time']");
            var month = monthinput.val();
            var startStr = month+"/01"
            startTime = new Date(Date.parse(startStr.replace(/-/g, "/")));;
            endTime = new Date(Date.parse(startStr.replace(/-/g, "/")));
            endTime.addMonths(1);

        } break;
        case "session": {
            var yearselect = li.find("select[class*='yearselect']");
            var year = yearselect.val();

            var sessionselect = li.find("select[class*='sessionselect']");
            var session = sessionselect.val();
            var startStr = year + "/" + (parseInt(session) - 1) * 3 + "/01";
            startTime = new Date(Date.parse(startStr.replace(/-/g, "/")));;
            endTime = new Date(Date.parse(startStr.replace(/-/g, "/")));
            endTime.addMonths(3);
        } break;
        case "year": {
            var yearinput = li.find("input[class*='time']");
            var year = yearinput.val();
            startStr = year + "/01/01";
            startTime = new Date(Date.parse(startStr.replace(/-/g, "/")));;
            endTime = new Date(Date.parse(startStr.replace(/-/g, "/")));
            endTime.addYears(1);

        } break;
    }

    return {
        cycle: cycle,
        startTime: startTime.Format("yyyy/MM/dd"),
        endTime: endTime.Format("yyyy/MM/dd")
    };
}

var initDateInput = function (option) {
    var op = {
        day: undefined,
        month: undefined,
        year: undefined
    };
    $.extend(true, op, option);
    var timeFormat = {
        day: "yyyy/MM/dd",
        month: "yyyy/MM",
        year:"yyyy"
    }
    var now = new Date();
    var dateinputs = $("#timeParam").find("input[class*='time']");
    dateinputs.each(function (index, el) {
        var parent = $(el).parent().parent();
        var cycle = $(el).parent().parent().data("cycle");
        $(el).on('click', function () {
            WdatePicker({ dateFmt: timeFormat[cycle] });
        });

        $(el).val(op[cycle]?op[cycle].Format(timeFormat[cycle]): now.Format(timeFormat[cycle]));
    });
}

function ModelBind(selector1,selector2,model)
{
    //var values = $("#MDValues").find("strong[class*='mdvalue']");
    var values = $(selector1).find(selector2);
    values.each(function (index, el) {
        var format = el.dataset.format;
        el.innerHTML = stringFormat(format, model); 
    });
}

function TitleBind(time,cycle)
{
    var format = {
        day: "yyyy年MM月dd日",
        month: "yyyy年MM月",
        year: "yyyy年"
    };

    time = new Date(time);
    $(".timetitle").each(function (index, el) {
        var fm = el.innerHTML;
        var titile = fm.replace(/基于(.*)数据/g, "基于{0}数据".format(time.Format(format[cycle])));
        el.innerHTML = titile;
    });
}

var stringFormat = function (format, model) {
    return format.replace(/{(.+?)}/g, function (match, col) {
        return  model[col]!=undefined? model[col]: eval(match)
        ;
    });
}

function initialNav()
{
    var pagename=parseURL(window.location.href).file;
    var li = $(".p-sidebar>.sidenav li");
    $(li).removeClass("active");
    var a = $(li).find("ul>li>a");
    var active = undefined;
    a.each(function (index, el) {
        var href = el.href;
        if (!active&&parseURL(href).file == pagename)
        {
            active = el;
            return;
        }
    });
    $(active).parents('li').addClass("active");
}

var chartColor=
    {
        red: "#cc0000",
        blue: "#6a6aff",
        purple: "#a069ff",
        black: "#000000",
        lightYellow: "#e0c604",
        yellow: "#e0e003",
        lightGreen: "#74c75f",
        green: "#3cc700",
        brown: "#b57f00",
    }
var funcColor = {
    A: chartColor.red,
    B: chartColor.blue,
    C: chartColor.lightGreen,
    D: chartColor.lightYellow,
    E: chartColor.purple,
    F: chartColor.black,
    G: chartColor.brown,
    Z: chartColor.brown
}

var side = {
    toHtml: function (groupid) {
        var result = "";
        var items = this[groupid];
        items.forEach(function (el, index) {
            result += "<li>";
            result += '<a href="javascript:;"><i class="ico {0}"></i><span class="nav-label">{1}</span></a>'.format(el.ico, el.title);
            var ul = "<ul>";
            el.item.forEach(function (e, i) {
                ul += '<li><a href="{1}">{0}</a></li>'.format(e.title, e.href);
            });
            ul += "</ul>";
            result += ul;
            result += "</li>";
        });
        return result;
    },
    toIndexHtml: function (groupid) {
        var result = "";
        var items = this[groupid];
        items.forEach(function (el, index) {
            result += "<li>";
            result += '<a href="#"><i class="ico {0}"></i>{1}</a>'.format(el.ico, el.title);
            var ul = '<div class="drop-nav">';
            el.item.forEach(function (e, i) {
                ul += '<a href="{1}">{0}</a>'.format(e.title, e.href);
            });
            ul += "</div>";
            result += ul;
            result += "</li>";
        });
        return result;
    }
};
side[1] = side[2] = [
    {
        title: "区域能耗分析",
        ico: "ico-flat",
        item: [
            { title: "能耗首页", href: "../Main/main.shtml" },
            { title: "区域能耗概览", href: "../EnergyOverview/EnergyOverView.shtml" },
            { title: "区域能耗排行", href: "../EnergyOverview/EnergyRank.shtml" },
            { title: "区域峰值概览", href: "../EnergyOverview/EnergyMD.shtml" },
            { title: "区域同环比", href: "../EnergyOverview/EnergyMoM.shtml" },
        ]
    },
    {
        title: "节能行政管理",
        ico: "ico-sun",
        item: [
            { title: "电子报表", href: "../EnergyManage/Report.shtml" },
            { title: "业务备案", href: "../SingerBuild/BusinessAduit.shtml" }
        ]
    },
    {
        title: "重点用能单位管理",
        ico: "ico-star",
        item: [
            { title: "单位信息管理", href: "../CompanyManage/CompanyInfo.shtml" },
            { title: "用能数据审核", href: "../CompanyManage/DataAudit.shtml" }
        ]
    },
    {
        title: "能耗对标与公示",
        ico: "ico-list",
        item: [
            { title: "区域对标分析", href: "../EnergyBenchMark/EnergyBench.shtml" },
            { title: "能耗对标公示", href: "../EnergyBenchMark/EnergyBenchToPublic.shtml" },
            { title: "市平台能耗对标", href: "../EnergyBenchMark/EnergyMark.shtml" },
            { title: "区平台能耗对标", href: "../EnergyBenchMark/AreaEnergyMark.shtml" }
        ]
    },
    {
        title: "能耗改造项目管理",
        ico: "ico-code",
        item: [
            { title: "长宁节能减排曲线", href: "../ProjectManage/EcerCurve.shtml" },
            { title: "长宁节能案例管理", href: "../ProjectManage/EcCase.shtml" }
        ]
    },
    {
        title: "单体建筑",
        ico: "ico-home",
        item: [
            { title: "单体能耗概览", href: "../SingerBuild/SingleEnergy1.shtml" },
            { title: "远程抄表", href: "../SingerBuild/RemoteReading1.shtml" },
            { title: "单体对标分析", href: "../SingerBuild/SingleBuildBench.shtml" },
            { title: "单体能耗查询", href: "../SingerBuild/SingleBuildEnergyQuery.shtml" },
            { title: "单体同环比", href: "../SingerBuild/SingleMomYoy.shtml" },
        ]
    },
    {
        title: "楼宇能耗分析",
        ico: "ico-volume",
        item: [
            { title: "多建筑能耗比对", href: "../EnergyAnaly/BuildContrast.shtml" },
            { title: "用能趋势分析", href: "../EnergyAnaly/EnergyTendencyAnaly.shtml" },
            { title: "异常用能提醒", href: "../EnergyAnaly/EnergyAbnormal.shtml" },
            { title: "数据质量判定", href: "../EnergyAnaly/EnergyQuality.shtml" },
            { title: "设备开关机时间", href: "../EnergyAnaly/EnergySwitch.shtml" },
        ]
    },
    {
        title: "需求响应",
        ico: "ico-notice",
        item: [
            { title: "数据管理", href: "../DR/DREventPage.shtml" },
            { title: "资源配置", href: "../DR/DRResourcesdeploment.shtml" },
            { title: "计划配置", href: "../DR/DRDistribution.shtml" }
        ]
    },
]
side[3] = [
        {
            title: "楼宇管理",
            ico: "ico-code",
            item: [
                { title: "工作台", href: "../WorkManage/WorkPlatform.shtml" },
                { title: "业务备案申报", href: "../SingerBuild/BusinessRecord.shtml" },
                { title: "远程抄表", href: "../SingerBuild/RemoteReading1.shtml" }
            ]
        },
         {
             title: "能效管理",
             ico: "ico-home",
             item: [
                 { title: "单体能耗概览", href: "../SingerBuild/SingleEnergy1.shtml" },
                 { title: "单体能耗查询", href: "../SingerBuild/SingleBuildEnergyQuery.shtml" },
             ]
         }
]
side[4] = side[5] = [
        {
            title: "楼宇管理",
            ico: "ico-code",
            item: [
                { title: "登记楼宇", href: "../SingerBuild/BusinessRecord.shtml" },
                { title: "远程抄表", href: "../SingerBuild/RemoteReading1.shtml" }
            ]
        },
        {
            title: "能效管理",
            ico: "ico-home",
            item: [
                { title: "用能数据申报", href: "../SingerBuild/EnergyDeclare.shtml" },
                { title: "单体能耗概览", href: "../SingerBuild/SingleEnergy1.shtml" },
                { title: "单体对标分析", href: "../SingerBuild/SingleBuildBench.shtml" },
                { title: "单体能耗查询", href: "../SingerBuild/SingleBuildEnergyQuery.shtml" },
                { title: "单体同环比", href: "../SingerBuild/SingleMomYoy.shtml" },
            ]
        },
        {
            title: "需求响应",
            ico: "ico-notice",
            item: [
                { title: "数据管理", href: "../DR/DREventPage.shtml" },
            ]
        },
]


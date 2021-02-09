
/*************************************************************************************
    * 文 件 名：       覆盖页面层
    * 创建时间：       2015-11-30 10:16:55
    * 作    者：       lcg
    * 说    明：
    * 修改时间：       2015-11-30
    * 修 改 人：       lcg
*************************************************************************************/


function coverPageDivShow()
{
    //var left1 = ($(window).width() - $('#popDiv').outerWidth()) / 2;
    var left1 = ($(window).width() - 400) / 2;
    var left2 = left1 / $(window).width()*100;

    var top1 = ($(window).height() - 300) / 2 + $(document).scrollTop();
    var top2 = top1 / $(window).height() * 100;
    //var demo = "<div id='popDiv' class='mydiv' style='display: none; width: 400px; height: 300px; left: 45%; top: 40%;'>请稍等，正在加载... ...</div><div id='bg' class='bg' style='display: none;'></div>";
    var demo = "<div id='popDiv' class='mydiv' style='display: none; width: 400px; height: 300px; left: " + left2 + "%; top: " + top2 + "%;'>请稍等，正在处理... ...</div><div id='bg' class='bg' style='display: none;'></div>";
            document.getElementById("cover").innerHTML = demo;

            document.getElementById('popDiv').style.display = 'block';
            document.getElementById('bg').style.display = 'block';
}



function coverPageDivHidden() {
    document.getElementById('popDiv').style.display = 'none';
    document.getElementById('bg').style.display = 'none';
}

//(function ($) {
//    var coverPageDivShow = function aa() { };
//    var coverPageDivHidden = function coverPageDivHidden() { };

//    $.aa = function () {
//        var demo = "<div id='popDiv' class='mydiv' style='display: none; width: 400px; height: 300px; left: 45%; top: 40%;'>请稍等，正在加载... ...</div><div id='bg' class='bg' style='display: none;'></div>";
//        document.getElementById("cover").innerHTML = demo;

//        document.getElementById('popDiv').style.display = 'block';
//        document.getElementById('bg').style.display = 'block';
//    };
//    $.coverPageDivHidden = function () {
//        document.getElementById('popDiv').style.display = 'none';
//        document.getElementById('bg').style.display = 'none';
//    }
//})(jQuery)
































$(function () {

    initUserlogData();

    $('#btnGlobalSearch').click(function () {

        //重置分页组件否则保留上次查询的，一般来说很多问题出现与这三行代码有关如：虽然数据正确但是页码不对仍然为上一次查询出的一致
        $('#pagination').empty();
        $('#pagination').removeData("twbs-pagination");
        $('#pagination').unbind("page");
        //设置默认当前页
        var pagenow = 1;
        //设置默认总页数
        var totalPage = 1;
        //设置默认可见页数
        var visiblecount = 7;
        //加载后台数据页面
        //每页显示条数
        var counts = 10;

        loaddata(pagenow, counts, visiblecount);
    })
})


function initUserlogData() {
    //重置分页组件否则保留上次查询的，一般来说很多问题出现与这三行代码有关如：虽然数据正确但是页码不对仍然为上一次查询出的一致
    $('#pagination').empty();
    $('#pagination').removeData("twbs-pagination");
    $('#pagination').unbind("page");
    //设置默认当前页
    var pagenow = 1;
    //设置默认总页数
    var totalPage = 1;
    //设置默认可见页数
    var visiblecount = 7;
    //加载后台数据页面
    //每页显示条数
    var counts = 10;



    //函数初始化是调用内部函数
    loaddata(pagenow, counts, visiblecount);
}
//initUserlogData();
function loaddata(pagenow, counts, visiblecount) {
    $.ajax({
        url: "/Home/GetArticles",
        type: "get",
        data: {
            "curPage": pagenow,
            "pagesize": counts,
            "categoryid":@ViewData["id"],
                "key": $('#globalSearch').val(),
            },
dataType: "json",
    beforeSend: function (jqXHR) {
        //loadingNo();
        //jqXHR.setRequestHeader('secretKey', key);
        //jqXHR.setRequestHeader('appKey', 'appKey');
    },
success: function (data) {
    $('#total').text('共' + data.totalCount + '条');
    var totalCount = data.totalCount;
    $('#render-div').empty();
    var html = '';
    if (data && data.data && data.data.length > 0) {
        //计算页码数
        totalCount = data.totalCount;//总条数
        var total_page = totalCount / counts;//计算总页码
        totalPage = (parseInt(total_page) == total_page) ? parseInt(total_page) : parseInt(total_page) + 1;
        //遍历渲染数据
        /*$.each(data.data, function (i, v) {
            html += '<div class="col-md-3">' +
                //'<img src="' + v.image + '" alt="' + v.title + '">' +
                '<h4 style="height: 32px;">' + v.title + '</h4>' +
                '<p style="height: 32px;">' + v.subtitle + '</p>' +
                '<p style="height: 32px;">' + v.timespan + '</p>' +
                '</div>';
        });*/

        $('#list_articles').html('');
        //var html=template('weather',data);
        //$("#info").html(html);
        $('#article-main-tpl').tmpl(data.data).appendTo('#list_articles');

    }
    //$('#render-div').append(html);

    //后台总页数与可见页数比较如果小于可见页数则可见页数设置为总页数，
    if (totalPage < visiblecount) {
        visiblecount = totalPage;
    }
    $('#pagination').twbsPagination({
        totalPages: totalPage,
        visiblePages: visiblecount,
        version: '1.1',
        first: "首页",
        prev: "下一页",
        next: "上一页",
        last: "尾页",
        //页面点击时触发事件
        onPageClick: function (event, page) {
            // 将当前页数重置为page
            pagenow = page;
            //调用后台获取数据函数加载点击的页码数据
            loaddata(pagenow, counts, visiblecount);
        }
    });
},
error: function (e) {
    alert("数据访问失败")
}
        });
    }
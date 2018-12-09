
//绑定文章类别
function bindArticleTypeCombox(id) {

    $.ajax({
        type: 'GET',
        url: '/Admins/Article/GetArticleTypesForCombox',
        data: null,
        async: false,
        success: function (json) {

            //根据id查找对象，
            var obj = document.getElementById(id);

            $.each(json, function (i, o) {
                //这个兼容IE与firefox
                obj.options.add(new Option(o.Name, o.Id));
            });

            if ($('#' + id + '').val() == null || $('#' + id + '').val() == '')
                $('#' + id + '').val(-1);
        }
    });
}

//绑定标签
function bindTagsCombox(id) {

    $.ajax({
        type: 'GET',
        url: '/Admins/Article/GetTagsForCombox',
        data: null,
        async: false,
        success: function (json) {

            //根据id查找对象，
            var obj = document.getElementById(id);

            $.each(json, function (i, o) {
                //这个兼容IE与firefox
                obj.options.add(new Option(o.Name, o.Id));
            });

            if ($('#' + id + '').val() == null || $('#' + id + '').val() == '')
                $('#' + id + '').val(-1);
        }
    });
}

function GetAppKey() {

    $.ajax({
        url: '/DeveloperApply/BuildAppKey',
        data: null,
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#AppKey2').val(result.Notice);
            }
            else {
                bootbox.alert(result.Notice);
            }
        },
        error: function (e) {
            console.log("Add异常:" + e.responseText);
        }
    });
}

function GetAppSecrect() {

    $.ajax({
        url: '/DeveloperApply/BuildAppSecrect',
        data: null,
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#AppSecrect2').val(result.Notice);
            }
            else {
                bootbox.alert(result.Notice);
            }
        },
        error: function (e) {
            console.log("Add异常:" + e.responseText);
        }
    });
}

function ShowModal(id, articletypeId, tagIds, title, linkurl, cover,content, summary, description) {
    if (id == 0) {
        //add
        $('#articleModal .modal-title').html('添加');
        $('#Id').val('');
        $('#dp_articleType2').val('');
        $('#dp_tags2').val('');
        $('#Title2').val('');
        $('#LinkUrl2').val('');
        $('#Cover2').val('');
        $('#Summary2').val('');
        $('#Content2').val('');
        $('#Description2').val('');

        $('#articleModal').modal('show');
    } else {
        //update
        $('#articleModal .modal-title').html('修改');

        $('#Id').val(id);
        $('#dp_articleType2').val(articletypeId);
        $('#dp_tags2').val(tagIds);
        $('#Title2').val(title);
        $('#LinkUrl2').val(linkurl);
        $('#Cover2').val(cover);
        $('#Summary2').val(summary);
        $('#Content2').val(content);
        $('#Description2').val(description);

        $('#articleModal').modal('show');
    }
}

function AddOrUpdate() {

    var id = $('#Id').val();
    if (id == 0) {
        Add();
    } else {
        Update();
    }
}

//添加
function Add() {
    var articleTypeId = $('#dp_articleType2').val();
    var tagIds = $('#dp_tags2').val();
    var title = $('#Title2').val();
    var linkeurl = $('#LinkUrl2').val();
    var cover = $('#Cover2').val();
    var summary = $('#Summary2').val();
    var content = $('#Content2').val();
    var description = $('#Description2').val();

    //1.validation
    if (articleTypeId == '' || articleTypeId == null || articleTypeId == undefined || articleTypeId == 0) {
        bootbox.alert("文章类型必选！");
        return;
    }

    if (title == '' || title == undefined || title == null) {
        bootbox.alert("文章标题必填!");
        return;
    }

    if (summary == '' || summary == undefined || summary==null)
    {
        bootbox.alert("文章摘要必填!");
        return;
    }

    //2.add
    $.ajax({
        url: '/Admins/Article/Create',
        data: {
            articleTypeId: articleTypeId,
            tagIds: tagIds,
            title: title,
            linkUrl: linkeurl,
            cover: cover,
            summary:summary,
            content: content,
            description:description
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#articleModal').modal('hide');
                $("#article_datatable").dataTable().fnFilter();
            }
            else {
                bootbox.alert(result.Notice);
            }
        },
        error: function (e) {
            console.log("Add异常:" + e.responseText);
        }
    });
}

//修改
function Update() {

    var id = $('#Id').val();
    var title = $('#Title2').val();
    var linkUrl = $('#LinkUrl2').val();
    var cover = $('#Cover2').val();
    var summary = $('#Summary2').val();
    var content = $('#Content2').val();
    var description = $('#Description2').val();

    //1.validation
    if (title == '') {
        bootbox.alert("标题必填!");
        return;
    }

    //2.update
    $.ajax({
        url: '/Admins/Article/Update',
        data: {
            id: id,
            title: title,
            linkUrl: linkUrl,
            cover: cover,
            summary:summary,
            content:content,
            description: description
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#articleModal').modal('hide');
                $("#article_datatable").dataTable().fnFilter();
            }
            else {
                bootbox.alert(result.Notice);
            }
        },
        error: function (e) {
            console.log("Update异常:" + e.responseText);
        }
    });

}


//置顶
function Top(id) {

    $.ajax({
        url: '/Admins/Article/Top',
        data: {
            id: id
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            bootbox.alert(result.Notice);
        },
        error: function (e) {
            console.log("置顶异常:" + e.responseText);
        }
    });
}

function Recommend(id) {
    $.ajax({
        url: '/Admins/Article/Recommend',
        data: {
            id: id
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            bootbox.alert(result.Notice);
        },
        error: function (e) {
            console.log("推荐异常:" + e.responseText);
        }
    });
}

function Hot(id) {
    $.ajax({
        url: '/Admins/Article/Hot',
        data: {
            id: id
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            bootbox.alert(result.Notice);
        },
        error: function (e) {
            console.log("设置热门异常:" + e.responseText);
        }
    });
}
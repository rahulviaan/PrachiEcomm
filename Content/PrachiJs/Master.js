function AjaxCall(model) {
    var html = '';
    var html1 = '';
    $(".modal ").css("display", "block");
    $.getJSON('@Url.Action("GetItems", "Catalogue")', model, function (dataVal) {
        console.log('Product Data');
        console.log(dataVal.length);
        if (dataVal != null && dataVal.length > 0) {
            $.each(dataVal, function (i, item) {
                // design Change sart here by deepak :01_09_2016
                html = html + '<div class="item  col-xs-4 col-lg-4"  >';
                html = html + '<div class="thumbnail" style="margin-bottom:10px;">';
                html = html + '<img class="group list-group-image" src="http://www.prachipublications.com/uploads/books/32320439browseworld-01-fs.png" alt="" />';
                html = html + '<div class="caption"  style="min-height:auto;" >';// style="min-height:215px"
                html = html + '<h6 class="group inner list-group-item-heading">';
                html = html + '' + item.Title + '';
                html = html + '</h6>';
                html = html + '<div class="row">';//
                html = html + '<div class="">';//row
                html = html + '<div class="col-xs-12 col-md-3">';
                html = html + '<p class="">';
                html = html + 'ISBN:';
                html = html + '</p>';
                html = html + '</div>';
                html = html + '<div class="col-xs-12 col-md-9">';
                html = html + '<p class="">';
                html = html + '' + item.ISBN + '';
                html = html + '</p>';
                html = html + '</div>';
                html = html + '</div>';
                html = html + '<div class="">';//row
                html = html + '<div class="col-xs-12 col-md-3">';
                html = html + '<p class="">';
                html = html + 'Author:';
                html = html + '</p>';
                html = html + '</div>';
                html = html + '<div class="col-xs-12 col-md-9">';
                html = html + '<p class="">';
                html = html + '' + item.Author.substring(0, 15) + '';
                html = html + '</p>';
                html = html + '</div>';
                html = html + '</div>';
                html = html + '<div class="">';//row
                html = html + '<div class="col-xs-12 col-md-5">';
                html = html + '<p class="lead">';
                html = html + '<span class="fa fa-inr">' + item.Price + '</span>';
                html = html + '</p>';
                html = html + '</div>';
                html = html + '<div class="col-xs-12 col-md-7">';
                html = html + '<a class="btn btn-info btn-sm" onclick="addcart(' + item.Id + ');" data-id="' + item.Id + '" ><span class="glyphicon glyphicon-shopping-cart"></span> Add to cart</a>';
                html = html + '</div>';
                html = html + '</div>';
                html = html + '</div>';
                html = html + '</div>';
                html = html + '</div>';
                html = html + '</div>';
                html1 = '<em>Showing 1 – 10 Books of ' + item.count + ' Books</em>';
                // design Change end here by deepak :01_09_2016
            });
        }
        $('#items').append(html);
        $("#CountItem").empty();
        $("#CountItem").append(html1);
        //This Code Added For List And Grid View by Deepak start here:01_09_2016
        var list = $("#hdnlistgrid").val();
        if (list == "list") {
            $('#items .item').addClass('list-group-item');
        }
        if (list == "grid") {
            $('#items .item').removeClass('list-group-item'); $('#products .item').addClass('grid-group-item');
        }
        //This Code Added For List And Grid View by Deepak End here:01_09_2016
        $(".modal ").css("display", "none");
    });
}
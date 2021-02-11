// Change 05_12_2016
function Getcart() {
    var html = '';
    var total = 0;
    var count = 0;
    var elems = [];
    //"GetCarts", "Catalogue"
    $.getJSON('http://localhost:52292/' + 'Catalogue/GetCarts', {}, function (dataVal) {
        if (dataVal != null && dataVal.length > 0) {
            count = dataVal.length;
            $("#empty").css("display", "none");
            $("#nunempty").css("display", "block")
            $.each(dataVal, function (i, item) {
                var price = parseInt(item.Quantity) * parseFloat(item.Price);
                var discount = parseInt(item.Quantity) * parseFloat(item.Discount);
                var subtotal = price - discount;
                total = total + subtotal;
                html = html + '<tr>';
                html = html + '<td>  <img class="itme_pic" style="width:50px;" src="' + item.Image + '" alt=""></td>';
                html = html + '<td>';
                html = html + ' ' + item.Title + ' ';
                html = html + '<br />';
                //html = html + '<span style="margin-top: 3px; float: left;"> <a onclick="RemoveAddCart(' + item.Id + ','+item.ItemId+')" style="text-decoration: none;" class="glyphicon glyphicon-trash" title="Remove" href="#"></a></span>';
                html = html + '</td>';
                html = html + '<td class="tdPrice">';
                html = html + '<i class="fa fa-inr" /> <span class="price"><label>' + price + '</label> </span>';
                html = html + '</td>';
                html = html + '<td class="Quantity">';
                html = html + '<input class="qty" id="Qunt_' + item.Id + '" onChange = "UpdateQuntiy(' + item.Id + ');" style = "width: 25px;margin-left: 25px;" type="text" value="' + item.Quantity + '"/>';
                html = html + '<span id="spnAddCart_' + item.Id + '" style="display: none;text-align: center;"> <a style="margin-left: -80px;" onclick="AddMoreToCart(' + item.Id + ')" title="Update Quentity" href="#">Save</a></span>';
                html = html + '</td>';
                html = html + '<td>';
                html = html + '<i class="fa fa-inr" /> <label>0.00</label>';
                html = html + '</td>';
                html = html + '<td class="Subtotal">';
                html = html + '<i class="fa fa-inr" />';
                html = html + '<label>';
                html = html + '' + subtotal + '.00';
                html = html + '</label>';
                html = html + '</td>';
                html = html + '<td>';
                html = html + '<span style="margin-top: 3px; float: left;"> <a onclick="RemoveAddCart(' + item.Id + ',' + item.ItemId + ')" style="text-decoration: none;" class="glyphicon glyphicon-trash" data-toggle="tooltip" title="Remove" href="#"></a></span>';
                html = html + '</td>';
                html = html + '</tr>';
                elems.push('' + item.ItemId + '');

            });
        }
        else {
            $("#empty").css("display", "block");
            $("#nunempty").css("display", "none")
        }
        window.localStorage.setItem('elems', JSON.stringify(elems));
        $("#MyCart").empty();
        $("#MyCart").css("opacity", "1.0")
        var htmls = ' ' + count + '';
        $("#MyCart").append(htmls);
        $('#tblData').empty();
        $('#tblData').append(html);
        $('#totalvalue').empty();
        $('#totalvalue').html(total + '.00');
    });
}
//Remove From Add Cart Start Here By Deepak:10_09_2016
function RemoveAddCart(id, itemid) {
    var isGood = confirm('Do you wish to remove this item');
    if (isGood) {
        var getUrl = 'http://localhost:52292/' + 'Catalogue/RemoveCart';
        $.getJSON(getUrl, { Id: id }, function (dataVal) {
            $("#cartcheck_" + itemid).removeClass("btn btn-success");
            $("#cartcheck_" + itemid).addClass("btn btn-primary");
            $("#cartcheck_" + itemid).attr("data-toggle", "tooltip")
            $("#cartcheck_" + itemid).attr("data-original-title", "")
            $(".elem").notify("Item Remove!", "success");
            Getcart();
        });
    } else {
    }
}
//Remove From Add Cart End Here By Deepak:10_09_2016

//Update Quantity At Key Press Start here BY Deepak:10_09_2016
function UpdateQuntiy(id) {
    //AddMoreToCart
    var Quantity = $("#Qunt_" + id).val();
    if ($.isNumeric(Quantity) && Quantity > 0) {
        var getUrl = 'http://localhost:52292/' + 'Catalogue/UpdateItemQuntity';
        $.getJSON(getUrl, { Quantity: Quantity, Id: id }, function (dataVal) {
            $(".elem").notify("Item Added!", "success");
            Getcart();
        });
    }
    else {
        $.notify("Enter Onely Number", "error");
        Getcart();
    }
}
function AddMoreToCart(id) {

    var Quantity = $("#Qunt_" + id).val();
    var getUrl = 'http://localhost:52292/' + 'Catalogue/UpdateItemQuntity';
    $.getJSON(getUrl, { Quantity: Quantity, Id: id }, function (dataVal) {
        location.reload(true);
    });
}
function bindmenus() {
    var html = '';
    var getUrl = 'http://localhost:52292/'+'Catalogue/GetMeus';
    $.getJSON('@Url.Action("GetMeus", "Catalogue")', {}, function (dataVal) {
        var Data = JSON.stringify(dataVal);
        if (dataVal != null && dataVal.length > 0) {
            $.each(dataVal, function (i, item) {
                html = html + '<li class="col-sm-3">';
                html = html + '<ul>';
                html = html + '<li><a href="#" id="menu_' + item.Id + '" onclick="ProductDetails(' + item.Id + ')">' + item.Title + '</a></li>';
                html = html + '</ul>';
                html = html + '</li>';
                $("#BookMenus").append(html);
                html = "";
            })
        }
    });
}
$("#btnCheckOut").click(function () {
    var url = "http://localhost:52292/Payment/index";
    window.location.href = url;
});

function addcart(id) {
    alert(id + "Add To Cart");
    var BId = 0;
    BId = id;
    var IsAutheticated = '@Request.IsAuthenticated';
    if (IsAutheticated == 'False') {
        // Login
        //alert("Add To Cart Block");
        $("#hdnretururl").val("Catalogue," + id);
        $('#Login').modal('show');
        $("#sign-up").css("display", "none");
        $("#sign-in").css("display", "block");
        $("#loginidfail").css("display", "none");
    } else {
        //alert("else");
        //alert(BId);
        $.post('@Url.Action("AddToCart", "Catalogue")', { BookId: BId },
          function (returnedData) {
              //console.log(returnedData);
              location.reload(true);
          });
    }
}

//End Change
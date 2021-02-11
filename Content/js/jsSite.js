/////////////////////////////////////////////////////////
function jsLoads() {


    var div = document.createElement("div");
    div.innerHTML = "<!--[if lt IE 9]><i></i><![endif]-->";
    var isIeLessThan9 = (div.getElementsByTagName("i").length == 1);
    if (isIeLessThan9) {
        div.innerHTML = "";

        location.href = "https://prachiindia.com/browser.html";


    }

    //window.onload=jsCategory();
    window.onload = jsR0();
    window.onload = jsR1();
    window.onload = jsR2();
    window.onload = jsR3();
    window.onload = jsR7();
    window.onload = jsR8();
    window.onload = jsR9();


}

//////////////////////////////////////////////////////////////////////////////////////////////////////////

function jsLoadsHome() {
    jsLoads()
    jsShowSeries()
}


function jsCallImage(isId) {
    myurl = "./functions/funCallback_ImageItem.asp?isId=" + isId;

    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("imgItem" + isId + "").innerHTML = xmlhttp.responseText;

        }
        else {
            document.getElementById("imgItem" + isId + "").innerHTML = '<img src=./js/spinner.gif >';
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();


}

///////////////////////////////load banner////////////////////

function makeHttpRequest(url, callback_function, return_xml) {
    var http_request = false;

    if (window.XMLHttpRequest) { // Mozilla, Safari,...
        http_request = new XMLHttpRequest();
        if (http_request.overrideMimeType) {
            http_request.overrideMimeType('text/xml');
        }

    } else if (window.ActiveXObject) { // IE
        try {
            http_request = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                http_request = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) { }
        }
    }

    if (!http_request) {
        alert('Unfortunatelly you browser doesn\'t support this feature.');
        return false;
    }
    http_request.onreadystatechange = function () {
        if (http_request.readyState == 4) {
            if (http_request.status == 200) {

                eval(callback_function + '()');

            } else {
                alert('There was a problem with the request.(Code: ' + http_request.status + ')');
            }
        }
    }
    http_request.open('GET', url, true);
    http_request.send(null);
}

function loadBanner() {

    var reload_after = "3000";

    myurl = "./functions/funBanner.asp";

    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById('ajax-banner').innerHTML = xmlhttp.responseText;
        }
        else {
            //document.getElementById('ajax-banner').innerHTML='<img src=./js/spinner.gif >'; 
            ////jsSpinner('ajax-banner');
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();



    try {
        clearTimeout(to);
    } catch (e) { }

    to = setTimeout("nextAd()", parseInt(reload_after));


}

function nextAd() {
    var now = new Date();
    var url = './functions/funBanner.asp?ts=' + now.getTime();
    makeHttpRequest(url, 'loadBanner', true);
}

/////////////////////////Session//////////////////////////////

function jsRedirect(what, ihash) {
    location.href = what + ".html#" + ihash;

}
///////////////////////catalog starts here ///////////////////////////////////

function jsLoadsCatalog() {
    jsLoads();
    document.getElementById("pageCatalog").innerHTML = "";

    var loc = location.href;
    var current = loc.split('#');
    window.onload = jsCatalogList(current[1], "ALL", "ALL", "ALL", "ALL");
}
////



////show catalog /////

function jsShowSeries() {

    myurl = "./functions/funShowSeries.asp";

    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("opShowSeries").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("opShowSeries").innerHTML = '<img src=./js/spinner.gif >';
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}







///////////////////////catalog Ends here ///////////////////////////////////



///////////////////////produc starts here ///////////////////////////////////
function jsUnLoadProduct(whichId) {


    document.getElementById("orderPage").style.display = "none";

    document.getElementById("orderPageWrapper").style.display = "none";


    document.getElementById("orderPage").innerHTML = "";

}


function jsLoadProduct(whichId) {
    document.getElementById("orderPageWrapper").style.display = "block";
    document.getElementById("orderPage").style.display = "block";

    jsProduct(whichId)

}

////// 

function jsProduct(ihash) {
    myurl = "./functions/funProduct.asp?which=" + ihash;
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("orderPage").innerHTML = xmlhttp.responseText;

            document.getElementById("productTab1").style.borderBottom = "thick solid silver";
            document.getElementById("productTab2").style.borderBottom = "thick solid silver";
            document.getElementById("productTab3").style.borderBottom = "thick solid silver";
            document.getElementById("productTab4").style.borderBottom = "thick solid silver";
            document.getElementById("productTab5").style.borderBottom = "thick solid silver";


            document.getElementById("productTab5").style.backgroundColor = "#fff";
            document.getElementById("productTab5").style.borderBottom = "thick solid #fff";
            document.getElementById("showTab5").style.display = "block";

        }
        else {
            document.getElementById("orderPage").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner("orderPage");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}

////

function jsShowtab(which) {

    document.getElementById("productTab1").style.backgroundColor = "#fcfcfc";
    document.getElementById("productTab2").style.backgroundColor = "#fcfcfc";
    document.getElementById("productTab3").style.backgroundColor = "#fcfcfc";
    document.getElementById("productTab4").style.backgroundColor = "#fcfcfc";
    document.getElementById("productTab5").style.backgroundColor = "#fcfcfc";

    document.getElementById("productTab1").style.borderBottom = "thick solid silver";
    document.getElementById("productTab2").style.borderBottom = "thick solid silver";
    document.getElementById("productTab3").style.borderBottom = "thick solid silver";
    document.getElementById("productTab4").style.borderBottom = "thick solid silver";
    document.getElementById("productTab5").style.borderBottom = "thick solid silver";

    document.getElementById("showTab1").style.display = "none";
    document.getElementById("showTab2").style.display = "none";
    document.getElementById("showTab3").style.display = "none";
    document.getElementById("showTab4").style.display = "none";
    document.getElementById("showTab5").style.display = "none";

    document.getElementById("productTab" + which + "").style.backgroundColor = "#fff";
    document.getElementById("productTab" + which + "").style.borderBottom = "thick solid #fff";

    document.getElementById("showTab" + which + "").style.display = "block";

}

///////////////////////product Ends here /////////////////////////////////////

function jsShowHide(show, hide) {
    document.getElementById(show).style.display = "block";
    document.getElementById(hide).style.display = "none";
}

//////////////////////////////////////////////////////////

function jsMyAccountShow() {

    document.getElementById('myToolTip2').style.display = "block";

    myurl = "./functions/funShowMyAccount.asp";
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("myToolTip2").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("myToolTip2").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner("myToolTip2");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}

function jsMyAccountHide() {
    document.getElementById('myToolTip2').style.display = "none";
}

/////////////////////////////////////////////////////////////////////////////////////


function jsContactusHide() {
    document.getElementById('myToolTip3').innerHTML = "";
    document.getElementById('myToolTip3').style.display = "none";
}


function jsContactus(iwhat) {

    document.getElementById('myToolTip3').style.display = "block";

    myurl = "./functions/funContactus.asp";
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("myToolTip3").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("myToolTip3").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner("myToolTip3");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}

//////////////////////////////////////////////////////////////

function jsFeedback() {


    txtFeedback = document.getElementById('txtFeedback').value;

    myurl = "./functions/funFeedback.asp?txtFeedback=" + txtFeedback;


    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("opFeedback").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("opFeedback").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner("opFeedback");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}
/////////////////////////////////////////////////////////////

function jsCartClose() {
    document.getElementById("orderPageWrapper").style.display = "none";
    document.getElementById("orderPage").style.display = "none";
}

function jsCartOpen() {
    document.getElementById("orderPageWrapper").style.display = "block";
    document.getElementById("orderPage").style.display = "block";


}



function jsCartUpdate(itemId) {

    if (notEmpty(document.getElementById("txtUpdateQTY" + itemId + ""), document.getElementById("errUpdateQTY" + itemId + ""), "[*Invalid input ! Required]")) {
        if (isNumeric(document.getElementById("txtUpdateQTY" + itemId + ""), document.getElementById("errUpdateQTY" + itemId + ""), "[*Invalid Input ! Please Enter Numer Value only ]")) {
            document.getElementById("errUpdateQTY" + itemId + "").innerHTML = "<img src=../images/green_check.gif>";


            document.getElementById("itmId").value = itemId;

            document.getElementById("cartAction").value = "update";

            newitemid = document.getElementById("itmId").value;
            newQTY = document.getElementById("txtUpdateQTY" + newitemid + "").value;

            document.getElementById("newUpdateQTY").value = newQTY;

            opWhich = "orderPage"
            jsWhich = "Cart2";

            jsSession(jsWhich, opWhich);

        }
    }

}

///////////////////////////////////////////////////////////

function jsCart(itemId) {


    if (notEmpty(document.getElementById("txtQty" + itemId + ""), document.getElementById("errQty" + itemId + ""), "[*Invalid input ! Required]")) {
        if (isNumeric(document.getElementById("txtQty" + itemId + ""), document.getElementById("errQty" + itemId + ""), "[*Invalid Input ! Please Enter Numer Value only ]")) {
            document.getElementById("errQty" + itemId + "").innerHTML = "<span class='fa fa-ok'></span>";

            document.getElementById("itmId").value = itemId;

            jsCart1(itemId);
        }
    }
}

function jsCart0(opWhich, jsWhich) {

    document.getElementById("cartAction").value = "cart";
    document.getElementById("itmId").value = "0";
    document.getElementById("newUpdateQTY").value = "0";

    jsCartOpen();
    opWhich = "orderPage"
    jsWhich = "Cart2";
    jsSession(jsWhich, opWhich);
}

function jsCart1(itemId) {

    $("#myModal").modal();

    jsCartOpen();

    document.getElementById("orderPage").innerHTML = '<img src=./js/spinner.gif >';

    document.getElementById("itemidss").innerHTML = itemId;

    document.getElementById("cartAction").value = "new";

    newitemid = document.getElementById("itmId").value;
    newQTY = document.getElementById("txtQty" + newitemid + "").value;

    document.getElementById("newUpdateQTY").value = newQTY;

    opWhich = "orderPage"
    jsWhich = "Cart2";
    jsSession(jsWhich, opWhich);
}



function jsCart2(jsWhich, opWhich) {
    cartAction = document.getElementById("cartAction").value;
    newITEMID = document.getElementById("itmId").value;
    newQTY = document.getElementById("newUpdateQTY").value;


    if (cartAction == "checkout2") {
        txtBillingName = document.getElementById("txtBillingName").value;
        txtBillingPhone = document.getElementById("txtBillingPhone").value;
        txtBillingEmail = document.getElementById("txtBillingEmail").value;
        txtBillingAddress = document.getElementById("txtBillingAddress").value;
        txtBillingCity = document.getElementById("txtBillingCity").value;
        txtBillingState = document.getElementById("txtBillingState").value;
        txtBillingPincode = document.getElementById("txtBillingPincode").value;

        txtShippingPhone = document.getElementById("txtShippingPhone").value;
        txtShippingName = document.getElementById("txtShippingName").value;
        txtShippingPhone = document.getElementById("txtShippingPhone").value;
        txtShippingEmail = document.getElementById("txtShippingEmail").value;
        txtShippingAddress = document.getElementById("txtShippingAddress").value;
        txtShippingCity = document.getElementById("txtShippingCity").value;
        txtShippingState = document.getElementById("txtShippingState").value;
        txtShippingPincode = document.getElementById("txtShippingPincode").value;

        txtRemark = document.getElementById("txtRemark").value;

        myurl = "./functions/funCart2.asp?";
        myurl = myurl + "cartAction=" + cartAction;
        myurl = myurl + "&newITEMID=" + newITEMID;
        myurl = myurl + "&newQTY=" + newQTY;

        myurl = myurl + "&jsWhich=" + jsWhich;
        myurl = myurl + "&opWhich=" + opWhich;

        myurl = myurl + "&txtShippingName=" + txtShippingName;
        myurl = myurl + "&txtShippingPhone=" + txtShippingPhone;
        myurl = myurl + "&txtShippingEmail=" + txtShippingEmail;
        myurl = myurl + "&txtShippingAddress=" + txtShippingAddress;
        myurl = myurl + "&txtShippingCity=" + txtShippingCity;
        myurl = myurl + "&txtShippingState=" + txtShippingState;
        myurl = myurl + "&txtShippingPincode=" + txtShippingPincode;

        myurl = myurl + "&txtBillingName=" + txtBillingName;
        myurl = myurl + "&txtBillingPhone=" + txtBillingPhone;
        myurl = myurl + "&txtBillingEmail=" + txtBillingEmail;
        myurl = myurl + "&txtBillingAddress=" + txtBillingAddress;
        myurl = myurl + "&txtBillingCity=" + txtBillingCity;
        myurl = myurl + "&txtBillingState=" + txtBillingState;
        myurl = myurl + "&txtBillingPincode=" + txtBillingPincode;
        myurl = myurl + "&txtRemark=" + txtRemark;
    }

    else {

        myurl = "./functions/funCart2.asp?cartAction=" + cartAction + "&newITEMID=" + newITEMID + "&newQTY=" + newQTY + "&jsWhich=" + jsWhich + "&opWhich=" + opWhich;

    }

    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("" + opWhich + "").innerHTML = xmlhttp.responseText;

            document.getElementById("cartAction").value = "";
            document.getElementById("itmId").value = "";
            document.getElementById("newUpdateQTY").value = "";
        }
        else {

            document.getElementById("" + opWhich + "").innerHTML = '<img src=./js/spinner.gif >';

        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}

/////////////////////////////////////////////////////////////// 

function jsSession(jsWhich, opWhich) {

    myurl = "./functions/funSession.asp?jsWhich=" + jsWhich + "&opWhich=" + opWhich;

    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("ssnClient").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("ssnClient").innerHTML = '<img src=./js/spinner.gif >';
            ////jsSpinner("ssnClient");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}

///////////////////////////////////////////////////////////////////////////////////////

function jsLogout() {

    myurl = "./functions/funLogout.asp";

    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("orderPage").innerHTML = xmlhttp.responseText;

            document.getElementById("orderPage").innerHTML = "";
            document.getElementById("orderPageWrapper").style.display = "none";
            document.getElementById("orderPage").style.display = "none";
        }
        else {
            document.getElementById("orderPage").innerHTML = '<img src=./js/spinner.gif >';
            $("#myModal").modal('hide');
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}
///
function jsLogin(jsWhich, opWhich) {

    myurl = "./functions/funLogin.asp?jsWhich=" + jsWhich + "&opWhich=" + opWhich;
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("" + opWhich + "").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("" + opWhich + "").innerHTML = '<img src=./js/spinner.gif >';
            ////jsSpinner(""+opWhich+"");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}

///////////////////////////////////////////////////////////////////////////

function jsLogin1(jsWhich, opWhich) {
    myurl = "./functions/funLogin1.asp?jsWhich=" + jsWhich + "&opWhich=" + opWhich;
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("" + opWhich + "").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("" + opWhich + "").innerHTML = '<img src=./js/spinner.gif >';
            ////jsSpinner(""+opWhich+"");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}
//////////////////////////////////////////////////////////////////
function jsLogin2(jsWhich, opWhich) {
    if (notEmpty(document.getElementById("txtLoginId"), document.getElementById("errLoginId"), "[*Required]")) {
        document.getElementById("errLoginId").innerHTML = "<img src=../images/green_check.gif>";
        if (emailValidator(document.getElementById("txtLoginId"), document.getElementById("errLoginId"), "[*Invalid Email Id Format ]")) {
            document.getElementById("errLoginId").innerHTML = "<img src=../images/green_check.gif>";

            jsLogin3(jsWhich, opWhich);
        }
    }
}
/// 
function jsLogin3(jsWhich, opWhich) {

    txtLoginid = document.getElementById("txtLoginId").value;
    txtPassword = document.getElementById("txtPassword").value

    myurl = "./functions/funLogin3.asp?txtLoginid=" + txtLoginid + "&txtPassword=" + txtPassword + "&jsWhich=" + jsWhich + "&opWhich=" + opWhich;
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("" + opWhich + "").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("" + opWhich + "").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner(""+opWhich+"");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}

function jsLoginidfail(isWhat) {
    if (isWhat == 1) {
        document.getElementById("loginidpassword").style.display = "none";
        document.getElementById("loginidfail").style.display = "block";
    }

    if (isWhat == 0) {
        document.getElementById("loginidpassword").style.display = "block";
        document.getElementById("loginidfail").style.display = "none";
    }



}

function jsLoginidfail1(jsWhich, opWhich) {
    if (notEmpty(document.getElementById("txtLoginIDFail"), document.getElementById("errLoginIdFail"), "[*Required]")) {
        if (emailValidator(document.getElementById("txtLoginIDFail"), document.getElementById("errLoginIdFail"), "[*Invalid Email Id Format ]")) {
            document.getElementById("errLoginIdFail").innerHTML = "<img src=../images/green_check.gif>";
            jsLoginidFail2(jsWhich, opWhich);

        }
    }
}

function jsLoginidFail2(jsWhich, opWhich) {

    txtLoginIDFail = document.getElementById("txtLoginIDFail").value;

    myurl = "./functions/funLoginidFail.asp?txtLoginIDFail=" + txtLoginIDFail;
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("loginidfail").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("loginidfail").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner("loginidfail");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}

/////////////////////////////////////checkout starts here//////////////////////////////////////////////////


function jsRegister(jsWhich, opWhich) {

    if (emailValidator(document.getElementById("txtRegisterId"), document.getElementById("errRegisterId"), "[*Invalid Email Id Format ]")) {
        document.getElementById("errRegisterId").innerHTML = "<img src=../images/green_check.gif>";

        isAlreadyEmail();

    }
}


function isAlreadyCallback() {

    if (isAlreadyRegister(document.getElementById("txtReturnId"), document.getElementById("errRegisterId"), "[* Email Already Exist ]")) {
        document.getElementById("errRegisterId").innerHTML = "<img src=../images/green_check.gif >";

        document.getElementById("fieldEmail").style.display = "none";
        document.getElementById("sectionSignIn").style.display = "none";

        document.getElementById("fieldProfile").style.display = "block";




        document.getElementById("lblRegisterId").innerHTML = document.getElementById("txtRegisterId").value;
    }

}

function jsChangeRegisterId() {
    document.getElementById("fieldEmail").style.display = "block";
    document.getElementById("fieldProfile").style.display = "none";
    document.getElementById("lblRegisterId").innerHTML = "";
}


function isAlreadyRegister(elem, erElem, errMsg) {

    if (elem.value == "1") {
        erElem.style.color = "red";
        erElem.innerHTML = errMsg;
        document.getElementById("txtRegisterId").focus();
        return false;

    }
    return true;

}


function isAlreadyEmail() {
    txtRegisterId = document.getElementById("txtRegisterId").value;

    myurl = "./functions/funRegisterIsAlready.asp?txtRegisterId=" + txtRegisterId;
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("errReturn").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("errReturn").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner("errReturn");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();

}

function jsRegister2(jsWhich, opWhich) {



    if (notEmpty(document.getElementById("txtRegisterName"), document.getElementById("errRegisterName"), "[*Required]")) {
        document.getElementById("errRegisterName").innerHTML = " ";
        if (notEmpty(document.getElementById("txtRegisterPhone"), document.getElementById("errRegisterPhone"), "[*Required]")) {
            document.getElementById("errRegisterPhone").innerHTML = " ";
            if (notEmpty(document.getElementById("txtRegisterAddress"), document.getElementById("errRegisterAddress"), "[*Required]")) {
                document.getElementById("errRegisterAddress").innerHTML = " ";
                if (notEmpty(document.getElementById("txtRegisterCity"), document.getElementById("errRegisterCity"), "[*Required]")) {
                    document.getElementById("errRegisterCity").innerHTML = " ";
                    if (notEmpty(document.getElementById("txtRegisterState"), document.getElementById("errRegisterState"), "[*Required]")) {
                        document.getElementById("errRegisterState").innerHTML = " ";
                        if (notEmpty(document.getElementById("txtRegisterPincode"), document.getElementById("errRegisterPincode"), "[*Required]")) {
                            document.getElementById("errRegisterPincode").innerHTML = " ";

                            opWhich = "orderPage"
                            jsWhich = "Cart2";

                            jsRegister3(jsWhich, opWhich);



                        }
                    }
                }
            }
        }
    }

}

function jsRegister3(jsWhich, opWhich) {

    txtRegisterName = document.getElementById("txtRegisterName").value;
    txtRegisterPhone = document.getElementById("txtRegisterPhone").value;
    txtRegisterId = document.getElementById("txtRegisterId").value;
    txtRegisterAddress = document.getElementById("txtRegisterAddress").value;
    txtRegisterCity = document.getElementById("txtRegisterCity").value;
    txtRegisterState = document.getElementById("txtRegisterState").value;
    txtRegisterPincode = document.getElementById("txtRegisterPincode").value;


    myurl = "./functions/funRegisterNow.asp?";

    myurl = myurl + "&jsWhich=" + jsWhich;
    myurl = myurl + "&opWhich=" + opWhich;

    myurl = myurl + "&txtRegisterName=" + txtRegisterName;
    myurl = myurl + "&txtRegisterPhone=" + txtRegisterPhone;
    myurl = myurl + "&txtRegisterId=" + txtRegisterId;
    myurl = myurl + "&txtRegisterAddress=" + txtRegisterAddress;
    myurl = myurl + "&txtRegisterCity=" + txtRegisterCity;
    myurl = myurl + "&txtRegisterState=" + txtRegisterState;
    myurl = myurl + "&txtRegisterPincode=" + txtRegisterPincode;

    var xmlhttp;

    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {

        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("" + opWhich + "").innerHTML = xmlhttp.responseText;
        }
        else {
            document.getElementById("" + opWhich + "").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner(""+opWhich+"");
        }
    }

    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();

}

//////////////////////////////////////////////////////////////////////////////////////////////////////////

function jsCheckout(level) {

    document.getElementById("cartAction").value = level;

    opWhich = "orderPage"
    jsWhich = "Cart2";
    jsSession(jsWhich, opWhich);


}


function jsCheckoutValidate() {
    if (notEmpty(document.getElementById("txtShippingName"), document.getElementById("errShippingName"), "[*Required]")) {
        document.getElementById("errShippingName").innerHTML = "<span class='glyphicon glyphicon-ok'></span>";
        if (isNumeric(document.getElementById("txtShippingPhone"), document.getElementById("errShippingPhone"), "[*Numeric Numbers Only ]")) {
            document.getElementById("errShippingPhone").innerHTML = "<img src=../images/green_check.gif>";
            if (notEmpty(document.getElementById("txtShippingAddress"), document.getElementById("errShippingAddress"), "[*Required]")) {
                document.getElementById("errShippingAddress").innerHTML = "<span class='glyphicon glyphicon-ok'></span>";

                if (notEmpty(document.getElementById("txtShippingCity"), document.getElementById("errShippingCity"), "[*Required]")) {
                    document.getElementById("errShippingCity").innerHTML = "<span class='glyphicon glyphicon-ok'></span>";

                    if (notEmpty(document.getElementById("txtShippingState"), document.getElementById("errShippingState"), "[*Required]")) {
                        document.getElementById("errShippingState").innerHTML = "<span class='glyphicon glyphicon-ok'></span>";

                        if (isNumeric(document.getElementById("txtShippingPincode"), document.getElementById("errShippingPincode"), "[*Numeric Numbers Only ]")) {
                            document.getElementById("errShippingPincode").innerHTML = "<span class='glyphicon glyphicon-ok'></span>";
                            return true;
                        }
                    }
                }
            }
        }
    }
}


function jsCheckout2() {
    if (jsCheckoutValidate()) {
        document.getElementById("cartAction").value = "checkout2";
        level = "checkout2"
        jsCheckout(level)
    }
}

function jsShippingOptions(which) {

    document.getElementById("shippingoptions").style.display = "none";

    if (which == 1) {
        document.getElementById("shippingForm").style.display = "block";
    }

    if (which == 4) {
        document.getElementById("shippingFormSame").style.display = "block";
    }

}
///////////////////////////////////////////////////////////////////

function jsMyOrders() {

    document.getElementById("cartAction").value = "myorders";
    document.getElementById("itmId").value = "0";
    document.getElementById("newUpdateQTY").value = "0";

    opWhich = "orderPage";
    jsWhich = "Cart2";

    jsSession(jsWhich, opWhich);

}

function jsMyOrders1() {
    jsMyOrders2();
}

function jsMyOrders2() {
    myurl = "./functions/funMyorders.asp"
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("opMyorders").innerHTML = xmlhttp.responseText;

        }
        else {
            document.getElementById("opMyorders").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner("opMyorders");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();
}


///////////////////////////////////////////////////////////////////

function jsCP() {

    document.getElementById("cartAction").value = "CP";
    document.getElementById("itmId").value = "0";
    document.getElementById("newUpdateQTY").value = "0";
    opWhich = "orderPage";
    jsWhich = "Cart2";

    jsSession(jsWhich, opWhich);

}

function jsCP1() {

    if (notEmpty(document.getElementById("txtNewPass"), document.getElementById("errNewPass"), "[ * Required ]")) {
        document.getElementById("errNewPass").innerHTML = "<img src=../images/green_check.gif>";
        if (lengthRestriction(document.getElementById("txtNewPass"), document.getElementById("errNewPass"), "6", "100", "[ * Required Minimum 6 Characters ]")) {
            document.getElementById("errNewPass").innerHTML = "<img src=../images/green_check.gif>";
            jsCP2();

        }
    }


}

function jsCP2() {


    txtNewPass = document.getElementById("txtNewPass").value;

    myurl = "./functions/funCP.asp?txtNewPass=" + txtNewPass;
    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("opCP").innerHTML = xmlhttp.responseText;

        }
        else {
            document.getElementById("opCP").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner("opCP");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();

}

//////////////////////////////////////////////////////////////////////////////////////////////////////////


function jsReferus() {

    if (notEmpty(document.getElementById("txtReferus"), document.getElementById("errReferus"), "[ * Required ]")) {
        document.getElementById("errReferus").innerHTML = "<img src=../images/green_check.gif>";
        if (emailValidator(document.getElementById("txtReferus"), document.getElementById("errReferus"), "[ * Invalid Email Id Format ]")) {
            document.getElementById("errReferus").innerHTML = "<img src=../images/green_check.gif>";

            txtReferus = document.getElementById("txtReferus").value;

            myurl = "./functions/funReferus.asp?txtReferus=" + txtReferus;
            var xmlhttp;
            if (window.XMLHttpRequest) {
                // code for IE7+, Firefox, Chrome, Opera, Safari
                xmlhttp = new XMLHttpRequest();
            }
            else {
                // code for IE6, IE5
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                    document.getElementById("opReferus").innerHTML = xmlhttp.responseText;

                }
                else {
                    document.getElementById("opReferus").innerHTML = '<img src=./js/spinner.gif >';
                    //jsSpinner("opReferus");
                }
            }
            xmlhttp.open("GET", myurl, true);
            xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            xmlhttp.send();

        }
    }


}

////////////////////////////////////////
///////////////////////////////////////////////////////////////////

function jsMyProfile() {

    document.getElementById("cartAction").value = "MyProfile";
    document.getElementById("itmId").value = "0";
    document.getElementById("newUpdateQTY").value = "0";
    opWhich = "orderPage";
    jsWhich = "Cart2";

    jsSession(jsWhich, opWhich);

}

function jsMyProfile1() {

    if (notEmpty(document.getElementById("txtName"), document.getElementById("errName"), "[ * Required ]")) {
        document.getElementById("errName").innerHTML = "<img src=../images/green_check.gif>";
        if (lengthRestriction(document.getElementById("txtName"), document.getElementById("errName"), "3", "100", "[ * Required Minimum 3 Characters ]")) {
            document.getElementById("errName").innerHTML = "<img src=../images/green_check.gif>";

            if (notEmpty(document.getElementById("txtAddress"), document.getElementById("errAddress"), "[ * Required ]")) {
                document.getElementById("errAddress").innerHTML = "<img src=../images/green_check.gif>";
                if (notEmpty(document.getElementById("txtCity"), document.getElementById("errCity"), "[ * Required ]")) {
                    document.getElementById("errCity").innerHTML = "<img src=../images/green_check.gif>";
                    if (notEmpty(document.getElementById("txtState"), document.getElementById("errState"), "[ * Required ]")) {
                        document.getElementById("errState").innerHTML = "<img src=../images/green_check.gif>";
                        if (notEmpty(document.getElementById("txtPincode"), document.getElementById("errPincode"), "[ * Required ]")) {
                            document.getElementById("errPincode").innerHTML = "<img src=../images/green_check.gif>";
                            if (lengthRestriction(document.getElementById("txtPincode"), document.getElementById("errPincode"), "6", "100", "[ * Required Minimum 6 Characters ]")) {
                                document.getElementById("errPincode").innerHTML = "<img src=../images/green_check.gif>";
                                if (notEmpty(document.getElementById("txtPhone"), document.getElementById("errPhone"), "[ * Required ]")) {
                                    document.getElementById("errPhone").innerHTML = "<img src=../images/green_check.gif>";


                                    jsMyProfile2();

                                }
                            }
                        }
                    }
                }
            }
        }
    }


}

function jsMyProfile2(errmsg) {


    txtName = document.getElementById("txtName").value;
    txtAddress = document.getElementById("txtAddress").value;
    txtCity = document.getElementById("txtCity").value;
    txtState = document.getElementById("txtState").value;
    txtPincode = document.getElementById("txtPincode").value;
    txtPhone = document.getElementById("txtPhone").value;

    myurl = "./functions/funMyProfile.asp?errmsg=" + errmsg + "&txtName=" + txtName + "&txtAddress=" + txtAddress + "&txtCity=" + txtCity + "&txtState=" + txtState + "&txtPincode=" + txtPincode + "&txtPhone=" + txtPhone + "";

    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            document.getElementById("opMyProfile").innerHTML = xmlhttp.responseText;

        }
        else {
            document.getElementById("opMyProfile").innerHTML = '<img src=./js/spinner.gif >';
            //jsSpinner("opCP");
        }
    }
    xmlhttp.open("GET", myurl, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send();

}

//////////////////////////////////////////////////


function jsCartDelete(itemId) {

    document.getElementById("itmId").value = itemId;
    document.getElementById("cartAction").value = "delete";

    newitemid = document.getElementById("itmId").value;
    newQTY = document.getElementById("txtUpdateQTY" + newitemid + "").value;

    document.getElementById("newUpdateQTY").value = newQTY;

    opWhich = "orderPage"
    jsWhich = "Cart2";

    jsSession(jsWhich, opWhich);

}


//////////////////////////////////////////////////////////////////////////////////////////////////////////










//////////////////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////// RAPHEL SPINNER/////////////////////////
function jsSpinner(spinDiv) {
    var remove = spinner(spinDiv, 70, 120, 12, 25, "#555");
    var form = {
        form: document.getElementsByTagName("form")[0],
        r1: document.getElementById("radius1"),
        r2: document.getElementById("radius2"),
        count: document.getElementById("count"),
        width: document.getElementById("width"),
        color: document.getElementById("color")
    };
    form.form.onsubmit = function () {
        remove();
        remove = spinner(spinDiv, +form.r1.value, +form.r2.value, +form.count.value, +form.width.value, form.color.value);
        return false;
    };
};

function spinner(holderid, R1, R2, count, stroke_width, colour) {

    var sectorsCount = count || 12,
        color = colour || "#fff",
        width = stroke_width || 15,
        r1 = Math.min(R1, R2) || 35,
        r2 = Math.max(R1, R2) || 60,
        cx = r2 + width,
        cy = r2 + width,
        r = Raphael(holderid, r2 * 2 + width * 2, r2 * 2 + width * 2),

        sectors = [],
        opacity = [],
        beta = 2 * Math.PI / sectorsCount,

        pathParams = { stroke: color, "stroke-width": width, "stroke-linecap": "round" };
    Raphael.getColor.reset();

    for (var i = 0; i < sectorsCount; i++) {
        var alpha = beta * i - Math.PI / 2,
            cos = Math.cos(alpha),
            sin = Math.sin(alpha);
        opacity[i] = 1 / sectorsCount * i;
        sectors[i] = r.path([["M", cx + r1 * cos, cy + r1 * sin], ["L", cx + r2 * cos, cy + r2 * sin]]).attr(pathParams);
        if (color == "rainbow") {
            sectors[i].attr("stroke", Raphael.getColor());
        }
    }

    var tick;

    (function ticker() {
        opacity.unshift(opacity.pop());
        for (var i = 0; i < sectorsCount; i++) {
            sectors[i].attr("opacity", opacity[i]);
        }
        r.safari();
        tick = setTimeout(ticker, 1000 / sectorsCount);
    })();

    return function () {
        clearTimeout(tick);
        r.remove();
    };
}

//////////////////////////////////////////////////////////////////////////////////////


function notEmpty(elem, erElem, errMsg) {
    if (elem.value.length == 0) {
        erElem.style.color = "red";
        erElem.innerHTML = errMsg;
        elem.focus();
        return false;
    }
    return true;
}

function isNumeric(elem, erElem, errMsg) {
    var numericExpression = /^[0-9]+$/;
    if (elem.value.match(numericExpression)) {
        return true;
    } else {
        erElem.style.color = "red";
        erElem.innerHTML = errMsg;
        elem.focus();
        return false;
    }
}

function isAlphabet(elem, erElem, errMsg) {
    var alphaExp = /^[a-zA-Z]+$/;
    if (elem.value.match(alphaExp)) {
        erElem.style.color = "green";
        erElem.innerHTML = "ok";
        return true;
    } else {
        erElem.style.color = "red";
        erElem.innerHTML = errMsg;
        elem.focus();
        return false;
    }
}

function isAlphanumeric(elem, erElem, errMsg) {
    var alphaExp = /^[0-9a-zA-Z]+$/;
    if (elem.value.match(alphaExp)) {
        return true;
    } else {
        erElem.style.color = "red";
        erElem.innerHTML = errMsg;
        elem.focus();
        return false;
    }
}

function lengthRestriction(elem, erElem, min, max, errMsg) {
    var uInput = elem.value;
    if (uInput.length >= min && uInput.length <= max) {
        return true;
    } else {
        erElem.style.color = "red";
        erElem.innerHTML = errMsg;
        elem.focus();
        return false;
    }
}

function madeSelection(elem, erElem, errMsg) {
    if (elem.value == "Please Choose") {
        erElem.style.color = "red";
        erElem.innerHTML = errMsg;
        elem.focus();
        return false;
    } else {
        return true;
    }
}

function emailValidator(elem, erElem, errMsg) {
    var emailExp = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
    if (elem.value.match(emailExp)) {
        return true;
    } else {
        erElem.style.color = "red";
        erElem.innerHTML = errMsg;
        elem.focus();
        return false;
    }
}
/////////////VALIDATOR ENDS//////////////////////////////////////////////////

function NewWindow(mypage, myname, w, h, scroll) {
    LeftPosition = (screen.width) ? (screen.width - w) / 2 : 0;
    TopPosition = (screen.height) ? (screen.height - h) / 2 : 0;
    settings =
    'height=' + h + ',width=' + w + ',top=' + TopPosition + ',left=' + LeftPosition + ',scrollbars=yes,resizable'
    win = window.open(mypage, myname, settings)
    win.focus();
}



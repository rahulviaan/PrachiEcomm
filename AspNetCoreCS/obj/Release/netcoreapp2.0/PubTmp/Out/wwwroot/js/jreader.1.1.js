$(function () {
$('body').bind('copy paste',function(e) {
    e.preventDefault(); return false; 
});
document.addEventListener("contextmenu", function(e){
    e.preventDefault();
}, false);
    var isCall = true;
    $(document).on('click', 'body', function () { 
        $(".cssnote").remove(); 
    });
    $(document).on('click', '.jsshownotes', function () {       
        isCall = false;
        $(".cssnote").remove();
        var div = '<span class="cssnote"><button title="Add Note" class="jsAddNote text-center" type="button"></button><button title="Remove" class="text-center jsremove text-danger" type="button"></button></span>';
        $(this).html(div + $(this).html())
        return false;
    });
    $(document).on('click', '.jsremove', function () {
       
        var key = $(this).parent().parent().attr("id");
        
        $(this).parent().parent().removeAttr('class');
        $(this).parent().parent().removeAttr('style');
        $(this).parent().parent().replaceWith('<delhighlightdiv style="display:inline">' + $(this).parent().parent().text() + '</delhighlightdiv>')
        
        //$(this).parent().parent().replace('<delhighlightdiv style="display:inline">', "");
        //$(this).parent().parent().replace('</delhighlightdiv>', "");
        //var Htm = document.getElementsByTagName("body")[0].innerHTML;
        //Htm = Htm.replace('<delhighlightdiv style="display:inline"> ', '')
        //Htm = Htm.replace('</delhighlightdiv>', '')       
        //document.getElementsByTagName("body")[0].innerHTML = Htm;
       
        window.external.RemoveNote(document.documentElement.innerHTML, key)
    });
    $(document).on('click', '.jsAddNote', function () {
        var selectedtext = $.trim($(this).parent().parent().text())

        var dvid = "key_" + Date.now();
        var _key = dvid;
        $(this).parent().parent().addClass("cssAddNote")
        var key = $(this).parent().parent().attr("id");

        //if (key == null) {

        //    $(this).parent().parent().attr("key", dvid);
        //}
        //else
        //{
        //    _key = dvid;
        //}

        var x = window.external.AddNote(selectedtext, document.documentElement.innerHTML, key);

        if (x == "Added") {
            $(".cssnote").remove();
        }
        return false;
    });
});
function SetTooltip(key, note) {
    $(".cssnote").remove();
    if (note != "") {
        
        $("#" + key + "").attr("title", note);
        window.external.HighLightText(document.documentElement.innerHTML);
    }
    else {
        $("#" + key + "").removeAttr("title");
        $("#" + key + "").removeClass("cssAddNote");
        window.external.HighLightText(document.documentElement.innerHTML);
    } 
}
function SetFocus(key)
{
    try
    {
        var htm = $("#" + key + "").position().top;
  
        window.scrollTo(0,htm);
    }
   catch(ex){}
}
function getSafeRanges(a) {
    var b = a.commonAncestorContainer, c = new Array(0), d = new Array(0);
    if (a.startContainer != b)
        for (var e = a.startContainer; e != b; e = e.parentNode)
            c.push(e);
    if (0 < c.length)
        for (var e = 0; e < c.length; e++) { var f = document.createRange(); e ? (f.setStartAfter(c[e - 1]), f.setEndAfter(c[e].lastChild)) : (f.setStart(c[e], a.startOffset), f.setEndAfter(c[e].nodeType == Node.TEXT_NODE ? c[e] : c[e].lastChild)), d.push(f) } var g = new Array(0), h = new Array(0); if (a.endContainer != b) for (var e = a.endContainer; e != b; e = e.parentNode) g.push(e); if (0 < g.length) for (var e = 0; e < g.length; e++) { var i = document.createRange(); e ? (i.setStartBefore(g[e].firstChild), i.setEndBefore(g[e - 1])) : (i.setStartBefore(g[e].nodeType == Node.TEXT_NODE ? g[e] : g[e].firstChild), i.setEnd(g[e], a.endOffset)), h.unshift(i) } if (!(0 < c.length && 0 < g.length)) return [a]; var j = document.createRange(); return j.setStartAfter(c[c.length - 1]), j.setEndBefore(g[g.length - 1]), d.push(j), response = d.concat(h), response
} function RemoveHighLights() { this.style = 'display: inline' }
function highlightRange(a) {
    if ($.trim(a) == '') { return false; }

    var dvid = "dv_" + Date.now();
    var b = document.createElement('highlightdiv');
    b.className = 'jsshownotes';
    b.id = dvid;
    b.ondblclick = RemoveHighLights,
    b.setAttribute('style', 'color:#000;background-color: yellow; display: inline;'),
    a.surroundContents(b)
    window.external.HighLightText(document.documentElement.innerHTML)
}
//function highlightSelection() {
   // for (var a = window.getSelection().getRangeAt(0), b = getSafeRanges(a), c = 0; c < b.length; c++)
        //highlightRange(b[c]);
//}
function getSelectedText(el) {
    var sel, range;
    if (typeof el.selectionStart == 'number' && typeof el.selectionEnd == 'number') {
        return el.value.slice(el.selectionStart, el.selectionEnd);
    } else if (
            (sel = document.selection) &&
            sel.type == 'Text' &&
            (range = sel.createRange()).parentElement() == el) {
        return range.text;
    }
    return '';
}
 



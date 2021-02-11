function ShowPopup() {
    bootbox.hideAll();
    bootbox.dialog({
        title: "",
        message: "<h4>Please wait....</h4>",
        closeButton: false
    });
}

function HidePopup() {

    bootbox.hideAll();

}
"use strict";


function UploadConfiguration() {
    var connection = new signalR.HubConnectionBuilder().withUrl("/BroadcastHub").build();

    connection.on("ReceiveMessage", function (message, percentage) {
        if (message != "") {
            $("#Progresstext").css("display", "block");
            $("#Progressbar").css("display", "flex");
            $(".progresConfiguration").css("display", "flex");
            $("#Progresstext").html(message);
            $(".progresConfiguration").html(percentage + "%");
            $(".progress-bar").css("width", percentage + "%");
        }
        if (message == "ChapterContents" && percentage == 100) {
            $("#Progresstext").css("display", "none");
            $("#Progressbar").css("display", "none");
        }
    });

    connection.start().then(() => {
        var msg = "";
        var progressmsg = 0;
        connection.invoke("SendMessage", msg, progressmsg).catch(err => console.error(err.toString()));
    });
}

function UploadbBundle() {
    var connection = new signalR.HubConnectionBuilder().withUrl("/BroadcastHub").build();

    connection.on("ReceiveMessageBundle", function (message, percentage) {
        if (message != "") {
            $("#Progresstextbundle").css("display", "block");
            $("#Progressbarbundle").css("display", "flex");
            $(".progresBundle").css("display", "flex");
            $("#Progresstextbundle").html(message);
            $(".progresBundle").html(percentage + "%");
            $(".progress-bar").css("width", percentage + "%");
        }
        if (message == "" && percentage == 100) {
            $("#Progresstextbundle").css("display", "none");
            $("#Progressbarbundle").css("display", "none");
        }
    });

    connection.start().then(() => {
        var msg = "";
        var progressmsg = 0;
        connection.invoke("SendMessageBundle", msg, progressmsg).catch(err => console.error(err.toString()));
    });
}
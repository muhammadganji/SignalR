var chatBox = $("#ChatBox");

// اتصال با هاب کانکشن پشتیبانی
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

connection.start();

//connection.invoke('SendNewMessage', "محمدشون", "اولین پیام ارسالی");

// نمایش چت باکس به کاربر
function showChatDialog() {
    chatBox.css("display", "block");
}

function Init() {
    setTimeout(showChatDialog, 1000);


    var NewMessageForm = $("#NewMessageForm");
    NewMessageForm.on("submit", function (e) {
        e.preventDefault();
        var message = e.target[0].value;
        e.target[0].value = '';
        sendMessage(message);
    });






}

// ارسال پیام به سرور
function sendMessage(text) {
    connection.invoke('SendMessage', "بازدید کننده", text);

}

// دریافت پیام از سرور
connection.on("getNewMessage", getNewMessage);
function getNewMessage(sender, message, time) {
    // <li> <div> <span class='name'>Sender</span> <span class='time'>time</span> </div> <div class='message'> message </div> </li>
    $("#Messages").append("<li> <div> <span class='name'>" + sender + "</span> <span class='time'>" + time + "</span> </div> <div class='message'> " + message + "</div> </li>")
}








$(document).ready(function () {
    Init();
})
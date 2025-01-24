"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disable the 'send' button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    const li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user}: ${message}`;
})

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
})

document.getElementById("sendButton").addEventListener("click", function (event) {
    const user = document.getElementById("userInput").value;
    const toWho = document.getElementById("toWhoInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", toWho, user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
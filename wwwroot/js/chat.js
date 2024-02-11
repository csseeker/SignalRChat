"use strict";

const DEFAULT_SERVER_TIMEOUT_MILISECODNS = 60000;
const KEEP_ALIVE_INTERVAL_IN_MILLISECONDS = 5000;

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub")
                            .withServerTimeout(DEFAULT_SERVER_TIMEOUT_MILISECODNS)
                            .withAutomaticReconnect()
                            .withKeepAliveInterval(KEEP_ALIVE_INTERVAL_IN_MILLISECONDS) // Send a keep-alive message every KEEP_ALIVE_INTERVAL_IN_MILLISECONDS seconds
                            .build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    // alert("connection.state: " + connection.state);

    if (connection.state !== "Connected" || connection.state !== "Connecting") {
        // if the connection is not open, start it
        connection.start().then(function () {
                    document.getElementById("sendButton").disabled = false;
                }).catch(function (err) {
                    return console.error(err.toString());
                });
    }
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

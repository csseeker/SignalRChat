"use strict";

const DEFAULT_SERVER_TIMEOUT_MILISECODNS = 60000;
const KEEP_ALIVE_INTERVAL_IN_MILLISECONDS = 3000;

// Create an array of strings that will be used to generate a random user name
const userNames = ["Shripad-T1", "Jatan-T2", "Shripad-T3", "Jatan-T1", "Shripad-T2", "Jatan-T3"];

// Pick a user name at random from the array userNames
var hereUser = userNames[Math.floor(Math.random() * userNames.length)];
// alert("hereUser: " + hereUser);

document.getElementById("userInput").value = hereUser;

var connection = new signalR.HubConnectionBuilder().withUrl(`/chatHub?userName=${hereUser}`) // we can pass the token here with short ttl
                            .withServerTimeout(DEFAULT_SERVER_TIMEOUT_MILISECODNS) // We can control how much time to give server to respond
                            .withAutomaticReconnect() // Tries to reconnect (we can also decide policy) if the connection is lost
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

// connection.start().then(function () {
//     document.getElementById("sendButton").disabled = false;
//     // alert("connectionId: " + connection.connectionId);
//     document.getElementById("connectionId").value = connection.connectionId;
// }).catch(function (err) {
//     return console.error(err.toString());
// });

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    // alert("connection.state: " + connection.state);

    if (connection.state === "Disconnected") {
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

async function start() {
    try {
        // Get the latest token by calling some API and Rebuild connection here

        await connection.start();
        // await connection.start({
        //     withCredentials: true, // Set to true if you want to include cookies in the request
        //     headers: {
        //         "Custom-Header": "Custom-Value" // Add custom headers to the request
        //     },
        //     queryString: `userName=${hereUser}&userConnectionId=${connection.connectionId}` // Add query string parameters to the request
        // });
        console.log("SignalR Connected.");

        document.getElementById("sendButton").disabled = false;
        // alert("connectionId: " + connection.connectionId);
        document.getElementById("connectionId").value = connection.connectionId;

    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

// Start the connection.
// alert("Starting connection...");
start();

let connection = null;

function connectToChat() {
    const token = localStorage.getItem('accessToken');

    if (!token) {
        window.location.href = 'login.html';
        return;
    }

    connection = new signalR.HubConnectionBuilder()
        .withUrl(`${API_URL}/chatHub`, {
            accessTokenFactory: () => token
        })
        .build();


    connection.on("ReceiveMessage", (messageDto) => {

        displayMessage(
            messageDto.userName,
            messageDto.text,
            formatTime(messageDto.timestamp),
            messageDto.sentiment,
            messageDto.userName === localStorage.getItem('username')
        );
    });

    connection.start()
        .then(() => {

            loadMessageHistory();
        })
        .catch(err => {


            if (err.statusCode === 401) {
                logout();
            } else {
                setTimeout(connectToChat, 5000);
            }
        });
}


function formatTime(timestamp) {
    const date = new Date(timestamp);
    return date.toLocaleTimeString('uk-UA', {
        hour: '2-digit',
        minute: '2-digit'
    });
}


function getSentimentColor(sentiment) {
    switch (sentiment) {
        case 1: return '#d4edda';
        case -1: return '#f8d7da';
        default: return '#e2e3e5';
    }
}

function getSentimentEmoji(sentiment) {
    switch (sentiment) {
        case 1: return '😊';
        case -1: return '😔';
        default: return '😐';
    }
}


function displayMessage(user, message, time, sentiment, isOwn = false) {
    const messagesContainer = document.getElementById('messagesContainer');
    const welcomeMessage = document.getElementById('welcomeMessage');

    if (welcomeMessage) {
        welcomeMessage.style.display = 'none';
    }

    const messageElement = document.createElement('div');
    messageElement.className = `message ${isOwn ? 'own' : 'other'}`;


    messageElement.style.backgroundColor = getSentimentColor(sentiment);

    messageElement.innerHTML = `
        <div class="fw-bold small">${user} ${getSentimentEmoji(sentiment)}</div>
        <div>${message}</div>
        <div class="small opacity-75 mt-1">${time}</div>
    `;

    messagesContainer.appendChild(messageElement);
    messagesContainer.scrollTop = messagesContainer.scrollHeight;
}

function sendMessage() {
    const messageInput = document.getElementById('messageInput');
    const message = messageInput.value.trim();

    if (message && connection) {
        const username = localStorage.getItem('username');

        connection.invoke("SendMessage", username, message)
            .then(() => { })
            .catch(err => {

                if (err.statusCode === 401) {
                    logout();
                }
            });
        messageInput.value = "";
    }
}

async function loadMessageHistory() {

    if (connection) {
        await connection.invoke("GetMessageHistory");
    }

}

document.addEventListener('DOMContentLoaded', function () {
    const messageInput = document.getElementById('messageInput');
    if (messageInput) {
        messageInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                sendMessage();
            }
        });

        const token = localStorage.getItem('accessToken');
        if (token) {
            connectToChat();
        } else {
            window.location.href = 'login.html';
        }
    }
});

window.sendMessage = sendMessage;
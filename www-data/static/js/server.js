document.addEventListener('DOMContentLoaded', () => {

    const username = document.querySelector('#get-username').innerHTML;
    const user_id = document.querySelector('#get-user_id').innerHTML;

    let room_id = '1';
    let room_name;

    // Connect to websocket
    const socket = new WebSocket('ws://' + location.host + '/api');
    socket.onopen = ws => {
        let message = {
            message: 'open',
            user_id: user_id,
            username: username,
            text: 'Connection is open.',
            room_id: room_id,           //TODO : change to room_id later
        }
        socket.send(JSON.stringify(message));
        message.message = 'connected';
        message.text = 'connected';
        socket.send(JSON.stringify(message));
        console.log(`User {{ username }} has  connected.`);
    };
    // Retrieve username


    // Set default room


    // joinRoom("Lounge");

    // Send messages
    document.querySelector('#send_message').onclick = () => {
        let datetime = new Date().toLocaleString();
        let message = {
            message: 'message',
            user_id: user_id,
            username: username,
            text: document.querySelector('#user_message').value,
            room_id: room_id,
            created_at: datetime
        }
        socket.send(JSON.stringify(message));
        // socket.send('incoming-msg', {
        //     'msg': document.querySelector('#user_message').value,
        //     'username': username, 'room': room
        // });

        document.querySelector('#user_message').value = '';
    };

    // Display all incoming messages
    socket.onmessage = ev => {

        // Display current message
        let data = JSON.parse(ev.data)
        if (data.text) {
            const p = document.createElement('p');
            const span_username = document.createElement('span');
            const span_timestamp = document.createElement('span');
            const br = document.createElement('br')
            // Display user's own message
            if (data.username == username) {
                p.setAttribute("class", "my-msg");

                // Username
                span_username.setAttribute("class", "my-username");
                span_username.innerText = data.username;

                // Timestamp
                span_timestamp.setAttribute("class", "timestamp");
                span_timestamp.innerText = data.created_at;

                // HTML to append
                p.innerHTML += span_username.outerHTML + br.outerHTML + data.text + br.outerHTML + span_timestamp.outerHTML

                //Append
                document.querySelector('#display-message-section').append(p);
            }
            // Display other users' messages
            else if (typeof data.username !== 'undefined') {
                p.setAttribute("class", "others-msg");

                // Username
                span_username.setAttribute("class", "other-username");
                span_username.innerText = data.username;

                // Timestamp
                span_timestamp.setAttribute("class", "timestamp");
                span_timestamp.innerText = data.created_at;

                // HTML to append
                p.innerHTML += span_username.outerHTML + br.outerHTML + data.text + br.outerHTML + span_timestamp.outerHTML;

                //Append
                document.querySelector('#display-message-section').append(p);
            }
            // Display system message
            else {
                printSysMsg(data.text);
            }
        }
        scrollDownChatWindow();
    };

    // Select a room
    document.querySelectorAll('.select-room').forEach(p => {
        p.onclick = () => {
            let newRoom_name = p.innerHTML
            let newRoom_id = p.id
            // Check if user already in the room
            // leaveRoom(room_id);
            // joinRoom(newRoom_id);
            // room_id = newRoom_id;
            // room_name = newRoom_name;
            if (newRoom_id === room_id) {
                let msg = `You are already in ${room_name} room.`;
                printSysMsg(msg);
            } else {
                leaveRoom(room_id);
                joinRoom(newRoom_id);
                room_id = newRoom_id;
                room_name = newRoom_name;

            }
        }
    });

        // Logout from chat
        document.querySelector("#logout-btn").onclick = () => {
            leaveRoom(room_id);
        };

        // Trigger 'leave' event if user was previously on a room
        function leaveRoom(room_id) {
            let message = {
                message: 'leave',
                user_id: user_id,
                username: username,
                text: 'has left the room',
                room_id: room_id,
            }
            // Join room
            socket.send(JSON.stringify(message));

            document.querySelectorAll('.select-room').forEach(p => {
                p.style.color = "black";
            });
        }

        // Trigger 'join' event
        function joinRoom(room_id) {

            let message = {
                message: 'join',
                user_id: user_id,
                username: username,
                text: 'has joined to room',
                room_id: room_id,
            }
            // Join room
            socket.send(JSON.stringify(message));

            // Highlight selected room
            document.querySelector('#' + CSS.escape(room_id)).style.color = "#ffc107";
            document.querySelector('#' + CSS.escape(room_id)).style.backgroundColor = "white";

            // Clear message area
            document.querySelector('#display-message-section').innerHTML = '';

            // Autofocus on text box
            document.querySelector("#user_message").focus();
        }

        // Scroll chat window down
        function scrollDownChatWindow() {
            const chatWindow = document.querySelector("#display-message-section");
            chatWindow.scrollTop = chatWindow.scrollHeight;
        }

        // Print system messages
        function printSysMsg(msg) {
            const p = document.createElement('p');
            p.setAttribute("class", "system-msg");
            p.innerHTML = msg;
            document.querySelector('#display-message-section').append(p);
            scrollDownChatWindow()

            // Autofocus on text box
            document.querySelector("#user_message").focus();
        }
    });

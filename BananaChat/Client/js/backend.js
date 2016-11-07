function ChatSession(server) {
    var state = {
        chatId: null,
        customerName: null,
        messagesRetrieved: 0,
        listening: false,
        notifyNewMessages: nop,
        notifyChats: nop
    };
    this.startChat = function (customerName) {
        state.chatId = newId();
        state.customerName = customerName;
        state.listening = true;
        state.messagesRetrieved = 0;

        var data = {
            chatId: state.chatId,
            customerName: state.customerName
        };

        server.post("start", data);

        pollNewMessages();
    };
    this.joinChat = function (chatId) {
        state.chatId = chatId;
        state.listening = true;
        state.messagesRetrieved = 0;

        var data = {
            chatId: state.chatId
        };

        server.post("join", data);

        getAllMessages();
    };
    this.sendCustomerMessage = function (text) {
        var data = {
            chatId: state.chatId,
            text: text
        };

        server.post("customer-message", data);
    };
    this.sendSupportMessage = function (text) {
        var data = {
            chatId: state.chatId,
            text: text
        };

        server.post("support-message", data);
    };
    this.closeChat = function () {
        state.listening = false;

        var data = {
            chatId: state.chatId
        };

        server.post("close", data);

    };
    this.getChats = function () {
        pollChats();
    };
    this.whenMessagesArrive = function (fn) {
        state.notifyNewMessages = fn;
    };
    this.whenChatsArrive = function (fn) {
        state.notifyChats = fn;
    };

    function pollChats() {
        server.get(
            "list",
            null,
            function (chats) {
                state.notifyChats(chats);

                setTimeout(pollChats, 5000);
            });

    }

    function pollNewMessages() {
        var query = {
            chatId: state.chatId,
            skip: state.messagesRetrieved
        };
        server.get(
            "new-messages",
            query,
            function (messages) {
                state.notifyNewMessages(messages);
                state.messagesRetrieved += messages.length;

                if (state.listening) {
                    setTimeout(pollNewMessages, 500);
                }
            });
    }

    function getAllMessages() {
        var query = {
            chatId: state.chatId
        };
        server.get(
            "all-messages",
            query,
            function (messages) {
                state.notifyNewMessages(messages);
                state.messagesRetrieved += messages.length;
                pollNewMessages();
            });
    }

    function nop() {

    }


    function newId() {
        var arr = new Uint8Array(5);
        window.crypto.getRandomValues(arr);
        return [].map.call(arr, fromByte).join("");

        function fromByte(byte) {
            return ("0" + byte.toString(36)).slice(-2);
        }

    }
}

function Server(baseUrl, target) {

    this.post = function (url, data) {
        post(baseUrl + url, data);
    };
    this.get = function (url, query, fn) {
        JSONP.get(baseUrl + url, query, fn);
    };

    function post(url, data) {
        event.preventDefault();

        var form = document.createElement("form");
        form.setAttribute("method", "post");
        form.setAttribute("target", target);
        form.setAttribute("action", url);

        var message = document.createElement("input");
        message.setAttribute("name", "message");
        message.setAttribute("type", "hidden");
        message.setAttribute("value", JSON.stringify(data));

        form.appendChild(message);

        document.body.appendChild(form);
        form.submit();
        form.remove();
    }
}
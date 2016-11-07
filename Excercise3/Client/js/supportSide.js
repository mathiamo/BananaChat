function SupportSide(session) {
    var show = {
        start: ko.observable(true),
        requests: ko.observable(false),
        title: ko.observable(false),
        messages: ko.observable(false)
    };
    var focus = {
        name: ko.observable(),
        message: ko.observable()
    };
    var name = ko.observable("");
    var customerName = ko.observable();
    var chats = ko.observableArray();
    var messages = ko.observableArray();
    var message = ko.observable("");

    var inactive = this.inactive = ko.observable(true);

    this.panes = {
        start: {
            show: show.start,
            showRequests: showRequests,
            focus: focus.name
        },
        requests: {
            show: show.requests,
            chats: chats,
            joinChat: joinChat,
            name: name
        },
        title: {
            show: show.title,
            closeChat: closeChat
        },
        messages: {
            show: show.messages,
            messages: messages,
            message: message,
            sendMessage: sendMessage,
            customerName: customerName,
            focus: focus.message
        }
    };

    function showRequests() {
        show.start(false);
        show.title(true);
        show.requests(true);
        show.messages(false);
        inactive(false);

        focus.name(true);

        session.getChats();
        session.whenChatsArrive(listChats);
    }

    function joinChat(chat) {
        show.requests(false);
        show.title(true);
        show.messages(true);
        focus.message(true);
        customerName(chat.customerName);
        messages([]);
        session.joinChat(chat.id, name());
        session.whenMessagesArrive(appendMessages);
    }

    function sendMessage() {
        session.sendSupportMessage(message());
        message("");
        focus.message(true);
    }

    function closeChat() {
        show.start(true);
        show.title(false);
        show.requests(false);
        show.messages(false);
        inactive(true);
    }

    function appendMessages(changes) {
        ko.utils.arrayPushAll(messages, changes);
        if (changes.length) {
            var target = document.getElementById("support-marker");
            target.parentNode.scrollTop = target.parentNode.scrollHeight;
        }
    }

    function listChats(changes) {
        chats(changes.map(function (data) {
            return new Chat(data);
        }));
    }
}

function Chat(data) {
    var selectable = ko.observable(false);
    this.id = data.chatId;
    this.customerName = data.customerName;
    this.waited = "Ventet " + formatWaitTime();
    this.selectable = selectable;
    this.notSelectable = ko.computed(function () {
        return !selectable();
    });

    this.enable = function () {
        selectable(true);
    };
    this.disable = function () {
        selectable(false);
    };

    function formatWaitTime() {
        var seconds = data.waitedForSeconds;
        if (seconds < 0) {
            return "0 sek";
        }
        if (seconds < 60) {
            return seconds + " sek";
        }
        if (seconds < 3600) {
            var minutes = Math.floor(seconds / 60);
            return minutes + " min";
        }

        return "> 1 time";

    }
}
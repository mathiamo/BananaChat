﻿function CustomerSide(session) {
    var show = {
        help: ko.observable(true),
        title: ko.observable(false),
        prepare: ko.observable(false),
        messages: ko.observable(false)
    };
    var focus = {
        name: ko.observable(),
        message: ko.observable()
    };
    var name = ko.observable("");
    var messages = ko.observableArray();
    var message = ko.observable("");
    var inactive = this.inactive = ko.observable(true);

    this.panes = {
        help: {
            show: show.help,
            prepareChat: prepareChat
        },
        title: {
            show: show.title,
            closeChat: closeChat
        },
        prepare: {
            show: show.prepare,
            startChat: startChat,
            name: name,
            focus: focus.name
        },
        messages: {
            show: show.messages,
            messages: messages,
            message: message,
            sendMessage: sendMessage,
            name: name,
            focus: focus.message
        }
    };

    function prepareChat() {
        show.help(false);
        show.title(true);
        show.prepare(true);
        focus.name(true);
        inactive(false);
    }

    function startChat() {
        show.prepare(false);
        show.messages(true);
        focus.message(true);
        messages([]);
        session.startChat(name());
        session.whenMessagesArrive(appendMessages);
    }

    function sendMessage() {
        session.sendCustomerMessage(message());
        message("");
        focus.message(true);
    }

    function closeChat() {
        show.help(true);
        show.title(false);
        show.prepare(false);
        show.messages(false);
        session.closeChat();
        inactive(true);
    }

    function appendMessages(changes) {
        ko.utils.arrayPushAll(messages, changes);
        if (changes.length) {
            var target = document.getElementById("customer-marker");
            target.parentNode.scrollTop = target.parentNode.scrollHeight;
        }
    }
}

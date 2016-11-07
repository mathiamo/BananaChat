function ChatSession() {
    var state = {
        chatId: null,
        customerName: null,
        messagesRetrieved: 0,
        messages: [],
        listening: false,
        notifyNewMessages: nop,
        notifyChats: nop
    };
    this.startChat = function (customerName) {
        state.chatId = newId();
        state.customerName = customerName;
        state.listening = true;
        state.messagesRetrieved = 0;
    };
    this.sendCustomerMessage = function (text) {
        var message = {
            chatId: state.chatId,
            text: text,
            isSupport: false,
            isCustomer: true
        };

        state.messages.push(message);
        state.notifyNewMessages([message]);
    };
    this.closeChat = function () {
        state.listening = false;
        state.messages.length = 0;
    };
    this.whenMessagesArrive = function (fn) {
        state.notifyNewMessages = fn;
    };

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
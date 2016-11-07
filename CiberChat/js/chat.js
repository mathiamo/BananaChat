function ChatViewModel() {
    this.startChat = ko.observable();
    this.nameInput = ko.observable("");
}
$("#hidechat").click(function () {
    $(".right").slideToggle({
        direction: "down"
    }, 300);
});
$("#hideadmin").click(function () {
    $(".left").slideToggle({
        direction: "down"
    }, 300);
});
// Activates knockout.js
ko.applyBindings(new ChatViewModel());
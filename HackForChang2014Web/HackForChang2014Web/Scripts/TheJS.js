$(function () {
    // Declare a proxy to reference the hub. 
    var chat = $.connection.infoDataHub;
    // Create a function that the hub can call to broadcast messages.
    chat.client.sendTemperature = function (temp) {
        $('#lblTemperature').html(temp);

    };

    chat.client.sendLightLevel = function (light) {
        $('#lblLightLevel').html(light);
    };

    chat.client.sendCrowdAverage = function (average) {
        $('#lblAverage').html(average);
    };

    chat.client.sendCrowdCountTotal = function (total) {
        $('#lblTotal').html(total);
    };

    // Start the connection.
    $.connection.hub.start().done(function () {

    });
});
var lightIndex = 0;
var crowdAverage = 0;
$(function () {
    // Declare a proxy to reference the hub. 
    var chat = $.connection.infoDataHub;
    // Create a function that the hub can call to broadcast messages.
    chat.client.sendTemperature = function (temp) {
        $('#lblTemperature').html(temp + "&deg;");

    };

    chat.client.sendLightLevel = function (light) {
        lightIndex = parseInt(light);
        $('#lblLightLevel').html(light + "%");
        CalculateSafetyIndexTotal();
    };

    chat.client.sendCrowdAverage = function (average) {
        crowdAverage = parseInt(average);
        $('#lblAverage').html(average);
    };

    chat.client.sendCrowdCountTotal = function (total) {
        $('#lblTotal').html(total);
    };

    // Start the connection.
    $.connection.hub.start().done(function () {

    });
});


function CalculateSafetyIndexTotal() {
    var totalIndex = 0;
    var policeIndex = parseInt($("#hideInformation").data("policeindex"));
    var fireIndex = parseInt($("#hideInformation").data("fireindex"));
    if (lightIndex > 75) {
        totalIndex += 75;
    } else if (lightIndex > 30) {
        totalIndex += 50;
    }

    totalIndex += (policeIndex * 5);
    totalIndex += fireIndex;
    totalIndex += crowdAverage;

    var className = "label ";
    var messageClass = "";
    var indexMessage = "";
    if (totalIndex <= 25) {
        className += "label-danger";
        messageClass += "text-danger";
        indexMessage += "Proceed with extreme caution. Entry is not advisable";
    } else if (totalIndex > 25 && totalIndex <= 50) {
        className += "label-warning";
        messageClass += "text-warning";
        indexMessage += "Dangerous elements may be present. Travel in groups.";
    } else if (totalIndex > 50 && totalIndex <= 75) {
        className += "label-info";
        messageClass += "text-info";
        indexMessage += "Average safety level. Just use common sense and have fun.";
    } else {
        className += "label-success";
        messageClass += "text-success";
        indexMessage += "Maximum safety level. Enjoy your visit to this park";
    }

    $('#totalIndex').removeAttr("class");
    $('#totalIndex').addClass(className);

    $('#indexMessage').removeAttr("class");
    $('#indexMessage').addClass(messageClass);

    $('#totalIndex').html(totalIndex);
    $('#indexMessage').html(indexMessage);
}
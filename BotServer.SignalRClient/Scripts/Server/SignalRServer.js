var SignalRServer = (function () {
    
    var tryingToReconnect = false;
    var shouldAutoReconnect = true;
    var connection;
    var botServerHubProxy;

    // Public interfaces
    return {
        addHandler: function(message, handler) {
            botServerHubProxy.on(message, handler);
        },
        invoke: function() {
            botServerHubProxy.invoke.apply(botServerHubProxy, arguments);
        },
        connect: function() {
            connection = $.hubConnection();
            connection.url = 'http://localhost:8080/signalr';
            botServerHubProxy = connection.createHubProxy('BotServerHub');
        },
        start: function (onDone, onFail) {
            // Start the connection
            // connection.logging = true; // turns on client side logging
            connection.start()
                .done(function () {
                    console.log('Now connected, connection ID=' + connection.id);
                    if (onDone) { onDone(); }
                })
                .fail(function () {
                    console.log('Could not Connection!');
                    if (onFail) { onFail(); }
                });
        }
    };

})();
var signalR = (function () {
    var instance;

    return function Construct_signalR() {
        if (instance) {
            return instance;
        }
        if (this && this.constructor === Construct_signalR) {
            instance = this;
        } else {
            return new Construct_signalR();
        }

        var that = this;
        var chart = null;

        that.connectionId;

        var _init = function () {
            that.chart = $.connection.chartHub;
            $.connection.hub.start().done(function () {
                that.connectionId = $.connection.hub.id;
            });
        }

        that.startDone = function (callback) {
            $.connection.hub.start().done(callback);
        }

        that.getServer = function () {
            return that.chart.server;
        }

        that.getClient = function () {
            return that.chart.client;
        }

        _init();
    }
}());
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getLatency() {
    var started = new Date().getTime();
    var url = "/api/data?t=" + (+new Date());
    fetch(url)
        .then(function (response) {
            var ended = new Date().getTime();
            var milliseconds = ended - started;
            document.getElementById("latency").innerHTML = milliseconds + " ms";
        }).catch(function (error) {
            document.getElementById("latency").innerHTML = "? ms";
        });

}
var timerLatency = window.setInterval(getLatency, 1000);
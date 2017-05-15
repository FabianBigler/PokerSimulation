$(document).ready(function () {        
    setInterval(doPollSessions, 5000);
});

function doPollSessions() {
    $('.running-session').each(function () {      
        $.ajax({
            url: '/session/GetById',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { sessionId: $(this).attr('id')},
            success: function (data) {
                alert(data.State);
                $(this).find('progress-bar')
            },
            error: function (data) {
                //alert('error!');
            }
        });
    })    
}

function DeleteSession(sessionIdToDelete) {
    $("#" + sessionIdToDelete).remove();

    $.ajax({
        url: '/session/Delete',
        type: 'POST',
        data: { sessionId: sessionIdToDelete },
        success: function (data) {            
        }
    });
}

function PauseSession(sessionIdToPause) {
    alert('HELLO');
    $.ajax({
        url: '/session/Pause',
        type: 'POST',
        data: { sessionId: sessionIdToPause },
        success: function (data) {
            alert('paused!');
            doPollSessions();            
        }
    });
}
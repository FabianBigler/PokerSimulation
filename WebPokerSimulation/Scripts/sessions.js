$(document).ready(function () {        
    setInterval(doPollSessions, 1000);
});

function doPollSessions() {
    $('.running-session').each(function () {        
        $.ajax({
            url: '/session/GetById',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { sessionId: $(this).attr('id')},
            success: function (data) {
                var percentCompleted = (data.PlayedHandsCount * 100 / data.TotalHandsCount);              
                $("#" + data.Id).find('.progress-bar').css('width', percentCompleted + '%')
                $("#" + data.Id).find('.progress-bar').text(data.PlayedHandsCount + " / " + data.TotalHandsCount)
                var stateText = '';
                switch (data.State)
                {
                    case 0: stateText = 'Ready';
                        break;
                    case 1: stateText = 'Running';
                        break;
                    case 2: stateText = 'Paused';
                        break;
                    case 3: stateText = 'Completed';
                    default: stateText = 'Completed'   
                }                
                $("#" + data.Id).find('.state').text(stateText);
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
    $.ajax({
        url: '/session/Pause',
        type: 'POST',
        data: { sessionId: sessionIdToPause },
        success: function (data) {            
            doPollSessions();            
        }
    });
}

//$(document).ajaxStart(function () {

//    $(document.body).css({ 'cursor': 'wait' });
//}).ajaxStop(function () {
//    $(document.body).css({ 'cursor': 'default' });
//});
$(document).ready(function () {        
    setInterval(pollGameState, 1000);
});

function pollGameState() {
    $.ajax({
        url: '/game/GetGame',
        contentType: "application/json; charset=utf-8",
        dataType: "json",        
        success: function (data) {            
            $('#phase').empty();
            $('#phase').append(data.Phase);
            $('#potSize').empty();
            $('#potSize').append(data.PotSize);            

            for (var i = 0; i < data.Board.length; i++) {
                alert(data.Board[i]);
            }
        },
        error: function (data) {            
            //alert('error!');
        }
    });
}

function SetAction(actionType, amount) {    
    $.ajax({
        url: '/game/SetAction',
        type: 'POST',
        data: { actionType: actionType, amount: amount },
        success: function (data) {

        }
    });
}
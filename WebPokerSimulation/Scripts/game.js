$(document).ready(function () {
    setInterval(pollGameState, 2000);
});

function pollGameState() {
    $.ajax({
        url: '/game/GetGame',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            let gameHash = document.getElementById('gameHash').innerHTML;
            if (gameHash != data.HashCode) {                
                document.getElementById('gameHash').innerHTML = data.HashCode;
                
                $('#botName').empty();
                $('#botName').append('<h3> Bot: ' + data.Bot.Name + '</h3>');
                $('#phase').empty();
                $('#phase').append('<h3> Phase: ' + data.Phase + '</h3>');
                $('#potSize').empty();
                $('#potSize').append('<h3> Potsize: ' + data.PotSize + '</h3>');

                let playerHoleCards = '';
                playerHoleCards = playerHoleCards.concat('<div class="card-image"><img src="/Images/' + data.Human.HoleCard1 + '.png" /></div>')
                playerHoleCards = playerHoleCards.concat('<div class="card-image"><img src="/Images/' + data.Human.HoleCard2 + '.png" /></div>')
                $('#playerHoleCards').empty();
                $('#playerHoleCards').append(playerHoleCards);

                let botHoleCards = '';
                let botHoleCard1Icon = '';
                let botHoleCard2Icon = '';

                if (opponentCardsVisible) {
                    botHoleCard1Icon = '<div class="card-image"><img style="max-width: 10%;" src="/Images/' + data.Bot.HoleCard1 + '.png" /></div>';
                    botHoleCard2Icon = '<div class="card-image"><img style="max-width: 10%;" src="/Images/' + data.Bot.HoleCard2 + '.png" /></div>';
                } else {
                    botHoleCard1Icon = '<div class="card-image"><img style="max-width: 10%;" src="/Images/back.png" /></div>';
                    botHoleCard2Icon = '<div class="card-image"><img style="max-width: 10%;" src="/Images/back.png" /></div>';
                }
                botHoleCards = botHoleCards.concat(botHoleCard1Icon)
                botHoleCards = botHoleCards.concat(botHoleCard2Icon)
                $('#botHoleCards').empty();
                $('#botHoleCards').append(botHoleCards);

                let boardCardIcons = '';
                for (var i = 0; i < data.Board.length; i++) {
                    if (data.Board[i]) {
                        boardCardIcons = boardCardIcons.concat('<div class="card-image"><img src="/Images/' + data.Board[i] + '.png" /></div>')
                    }
                }

                $('#boardCards').empty();
                $('#boardCards').append(boardCardIcons);

                $('#actionBar').empty();
                let actions = '<div class="form-group">';                
                if (data.PendingAction) {
                    let isBet = false;
                    for (var i = 0; i < data.PendingAction.PossibleActions.length; i++) {
                        let action = data.PendingAction.PossibleActions[i];

                        let actionText = '';

                        switch(action)
                        {
                            case 0: //FOLD
                                actionText = 'Fold';
                                break;
                            case 1: // CHECK
                                actionText = 'Check';
                                break;
                            case 2: // CALL
                                actionText = actionText.concat('Call ' + data.PendingAction.AmountToCall);
                                break;
                            case 3: // RAISE
                                actionText = 'Raise';
                                break;
                            case 4: // BET
                                actionText = 'Bet';
                                isBet = true;
                                break;                               
                        }

                        actions = actions.concat('<button type="button" class="action btn btn-lg btn-default" onclick="SetAction(\'' + action + '\');">' + actionText + '</button>')
                    }

                    let minAmount = 0;
                    if (isBet) {
                        minAmount = data.PendingAction.MinAmount;
                    } else {
                        minAmount = data.PendingAction.MinAmountToRaise;
                    }

                    if (data.PendingAction.AmountToCall) {
                        actions = actions.concat('<div id="amountToCall" hidden>' + data.PendingAction.AmountToCall + '</div>');
                    }

                    let actionControl = '<input type="number" onchange="updateAmount(this.value)" id="amount" min="' + minAmount + '" max="' +
                                        data.PendingAction.MaxAmount + '" class="form-control autoselect text-right" value="' + minAmount + '">'
                    //onchange = "updateInput(this.value)"
                    actions = actions.concat(actionControl);
                }

                actions = actions.concat('</div>')

                $('#actionBar').append(actions);

                $('#statistics').empty();

                let statistics = '<h1> Amount won: '  + data.Statistics.AmountWon + '</h1>';
                statistics = statistics.concat('<h3>Played Hands / Total Hands: ' + data.Statistics.PlayedHandsCount + ' / ' +
                                                                        data.Statistics.TotalHandsCount + '</h3>');

                $('#statistics').append(statistics);

                //PlayedHandsCount = currentSession.PlayedHandsCount,
                //  TotalHandsCount = currentSession.TotalHandsCount,
                //  AmountWon = humanGameService.GetAmountWon()
            }
        },
        error: function (data) {
            //alert('error!');
        }
    });
}

function SetAction(actionType) {    
    let amount = $('#amount').val();
    let amountToCall = 0
    if(document.getElementById('amountToCall') != null)
    {
        amountToCall = document.getElementById('amountToCall').innerHTML;
    }
    
    let actionIndex = 0;
    if (actionType == 2) {
        amount = amountToCall;
    }

    $('#actionBar').empty();

    $.ajax({
        url: '/game/SetAction',
        type: 'POST',
        data: { actionType: actionType, amount: amount },
        success: function (data) {

        }
    });
}

var opponentCardsVisible = true;
function toggleOpponentCardVisibility() {
    opponentCardsVisible = !opponentCardsVisible;
    document.getElementById('gameHash').innerHTML = 0;
}

function updateAmount(value)
{    
    let max = document.getElementById("amount").max;  
    if (parseInt(value) > parseInt(max))
    {        
        document.getElementById("amount").value = max;
    }    
}
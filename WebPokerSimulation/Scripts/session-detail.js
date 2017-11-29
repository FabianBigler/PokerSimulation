$(document).ready(function () {
    if (document.getElementById('SessionID').innerHTML)
    {
        let sessionId = document.getElementById('SessionID').innerHTML;
        GetStatistics(sessionId);
    }        
});

function GetStatistics(sessionId)
{
    $.ajax({
        url: '/session/GetStatistics',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { sessionID: sessionId },
        success: function (data) {
            $('#stats').empty();

            let stats = '<h2>Winner: <b>' + data.Winner + ' </b></h2>';
            stats = stats.concat('<h2> Total amount won:' + data.TotalAmountWon + '</h2>');
            stats = stats.concat('<h2> Total big blinds won:' + data.TotalBigBlindsWon + '</h2>');
            stats = stats.concat('<h2> Played hands count:' + data.PlayedHandsCount + '</h2>');
            stats = stats.concat('<h2> Big blinds per 100 hands won:' + data.BigBlindsPer100HandsWon + '</h2>');

            for (let j = 0; j < data.StatisticsDetails.length; j++) {
                let statsDetail = data.StatisticsDetails[j];
                stats = stats.concat('<h2> Phase:' + statsDetail.PhaseName + '</h2>');
                stats = stats.concat('<h3> Winner:' + statsDetail.Winner + '</h3>');
                stats = stats.concat('<h3> Amount won:' + statsDetail.AmountWon + '</h3>');
                stats = stats.concat('<h3> Played hands count:' + statsDetail.PlayedHandsCount + '</h3>');
                stats = stats.concat('<h3> Big blinds per 100 hands won:' + statsDetail.BigBlindsPer100HandsWon + '</h3>');
                stats = stats.concat('<hr />');
            }

            $('#stats').append(stats);
        },
        error: function (data) {
            alert('error!');
        }
    });
}

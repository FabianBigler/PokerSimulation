# PokerSimulation
PokerSimulation lets bots compete against each other.

# Setup SQL:
1) Before you can use this project, you have to setup a SQL Server.
2) Run the script 'CreateDatabaseWithPlayers.sql' in the folder 
'PokerSimulation.Infrastructure/Scripts'.
3) change the connection string in the Web.config accordingly.

# Setup Training Data:
After Building your software:
If you wish to use the CFR and the Opponent Modelling Algorithm:
1) Copy the folders PokerSimulation\Setup\CFR and PokerSimulation\Setup\OpponentModelling to PokerSimulation\PokerSimulation.Test\bin\Debug and to PokerSimulation\WebPokerSimulation\bin
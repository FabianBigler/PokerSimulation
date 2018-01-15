# Prerequisites
- Microsoft Visual Studio 2015
- Microsoft SQL Server 2014 (other versions not tested)
- .NET Framework 4.5.2

# Setup SQL:
1) Before you can use this project, you have to setup a SQL Server.
2) Run the script 'CreateDatabaseWithPlayers.sql' in the folder 'PokerSimulation.Infrastructure/Scripts'.
3) Change the connection string in the Web.config accordingly.

# Setup Training Data:
If you wish to use the CFR and the Opponent Modelling algorithms:
1) Build the software solution in debug mode
2) Copy the folders PokerSimulation\Setup\CFR and PokerSimulation\Setup\OpponentModelling to PokerSimulation\PokerSimulation.Test\bin\Debug and to PokerSimulation\WebPokerSimulation\bin
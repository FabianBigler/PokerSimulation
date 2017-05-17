# PokerSimulation
PokerSimulation lets bots compete against each other.

Before you can use this project, you have to setup a SQL Server, run the script 'CreateDatabaseWithPlayers.sql' in the folder 'PokerSimulation.Infrastructure/Scripts'
and then change the connection string in the Web.config.

As the default there are three different bots available:
CallingStation: This Bot always calls or checks, no matter what.
AlwaysMinRaiseBot: This bot always min raises.
RandomBot: Picks each action randomly. If the aciton is bet or raise, the amount is also random.

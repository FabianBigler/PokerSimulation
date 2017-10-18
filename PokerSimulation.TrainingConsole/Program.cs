using PokerSimulation.Algorithms.TexasHoldem;
using PokerSimulation.Core;
using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Model;
using PokerSimulation.Core.Model.Bots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.TrainingConsole
{
    class Program
    {
        static void Main(string[] args)
        {       
            //this.player1 = factory.GetPlayer(entity.PlayerEntity1);
            //this.player2 = factory.GetPlayer(entity.PlayerEntity2);

            var entity = new PlayerEntity();
            entity.Type = PlayerType.CounterFactualRegret;
            var player1 = new CounterFactualRegretBot(entity);
            var player2 = new CounterFactualRegretBot(entity);
            var players = new List<Player>();
            players.Add(player1);
            players.Add(player2);
            var dealer = new RandomDealer();
            var game = new HeadsupGame(players, dealer);            

            //var trainer = new TexasHoldemTrainer();
            //trainer.Train(100000);

            for (int i = 0; i < 10000; i++)
            {
                game.PlayHand();
                //var shuffled = cards.OrderBy(x => rand.Next()).ToArray();
                //util += CalculateCounterFactualRegret(shuffled, new List<GameAction>(), 1, 1);
            }


            //gamestatechanged


        }
    }
}

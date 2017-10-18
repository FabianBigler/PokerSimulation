using PokerSimulation.Algorithms.TexasHoldem;
using PokerSimulation.Core;
using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Model;
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
            var trainer = new TexasHoldemTrainer();
            trainer.Train(100000);

            var entity = new PlayerEntity();
            var player = new Player(entity);
            var game = new HeadsupGame()

               //gamestatechanged


        }
    }
}

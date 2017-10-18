using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem
{
    public class TexasHoldemTrainer : ITrainer
    {
        public Dictionary<int, RegretGameNode<ActionBucket>> GameNodes { get; private set; }

        public void Train(int numberOfHands)
        {
            GameNodes = new Dictionary<int, RegretGameNode<ActionBucket>>();

            

            //int[] cards = { (int)CardValue.Jack, (int)CardValue.Queen, (int)CardValue.King };
            //float util = 0;
            //var rand = new Random();

            //for (int i = 0; i < numberOfhands; i++)
            //{
            //    var shuffled = cards.OrderBy(x => rand.Next()).ToArray();
            //    util += CalculateCounterFactualRegret(shuffled, new List<GameAction>(), 1, 1);
            //}

            throw new NotImplementedException();
        }


    }
}

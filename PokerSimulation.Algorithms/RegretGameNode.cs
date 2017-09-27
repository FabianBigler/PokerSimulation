using PokerSimulation.Algorithms.KuhnPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms
{
    public class RegretGameNode<T>
    {        
        public InformationSet<T> InfoSet { get; set; }

        public List<float> RegretSum { get; set; }
        private List<float> strategy { get; set; }
        private List<float> strategySum { get; set; }
        private List<float> averageStrategy { get; set; }

        public RegretGameNode(int numberOfAction)
        {
            RegretSum = new List<float>(numberOfAction);
            strategy = new List<float>(numberOfAction);
            strategySum = new List<float>(numberOfAction);
            averageStrategy = new List<float>(numberOfAction);

            //initialize lists with 0s
            for (int i = 0;i < numberOfAction; i++)
            {
                RegretSum.Add(0);
                strategy.Add(0);
                strategySum.Add(0);
                averageStrategy.Add(0);
            }
        }

        public List<float> calculateStrategy(float realizationWeight)
        {
            float normalizingSum = 0;
            for (int i = 0; i < strategy.Count; i++)
            {
                strategy[i] = RegretSum[i] > 0 ? RegretSum[i] : 0;
                normalizingSum += strategy[i];
            }

            for (int i = 0; i < strategy.Count; i++)
            {
                if (normalizingSum > 0)
                    strategy[i] = strategy[i] / normalizingSum;
                else
                    strategy[i] = 1.0f / strategy.Count;

                strategySum[i] += realizationWeight * strategy[i];
            }

            return strategy;
        }

        public List<float> calculateAverageStrategy()
        {                   
            float normalizingSum = strategySum.Sum();                        
            for (int i = 0; i < Settings.NumberOfActions; i++)
                if (normalizingSum > 0)
                    averageStrategy[i] = strategySum[i] / normalizingSum;
                else
                    averageStrategy[i] = 1.0f / Settings.NumberOfActions;

            return averageStrategy;
        }

        public override string ToString()
        {
            var averageStrategy = string.Join(";", calculateAverageStrategy());
            var infoSet = InfoSet.ToString();
            return string.Format("{0} {1}", InfoSet, averageStrategy);
        }
    }
}

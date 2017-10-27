using PokerSimulation.Algorithms;
using PokerSimulation.Algorithms.TexasHoldem;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Core.Bots;
using PokerSimulation.Game;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.TrainingConsole
{
    public class TrainingGroundProgram
    {
        static void Main(string[] args)
        {                                                    
            var trainer = new TexasHoldemTrainer();
            int numberOfTrainings = 1000;
            int handsPerTraining = 100;
            int handsCounter = 0;
            int numberOfTotalhands = numberOfTrainings * handsPerTraining;
            var trainedTree = loadSnapShot();
            if(trainedTree != null)
            {
                trainer.GameNodes = trainedTree;
            }

            var stopwatch = new Stopwatch();            
            for (int i = 0; i < numberOfTrainings; i++)
            {
                stopwatch.Restart();
                trainer.Train(handsPerTraining);
                double duration = stopwatch.Elapsed.TotalSeconds;
                handsCounter += handsPerTraining;
                var msg = string.Format("{0}/{1} hands trained in {2}s. ", handsCounter, numberOfTotalhands, duration);
                takeSnapShot(trainer.GameNodes, handsCounter);
                log(msg);                       
            }                                                            
        }

        private static void takeSnapShot(Dictionary<long, RegretGameNode<ActionBucket>> trainedTree, int handsPlayed)
        {
            try
            {
                var fileName = string.Format("cfr-tree_{0}.proto", handsPlayed);
                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    Serializer.Serialize(stream, trainedTree);                    
                }                                
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
        }

        private static Dictionary<long, RegretGameNode<ActionBucket>> loadSnapShot()
        {
            try
            {
                if (!File.Exists("cfr-tree.proto")) return null;

                var fs = new FileStream("cfr-tree.proto", FileMode.Open);                
                return Serializer.Deserialize<Dictionary<long, RegretGameNode<ActionBucket>>>(fs);            
            }
            catch (Exception ex)
            {
                log(ex.ToString());
                return null;
            }
        }

        private static void log(string msg)
        {
            File.AppendAllText("log.txt", string.Format("{0}: {1}", DateTime.Now.ToString("yyyyMMddHHmmss"), msg));
            Console.WriteLine(msg);
        }
    }
}
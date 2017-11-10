using PokerSimulation.Algorithms;
using PokerSimulation.Algorithms.TexasHoldem;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Algorithms.TexasHoldem.CounterFactualRegret;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.TrainingConsole
{
    public class TrainingGroundProgram
    {
        static void Main(string[] args)
        {            
            if(args.Length > 0)
            {
                switch(args[0])
                {
                    case "merge":
                        merge();
                        break;
                    case "train":
                        train();
                        break;
                }

            } else
            {
                train();
            }                                                      
        }

        private static void train()
        {
            var dealer = new RandomDealer();
            var trainer = new TexasHoldemTrainer(dealer);
            int numberOfTrainings = 10000;
            int handsPerTraining = 100;
            int handsCounter = 0;
            int numberOfTotalhands = numberOfTrainings * handsPerTraining;
            var trainedTree = loadSnapShot();
            if (trainedTree != null)
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
                takeSnapShot(trainer.GameNodes, "cfr-tree.proto");                                        
                log(msg);
            }
        }

        private static void merge()
        {
            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var treesToMerge = new List<Dictionary<long, RegretGameNode<ActionBucket>>>();

            foreach (var filePath in Directory.GetFiles(assemblyDirectory))
            {
                if (filePath.EndsWith(".proto"))
                {
                    var fs = new FileStream(filePath, FileMode.Open);
                    var treeTomerge = Serializer.Deserialize<Dictionary<long, RegretGameNode<ActionBucket>>>(fs);
                    treesToMerge.Add(treeTomerge);
                }              
            }

            var mergedTree = new Dictionary<long, RegretGameNode<ActionBucket>>();
            if (treesToMerge.Count > 0)
            {
                foreach(var tree in treesToMerge)
                {
                    foreach(var node in tree)
                    {
                        RegretGameNode<ActionBucket> gameNode;
                        if (mergedTree.TryGetValue(node.Key, out gameNode))
                        {
                            node.Value.Merge(gameNode);     
                        } else
                        {
                            mergedTree.Add(node.Key, node.Value);
                        }                        
                    }
                    //var infoSet = new InformationSet<ActionBucket>()
                    //{
                    //    CardBucket = (int)StartHandBucket.VeryGood,
                    //    ActionHistory = new List<ActionBucket>()
                    //};

                    //var gameNode = trainer.GameNodes[infoSet.GetHashCode()];
                }

                takeSnapShot(mergedTree, "cfr-merged.proto");                               
            }            

        }

        private static void takeSnapShot(Dictionary<long, RegretGameNode<ActionBucket>> trainedTree, string filename)
        {
            try
            {                
                using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
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
            File.AppendAllText("log.txt", string.Format("{0}: {1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), msg, Environment.NewLine));
            Console.WriteLine(msg);
        }
    }
}
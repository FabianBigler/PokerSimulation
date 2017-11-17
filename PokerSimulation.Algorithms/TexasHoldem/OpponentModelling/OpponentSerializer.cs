using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PokerSimulation.Algorithms.TexasHoldem.OpponentModelling
{
    public class OpponentSerializer
    {
        private string filePath;

        public OpponentSerializer()
        {
            var appDomain = System.AppDomain.CurrentDomain;
            var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
            filePath = Path.Combine(basePath, "OpponentModelling", "opponent.xml");
        }

        public void Serialize(Opponent opponent)
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            {          
                var serializer = new XmlSerializer(typeof(Opponent));
                serializer.Serialize(fs, opponent);
            }
        }

        public Opponent Deserialize()
        {
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(Opponent));
                return (Opponent) serializer.Deserialize(fs);
            }
        }
    }
}

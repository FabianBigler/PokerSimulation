using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms
{
    public class InformationSet<T>
    {
        public int CardBucket { get; set; } 
           
        public List<T> Actions { get; set; } 

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var action in Actions)
                {
                    hash = hash * 23 + action.GetHashCode();
                }
                hash = hash * 23 + CardBucket.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {            
            return string.Format("{0}:{1}", CardBucket, string.Join(";", Actions));
        }
    }
}

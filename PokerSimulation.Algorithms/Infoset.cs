using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms
{
    [ProtoContract]
    public class InformationSet<T>
    {
        [ProtoMember(1)]
        public int CardBucket { get; set; }

        [ProtoMember(2)]
        public List<T> ActionHistory { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var action in ActionHistory)
                {
                    hash = hash * 23 + action.GetHashCode();
                }
                hash = hash * 23 + CardBucket.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Gets a hash code in type long to further avoid collisions
        /// </summary>
        /// <returns></returns>
        public long GetLongHashCode()
        {
            unchecked
            {
                // prime numbers are chosen based on example of stackoverflow.
                // source: https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
                long hash = 17;
                foreach (var action in ActionHistory)
                {
                    hash = hash * 23 + action.GetHashCode();
                }
                hash = hash * 23 + CardBucket.GetHashCode();
                return hash;
            }
        }


        public override string ToString()
        {            
            return string.Format("{0}:{1}", CardBucket, string.Join(";", ActionHistory));
        }
    }
}

using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    public class StartHandAbstracter
    {
        //lookup table based on https://www.pokernews.com/strategy/artificial-intelligence-hold-em-part-2-23188.htm
        private static byte[,] lookup = { { 4,1,1,1,1,1,2,2,2,4,4,4,6 },
                                           { 1,6,1,1,1,1,2,2,3,4,4,6,6 },
                                            { 1,1,6,1,1,2,2,2,3,4,4,6,6 },
                                            { 1,1,1,6,3,3,3,3,3,4,4,6,6 },
                                            { 1,1,1,1,7,3,3,3,3,4,5,6,6 },
                                           { 1,1,1,2,3,7,3,3,4,4,4,6,7 },
                                            { 1,1,2,2,3,3,8,3,5,5,5,6,7 },
                                            { 2,2,2,2,3,3,3,8,5,5,5,7,7 },
                                           { 2,2,2,2,3,3,3,5,8,5,7,7,7 },
                                           { 2,2,4,4,4,4,5,5,5,8,7,7,7 },
                                           { 4,4,4,4,4,4,5,5,5,5,8,7,7 },
                                           { 4,4,4,6,6,6,6,6,7,7,7,8,7 },
                                           { 6,6,6,6,6,6,6,7,7,7,7,7,8 } };

        public static StartHandBucket MapToBucket(Card card1, Card card2)
        {
            byte index1 = (byte)card1.Value;
            byte index2 = (byte)card2.Value;
            
            if (card1.Suit == card2.Suit)
            {
                if(index1 > index2)
                {
                    return (StartHandBucket) lookup[index2, index1];
                } else
                {
                    return (StartHandBucket) lookup[index1, index2];
                }                                        
            } else
            {
                if (index1 > index2)
                {
                    return (StartHandBucket) lookup[index1, index2];
                }
                else
                {
                    return (StartHandBucket) lookup[index2, index1];
                }                
            }            
        }        
    }
}

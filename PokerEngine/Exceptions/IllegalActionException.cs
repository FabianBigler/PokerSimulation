using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Exceptions
{
    public class IllegalActionException : Exception
    {
        public IllegalActionException(string message) : base(message)
        {
        }
    }
}

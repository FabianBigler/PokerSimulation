﻿using PokerEngine.Core;
using PokerEngine.Enumerations;
using PokerEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Interfaces
{
    public interface ICanPlay
    {
        GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall);
    }
}

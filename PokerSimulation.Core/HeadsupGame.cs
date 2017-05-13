using System;
using System.Collections.Generic;
using System.Linq;
using PokerSimulation.Core.Model;
using PokerSimulation.Core.Interfaces;
using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Exceptions;
using PokerSimulation.Core.Helpers;
using PokerSimulation.Core.Extensions;

namespace PokerSimulation.Core
{
    public class HeadsupGame
    {
        public const int BigBlindSize = 2;
        public const int SmallBlindSize = 1;
        public const int StackSize = 200;
        public const int MinAmountToBetFactor = 2;

        private ICanDeal dealer;
        private List<Player> players;
        private int oldIndexSmallBlind;

        public List<Card> Board { get; private set; }
        public int PotSize { get; private set; }        
        
        public List<Player> Players
        {
            get { return players; }
        }

        public HeadsupGame(List<Player> players, ICanDeal dealer)
        {
            this.dealer = dealer;
            this.players = players;

            Board = new List<Card>(5);
            Random random = new Random();
            oldIndexSmallBlind = random.Next(players.Count - 1);                
        }

        private void initializeValues()
        {
            Board.Clear();
            //reset chip stack after each round
            foreach(var player in players)
            {
                player.ChipStack = StackSize;
                player.HoleCards = new List<Card>(2);
            }
            PotSize = 0;                  
        }                 

        private void moveBlinds()
        {
            var indexSmallBlind = (oldIndexSmallBlind + 1) % players.Count;
            var indexBigBlind = (indexSmallBlind + 1) % players.Count;
            this.players[indexSmallBlind].IsSmallBlind = true;
            this.players[indexBigBlind].IsBigBlind = true;
        }

        public PlayedHandEntity PlayHand()
        {
            initializeValues();

            var result = new PlayedHandEntity();

            moveBlinds();
            foreach (var player in players)
            {
                PotSize += player.GetBlind();
            }

            dealer.DealHoleCards(players);
            result.HoleCards1 = players[0].HoleCards.ToAbbreviations();
            result.HoleCards2 = players[1].HoleCards.ToAbbreviations();

            bool roundFinished = false;
            bool goToShowdown = false;
                        
            var smallBlindPlayer = players.First(x => x.IsSmallBlind);
            var bigBlindPlayer = players.First(x => x.IsBigBlind);            
            var possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call, ActionType.Raise };

                      
            var firstAmountToCall = SmallBlindSize;
            //in headsup: the small blind acts first before the Flop  
            playBettingRound(smallBlindPlayer, bigBlindPlayer, possibleActions, firstAmountToCall, result, out roundFinished, out goToShowdown);
            if (roundFinished)
            {                
                return result;
            }

            //flop
            possibleActions = new List<ActionType> { ActionType.Check, ActionType.Bet };
            firstAmountToCall = 0;
            dealer.DealFlop(Board);
            result.Board = this.Board.ToAbbreviations();

            if (!goToShowdown)
            {
                playBettingRound(bigBlindPlayer, smallBlindPlayer, possibleActions, firstAmountToCall, result, out roundFinished, out goToShowdown);
                if (roundFinished) return result;         
            }

            //turn
            possibleActions = new List<ActionType> { ActionType.Check, ActionType.Bet };
            firstAmountToCall = 0;
            dealer.DealTurn(Board);
            result.Board = this.Board.ToAbbreviations();

            if (!goToShowdown)
            {
                playBettingRound(smallBlindPlayer, bigBlindPlayer, possibleActions, firstAmountToCall, result, out roundFinished, out goToShowdown);
                if (roundFinished) return result;
            }

            //river
            possibleActions = new List<ActionType> { ActionType.Check, ActionType.Bet };
            firstAmountToCall = 0;
            dealer.DealRiver(Board);
            result.Board = this.Board.ToAbbreviations();

            if (!goToShowdown)
            {
                playBettingRound(smallBlindPlayer, bigBlindPlayer, possibleActions, firstAmountToCall, result,   out roundFinished, out goToShowdown);
                if (roundFinished) return result;
            }

            showDown(smallBlindPlayer, bigBlindPlayer, result);          

            return result;
        }

        private void showDown(Player smallBlindPlayer, Player bigBlindPlayer, PlayedHandEntity result)
        {
            //showdown
            var smallBlindEvaluator = new HandEvaluator(smallBlindPlayer.HoleCards.ToList(), Board);
            var smallBlindRank = smallBlindEvaluator.GetHandRank();
            var bigBlindEvaluator = new HandEvaluator(bigBlindPlayer.HoleCards.ToList(), Board);
            var bigBlindRank = bigBlindEvaluator.GetHandRank();
            if (smallBlindRank > bigBlindRank)
            {
                result.WinnerId = smallBlindPlayer.Id;                
            }
            if (bigBlindRank > smallBlindRank)
            {
                result.WinnerId = bigBlindPlayer.Id;                
            }
                        
            if(bigBlindRank == smallBlindRank)
            {
                var smallBlindCards = smallBlindEvaluator.GetBestFiveCards(smallBlindRank);
                var bigBlindCards = bigBlindEvaluator.GetBestFiveCards(bigBlindRank);

                for(int i = 0; i < 5; i++)
                {
                    if(smallBlindCards[i].Value > bigBlindCards[i].Value)
                    {
                        result.WinnerId = smallBlindPlayer.Id;
                        break;
                    } else if (bigBlindCards[i].Value > smallBlindCards[i].Value)
                    {
                        result.WinnerId = bigBlindPlayer.Id;
                        break;
                    }
                }
                //if no winner is determined = split pot!                                                                                                                                                 
           }

            result.AmountWon = PotSize / 2;
            result.PotSize = PotSize;
        }


        private void playBettingRound(Player playerFirstToAct, Player playerSecondToAct, List<ActionType> possibleActions, int amountToCall, PlayedHandEntity result, out bool roundFinished, out bool goToShowdown)
        {
            roundFinished = false;
            goToShowdown = false;

            var bothPlayersActed = false;
            var bettingRoundFinished = false;
            var playerToAct = playerFirstToAct;
            var minRaiseAmount = MinAmountToBetFactor * BigBlindSize;            
            
            while (!bettingRoundFinished)
            {                
                var playerAction = playerToAct.GetAction(possibleActions, this, amountToCall);
                playerAction.Timestamp = DateTime.Now;                                
                result.Actions.Add(playerAction);

                if (possibleActions.Contains(playerAction.ActionType))
                {        
                    switch (playerAction.ActionType)
                    {
                        case ActionType.Call:
                            if(bothPlayersActed)
                            {
                                bettingRoundFinished = true;
                            } else
                            {
                                possibleActions = new List<ActionType> { ActionType.Check, ActionType.Raise };
                            }
                            playerToAct.ChipStack -= playerAction.Amount;
                            PotSize += playerAction.Amount;
                            playerToAct.AmountAlreadyInPotThisRound = 0;
                            if (playerToAct.ChipStack == 0)
                            {
                                goToShowdown = true;
                            }
                            break;
                        case ActionType.Check:
                            if(bothPlayersActed)
                                bettingRoundFinished = true;
                            playerToAct.AmountAlreadyInPotThisRound = 0;
                            break;
                        case ActionType.Fold:
                            bettingRoundFinished = true;
                            roundFinished = true;
                            playerToAct.AmountAlreadyInPotThisRound = 0;                                                                                

                            //the other player wins this hand
                            if (playerToAct == playerFirstToAct)
                            {
                                result.WinnerId = playerSecondToAct.Id;                                
                            }
                            else
                            {
                                result.WinnerId = playerFirstToAct.Id;
                            }

                            if((PotSize - amountToCall) < 2 * BigBlindSize)
                            {         
                                //special cases: big blind or small blind won                                              
                                if(playerToAct.IsSmallBlind)
                                {
                                    result.AmountWon = SmallBlindSize;
                                } else
                                {
                                    result.AmountWon = BigBlindSize;
                                }                                                             
                            } else
                            {
                                result.AmountWon = (PotSize - amountToCall) / 2;
                            }
                            result.PotSize = PotSize;                                                    
                            break;
                        case ActionType.Bet:
                            if (playerAction.Amount < BigBlindSize && playerToAct.ChipStack >= BigBlindSize)
                            {                             
                                var exceptionmessage = String.Format("Player {0} invoked illegal action. Bet amount: {1} Min amount to bet: {2}", playerToAct.Name, playerAction.Amount, BigBlindSize);
                                throw new IllegalActionException(exceptionmessage);
                            }

                            minRaiseAmount = playerAction.Amount * 2;  
                            playerToAct.ChipStack -= playerAction.Amount;
                            amountToCall = playerAction.Amount;                            
                            playerToAct.AmountAlreadyInPotThisRound = playerAction.Amount;                            
                            PotSize += playerAction.Amount;
                            possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call, ActionType.Raise };
                            break;
                        case ActionType.Raise:
                            //e.g. SB raises to 6 preflop. Amount is 5 in this case. MoneyAlreadyInPot is 1      
                            var amountToCheck = (playerAction.Amount + playerToAct.AmountAlreadyInPotThisRound);
                            if (amountToCheck < minRaiseAmount && playerToAct.ChipStack > amountToCheck)
                            {
                                var exceptionmessage = String.Format("Player {0} invoked illegal action. Bet amount: {1} Min amount to bet: {2}", playerToAct.Name, playerAction.Amount, minRaiseAmount);
                                throw new IllegalActionException(exceptionmessage);
                            }                            
                            //minraise is not enough for second raise
                            playerToAct.ChipStack -= playerAction.Amount;                                
                            PotSize += playerAction.Amount;      
                                                                                                                                                                  
                            amountToCall = playerAction.Amount - amountToCall;           
                            
                            if(playerToAct.ChipStack > 0)
                            {
                                possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call, ActionType.Raise };
                            } else
                            {
                                possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call};
                            }                                                                                
                            break;
                    }
                }
                else
                {
                    var exceptionMessage = String.Format("Player {0} invoked impossible action {1}", playerToAct.Name, playerAction.ActionType);
                    throw new IllegalActionException(exceptionMessage);
                }

                if(!bettingRoundFinished)
                {                                         
                    //switch playerToAct
                    playerToAct = players.First(x => x != playerToAct);                                      
                    bothPlayersActed = true;
                }               
            }
        }        
    }
}

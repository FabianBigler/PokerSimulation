using System;
using System.Collections.Generic;
using System.Linq;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using PokerSimulation.Game.Exceptions;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Interfaces;
using PokerSimulation.Game.Helpers;
using PokerSimulation.Game.Extensions;

namespace PokerSimulation.Game
{
    public class HeadsupGame
    {        
        public delegate void ChangedPhaseEventHandler(List<Card> board, GamePhase phase);
        public event ChangedPhaseEventHandler ChangedPhase;

        public const int BigBlindSize = 2;
        public const int SmallBlindSize = 1;
        public const int StackSize = 200;
        public const int MinAmountToBetFactor = 2;

        private ICanDeal dealer;
        private List<Player> players;
        private int oldIndexSmallBlind;

        public List<Card> Board { get; private set; }
        public int PotSize { get; private set; }     
        public GamePhase Phase { get; private set; }

        public List<Player> Players
        {
            get { return players; }
        }

        public HeadsupGame(List<Player> players, ICanDeal dealer)
        {            
            this.dealer = dealer;
            this.players = players;
            foreach(var player in players)
            {
                player.AssignCurrentGame(this);
            }

            Board = new List<Card>(5);
            Random random = new Random();
            oldIndexSmallBlind = random.Next(players.Count);                
        }

        private void initializeValues()
        {
            Board.Clear();
            //reset chip stack after each round
            foreach(var player in players)
            {
                player.ChipStack = StackSize;
                player.AmountAlreadyInPotThisRound = 0;                
                player.HoleCards = new List<Card>(2);
                player.IsSmallBlind = false;
                player.IsBigBlind = false;
            }
            PotSize = 0;                  
        }                 

        private void moveBlinds()
        {
            var indexSmallBlind = (oldIndexSmallBlind + 1) % players.Count;
            var indexBigBlind = (indexSmallBlind + 1) % players.Count;
            this.players[indexSmallBlind].IsSmallBlind = true;
            this.players[indexBigBlind].IsBigBlind = true;
            oldIndexSmallBlind++;
        }

        public void StartNewRound(PlayedHandEntity result)
        {
            initializeValues();            
            moveBlinds();
            foreach (var player in players)
            {
                PotSize += player.GetBlind();
            }
            dealer.DealHoleCards(players);
            result.HoleCards1 = players[0].HoleCards.ToAbbreviations();
            result.HoleCards2 = players[1].HoleCards.ToAbbreviations();
        }

        public void StartNextPhase(GamePhase phase, out List<ActionType> possibleActions, out int firstAmountToCall)
        {
            possibleActions = new List<ActionType> { ActionType.Check, ActionType.Bet };
            firstAmountToCall = 0;
            switch(phase)
            {
                case GamePhase.Flop:
                    dealer.DealFlop(Board);
                    break;
                case GamePhase.Turn:
                    dealer.DealTurn(Board);
                    break;
                case GamePhase.River:
                    dealer.DealRiver(Board);
                    break;
            }
                     
            onChangedPhase(this.Board, phase);
        }

        public PlayedHandEntity PlayHand()
        {
            var playedHand = new PlayedHandEntity();
            bool roundFinished = false;
            bool goToShowdown = false;       
            StartNewRound(playedHand);

            var smallBlindPlayer = players.First(x => x.IsSmallBlind);
            var bigBlindPlayer = players.First(x => x.IsBigBlind);

            var possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call, ActionType.Raise };                      
            var firstAmountToCall = SmallBlindSize;
            //in headsup: the small blind acts first before the Flop  
            playBettingRound(smallBlindPlayer, bigBlindPlayer, possibleActions, firstAmountToCall, playedHand, out roundFinished, out goToShowdown);
            if (roundFinished)
            {                
                return playedHand;
            }

            //flop
            //possibleActions = new List<ActionType> { ActionType.Check, ActionType.Bet };
            //firstAmountToCall = 0;
            //dealer.DealFlop(Board);
            //
            //onChangedPhase(this.Board, GamePhase.Flop);
            StartNextPhase(GamePhase.Flop, out possibleActions, out firstAmountToCall);
            if (!goToShowdown)
            {                
                playBettingRound(bigBlindPlayer, smallBlindPlayer, possibleActions, firstAmountToCall, playedHand, out roundFinished, out goToShowdown);
                if (roundFinished)
                {
                    playedHand.Board = this.Board.ToAbbreviations();
                    return playedHand;
                }
            }

            //turn
            StartNextPhase(GamePhase.Turn, out possibleActions, out firstAmountToCall);
            if (!goToShowdown)
            {                
                playBettingRound(smallBlindPlayer, bigBlindPlayer, possibleActions, firstAmountToCall, playedHand, out roundFinished, out goToShowdown);
                if (roundFinished)
                {
                    playedHand.Board = this.Board.ToAbbreviations();
                    return playedHand;
                }
            }

            //river
            StartNextPhase(GamePhase.River, out possibleActions, out firstAmountToCall);            
            if (!goToShowdown)
            {                
                playBettingRound(smallBlindPlayer, bigBlindPlayer, possibleActions, firstAmountToCall, playedHand,   out roundFinished, out goToShowdown);
                if (roundFinished)
                {
                    playedHand.Board = this.Board.ToAbbreviations();
                    return playedHand;
                }
            }

            showDown(smallBlindPlayer, bigBlindPlayer, playedHand);
            playedHand.Board = this.Board.ToAbbreviations();
            return playedHand;
        }
        

        private void showDown(Player smallBlindPlayer, Player bigBlindPlayer, PlayedHandEntity result)
        {
            HandComparison comparison = HandComparer.Compare(smallBlindPlayer.HoleCards, bigBlindPlayer.HoleCards, Board);
            switch (comparison)
            {
                case HandComparison.None:     
                    //no winner is determined = split pot                               
                    break;
                case HandComparison.Player1Won:
                    result.WinnerId = smallBlindPlayer.Id;
                    break;
                case HandComparison.Player2Won:
                    result.WinnerId = bigBlindPlayer.Id;
                    break;
            }
                      
            result.AmountWon = PotSize / 2;
            result.PotSize = PotSize;
        }


        private void playBettingRound(Player playerFirstToAct, Player playerSecondToAct, List<ActionType> possibleActions, int amountToCall, PlayedHandEntity result, out bool roundFinished, out bool goToShowdown)
        {
            roundFinished = false;
            goToShowdown = false;

            bool bothPlayersActed = false;
            bool bettingRoundFinished = false;
            var playerToAct = playerFirstToAct;
            var minRaiseAmount = MinAmountToBetFactor * BigBlindSize;            
            
            while (!bettingRoundFinished)
            {                
                var playerAction = playerToAct.GetAction(possibleActions, this, amountToCall);
                playerAction.Timestamp = DateTime.Now;                                
                result.Actions.Add(playerAction);

                if (possibleActions.Contains(playerAction.ActionType))
                {        
                    //OnPlayerActed(playerToAct, playerAction, bothPlayersActed, out bettingRoundFinished)
                    switch (playerAction.ActionType)
                    {
                        case ActionType.Call:
                            if (bothPlayersActed)
                            {
                                bettingRoundFinished = true;
                            }
                            else
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
                            if (bothPlayersActed)
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

                            if ((PotSize - amountToCall) < 2 * BigBlindSize)
                            {
                                //special cases: big blind or small blind won                                              
                                if (playerToAct.IsSmallBlind)
                                {
                                    result.AmountWon = SmallBlindSize;
                                }
                                else
                                {
                                    result.AmountWon = BigBlindSize;
                                }
                            }
                            else
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

                            if (playerToAct.ChipStack > 0)
                            {
                                possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call, ActionType.Raise };
                            }
                            else
                            {
                                possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call };
                            }
                            break;
                    }
                }
                else
                {
                    var exceptionMessage = String.Format("Player {0} invoked illegal action {1}", playerToAct.Name, playerAction.ActionType);
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

        //private void OnPlayerActed(Player playerToAct, GameActionEntity playerAction, bool bothPlayersActed, out bool bettingRoundFinished, out bool roundFinished, out bool goToShowdown, out List<ActionType> possibleActions)
        //{
        //    bettingRoundFinished = false;
        //    roundFinished = false;
        //    goToShowdown = false;

        //    switch (playerAction.ActionType)
        //    {
        //        case ActionType.Call:
        //            if (bothPlayersActed)
        //            {
        //                bettingRoundFinished = true;
        //            }
        //            else
        //            {
        //                possibleActions = new List<ActionType> { ActionType.Check, ActionType.Raise };
        //            }
        //            playerToAct.ChipStack -= playerAction.Amount;
        //            PotSize += playerAction.Amount;
        //            playerToAct.AmountAlreadyInPotThisRound = 0;
        //            if (playerToAct.ChipStack == 0)
        //            {
        //                goToShowdown = true;
        //            }
        //            break;
        //        case ActionType.Check:
        //            if (bothPlayersActed)
        //                bettingRoundFinished = true;
        //            playerToAct.AmountAlreadyInPotThisRound = 0;
        //            break;
        //        case ActionType.Fold:
        //            bettingRoundFinished = true;
        //            roundFinished = true;
        //            playerToAct.AmountAlreadyInPotThisRound = 0;

        //            //the other player wins this hand
        //            if (playerToAct == playerFirstToAct)
        //            {
        //                result.WinnerId = playerSecondToAct.Id;
        //            }
        //            else
        //            {
        //                result.WinnerId = playerFirstToAct.Id;
        //            }

        //            if ((PotSize - amountToCall) < 2 * BigBlindSize)
        //            {
        //                //special cases: big blind or small blind won                                              
        //                if (playerToAct.IsSmallBlind)
        //                {
        //                    result.AmountWon = SmallBlindSize;
        //                }
        //                else
        //                {
        //                    result.AmountWon = BigBlindSize;
        //                }
        //            }
        //            else
        //            {
        //                result.AmountWon = (PotSize - amountToCall) / 2;
        //            }
        //            result.PotSize = PotSize;
        //            break;
        //        case ActionType.Bet:
        //            if (playerAction.Amount < BigBlindSize && playerToAct.ChipStack >= BigBlindSize)
        //            {
        //                var exceptionmessage = String.Format("Player {0} invoked illegal action. Bet amount: {1} Min amount to bet: {2}", playerToAct.Name, playerAction.Amount, BigBlindSize);
        //                throw new IllegalActionException(exceptionmessage);
        //            }

        //            minRaiseAmount = playerAction.Amount * 2;
        //            playerToAct.ChipStack -= playerAction.Amount;
        //            amountToCall = playerAction.Amount;
        //            playerToAct.AmountAlreadyInPotThisRound = playerAction.Amount;
        //            PotSize += playerAction.Amount;
        //            possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call, ActionType.Raise };
        //            break;
        //        case ActionType.Raise:
        //            //e.g. SB raises to 6 preflop. Amount is 5 in this case. MoneyAlreadyInPot is 1      
        //            var amountToCheck = (playerAction.Amount + playerToAct.AmountAlreadyInPotThisRound);
        //            if (amountToCheck < minRaiseAmount && playerToAct.ChipStack > amountToCheck)
        //            {
        //                var exceptionmessage = String.Format("Player {0} invoked illegal action. Bet amount: {1} Min amount to bet: {2}", playerToAct.Name, playerAction.Amount, minRaiseAmount);
        //                throw new IllegalActionException(exceptionmessage);
        //            }
        //            //minraise is not enough for second raise
        //            playerToAct.ChipStack -= playerAction.Amount;
        //            PotSize += playerAction.Amount;

        //            amountToCall = playerAction.Amount - amountToCall;

        //            if (playerToAct.ChipStack > 0)
        //            {
        //                possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call, ActionType.Raise };
        //            }
        //            else
        //            {
        //                possibleActions = new List<ActionType> { ActionType.Fold, ActionType.Call };
        //            }
        //            break;
        //    }
        //}

        private void onChangedPhase(List<Card> board, GamePhase phase)
        {
            Phase = phase;
            if(ChangedPhase != null)
            {
                ChangedPhase(board, phase);
            }            
        }
    }
}

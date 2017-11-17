using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerSimulation.Algorithms.TexasHoldem.OpponentModelling;
using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Test.Algorithms.OpponentModelling
{
    [TestClass]
    public class OpponentSerializerTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var opponent = new Opponent();
            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.Vpip,
                Phase = GamePhase.PreFlop,                                                                                                       
                ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet, FeatureAction.Call },
                IsGlobal = true
            });

            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.Pfr,
                Phase = GamePhase.PreFlop,                              
                ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet},
                IsGlobal = true
            });

            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.ThreeBet,
                Phase = GamePhase.PreFlop,
                BetMinThreshold = 0.4,
                BetMaxThreshold = 1,
                PassMinThreshold = 0,
                PassMaxThreshold = 0.2,
                ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet },
                PreCondition = new Precondition()
                {
                    LastActions = new List<FeatureAction>() { FeatureAction.Bet },                                        
                }
            });

            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.ThreeBetCall,
                Phase = GamePhase.PreFlop,
                BetMinThreshold = 0,
                BetMaxThreshold = 0.4,
                PassMinThreshold = 0.6,
                PassMaxThreshold = 1,
                ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Call },
                PreCondition = new Precondition()
                {
                    LastActions = new List<FeatureAction>() { FeatureAction.Bet, FeatureAction.Bet },
                }
            });

            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.ThreeBetFold,
                Phase = GamePhase.PreFlop,
                BetMinThreshold = 0.6,
                BetMaxThreshold = 1,
                PassMinThreshold = 0,
                PassMaxThreshold = 0.3,
                ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Pass },
                PreCondition = new Precondition()
                {
                    LastActions = new List<FeatureAction>() { FeatureAction.Bet, FeatureAction.Bet },
                }
            });

            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.FourBet,
                Phase = GamePhase.PreFlop,
                BetMinThreshold = 0.6,
                BetMaxThreshold = 1,
                PassMinThreshold = 0,
                PassMaxThreshold = 0.2,
                ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet },
                PreCondition = new Precondition()
                {
                    LastActions = new List<FeatureAction>() { FeatureAction.Bet, FeatureAction.Bet },
                }
            });

            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.FourBetCall,
                Phase = GamePhase.PreFlop,
                BetMinThreshold = 0,
                BetMaxThreshold = 0.2,
                PassMinThreshold = 0.8,
                PassMaxThreshold = 1,
                ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Call },
                PreCondition = new Precondition()
                {
                    LastActions = new List<FeatureAction>() { FeatureAction.Bet, FeatureAction.Bet, FeatureAction.Bet },
                }
            });

            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.FourBetFold,
                Phase = GamePhase.PreFlop,
                BetMinThreshold = 0.8,
                BetMaxThreshold = 1,
                PassMinThreshold = 0,
                PassMaxThreshold = 0.2,
                ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Pass },
                PreCondition = new Precondition()
                {
                    LastActions = new List<FeatureAction>() { FeatureAction.Bet, FeatureAction.Bet, FeatureAction.Bet },
                }
            });

            foreach (GamePhase phase in Enum.GetValues(typeof(GamePhase)))
            { 
                switch(phase)
                {
                    case GamePhase.Flop:
                    case GamePhase.Turn:
                    case GamePhase.River:
                        //continuation bet
                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.ContinuationBetIp,
                            Phase = phase,
                            BetMinThreshold = 0.6,
                            BetMaxThreshold = 1,
                            PassMinThreshold = 0,
                            PassMaxThreshold = 0.3,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet },
                            PreCondition = new Precondition()
                            {
                                Positioning = Positioning.InPosition,
                                Aggression = Aggression.IsLast,
                                LastActions = new List<FeatureAction>()
                            }
                        });

                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.ContinuationBetOop,
                            Phase = phase,
                            BetMinThreshold = 0.5,
                            BetMaxThreshold = 1,
                            PassMinThreshold = 0,
                            PassMaxThreshold = 0.2,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet },
                            PreCondition = new Precondition()
                            {
                                Positioning = Positioning.OutOfPosition,
                                Aggression = Aggression.IsLast,
                                LastActions = new List<FeatureAction>()
                            }
                        });

                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.ContinuationBetReraise,
                            Phase = phase,
                            BetMinThreshold = 0.5,
                            BetMaxThreshold = 1,
                            PassMinThreshold = 0,
                            PassMaxThreshold = 0.2,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet },
                            PreCondition = new Precondition()
                            {
                                Aggression = Aggression.IsLast,
                                LastActions = new List<FeatureAction>() { FeatureAction.Bet, FeatureAction.Bet }
                            }
                        });

                        // reactions to continuation bet
                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.ContinuationBetRaise,
                            Phase = phase,
                            BetMinThreshold = 0.5,
                            BetMaxThreshold = 1,
                            PassMinThreshold = 0,
                            PassMaxThreshold = 0.3,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet },
                            PreCondition = new Precondition()
                            {
                                Aggression = Aggression.IsNotLast,
                                LastActions = new List<FeatureAction>() { FeatureAction.Bet }
                            }
                        });

                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.ContinuationBetCall,
                            Phase = phase,
                            BetMinThreshold = 0,
                            BetMaxThreshold = 0.4,
                            PassMinThreshold = 0.8,
                            PassMaxThreshold = 1,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Call },
                            PreCondition = new Precondition()
                            {
                                Aggression = Aggression.IsNotLast,
                                LastActions = new List<FeatureAction>() { FeatureAction.Bet }
                            }
                        });

                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.ContinuationBetFold,
                            Phase = phase,
                            BetMinThreshold = 0.6,
                            BetMaxThreshold = 1,
                            PassMinThreshold = 0,
                            PassMaxThreshold = 0.2,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Pass },
                            PreCondition = new Precondition()
                            {
                                Aggression = Aggression.IsNotLast,
                                LastActions = new List<FeatureAction>() { FeatureAction.Bet }
                            }
                        });

                        // donk bet                    
                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.DonkBetIp,
                            Phase = phase,
                            BetMinThreshold = 0.3,
                            BetMaxThreshold = 1,
                            PassMinThreshold = 0,
                            PassMaxThreshold = 0.1,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet },
                            PreCondition = new Precondition()
                            {
                                Aggression = Aggression.IsNotLast,
                                Positioning = Positioning.InPosition,
                                LastActions = new List<FeatureAction>()
                            }
                        });

                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.DonkBetOop,
                            Phase = phase,
                            BetMinThreshold = 0.2,
                            BetMaxThreshold = 1,
                            PassMinThreshold = 0,
                            PassMaxThreshold = 0.05,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet },
                            PreCondition = new Precondition()
                            {
                                Aggression = Aggression.IsNotLast,
                                Positioning = Positioning.OutOfPosition,
                                LastActions = new List<FeatureAction>()
                            }
                        });

                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.DonkBetReraise,
                            Phase = phase,
                            BetMinThreshold = 0.7,
                            BetMaxThreshold = 1,
                            PassMinThreshold = 0,
                            PassMaxThreshold = 0.3,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Bet },
                            PreCondition = new Precondition()
                            {
                                Aggression = Aggression.IsNotLast,                                
                                LastActions = new List<FeatureAction>() { FeatureAction.Bet, FeatureAction.Bet}
                            }
                        });

                        // reactions to donk bet
                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.DonkBetCall,
                            Phase = phase,
                            BetMinThreshold = 0,
                            BetMaxThreshold = 0.3,
                            PassMinThreshold = 0.7,
                            PassMaxThreshold = 1,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Call },
                            PreCondition = new Precondition()
                            {
                                Aggression = Aggression.IsLast,                                
                                LastActions = new List<FeatureAction>() { FeatureAction.Bet }
                            }
                        });

                        opponent.Features.Add(new Feature()
                        {
                            Name = FeatureNames.DonkBetFold,
                            Phase = phase,
                            BetMinThreshold = 0.6,
                            BetMaxThreshold = 1,
                            PassMinThreshold = 0,
                            PassMaxThreshold = 0.1,
                            ActionsToMeasure = new List<FeatureAction>() { FeatureAction.Pass },
                            PreCondition = new Precondition()
                            {
                                Aggression = Aggression.IsLast,
                                LastActions = new List<FeatureAction>() { FeatureAction.Bet }
                            }
                        });

                        break;
                }
            }

            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.WentToShowdown,
                Phase = GamePhase.Showdown,       
                IsGlobal = true
            });

            opponent.Features.Add(new Feature()
            {
                Name = FeatureNames.WonShowdown,
                Phase = GamePhase.Showdown,
                BetMinThreshold = 0,
                BetMaxThreshold = 0.5,
                PassMinThreshold = 0.6,
                PassMaxThreshold = 1,
            });

            var serializer = new OpponentSerializer();
            serializer.Serialize(opponent);
        }
    }
}

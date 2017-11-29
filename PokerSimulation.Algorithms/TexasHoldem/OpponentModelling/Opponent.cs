using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.OpponentModelling
{
    public class Opponent
    {
        public List<Feature> Features { get; set; }

        public const double AggressiveThreshold = 0.2;
        public const double PassiveThreshold = 0.4;
        public const double TightThreshold = 0.3;
        public const double LooseThreshold = 0.4;
        public const int MinCountThreshold = 10;


        public Opponent()
        {
            this.Features = new List<Feature>();
        }

        public PlayStyle PlayStyle    
        {
            get
            {
                var vpipFeature = Features.First(x => x.Name == FeatureNames.Vpip);
                var pfrFeature = Features.First(x => x.Name == FeatureNames.Pfr);

                bool isAggressive;
                bool isPassive;

                bool isTight;
                bool isLoose;

                isAggressive = vpipFeature.Value - pfrFeature.Value < 0.2;
                isPassive = vpipFeature.Value - pfrFeature.Value > 0.4;
                isTight = vpipFeature.Value < 0.4;
                isLoose = vpipFeature.Value > 0.5;

                if(isTight && isAggressive)
                {
                    return PlayStyle.TightAggressive;
                }
                if(isTight && isPassive)
                {
                    return PlayStyle.TightPassive;
                }
                if(isLoose && isAggressive)
                {
                    return PlayStyle.LooseAggressive;
                }
                if(isTight && isAggressive)
                {
                    return PlayStyle.TightAggressive;
                }

                return PlayStyle.None;                                    
            }
        }

        public void UpdateFeaturesAfterAction(List<FeatureAction> lastActions, GamePhase phase, Aggression aggression, Positioning positioning)
        {
            //reverse action list to make it easier comparable
            var reversedActions = new List<FeatureAction>(lastActions.ToArray());
            reversedActions.Reverse();
            FeatureAction lastAction = reversedActions[0];
            reversedActions = reversedActions.Skip(1).ToList();

            var featuresToUpdate = Features.Where(x => x.AlreadyIncremented == false &&
                                x.Phase == phase &&
                                (x.PreCondition == null ||
                                ((x.PreCondition.Aggression == Aggression.None ||
                                x.PreCondition.Aggression == aggression) &&
                                (x.PreCondition.Positioning == Positioning.None ||
                                x.PreCondition.Positioning == positioning) &&
                                (x.PreCondition.LastActions.Count == 0 ||
                                reversedActions.Take(x.PreCondition.LastActions.Count).SequenceEqual<FeatureAction>(x.PreCondition.LastActions)))));

            foreach (var feature in featuresToUpdate)
            {                
                if (feature.ActionsToMeasure.Contains(lastAction))
                {
                    feature.Count++;
                    feature.AlreadyIncremented = true;
                }

                if(!feature.IsGlobal)
                {
                    feature.CountTotal++;
                }                
            }
        }

        public void UpdateFeaturesAfterShowdown(bool? isWinner)
        {
            var features = Features.Where(x => x.Phase == GamePhase.Showdown);
            foreach(var feature in features)
            {
                switch(feature.Name)
                {
                    case FeatureNames.WentToShowdown:
                        feature.Count++;
                        break;
                    case FeatureNames.WonShowdown:
                        if (isWinner == null)
                        {
                            // split pot: don't count
                        }
                        else if (isWinner.Value)
                        {                            
                            feature.Count++;
                            feature.CountTotal++;                            
                        }
                        else
                        {
                            feature.CountTotal++;
                        }
                        break;
                }
            }                                                                                                
        }

        /// <summary>
        /// After each hand, increment CountTotal of global features
        /// </summary>
        public void StartNewhand()
        {
            foreach(var feature in Features.Where(x=> x.IsGlobal))
            {
                feature.CountTotal++;
            }

            //reset alreadyincremented flag
            foreach (var feature in Features)
            {
                feature.AlreadyIncremented = false;
            }
        }


        /// <summary>
        /// Check if any past or future action is within bet or fold range depending on opponent's past actions
        /// </summary>
        public FeatureAction? GetCounterAction(List<FeatureAction> possibleActions, List<FeatureAction> lastActions, GamePhase phase, Aggression aggression, Positioning positioning)
        {            
            var reversedActions = new List<FeatureAction>(lastActions.ToArray());
            reversedActions.Reverse();

            //is there any past action feature?
            var pastActionFeature = Features.FirstOrDefault(x => x.Phase == phase &&
                                (x.PreCondition == null ||
                                ((x.PreCondition.Aggression == Aggression.None ||
                                x.PreCondition.Aggression == aggression) &&
                                (x.PreCondition.Positioning == Positioning.None ||
                                x.PreCondition.Positioning == positioning) &&
                                (x.PreCondition.LastActions.Count == 0 ||
                                reversedActions.Take(x.PreCondition.LastActions.Count).SequenceEqual<FeatureAction>(x.PreCondition.LastActions)))));

            if(pastActionFeature != null && pastActionFeature.CountTotal > MinCountThreshold)
            {
                if (pastActionFeature.IsInBetRange)
                {
                    return FeatureAction.Bet;
                }
                if (pastActionFeature.IsInPassRange)
                {
                    return FeatureAction.Pass;
                }
            }
          
            foreach (var possibleAction in possibleActions)
            {
                var actions = new List<FeatureAction>(lastActions.ToArray());
                actions.Add(possibleAction);
                actions.Reverse();

                var futureActionFeature = Features.FirstOrDefault(x => x.Phase == phase &&
                                (x.PreCondition != null &&
                                ((x.PreCondition.Aggression == Aggression.None ||
                                x.PreCondition.Aggression == aggression) &&
                                (x.PreCondition.Positioning == Positioning.None ||
                                x.PreCondition.Positioning == positioning) &&
                                (x.PreCondition.LastActions.Count == 0 ||
                                actions.Take(x.PreCondition.LastActions.Count).SequenceEqual<FeatureAction>(x.PreCondition.LastActions)))));

                if(futureActionFeature != null && futureActionFeature.CountTotal > MinCountThreshold)
                {
                    if(futureActionFeature.IsInBetRange)
                    {
                        return FeatureAction.Bet;
                    }
                    if(futureActionFeature.IsInPassRange)
                    {
                        return FeatureAction.Pass;
                    }
                }                
            }

                                                
            return null;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();                        
            sb.AppendLine("Playstyle:" + PlayStyle.ToString());

            bool isFirst = true;
            GamePhase previousPhase = GamePhase.PreFlop;
            foreach (var feature in Features)
            {
                if(feature.Phase != previousPhase || isFirst)
                {
                    sb.AppendLine(feature.Phase.ToString());
                    isFirst = false;
                    previousPhase = feature.Phase;
                }
                sb.AppendLine(feature.ToString());                
            }

            return sb.ToString();
        }

        public string GetFeatureNames()
        {
            var sb = new StringBuilder();
            foreach (var feature in Features)
            {
                sb.AppendLine(feature.Name);
            }
            return sb.ToString();
        }
    }
}
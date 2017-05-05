using PokerEngine.Entities;
using PokerEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Model
{
    public class Session  
    {
        private SessionEntity entity;

        //public Guid Id { get; set; }
        //private int totalHands;
        //private int playedHands;
        //private SessionState state;
        private Player player1;
        private Player player2;

        public int TotalHandsCount {
            get { return entity.TotalHandsCount;  }            
        }

        public int PlayedHandsCount
        {
            get { return entity.PlayedHandsCount; }
        }

        public SessionState State
        {
            get { return entity.State; }
        }        

        public Player Player1 {
            get { return player1; }
        }

        public Player Player2
        {
            get { return player2; }
        }      
        
        public Session(SessionEntity entity, Player player1, Player player2)
        {
            this.entity = entity;
            this.player1 = player1;
            this.player2 = player2;
        }                        
        
        public void Start()
        {
            var players = new List<Player>();
            players.Add(player1);
            players.Add(player2);

            var dealer = new RandomDealer();
            var game = new HeadsupGame(players, dealer);

            while(entity.PlayedHandsCount < entity.TotalHandsCount && State == SessionState.Running)
            {
                var playedHand = game.PlayHand();
                playedHand.SessionID = this.entity.Id;
                playedHand.Timestamp = DateTime.Now;
                //store played hand!    
                this.entity.PlayedHandsCount++;     
                //store session?                                                    
            }
        }

        public void Pause()
        {
            this.entity.State = SessionState.Paused;
        }
    }
}

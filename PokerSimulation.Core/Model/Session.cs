using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Repositories;
using PokerSimulation.Game;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;

namespace PokerSimulation.Core.Model
{
    public class Session  
    {
        private SessionEntity sessionEntity;
        private Player player1;
        private Player player2;
        private IRepository<SessionEntity> sessionRepository;
        private IRepository<PlayedHandEntity> playedHandRepository;
        private HeadsupGame game;


        public Guid Id { 
            get { return sessionEntity.Id; }
        }

        public int TotalHandsCount {
            get { return sessionEntity.TotalHandsCount;  }            
        }

        public int PlayedHandsCount
        {
            get { return sessionEntity.PlayedHandsCount; }
        }

        public SessionState State
        {
            get { return sessionEntity.State; }
        }        

        public Player Player1 {
            get { return player1; }
        }

        public Player Player2
        {
            get { return player2; }
        }      

        public HeadsupGame Game
        {
            get { return game; }
        }
        
        public Session(SessionEntity entity, IRepository<SessionEntity> sessionRepository, IRepository<PlayedHandEntity> playedHandRepository)
        {
            this.sessionEntity = entity;
            this.sessionRepository = sessionRepository;
            this.playedHandRepository = playedHandRepository;

            var factory = new PlayerFactory();
            this.player1 = factory.GetPlayer(entity.PlayerEntity1);
            this.player2 = factory.GetPlayer(entity.PlayerEntity2);
        }                        
        
        public void Start()
        {
            this.sessionEntity.State = SessionState.Running;
            var players = new List<Player>();
            players.Add(player1);
            players.Add(player2);

            var dealer = new RandomDealer();
            game = new HeadsupGame(players, dealer);

            while(sessionEntity.PlayedHandsCount < sessionEntity.TotalHandsCount && State == SessionState.Running)
            {
                var playedHand = game.PlayHand();
                playedHand.SessionId = this.sessionEntity.Id;
                playedHand.Timestamp = DateTime.Now;

                playedHandRepository.Insert(playedHand);                
                this.sessionEntity.PlayedHandsCount++;
                sessionRepository.Update(sessionEntity);                                                         
            }

            this.sessionEntity.State = SessionState.Completed;
            sessionRepository.Update(sessionEntity);
        }

        public void Pause()
        {
            if(this.sessionEntity.State != SessionState.Completed)
            {
                this.sessionEntity.State = SessionState.Paused;
                sessionRepository.Update(sessionEntity);
            }          
        }

        public void Resume()
        {
            if (this.sessionEntity.State != SessionState.Completed)
            {
                this.sessionEntity.State = SessionState.Running;
                sessionRepository.Update(sessionEntity);
            }          
        }
    }
}

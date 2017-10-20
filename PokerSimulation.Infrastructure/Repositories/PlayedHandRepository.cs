using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using PokerSimulation.Core.Repositories;
using PokerSimulation.Core.Model;
using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Interfaces;
using PokerSimulation.Game.Entities;

namespace PokerSimulation.Infrastructure.Repositories
{
    public class PlayedHandRepository : BaseRepository, IPlayedHandRepository
    {
        private IRepository<GameActionEntity> gameActonRepository;
        public PlayedHandRepository(IRepository<GameActionEntity> gameActonRepository)
        {
            this.gameActonRepository = gameActonRepository;
        }
        
        public void Delete(PlayedHandEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PlayedHandEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PlayedHandEntity> GetAllBySessionId(Guid sessionId)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var playedHands = db.Query<PlayedHandEntity>("SELECT * FROM PlayedHand WHERE [SessionId]=@SessionId",
                    new
                    {
                        SessionId = sessionId
                    });                        

                return playedHands;
            }
        }

        public PlayedHandEntity GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Insert(PlayedHandEntity hand)
        {
            hand.Id = Guid.NewGuid();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlInsert = @"INSERT INTO [dbo].[PlayedHand]
                   ([Id],[WinnerId],[PotSize],[Timestamp],[SessionId],[Holecards1],[Holecards2],[Board],[AmountWon])
                    VALUES
                   (@Id, @WinnerId, @Potsize, @Timestamp, @SessionId,@Holecards1,@Holecards2,@Board,@AmountWon)";

                db.Execute(sqlInsert,
                new
                {
                    Id = hand.Id,
                    WinnerId = hand.WinnerId,
                    PotSize = hand.PotSize,
                    Timestamp = hand.Timestamp,
                    SessionId = hand.SessionId,
                    Holecards1 = hand.HoleCards1,
                    Holecards2 = hand.HoleCards2,
                    Board = hand.Board,
                    AmountWon = hand.AmountWon
                });                
            }

            foreach (var action in hand.Actions)
            {
                action.HandId = hand.Id;                
                gameActonRepository.Insert(action);
            }
        }

        public void Update(PlayedHandEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

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
using PokerEngine.Model;
using PokerEngine.Entities;

namespace PokerEngine.Repositories
{
    public class SessionRepository : BaseRepository, IRepository<SessionEntity>
    {
        private IRepository<PlayerEntity> playerRepository;
        public SessionRepository(IRepository<PlayerEntity> playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        IEnumerable<SessionEntity> IRepository<SessionEntity>.GetAll()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sessions = db.Query<SessionEntity>("SELECT * FROM Session");
                foreach (var session in sessions)
                {
                    session.Player1 = playerRepository.GetById(session.Player1Id);
                    session.Player2 = playerRepository.GetById(session.Player2Id);                    
                }

                return sessions;
            }            
        }

        public void Insert(SessionEntity entity)
        {
            entity.Id = Guid.NewGuid();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"INSERT INTO [dbo].[Session](
                                    [Id],[Player1Id],[Player2Id],[State],[TotalHandsCount],[PlayedHandsCount]) 
                                    VALUES (@Id,@Player1Id,@Player2Id,@State, @TotalHandsCount, @PlayedHandsCount)";
                db.Execute(sqlQuery,
                    new
                    {
                        Id = entity.Id,
                        Player1Id = entity.Player1Id,
                        Player2Id = entity.Player2Id,
                        State = entity.State,
                        TotalHandsCount = entity.TotalHandsCount,
                        PlayedHandsCount = entity.PlayedHandsCount
                    });
            }
        }

        public void Delete(SessionEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(SessionEntity entity)
        {
            throw new NotImplementedException();
        }

        SessionEntity IRepository<SessionEntity>.GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SessionEntity> SearchFor(Expression<Func<SessionEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }       
    }
}

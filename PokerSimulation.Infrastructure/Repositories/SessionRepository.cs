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
using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Repositories;

namespace PokerSimulation.Infrastructure.Repositories
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
                    session.PlayerEntity1 = playerRepository.GetById(session.Player1Id);
                    session.PlayerEntity2 = playerRepository.GetById(session.Player2Id);                    
                }

                return sessions;
            }            
        }

        public void Insert(SessionEntity session)
        {
            session.Id = Guid.NewGuid();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlInsert = @"INSERT INTO [dbo].[Session](
                                    [Id],[Player1Id],[Player2Id],[State],[TotalHandsCount],[PlayedHandsCount]) 
                                    VALUES (@Id,@Player1Id,@Player2Id,@State, @TotalHandsCount, @PlayedHandsCount)";
                db.Execute(sqlInsert,
                    new
                    {
                        Id = session.Id,
                        Player1Id = session.Player1Id,
                        Player2Id = session.Player2Id,
                        State = session.State,
                        TotalHandsCount = session.TotalHandsCount,
                        PlayedHandsCount = session.PlayedHandsCount
                    });
            }

            session.PlayerEntity1 = playerRepository.GetById(session.Player1Id);
            session.PlayerEntity2 = playerRepository.GetById(session.Player2Id);
        }

        public void Delete(SessionEntity entity)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlDelete = @"DELETE FROM Session
                                     WHERE Id=@Id";
                //played hands and game actions are deleted too (added constraint with cascade delete)
                db.Execute(sqlDelete,
                    new
                    {
                        Id = entity.Id                       
                    });
            }
        }

        public void Update(SessionEntity entity)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlInsert = @" UPDATE[dbo].[Session]
                                     SET [Player1Id]=@Player1Id,[Player2Id]=@Player2Id,[State]=@State,
                                     [TotalHandsCount]=@TotalHandsCount,[PlayedHandsCount]=@PlayedHandsCount
                                     WHERE Id=@Id";
                db.Execute(sqlInsert,
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

        SessionEntity IRepository<SessionEntity>.GetById(Guid id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var session = db.Query<SessionEntity>("SELECT * FROM Session WHERE Id=@Id",
                    new
                    {
                        Id = id
                    }).FirstOrDefault();
             
                if(session != null)
                {
                    session.PlayerEntity1 = playerRepository.GetById(session.Player1Id);
                    session.PlayerEntity2 = playerRepository.GetById(session.Player2Id);
                }                          

                return session;
            }
        }

        public IEnumerable<SessionEntity> SearchFor(Expression<Func<SessionEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using PokerSimulation.Core.Model;
using PokerSimulation.Core.Repositories;
using PokerSimulation.Game.Entities;

namespace PokerSimulation.Infrastructure.Repositories
{
    public class GameActionRepository : BaseRepository, IRepository<GameActionEntity>
    {
        public void Delete(GameActionEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GameActionEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GameActionEntity> GetAllByHandId()
        {
            throw new NotImplementedException();
        }

        public GameActionEntity GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Insert(GameActionEntity entity)
        {
            entity.Id = Guid.NewGuid();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlInsert = @"INSERT INTO [dbo].[GameAction]
                   ([Id],[Timestamp],[HandId],[PlayerId],[ActionType],[Amount])
                    VALUES
                   (@Id, @Timestamp, @HandId, @PlayerId, @ActionType, @Amount)";

                db.Execute(sqlInsert,
                new
                {
                    Id = entity.Id,
                    Timestamp = entity.Timestamp,
                    HandId = entity.HandId,
                    PlayerId = entity.PlayerId,
                    ActionType = entity.ActionType,                                        
                    Amount = entity.Amount                    
                });
            }
        }   

        public void Update(GameActionEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

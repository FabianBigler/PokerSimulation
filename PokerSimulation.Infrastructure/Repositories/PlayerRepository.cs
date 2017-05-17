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
    public class PlayerRepository : BaseRepository, IRepository<PlayerEntity>
    {
        public void Delete(PlayerEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(PlayerEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(PlayerEntity entity)
        {
            throw new NotImplementedException();
        }

        IEnumerable<PlayerEntity> IRepository<PlayerEntity>.GetAll()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<PlayerEntity>("SELECT * FROM Player");                
            }
        }

        PlayerEntity IRepository<PlayerEntity>.GetById(Guid id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = "SELECT * FROM Player WHERE id = @id";
                return db.Query<PlayerEntity>(sql, new { id = id }).FirstOrDefault();
            }
        }
    }
}

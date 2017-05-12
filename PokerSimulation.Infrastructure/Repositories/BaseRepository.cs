using System;
using System.Configuration;

namespace PokerSimulation.Infrastructure.Repositories
{
    public class BaseRepository
    {
        protected string connectionString;

        public BaseRepository()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["Dbconnection"];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");
            connectionString = mySetting.ConnectionString;
        }
    }
}

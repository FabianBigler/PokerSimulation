using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Repositories
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

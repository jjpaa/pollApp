using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBackend
{
    class SqliteDataAccess
    {
        public List<Poll> LoadPolls()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Poll>("SELECT * FROM polls", new DynamicParameters());
                return output.ToList();
            }
        }
        public List<Poll> LoadPollWithId(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Poll>("SELECT * FROM polls WHERE id = " + id.ToString(), new DynamicParameters());
                return output.ToList();
            }
        }

        public List<Option> LoadOptionsWithPollId(int pollID)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Option>("SELECT * FROM options WHERE pollID = " + pollID.ToString(), new DynamicParameters());
                return output.ToList();
            }
        }

        public int SQLGetPollID(string sqlCommand)
        {
            int i;
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                i = cnn.QueryFirst<int>(sqlCommand);
            }
            return i;
        }

        public void InsertInto(string tableName, string columns, string values)
        {
            string sqlCommand = ("INSERT INTO " + tableName + " (" + columns + ") VALUES (" + values + ")");

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute(sqlCommand);
            }
        }

        public void Vote(int pollId, int id)
        {
            string sqlCommand = ("UPDATE options SET votes = votes + 1 WHERE pollId = " + pollId + " AND id = " + id);

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute(sqlCommand);
            }
        }

        public static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}

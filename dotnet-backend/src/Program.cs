using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            SqliteDataAccess sda = new SqliteDataAccess();
            SimpleHttpServer.Server(sda);
        }
    }
}

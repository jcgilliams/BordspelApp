using System;
using System.Collections.Generic;
using System.Text;

namespace BordspelApp_DAL
{
    public abstract class BaseRepository
    {
        protected string ConnectionString { get; }

        public BaseRepository()
        {
            ConnectionString = DatabaseConnection.Connectionstring("BordspelAppDB");
        }
    }
}

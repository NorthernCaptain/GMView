using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace ncUtils
{
    /// <summary>
    /// Class provides DBConnection pool and manages all connections to SQLite DB
    /// </summary>
    public class DBConnPool
    {
        private LinkedList<SQLiteConnection> conPool = new LinkedList<SQLiteConnection>();
        private string conString = @"Data Source=knowhere.sq3;New=False;Version=3";
        private static DBConnPool instance = null;

        /// <summary>
        /// Gets a single instance of DBConnPool
        /// </summary>
        public static DBConnPool singleton
        {
            get
            {
                if (instance == null)
                    instance = new DBConnPool();
                return instance;
            }
        }

        /// <summary>
        /// Sets DB name for db connection
        /// </summary>
        public string dbName
        {
            set
            {
                conString = "Data Source=" + value + ";New=False;Version=3";
            }
        }
        /// <summary>
        /// return DBConnection object and reserves it for usage
        /// Use releaseCon for freeing DbConnection
        /// </summary>
        /// <returns></returns>
        public DbConnection getCon()
        {
            lock (conPool)
            {
                if (conPool.First == null)
                    return newCon();
                DbConnection con = conPool.First.Value;
                conPool.RemoveFirst();
                return con;
            }
        }

        /// <summary>
        /// Creates and return query parameter with the given name, type and value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dtype"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public DbParameter newPar(string name, DbType dtype, object val)
        {
            DbParameter par = new SQLiteParameter(name, dtype);
            par.Value = val;
            return par;
        }

        public DbParameter newPar(string name, DbType dtype, int length, object val)
        {
            DbParameter par = new SQLiteParameter(name, dtype, length);
            par.Value = val;
            return par;

        }

        /// <summary>
        /// Releases connection and adds it to the pool of free connections
        /// </summary>
        /// <param name="con"></param>
        public void releaseCon(DbConnection con)
        {
            lock (conPool)
            {
                conPool.AddLast(con as SQLiteConnection);
            }
        }

        /// <summary>
        /// Always creates and returns new connection
        /// </summary>
        /// <returns></returns>
        private SQLiteConnection newCon()
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(conString);
                con.Open();
                return con;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }
    }
}

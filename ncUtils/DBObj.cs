using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace ncUtils
{
    /// <summary>
    /// Database object helper class
    /// </summary>
    public class DBObj : IDisposable
    {
        /// <summary>
        /// Database connection
        /// </summary>
        protected DbConnection con = null;

        /// <summary>
        /// Database command for this connection
        /// </summary>
        public DbCommand cmd = null;

        /// <summary>
        /// Construct DB object, gets connection to DB
        /// </summary>
        public DBObj()
        {
            con = DBConnPool.singleton.getCon();
            if (con == null)
                throw new ArgumentNullException("dbConnection");
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
        }

        /// <summary>
        /// Create DB object with the given query
        /// </summary>
        /// <param name="command"></param>
        public DBObj(string command)
        {
            con = DBConnPool.singleton.getCon();
            if (con == null)
                throw new ArgumentNullException("dbConnection");
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = command;
        }

        /// <summary>
        /// Gets or sets command text, and clear parameters on set
        /// </summary>
        public string commandText
        {
            get
            {
                return cmd.CommandText;
            }
            set
            {
                cmd.CommandText = value;
                cmd.Parameters.Clear();
            }
        }

        /// <summary>
        /// Adds string parameter to the query
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void addStringPar(string name, string val)
        {
            cmd.Parameters.Add(DBConnPool.singleton.newPar(name, DbType.String, val));
        }

        /// <summary>
        /// Adds integer param to the query
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void addIntPar(string name, int val)
        {
            cmd.Parameters.Add(DBConnPool.singleton.newPar(name, DbType.Int32, val));
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (cmd != null)
                cmd.Dispose();
            if (con != null)
                DBConnPool.singleton.releaseCon(con);
            con = null;
            cmd = null;
        }

        #endregion
    }
}

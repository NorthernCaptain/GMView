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
        public DBObj addStringPar(string name, string val)
        {
            cmd.Parameters.Add(DBConnPool.singleton.newPar(name, DbType.String, val));
            return this;
        }

        /// <summary>
        /// Adds integer param to the query
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public DBObj addIntPar(string name, int val)
        {
            cmd.Parameters.Add(DBConnPool.singleton.newPar(name, DbType.Int32, val));
            return this;
        }

        public DBObj addFloatPar(string name, double val)
        {
            cmd.Parameters.Add(DBConnPool.singleton.newPar(name, DbType.Double, val));
            return this;
        }

        /// <summary>
        /// Adds new parameter to the command. Can be used to pass any type of param
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dtype"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public DBObj addPar(string name, DbType dtype, object val)
        {
            cmd.Parameters.Add(DBConnPool.singleton.newPar(name, dtype, val));
            return this;
        }

        /// <summary>
        /// Execute query and return single scalar int value as the result;
        /// </summary>
        /// <returns></returns>
        public int executeIntValue()
        {
            long val = (long)cmd.ExecuteScalar();
            return (int)val;
        }

        /// <summary>
        /// Execute query and return single scalar string
        /// </summary>
        /// <returns></returns>
        public string executeStringValue()
        {
            return (string)cmd.ExecuteScalar();
        }

        /// <summary>
        /// Executes non-Select statement, i.e DML statement
        /// </summary>
        /// <returns></returns>
        public int executeNonQuery()
        {
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Select and return curval of the sequence by the table name.
        /// Execute only after insert operation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int seqCurval(string name)
        {
            this.commandText = @"select seq from sqlite_sequence where name=@NAME";
            this.addStringPar("@NAME", name);
            return this.executeIntValue();
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

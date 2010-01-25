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
        private static volatile DBConnPool instance = null;

        private List<SQLiteConnection> allConnections = new List<SQLiteConnection>(50);

        /// <summary>
        /// Simple class for holding connection and transaction data for the thread
        /// </summary>
        private class ThreadData
        {
            SQLiteTransaction tran;
            SQLiteConnection conn;

            internal SQLiteConnection connection
            {
                get { return conn; }
            }

            internal SQLiteTransaction transaction
            {
                get { return tran; }
            }

            internal ThreadData(SQLiteConnection iconn, SQLiteTransaction itran)
            {
                conn = iconn;
                tran = itran;
            }
        }

        private readonly Dictionary<System.Threading.Thread, ThreadData> threadTranList
            = new Dictionary<System.Threading.Thread, ThreadData>();

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
                checkDBschema(value);
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
                ThreadData tData;
                threadTranList.TryGetValue(System.Threading.Thread.CurrentThread, out tData);

                if(tData != null)
                {
                    return tData.connection;
                }

                if (conPool.First == null)
                    return newCon();
                DbConnection con = conPool.First.Value;
                conPool.RemoveFirst();
                return con;
            }
        }

        /// <summary>
        /// Opens DB transaction for current thread, return Transaction object.
        /// Every call to getCon will return the same transaction for the thread.
        /// </summary>
        /// <returns></returns>
        public DbTransaction beginThreadTransaction()
        {
            lock (conPool)
            {
                ThreadData tData;
                threadTranList.TryGetValue(System.Threading.Thread.CurrentThread, out tData);

                if (tData == null)
                {
                    SQLiteConnection con = getCon() as SQLiteConnection;
                    tData = new ThreadData(con, con.BeginTransaction());
                    threadTranList.Add(System.Threading.Thread.CurrentThread,
                        tData);
                }
                return tData.transaction;
            }
        }

        /// <summary>
        /// Commit transaction for current thread
        /// </summary>
        public void commitThreadTransaction()
        {
            lock (conPool)
            {
                ThreadData tData;
                threadTranList.TryGetValue(System.Threading.Thread.CurrentThread, out tData);

                if (tData == null)
                    return; //No transaction at all

                tData.transaction.Commit();
                tData.transaction.Dispose();
                threadTranList.Remove(System.Threading.Thread.CurrentThread);
            }
        }

        /// <summary>
        /// Rollback transaction for current thread
        /// </summary>
        public void rollbackThreadTransaction()
        {
            lock (conPool)
            {
                ThreadData tData;
                threadTranList.TryGetValue(System.Threading.Thread.CurrentThread, out tData);

                if (tData == null)
                    return; //No transaction at all

                tData.transaction.Rollback();
                tData.transaction.Dispose();
                threadTranList.Remove(System.Threading.Thread.CurrentThread);
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
            if (con == null)
                return;

            lock (conPool)
            {
                ThreadData tData;
                threadTranList.TryGetValue(System.Threading.Thread.CurrentThread, out tData);

                if (tData != null && tData.connection == con)
                {
                    return;
                }

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
                allConnections.Add(con);
                return con;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Close all opened connections
        /// </summary>
        public void closeAll()
        {
            foreach (SQLiteConnection conn in allConnections)
            {
                conn.Close();
                conn.Dispose();
            }
        }
        /// <summary>
        /// Check and initialized database schema if we don't have one
        /// Return true if schema exists
        /// </summary>
        /// <returns></returns>
        private bool checkDBschema(string fname)
        {
            if (System.IO.File.Exists(fname))
                return true;

            string constr = @"Data Source=" + fname + ";New=True;Version=3";
            SQLiteConnection con = null;
            try
            {
                con = new SQLiteConnection(constr);
                con.Open();
                DbTransaction tran = con.BeginTransaction();
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;

                System.IO.StringReader reader = 
                        new System.IO.StringReader(ncUtils.Properties.Resources.createDB);

                try
                {
                    string stmt = string.Empty;
                    string buf;
                    while((buf = reader.ReadLine()) != null)
                    {
                        if(buf.Trim().Length == 0)
                            continue;
                        if (buf.StartsWith("--"))
                        {
                            stmt = stmt.Trim();
                            if (stmt.Length == 0)
                                continue;
                            if (stmt[stmt.Length -1] == ';')
                                stmt = stmt.Remove(stmt.Length - 1, 1);

                            try
                            {
                                cmd.CommandText = stmt;
                                stmt = string.Empty;
                                cmd.ExecuteNonQuery();
                            }
                            catch (System.Exception e)
                            {

                            }
                        }
                        else
                            stmt += buf;                       
                    }
                }
                finally
                {
                    reader.Close();
                }
                tran.Commit();
            }
            catch (System.Exception e)
            {
            	
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }

            return false;
        }
    }
}

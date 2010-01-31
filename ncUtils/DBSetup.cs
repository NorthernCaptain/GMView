using System;
using System.Collections.Generic;
using System.Text;

namespace ncUtils
{
    /// <summary>
    /// Class holds interface for accessing setup table with its name - value data
    /// </summary>
    public class DBSetup
    {
        private static volatile DBSetup instance = null;

        /// <summary>
        /// Return single instance of the setup
        /// </summary>
        public static DBSetup singleton
        {
            get
            {
                if (instance == null)
                    instance = new DBSetup();
                return instance;
            }
        }

        /// <summary>
        /// Set integer value for the name entry in setup table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public DBSetup setInt(string name, int val)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"delete from setup where name = @NAME");
                dbo.addStringPar("@NAME", name);
                dbo.executeNonQuery();

                dbo = new DBObj(@"insert into setup (name, intval) values (@NAME, @INTVAL)");
                dbo.addStringPar("@NAME", name);
                dbo.addIntPar("@INTVAL", val);
                dbo.executeNonQuery();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }
            
            return this;
        }

        /// <summary>
        /// Set double value for the name entry in setup table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public DBSetup setFloat(string name, double val)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"delete from setup where name = @NAME");
                dbo.addStringPar("@NAME", name);
                dbo.executeNonQuery();

                dbo = new DBObj(@"insert into setup (name, floatval) values (@NAME, @INTVAL)");
                dbo.addStringPar("@NAME", name);
                dbo.addFloatPar("@INTVAL", val);
                dbo.executeNonQuery();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }

            return this;
        }

        /// <summary>
        /// Set string value for the name entry in setup table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public DBSetup setString(string name, string val)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"delete from setup where name = @NAME");
                dbo.addStringPar("@NAME", name);
                dbo.executeNonQuery();

                dbo = new DBObj(@"insert into setup (name, stringval) values (@NAME, @INTVAL)");
                dbo.addStringPar("@NAME", name);
                dbo.addStringPar("@INTVAL", val);
                dbo.executeNonQuery();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }

            return this;
        }

        /// <summary>
        /// Set DateTime value for the name entry in setup table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public DBSetup setFloat(string name, DateTime val)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"delete from setup where name = @NAME");
                dbo.addStringPar("@NAME", name);
                dbo.executeNonQuery();

                dbo = new DBObj(@"insert into setup (name, dateval) values (@NAME, @INTVAL)");
                dbo.addStringPar("@NAME", name);
                dbo.addPar("@INTVAL", System.Data.DbType.DateTime, val);
                dbo.executeNonQuery();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }

            return this;
        }


        /// <summary>
        /// Select value from setup by name and return it, or return default value if no such entry
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defval"></param>
        /// <returns></returns>
        public int getInt(string name, int defval)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"select intval from setup where name = @NAME");
                dbo.addStringPar("@NAME", name);
                return dbo.executeIntValue();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }

            return defval;
        }

        /// <summary>
        /// Select value from setup by name and return it, or return default value if no such entry
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defval"></param>
        /// <returns></returns>
        public double getFloat(string name, double defval)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"select floatval from setup where name = @NAME");
                dbo.addStringPar("@NAME", name);
                return dbo.executeFloatValue();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }

            return defval;
        }

        /// <summary>
        /// Select value from setup by name and return it, or return default value if no such entry
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defval"></param>
        /// <returns></returns>
        public string getString(string name, string defval)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"select stringval from setup where name = @NAME");
                dbo.addStringPar("@NAME", name);
                return dbo.executeStringValue();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }

            return defval;
        }

        /// <summary>
        /// Select value from setup by name and return it, or return default value if no such entry
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defval"></param>
        /// <returns></returns>
        public DateTime getDate(string name, DateTime defval)
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"select dateval from setup where name = @NAME");
                dbo.addStringPar("@NAME", name);
                return dbo.executeDateValue();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }

            return defval;
        }    
    }
}

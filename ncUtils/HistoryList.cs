using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Data;

namespace ncUtils
{
    /// <summary>
    /// Class implement history list with string items used for displaying in comboboxes
    /// </summary>
    public class HistoryList
    {
        /// <summary>
        /// Name of this history list
        /// </summary>
        protected string name;

        /// <summary>
        /// Class represents one item in the list
        /// </summary>
        public class HistoryItem
        {
            public int id = 0;
            public string value = string.Empty;

            public HistoryItem(string valueS)
            {
                value = valueS;
            }

            public HistoryItem(int iid, string valueS)
            {
                id = iid;
                value = valueS;
            }

            public override string ToString()
            {
                return value;
            }
        }

        /// <summary>
        /// List of items in the history in order from most recent to elder ones
        /// </summary>
        protected List<HistoryItem> itemList = new List<HistoryItem>(50);

        /// <summary>
        /// Get a list of history items
        /// </summary>
        public List<HistoryItem> items
        {
            get
            {
                return itemList;
            }
        }

        /// <summary>
        /// Constructor for one history list with the given name
        /// </summary>
        /// <param name="iname"></param>
        public HistoryList(string iname)
        {
            name = iname;
            loadItems();
        }

        /// <summary>
        /// Loads item for the given name from history table from DB
        /// </summary>
        protected void loadItems()
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"select id, value from history_items where typename=@TYPENAME order by created desc");
                dbo.addStringPar("@TYPENAME", name);
                DbDataReader reader = dbo.cmd.ExecuteReader();
                while (reader.Read())
                {
                    HistoryItem hi = new HistoryItem(reader.GetInt32(0), reader.GetString(1));
                    itemList.Add(hi);
                }
            }
            catch (System.Exception e)
            {
            	
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }
        }

        /// <summary>
        /// Appends new history entry or overwrites existing one
        /// </summary>
        /// <param name="valueS"></param>
        public void add(string valueS)
        {
            foreach (HistoryItem hi in itemList)
            {
                if (hi.value.Equals(valueS))
                {
                    delete(hi);
                    insert(valueS);
                    return;
                }
            }
            insert(valueS);
        }

        /// <summary>
        /// Always insert new entry to the history, do not check the existence of the same item
        /// </summary>
        /// <param name="valueS"></param>
        public void insert(string valueS)
        {
            HistoryItem hi = new HistoryItem(valueS);
            itemList.Insert(0, hi);

            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"insert into history_items(typename, value) values(@TYPENAME,@VALUE)");
                dbo.addStringPar("@TYPENAME", name);
                dbo.addStringPar("@VALUE", valueS);
                dbo.cmd.ExecuteNonQuery();

                dbo.commandText = @"select seq from sqlite_sequence where name=@NAME";
                dbo.addStringPar("@NAME", "history_items");
                long lid = (long)dbo.cmd.ExecuteScalar();
                hi.id = (int)lid;
            }
            catch (System.Exception e)
            {
            	
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }
        }

        /// <summary>
        /// Deletes the specified value from the list
        /// </summary>
        /// <param name="hi"></param>
        protected void delete(HistoryItem hi)
        {
            itemList.Remove(hi);
        }
    }
}

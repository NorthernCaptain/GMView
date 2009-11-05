using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

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
            SQLiteConnection conn = new SQLiteConnection();
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

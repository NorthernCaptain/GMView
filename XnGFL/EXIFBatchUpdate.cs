using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace XnGFL
{
    /// <summary>
    /// Class updates EXIF information to the files. Do it in background thread via exiftool call
    /// </summary>
    public class EXIFBatchUpdate: ncUtils.GUIWorkerThread<EXIFData>
    {
        private int count = 0;

        public EXIFBatchUpdate(Control ctrl): base(ctrl)
        {
            this.taskCompleted += this.exifUpdated;
        }

        /// <summary>
        /// Adds all data from the batch dictionary to the queue for processing
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public int addBatch(Dictionary<string, EXIFData> batch)
        {
            count = 0;
            foreach (KeyValuePair<string, EXIFData> pair in batch)
            {
                this.addTask(pair.Value);
                count++;
            }
            return count;
        }

        /// <summary>
        /// This delegate will be called after update of every file
        /// </summary>
        /// <param name="dat"></param>
        public delegate void onOneUpdateDelegate(EXIFData dat);

        /// <summary>
        /// Delegate to call on finish of the batch
        /// </summary>
        public delegate void onBatchFinishedDelegate();

        /// <summary>
        /// Event on finish of the batch processing, calls in main thread
        /// </summary>
        public event onBatchFinishedDelegate onBatchFinished;

        /// <summary>
        /// Event called on every update (after it) in main thread
        /// </summary>
        public event onOneUpdateDelegate onOneUpdate;

        /// <summary>
        /// Called in main thread after update of one EXIF file
        /// </summary>
        /// <param name="dat"></param>
        private void exifUpdated(EXIFData dat)
        {
            if (onOneUpdate != null)
                onOneUpdate(dat);
            count--;

            if (count <= 0 && onBatchFinished != null)
                onBatchFinished();
        }
    }
}

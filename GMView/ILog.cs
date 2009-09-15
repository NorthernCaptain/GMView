using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    interface ILog
    {
        void Log(string txt);
        void NMEALog(ncGeo.NMEAString str);
        void Err(string txt);

        bool needInvoke
        {
            get;
        }
    }
}

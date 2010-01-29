using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ncUtils
{
    /// <summary>
    /// Class contains tools for working with xml
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// Helper for retreiving xml data
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xpath"></param>
        /// <param name="nsm"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool xmltag(XmlNode node, string xpath, XmlNamespaceManager nsm, ref string val)
        {
            XmlNode result = node.SelectSingleNode(xpath, nsm);
            if (result != null)
            {
                val = result.InnerText;
                return true;
            }
            val = "";
            return false;
        }
    }
}

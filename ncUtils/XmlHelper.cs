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

        /// <summary>
        /// Creates XmlNamespaceManager for our document
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="defaultNamespace"></param>
        /// <returns></returns>
        public static XmlNamespaceManager getNSMforDoc(XmlDocument doc, string defaultNamespace)
        {
            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);

            XmlNodeList xmlNameSpaceList = doc.SelectNodes(@"//namespace::*[not(. = ../../namespace::*)]");

            bool setXmlNs = false;
            foreach (XmlNode nsNode in xmlNameSpaceList)
            {
                if (nsNode.LocalName == "xmlns")
                {
                    nsm.AddNamespace(defaultNamespace, nsNode.Value);
                    setXmlNs = true;
                }
                else
                    nsm.AddNamespace(nsNode.LocalName, nsNode.Value);
            }

            if(!setXmlNs)
            {
               nsm.AddNamespace(defaultNamespace, "");
            }

            if (!nsm.HasNamespace("knw"))
            {
                nsm.AddNamespace("knw", @"http://xnc.jinr.ru/knowhere/xmlns/knw/1/0");
            }

            return nsm;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ncGeo
{
    public class NMEAString
    {
        //Whole NMEA sentence
        [ XmlElementAttribute("nmea_text") ]
        public string sentence;
        //Parts of the sentence like separated strings, separator is ','
        [ XmlArrayItem("nmea_part", typeof(string)) ]
        public List<string> parts = new List<string>();
        //Error if occured
        protected Exception error_ex = null;

        [XmlElementAttribute("error_string")]
        public string error
        {
            get { return error_ex == null ? null : error_ex.Message.ToString(); }
        }

        public NMEAString()
        {
            sentence = "";
        }

        public NMEAString(string isentence)
        {
            splitSentence(isentence);
        }

        public NMEAString(Exception ex)
        {
            error_ex = ex;
        }

        public void splitSentence(string isentence)
        {
            sentence = isentence;
            char[] symb = new char [] { ',', '*'};

            isentence = sentence.Substring(3);
            string[] part_arr = isentence.Split(symb, StringSplitOptions.None);
            parts.AddRange(part_arr);
        }
    }
}

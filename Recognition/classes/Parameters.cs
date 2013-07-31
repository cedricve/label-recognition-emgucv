using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Recognition
{
    public class Parameters
    {
        Hashtable htable;
        public Parameters(string xml)
        { 
            // Parse XML
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xml);

                htable = new Hashtable();
                foreach(XmlNode node in doc.DocumentElement.ChildNodes){
                    htable[node.Name] = node.InnerText;   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not open XML. Original error: " + ex.Message);
            }
        }

        public object get(string key) {
            return htable[key];
        }

    }
}

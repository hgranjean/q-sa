using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace Atum.Utility.XML
{
    public class XPathUtil
    {
        public static long GetLong(XPathDocument xpathDoc, string xpathQuery, long lngDefault)
        {
            long retVal;
            try
            {
                retVal = long.Parse(GetString(xpathDoc, xpathQuery));
            }
            catch 
            {

                return lngDefault;
            }

            return retVal;
        }

        public static int GetInt(XPathDocument xpathDoc, string xpathQuery, int intDefault)
        {
            int retVal;
            try
            {
                retVal = int.Parse(GetString(xpathDoc, xpathQuery));
            }
            catch
            {
                return intDefault;
            }
            return retVal;
        }
        public static string GetString(XPathDocument xpathDoc, string xpathQuery)
        {
            string retVal = "";

            XPathNavigator nav = xpathDoc.CreateNavigator();
            XPathNodeIterator ni = nav.Select(xpathQuery);
			if(ni.MoveNext())
			{
				retVal = ni.Current.Value.ToString();
			}
            return retVal;
        }

        public static bool GetBool(XPathDocument xpathDoc, string xpathQuery)
        {
            bool retVal = false;
            try
            {
                retVal = bool.Parse(GetString(xpathDoc, xpathQuery));
            }
            catch
            {
                return false;
            }
            return retVal;
        }
    }
}

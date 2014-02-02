using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Reflection;

namespace Atum.Utility.XML
{
    /// <summary>
    /// XML serialization  utility methods
    /// </summary>
    public static class XmlSerializationUtility
    {
        /// <summary>
        /// Encode string for storage as xml string value
        /// </summary>
        /// <remarks>
        /// Supplements XmlWriter which does not check for valid Xml content
        /// </remarks>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string XmlEncode(string val)
        {
            if (!HasChrsRequiringEncoding(val))
            {
                return val;
            }

            //Must encode
            string ret = HtmlEncode(val);
            if (!HasChrsRequiringEncoding(ret))
            {
                return ret;
            }

            //Some chrs remain to encode

            return GetEncodedString(ret);

        }

        private static string HtmlEncode(string s)
        {
            if (s == null)
            {
                return null;
            }
            int num = IndexOfHtmlEncodingChars(s, 0);
            if (num == -1)
            {
                return s;
            }
            StringBuilder builder = new StringBuilder(s.Length + 5);
            int length = s.Length;
            int startIndex = 0;
        Label_002A:
            if (num > startIndex)
            {
                builder.Append(s, startIndex, num - startIndex);
            }
            char ch = s[num];
            if (ch > '>')
            {
                builder.Append("&#");
                builder.Append(((int)ch).ToString(NumberFormatInfo.InvariantInfo));
                builder.Append(';');
            }
            else
            {
                char ch2 = ch;
                if (ch2 != '"')
                {
                    switch (ch2)
                    {
                        case '<':
                            builder.Append("&lt;");
                            goto Label_00D5;

                        case '=':
                            goto Label_00D5;

                        case '>':
                            builder.Append("&gt;");
                            goto Label_00D5;

                        case '&':
                            builder.Append("&amp;");
                            goto Label_00D5;
                    }
                }
                else
                {
                    builder.Append("&quot;");
                }
            }
        Label_00D5:
            startIndex = num + 1;
            if (startIndex < length)
            {
                num = IndexOfHtmlEncodingChars(s, startIndex);
                if (num != -1)
                {
                    goto Label_002A;
                }
                builder.Append(s, startIndex, length - startIndex);
            }
            return builder.ToString();
        }

        private static int IndexOfHtmlEncodingChars(string s, int startPos)
        {
            int num = s.Length - startPos;

            var s2 = s.Substring(startPos);

            while (num > 0)
            {
                char ch = s2[0];

                if (ch <= '>')
                {
                    switch (ch)
                    {
                        case '<':
                        case '>':
                        case '"':
                        case '&':
                            return (s.Length - num);

                        case '=':
                            goto Label_007A;
                    }
                }
                else if ((ch >= '\x00a0') && (ch < 'Ā'))
                {
                    return (s.Length - num);
                }

            Label_007A:
                s2 = s2.Substring(1);
                num--;
            }

            return -1;
        }

        private static bool HasChrsRequiringEncoding(IEnumerable<char> value)
        {
            foreach (char c in value)
            {
                if (c > 0xFFFD)
                {
                    return true;
                }
                if (c < 0x20 && c != '\t' & c != '\n' & c != '\r')
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetEncodedString(IEnumerable<char> value)
        {
            StringBuilder ret = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 0xFFFD)
                {
                    throw new ApplicationException(String.Format("Can't encode value '{0}'", Convert.ToInt32(c)));
                }
                if (c < 0x20 && c != '\t' & c != '\n' & c != '\r')
                {
                    ret.Append("&#");
                    ret.Append(String.Format("x{0:X}", Convert.ToInt32(c)));
                    ret.Append(";");
                }
                else
                {
                    ret.Append(c);
                }
            }

            return ret.ToString();
        }

        /// <summary>
        /// Format xml string, specifying whether to use indentation
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="useFormatting"></param>
        /// <returns></returns>
        public static string FormatXmlString(string xml, bool useFormatting)
        {
            var reader = XmlReader.Create(new StringReader(xml));
            try
            {
                StringWriter stringWriter = new StringWriter();

                XmlWriterSettings xws = new XmlWriterSettings();
                xws.Encoding = Encoding.Unicode;
                xws.Indent = useFormatting;
                xws.NewLineOnAttributes = useFormatting;
                XmlWriter writer = XmlWriter.Create(stringWriter, xws);

                try
                {
                    //Don't write start doc for string output
                    WriteThroughFirstElement(reader, writer);
                }
                finally
                {
                    writer.Close();
                }
                return stringWriter.ToString();
            }
            finally
            {
                reader.Close();
            }
        }

        public static string RemoveXmlDeclarationFromXmlString(string xml, bool useFormatting)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xml));
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings() { Indent = useFormatting };
                StringWriter stringWriter = new StringWriter();
                XmlWriter writer = XmlWriter.Create(stringWriter, settings);
                try
                {
                    //Note this ignores all content if any prior to first element including xml declaration, processing instruction(s), cdata, comment
                    reader.MoveToContent(); //move past declaration
                    writer.WriteNode(reader, true);
                }
                finally
                {
                    writer.Close();
                }
                return stringWriter.ToString();
            }
            finally
            {
                reader.Close();
            }
        }


        [Obfuscation]
        internal static void WriteThroughFirstElement(XmlReader reader, XmlWriter writer)
        {
            while (!reader.EOF)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Comment:
                        writer.WriteComment(reader.Value);
                        reader.Read();
                        break;
                    case XmlNodeType.CDATA:
                        writer.WriteCData(reader.Value);
                        reader.Read();
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        writer.WriteProcessingInstruction(reader.Name, reader.Value);
                        reader.Read();
                        break;
                    case XmlNodeType.Element:
                        writer.WriteNode(reader,
                                         true); //defAttr
                        break;
                    case XmlNodeType.None:
                    case XmlNodeType.Whitespace:
                        reader.Read();
                        break;
                    default:
                        Debug.WriteLine("Ignoring nodetype: " + reader.NodeType);
                        reader.Read();
                        break;
                }
            }
        }

        /// <summary>
        /// Write xml string to xml writer
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="writer"></param>
        public static void StringToXmlWriter(string xml, XmlWriter writer)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xml));
            try
            {
                WriteThroughFirstElement(reader, writer);
            }
            finally
            {
                reader.Close();
            }
        }

#if !SILVERLIGHT
        /// <summary>
        /// Get xml string from xml file
        /// </summary>
        /// <param name="inFile"></param>
        /// <returns></returns>
        public static string GetStringFromXmlFile(FileInfo inFile)
        {
            return GetStringFromXmlFile(new FileSystem.FileInfo(inFile));
        }
#endif

        /// <summary>
        /// Get xml string from xml file
        /// </summary>
        /// <param name="inFile"></param>
        /// <returns></returns>
        internal static string GetStringFromXmlFile(FileSystem.FileInfo inFile)
        {
            XmlReader rdr = XmlReader.Create(inFile.FullName);
            rdr.MoveToContent();
            try
            {
                return rdr.ReadOuterXml();
            }
            finally
            {
                rdr.Close();
            }
        }

        [Obfuscation]
        internal static Regex HttpPath = new Regex(@"^https{0,1}://", RegexOptions.IgnoreCase);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use string overload for xmlFile arg")]
        internal static void FileToXmlWriter(FileSystem.FileInfo xmlFile, XmlWriter writer)
        {
            FileToXmlWriter(xmlFile.FullName, writer);
        }

        /// <summary>
        /// Write xml file to xml writer
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="writer"></param>
        public static void FileToXmlWriter(string xmlFile, XmlWriter writer)
        {
            XmlReader reader;
            if (HttpPath.Match(xmlFile).Success)
            {
                reader = XmlReader.Create(xmlFile);
            }
            else
            {
                // Changed FileShare settings - Read for reading, None for writing (Case 7819) - 2007/08/27 (DJ) 
                Stream readStream = FileSystem.File.Open(xmlFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                reader = XmlReader.Create(readStream);
            }

            using (reader)
            {
                WriteThroughFirstElement(reader, writer);
            }
        }

        /// <overloads>
        /// Copy object (using Xml serialization)
        /// </overloads>
        /// <summary>
        /// Copy object (using Xml serialization)
        /// </summary>
        /// <param name="fromObject"></param>
        /// <returns></returns>
        public static object CopyObject(object fromObject)
        {
            return GetObjectFromXmlString(ObjectToXML(fromObject), fromObject.GetType());
        }

        /// <summary>
        /// Copy object using XML Serialization, specifying a target type.
        /// </summary>
        /// <param name="fromObject"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        /// <remarks>
        /// If targetType is an XmlInclude attribute of fromObject type, then that type is used for the returned object.
        /// </remarks>
        public static object CopyObject(object fromObject, Type targetType)
        {
            return CopyObject(fromObject, targetType,
                              false); //modifyNamespaceAttribs
        }

        /// <summary>
        /// Copy object using XML Serialization, specifying a target type, and whether to modify namespace attribs.
        /// </summary>
        /// <param name="fromObject"></param>
        /// <param name="targetType"></param>
        /// <param name="modifyNamespaceAttribs"></param>
        /// <returns></returns>
        /// <remarks>
        /// If targetType is an XmlInclude attribute of fromObject type, then that type is used for the returned object.
        /// </remarks>
        [Obfuscation]
        internal static object CopyObject(object fromObject, Type targetType, bool modifyNamespaceAttribs)
        {
            string xmlStr = ObjectToXML(fromObject);

            Type targetTypeToUse = targetType;
            if (!targetType.Equals(fromObject.GetType()))
            {
#pragma warning disable 618
                xmlStr = GetRenamespacedXmlStringFromXmlString(xmlStr, targetType, modifyNamespaceAttribs,
                                                               true, //useXmlIncludeInfo
                                                               ref targetTypeToUse);
#pragma warning restore 618
            }
            else if (modifyNamespaceAttribs)
            {
                throw new ApplicationException("modifyNamespaceAttribs true when source and dest types the same.");
            }
            try
            {
                return GetObjectFromXmlString(xmlStr, targetTypeToUse);
            }
            catch
            {
                Debug.WriteLine(xmlStr);
                throw;
            }
        }

        /// <summary>
        /// Copy object using XML Serialization, casting down to the specified target type.
        /// </summary>
        /// <param name="fromObject"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object CopyObjectCastingDownToBaseType(object fromObject, Type targetType)
        {
            string xmlStr = ObjectToXML(fromObject);

            Type dummy = targetType;
            if (!targetType.Equals(fromObject.GetType()))
            {
                const bool modifyNamespaceAttribs = false;
#pragma warning disable 618
                xmlStr = GetRenamespacedXmlStringFromXmlString(xmlStr, targetType, modifyNamespaceAttribs,
                                                               false, //useXmlIncludeInfo
                                                               ref dummy);
#pragma warning restore 618
            }
            try
            {
                return GetObjectFromXmlString(xmlStr, targetType);
            }
            catch
            {
                Debug.WriteLine(xmlStr);
                throw;
            }
        }

        /// <summary>
        /// Change namespacing of xml representation of a type in order to facilitate running xml deserializer on it.
        /// </summary>
        /// <remarks>
        /// <para>This currently assumes non-custom xml serialization (using the default XML serializer for the specified type). Either an "XmlRoot" or "XmlType" tag is required specifying the Namespace to be used.</para>
        /// <para>It also currently assumes that the input xml does not have namespace prefixes on the level 0 (xmlroot scenario) or level1 (xmltype scenario) elements.</para>
        /// </remarks>
        /// <param name="xmlIn">Reader open on the input xml.</param>
        /// <param name="xmlout">Writer open on the output xml.</param>
        /// <param name="destType">Type to be deserialized to <paramref name="xmlout"/>.</param>
        public static void GetRenamespacedXmlToWriter(XmlReader xmlIn, XmlWriter xmlout, Type destType)
        {
            string destNsToUse = null;

            foreach (Attribute locAttr in destType.GetCustomAttributes(true))
            {
                if (locAttr.GetType().Equals(typeof(XmlRootAttribute)))
                {
                    XmlRootAttribute specAttr = (XmlRootAttribute)locAttr;
                    if (specAttr.Namespace != null && specAttr.Namespace.Length > 0)
                    {
                        destNsToUse = specAttr.Namespace;
                        break;
                    }
                }
                if (locAttr.GetType().Equals(typeof(XmlTypeAttribute)))
                {
                    XmlTypeAttribute specAttr = (XmlTypeAttribute)locAttr;
                    if (specAttr.Namespace != null && specAttr.Namespace.Length > 0)
                    {
                        destNsToUse = specAttr.Namespace;
                        break;
                    }
                }
            }

            if (destNsToUse == null)
            {
                throw new Exception("No XmlType or XmlRoot attrib found on destination type: " + destType.Name);
            }

            XDocument srcDoc = XDocument.Load(xmlIn);

            //e.g. <diffgr:diffgram xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:diffgr="urn:schemas-microsoft-com:xml-diffgram-v1" />
            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(srcDoc.NameTable());
            //nsmgr.AddNamespace("diffgr", "urn:schemas-microsoft-com:xml-diffgram-v1");
            //nsmgr.AddNamespace("msdata", "urn:schemas-microsoft-com:xml-msdata");

            XName hasChangesAttributeName = XName.Get("hasChanges", "urn:schemas-microsoft-com:xml-diffgram-v1");
            XName diffGramName = XName.Get("diffgram", "urn:schemas-microsoft-com:xml-diffgram-v1");

            foreach (XElement element in srcDoc.Descendants(diffGramName))
            {
                XElement dataSetElement = element.Descendants().FirstOrDefault();
                if (dataSetElement != null)
                {
                    foreach (XElement childElement in dataSetElement.Descendants())
                    {
                        XAttribute xmlnsAttr = dataSetElement.Attribute("xmlns");
                        if (xmlnsAttr == null)
                        {
                            //Append empty-ns attrib
                            childElement.SetAttributeValue(XName.Get("xmlns"), string.Empty);
                        }
                        XAttribute hasChangesAttribute = childElement.Attribute(hasChangesAttributeName);
                        if (hasChangesAttribute != null)
                        {
                            hasChangesAttribute.Remove();
                        }
                    }
                }
            }

            if (srcDoc.Root != null)
            {
                XAttribute xmlnsAttr = srcDoc.Root.Attribute("xmlns");
                if (xmlnsAttr == null)
                {
                    srcDoc.Root.Name = XName.Get(srcDoc.Root.Name.LocalName, destNsToUse);
                }
            }

            foreach (XElement element in srcDoc.Descendants())
            {
                XAttribute xmlnsAttr = element.Attribute("xmlns");
                if (xmlnsAttr == null && string.IsNullOrEmpty(element.Name.NamespaceName))
                {
                    element.Name = XName.Get(element.Name.LocalName, destNsToUse);
                }
            }

            using (XmlReader reader = srcDoc.CreateReader())
            {
                reader.MoveToContent();
                xmlout.WriteNode(reader, false);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use GetRenamespacedXmlToWriter instead.")]
        internal static string GetRenamespacedXmlStringFromXmlString(string xmlStr, Type objectType,
                                                                   bool modifyNamespaceAttribs, bool useXmlIncludeInfo,
                                                                   ref Type objectTypeToUse)
        {
            string destNsToUse = string.Empty;
            const string srcNsToStrip = "http://tempuri.org/";

            string destTopName = objectType.Name;
            if (modifyNamespaceAttribs)
            {
                foreach (Attribute locAttr in objectType.GetCustomAttributes(true))
                {
                    if (locAttr.GetType().Equals(typeof(XmlRootAttribute)))
                    {
                        XmlRootAttribute specAttr = (XmlRootAttribute)locAttr;
                        if (specAttr.Namespace != null && specAttr.Namespace.Length > 0)
                        {
                            if (specAttr.ElementName != null && specAttr.ElementName.Length > 0)
                            {
                                destTopName = specAttr.ElementName;
                            }
                            destNsToUse = specAttr.Namespace;
                            break;
                        }
                    }
                    if (locAttr.GetType().Equals(typeof(XmlTypeAttribute)))
                    {
                        XmlTypeAttribute specAttr = (XmlTypeAttribute)locAttr;
                        if (specAttr.Namespace != null && specAttr.Namespace.Length > 0)
                        {
                            destNsToUse = specAttr.Namespace;
                            break;
                        }
                    }
                }
            }

            StringReader srdr = new StringReader(xmlStr);

            var xrdr = XmlReader.Create(srdr);
            try
            {
                StringWriter wr = new StringWriter();
                XmlWriter xwr = XmlWriter.Create(wr);
                try
                {
                    xrdr.MoveToContent();

                    int initDepth = xrdr.Depth;

                    bool isEmptyEl = xrdr.IsEmptyElement;
                    if (xrdr.Name != destTopName)
                    {
                        if (useXmlIncludeInfo)
                        {
                            foreach (Attribute locAttr in objectType.GetCustomAttributes(true))
                            {
                                if (locAttr.GetType().Equals(typeof(XmlIncludeAttribute)))
                                {
                                    XmlIncludeAttribute specAttr = (XmlIncludeAttribute)locAttr;
                                    if (specAttr.Type.Name == xrdr.Name)
                                    {
                                        destTopName = xrdr.Name;
                                        objectTypeToUse = specAttr.Type;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    xwr.WriteStartDocument();
                    xwr.WriteStartElement(destTopName);
                    for (int i = 0; i <= xrdr.AttributeCount - 1; i++)
                    {
                        xrdr.MoveToAttribute(i);
                        if (modifyNamespaceAttribs && xrdr.Name == "xmlns" && xrdr.Value == srcNsToStrip)
                        {
                            continue;
                        }
                        xwr.WriteAttributeString(xrdr.LocalName, xrdr.NamespaceURI, xrdr.Value);
                    }
                    if (!isEmptyEl)
                    {
                        xrdr.Read();
                        while (!xrdr.EOF)
                        {
                            if (xrdr.NodeType == XmlNodeType.Element)
                            {
                                int depth = xrdr.Depth;
                                bool innerIsEmptyEl = xrdr.IsEmptyElement;

                                if (modifyNamespaceAttribs && destNsToUse != string.Empty)
                                {
                                    xwr.WriteStartElement(xrdr.Prefix, xrdr.LocalName, xrdr.NamespaceURI);

                                    bool foundExistingXmlNs = false;
                                    for (int i = 0; i <= xrdr.AttributeCount - 1; i++)
                                    {
                                        xrdr.MoveToAttribute(i);
                                        if (xrdr.Name == "xmlns")
                                        {
                                            foundExistingXmlNs = true;
                                        }
                                        xwr.WriteAttributeString(xrdr.Prefix, xrdr.LocalName, xrdr.NamespaceURI,
                                                                 xrdr.Value);
                                    }
                                    if (!foundExistingXmlNs)
                                    {
                                        xwr.WriteAttributeString("xmlns", destNsToUse);
                                    }
                                    if (!innerIsEmptyEl)
                                    {
                                        xrdr.Read();
                                        while (!xrdr.EOF)
                                        {
                                            xrdr.MoveToContent();
                                            if (xrdr.Depth <= depth)
                                            {
                                                break;
                                            }
                                            xwr.WriteNode(xrdr, false);
                                        }
                                    }
                                    else
                                    {
                                        xwr.WriteString(xrdr.Value);
                                        xwr.WriteEndElement();
                                        xrdr.Read();
                                        xrdr.MoveToContent();
                                    }
                                    if (depth <= xrdr.Depth && xrdr.NodeType == XmlNodeType.EndElement)
                                    {
                                        xwr.WriteEndElement();
                                        xrdr.ReadEndElement();
                                    }
                                }
                                else
                                {
                                    xwr.WriteNode(xrdr, false);
                                }
                                xrdr.MoveToContent();
                                if (xrdr.Depth <= initDepth && xrdr.NodeType == XmlNodeType.EndElement)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                xrdr.MoveToContent();
                                if (xrdr.Depth <= initDepth && xrdr.NodeType == XmlNodeType.EndElement)
                                {
                                    break;
                                }
                                if (xrdr.Depth == initDepth && xrdr.NodeType == XmlNodeType.EndElement)
                                {
                                    xrdr.ReadEndElement();
                                    xwr.WriteEndElement();
                                }
                                xrdr.Read();
                            }
                        }
                        xwr.WriteEndElement();
                        xwr.Flush();
                        xmlStr = wr.ToString();
                    }
                }
                finally
                {
                    xwr.Close();
                }
            }
            finally
            {
                xrdr.Close();
            }

            return xmlStr;
        }

        /// <summary>
        /// Get object from XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static object GetObjectFromXmlString(string xml, Type objectType)
        {
            return GetObjectFromXmlString(xml, objectType,
                                          false); //throwIfInvalidItems
        }

        //Experimental - intended for internal use only
        [Obfuscation]
        internal static object GetObjectFromXmlString(string xml, Type objectType, bool throwIfInvalidItems)
        {
            StringReader rdr = new StringReader(xml);
            try
            {
                var xrdr = XmlReader.Create(rdr);
                try
                {
                    return GetObjectFromXmlReader(xrdr, objectType, throwIfInvalidItems);
                }
                catch (Exception ex)
                {
                    //Try again with formatted string, in order to get useful line# info
                    try
                    {
                        xml = FormatXmlString(xml, true); //useFormatting
                    }
                    catch (Exception)
                    {
                        //Must be invalid XML
                        throw new Exception("Invalid xml: " + ex.Message + " Xml: " + xml, ex);
                    }

                    //Try getting top-lev element name
                    StringReader rdr2 = new StringReader(xml);
                    var xrdr2 = XmlReader.Create(rdr2);
                    try
                    {
                        string topElName;
                        try
                        {
                            xrdr2.MoveToContent();
                            Debug.WriteLine(xml);
                            topElName = xrdr2.Name;
                        }
                        catch (Exception)
                        {
                            string xmlStart = string.Empty;
                            const int xmlStartLen = 10;
                            for (int i = 0; i < xmlStartLen && i < xml.Length; i++)
                            {
                                xmlStart += xml.Substring(i, 1);
                            }
                            if (xml.Length > xmlStartLen)
                            {
                                xmlStart += "...";
                            }

                            throw new Exception(String.Format("Invalid xml '{0}': {1} Xml: {2}", xmlStart, ex.Message, xml),  ex);
                        }

                        //Re-run from-reader - will give formatted line#s this time
                        try
                        {
                            return GetObjectFromXmlReader(xrdr2, objectType, throwIfInvalidItems);
                        }
                        catch (Exception ex2)
                        {
                            throw new Exception(string.Format("Invalid xml having top element '{0}': {1} Xml: {2}", topElName, ex2.Message, xml), ex2);
                        }
                    }
                    finally
                    {
                        xrdr2.Close();
                    }
                }
                finally
                {
                    xrdr.Close();
                }
            }
            finally
            {
                rdr.Close();
            }
        }

        /// <overloads>
        /// Get xml string from object instance
        /// </overloads>
        /// <summary>
        /// Get xml string from object instance, specifying whether to use formatting
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useFormatting"></param>
        /// <returns></returns>
        public static string ObjectToXML(object obj, bool useFormatting)
        {
            return (ObjectToXML(obj, useFormatting, false));
        }

        /// <overloads>
        /// Get xml string from object instance
        /// </overloads>
        /// <summary>
        /// Get xml string from object instance, specifying whether to use formatting
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="output"></param>
        /// <param name="useFormatting"></param>
        /// <returns></returns>
        public static void ObjectToStream(object obj, StreamWriter output, bool useFormatting)
        {
            ObjectToStream(obj, output, useFormatting, null);
        }

        public static string ObjectToXML(object obj, bool useFormatting, bool omitXmlDeclaration)
        {
            //using (CultureUtility.SwitchThreadCultureToInvariant())
            //{
                try
                {
                    StringBuilder sb = new StringBuilder();
                    XmlSerializer xs = new XmlSerializer(obj.GetType());
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.Encoding = Encoding.Unicode;
                    xws.Indent = useFormatting;
                    xws.OmitXmlDeclaration = omitXmlDeclaration;
                    xws.NewLineOnAttributes = useFormatting;
                    using (XmlWriter writer = XmlWriter.Create(sb, xws))
                    {
                        xs.Serialize(writer, obj);
                    }
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw Get2TopExMessages(String.Format("Error serializing type '{0}': {1}", obj.GetType().Name, ex.Message), ex);
                }
            //}
        }

        public static void ObjectToStream(object obj, XmlWriter wr, bool useFormatting, bool omitXmlDeclaration)
        {
            //using (CultureUtility.SwitchThreadCultureToInvariant())
            //{
                try
                {
                    XmlSerializer xs = new XmlSerializer(obj.GetType());
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.Encoding = Encoding.Unicode;
                    xws.Indent = useFormatting;
                    xws.OmitXmlDeclaration = omitXmlDeclaration;
                    xws.NewLineOnAttributes = useFormatting;
                    XmlWriter writer = XmlWriter.Create(wr, xws);
                    xs.Serialize(writer, obj);
                    writer.Flush();
                }
                catch (Exception ex)
                {
                    throw Get2TopExMessages(String.Format("Error serializing type '{0}': {1}", obj.GetType().Name, ex.Message), ex);
                }
            //}
        }

        [Obfuscation]
        internal static string ObjectToXML(object obj, params Type[] extraTypes)
        {
            //used in RegressionTester
            const bool useFormatting = true;
            //using (CultureUtility.SwitchThreadCultureToInvariant())
            //{
                try
                {
                    StringBuilder sb = new StringBuilder();
                    XmlSerializer xs = new XmlSerializer(obj.GetType());
                    XmlWriterSettings xws = new XmlWriterSettings
                    {
                        Encoding = Encoding.Unicode,
                        Indent = useFormatting,
                        OmitXmlDeclaration = false,
                        NewLineOnAttributes = useFormatting
                    };

                    using (XmlWriter xmlWr = XmlWriter.Create(sb, xws))
                    {
                        xs.Serialize(xmlWr, obj);
                    }
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw Get2TopExMessages(String.Format("Error serializing type '{0}': {1}", obj.GetType().Name, ex.Message), ex);
                }
            //}
        }

        /// <summary>
        /// Get xml string from object instance
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToXML(object obj)
        {
            return ObjectToXML(obj, false); //useFormatting
        }

        /// <overloads>
        /// Write out object instance to xml writer
        /// </overloads>
        /// <summary>
        /// Write out object instance to xml writer
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="o"></param>
        public static void ObjectToXmlWriter(XmlWriter writer, object o)
        {
            ObjectToXmlWriter(writer, o,
                              false, //stripDefaultNs
                              null); //nsToUse
        }

        /// <summary>
        /// Write out object instance to xml writer, specifying whether to replace ns with another
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="o"></param>
        /// <param name="nsToUse"></param>
        public static void ObjectToXmlWriter(XmlWriter writer, object o, string nsToUse)
        {
            ObjectToXmlWriter(writer, o, false, //stripDefaultNs
                nsToUse);
        }

        /// <summary>
        /// Write out object instance to xml writer, specifying whether to replace ns with another
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="o"></param>
        /// <param name="stripDefaultNs"></param>
        /// <param name="nsToUse"></param>
        public static void ObjectToXmlWriter(XmlWriter writer, object o, bool stripDefaultNs, string nsToUse)
        {
            try
            {
                if (stripDefaultNs)
                {
                    string xml = ObjectToXML(o);
                    XDocument xdoc = XDocument.Parse(xml);

                    if (xdoc.Root != null)
                    {
                        XAttribute xmlnsNode = xdoc.Root.Attribute("xmlns");
                        if (xmlnsNode == null)
                        {
                            xmlnsNode = new XAttribute("xmlns", string.Empty);
                            xdoc.Root.Add(xmlnsNode);
                        }

                        xmlnsNode.Value = nsToUse;
                    }
                    xdoc.WriteTo(writer);
                }
                else
                {
                    //using (CultureUtility.SwitchThreadCultureToInvariant())
                    //{
                        XmlSerializer xs = new XmlSerializer(o.GetType());
                        xs.Serialize(writer, o);
                        writer.Flush();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw Get2TopExMessages("ObjectToXmlWriter error : " + ex.Message, ex);
            }
        }

        /// <overloads>
        /// Write out object instance to stream
        /// </overloads>
        /// <summary>
        /// Write out object instance to stream
        /// </summary>
        /// <param name="output"></param>
        /// <param name="obj"></param>
        public static void ObjectToStream(object obj, StreamWriter output)
        {
            ObjectToStream(obj, output,
                              false, //stripDefaultNs
                              string.Empty); //nsToUse
        }

        /// <summary>
        /// Write out object instance to stream, specifying whether to replace ns with another
        /// </summary>
        /// <param name="output"></param>
        /// <param name="obj"></param>
        /// <param name="stripDefaultNs"></param>
        /// <param name="nsToUse"></param>
        public static void ObjectToStream(object obj, StreamWriter output, bool stripDefaultNs, string nsToUse)
        {
            try
            {
                if (stripDefaultNs)
                {
                    XDocument xdoc = new XDocument();
                    using (var writer = xdoc.CreateWriter())
                    {
                        ObjectToStream(obj, writer, false, false);
                    }

                    if (xdoc.Root != null)
                    {
                        XAttribute xmlnsNode = xdoc.Root.Attribute("xmlns");
                        if (xmlnsNode == null)
                        {
                            xmlnsNode = new XAttribute("xmlns", string.Empty);
                            xdoc.Root.Add(xmlnsNode);
                        }

                        xmlnsNode.Value = nsToUse;
                    }
                    XmlWriter xmlWriter = XmlWriter.Create(output);
                    xdoc.WriteTo(xmlWriter);
                }
                else
                {
                    //using (CultureUtility.SwitchThreadCultureToInvariant())
                    //{
                        XmlSerializer xs = new XmlSerializer(obj.GetType());
                        xs.Serialize(output, obj);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw Get2TopExMessages(String.Format("ObjectToStreamWriter error : {0}", ex.Message), ex);
            }
        }

        [Obfuscation]
        internal static object GetObjectFromXmlReaderStripDefaultNs(XmlReader reader, Type objectType, bool stripDefaultNs)
        {
            if (stripDefaultNs)
            {
                reader.MoveToContent();
                string topElName = reader.LocalName;
                bool isEmptyElement = reader.IsEmptyElement;
                int depth = reader.Depth;

                using (MemoryStream mem = new MemoryStream())
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Encoding = Encoding.Unicode };

#if !SILVERLIGHT
                    if (reader is XmlTextReader)
                    {
                        //Read past xml declaration (but not processing instructions) if present to set .Encoding
                        if (reader.NodeType == XmlNodeType.None)
                        {
                            reader.Read();
                        }
                        if (reader.NodeType == XmlNodeType.XmlDeclaration)
                        {
                            reader.Read();
                        }

                        Encoding enc = ((XmlTextReader)reader).Encoding;

                        if (enc != null)
                        {
                            settings.Encoding = enc;
                        }
                    }
#endif

                    XmlWriter wr = XmlWriter.Create(mem, settings);

                    wr.WriteStartDocument();
                    wr.WriteStartElement(reader.LocalName); //ASSUME blank ns for top el

                    //Write out all attribs except for xmlns= one
                    for (int i = 0; i < reader.AttributeCount; i++)
                    {
                        reader.MoveToAttribute(i);
                        if (reader.LocalName == "xmlns")
                        {
                            //skip
                            //Note: This is sometimes and possibly always not even found by reader - doing the wr.WriteStartElement() above with no ns appears to be alone sufficient.
                        }
                        else
                        {
                            wr.WriteAttributeString(reader.LocalName, reader.Value);
                            //ASSUME non-namespaced attribs on top-el
                        }
                    }

                    //Write out all sub-nodes
                    if (isEmptyElement)
                    {
                        reader.Read();
                    }
                    else
                    {
                        reader.Read();
                        while (reader.Depth > depth)
                        {
                            //TODO: Test with text content on root
                            reader.MoveToContent();
                            wr.WriteNode(reader,
                                         true); //defAttr
                            reader.MoveToContent();
                        }

                        reader.MoveToContent();
                        AssertNameCheckReadEndElement(topElName, reader);
                    }
                    wr.WriteEndElement(); //topElName
                    wr.Flush();

                    mem.Position = 0;
                    XmlReader rd = XmlReader.Create(mem);
                    return GetObjectFromXmlReader(rd, objectType);
                }
            }

            return GetObjectFromXmlReader(reader, objectType);
        }

        private static void AssertNameCheckReadEndElement(string name, XmlReader reader)
        {
            if (reader.NodeType != XmlNodeType.EndElement)
            {
                throw new Exception(String.Format("Expected EndElement named '{0}' but got element type '{1}' named '{2}'", name, reader.NodeType, reader.LocalName));
            }

            if (name != null && name != reader.LocalName)
            {
                throw new Exception(String.Format("Expected EndElement named '{0}' but got EndElement name '{1}'", name, reader.LocalName));
            }

            reader.ReadEndElement();
        }

        /// <summary>
        /// Get object instance from xml file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static object GetObjectFromFile(string filename, Type objectType)
        {
            return GetObjectFromFile(filename, objectType,
                                     false); //throwIfInvalidItems
        }

        //Experimental - intended for internal use only
        [Obfuscation]
        internal static object GetObjectFromFile(string filename, Type objectType, bool throwIfInvalidItems)
        {
            // Changed FileShare settings - Read for reading, None for writing (Case 7819) - 2007/08/27 (DJ) 
            using (Stream fs = FileSystem.File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var xtr = XmlReader.Create(fs);
                return GetObjectFromXmlReader(xtr, objectType, throwIfInvalidItems);
            }
        }

        [Obfuscation]
        internal static Exception Get2TopExMessages(string prefix, Exception ex)
        {
            

            string ret = String.Format("{0}: {1} ({2})", prefix, ex.Message, ex.GetType().Name);
            if (ex.InnerException != null)
            {
                ret += String.Format(" -> {0} ({1})", ex.InnerException.Message, ex.InnerException.GetType().Name);
            }
            return new Exception(ret, ex); //ex included in stack trace to ensure full stack trace preserved.
        }

        /// <summary>
        /// Get object instance from xml stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static object GetObjectFromStream(Stream stream, Type objectType)
        {
            return GetObjectFromStream(stream, objectType,
                                       false); //throwIfInvalidItems
        }

        //Experimental - intended for internal inrule use only
        [Obfuscation]
        internal static object GetObjectFromStream(Stream stream, Type objectType, bool throwIfInvalidItems)
        {
            //using (CultureUtility.SwitchThreadCultureToInvariant())
            //{
                XmlSerializer xs = new XmlSerializer(objectType);
                try
                {
                    // not all streams support seeking, if not, read forward only, throw if appropriate.
                    if (stream.CanSeek)
                    {
                        stream.Position = 0;
                    }

#if !SILVERLIGHT
                    if (throwIfInvalidItems)
                    {
                        xs.UnknownAttribute += xs_UnknownAttribute;
                        xs.UnknownElement += xs_UnknownElement;
                        xs.UnknownNode += xs_UnknownNode;
                        xs.UnreferencedObject += xs_UnreferencedObject;
                    }
#endif
                    return xs.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    throw Get2TopExMessages("Error deserializing", ex);
                }
                finally
                {
#if !SILVERLIGHT
                    xs.UnknownAttribute -= xs_UnknownAttribute;
                    xs.UnknownElement -= xs_UnknownElement;
                    xs.UnknownNode -= xs_UnknownNode;
                    xs.UnreferencedObject -= xs_UnreferencedObject;
#endif
                }
            //}
        }

        /// <summary>
        /// Get object instance from xml reader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static object GetObjectFromXmlReader(XmlReader reader, Type objectType)
        {
            return GetObjectFromXmlReader(reader, objectType,
                                          false); //throwIfInvalidItems
        }

        //Experimental - intended for internal inrule use only
        [Obfuscation]
        internal static object GetObjectFromXmlReader(XmlReader reader, Type objectType, bool throwIfInvalidItems)
        {
            return GetObjectFromXmlReader(reader, objectType, throwIfInvalidItems, null);
        }

        //Experimental - intended for internal inrule use only
        [Obfuscation]
        internal static object GetObjectFromXmlReader(XmlReader reader, Type objectType, bool throwIfInvalidItems, string defaultNamespace)
        {
            if (reader.Depth == 0 && reader.NodeType != XmlNodeType.Element)
            {
                reader.MoveToContent();
            }

            if (reader.NodeType != XmlNodeType.Element)
            {
                throw new Exception(String.Format("Expected Element nodetype but got '{0}'", reader.NodeType));
            }

            XmlSerializer xs = String.IsNullOrEmpty(reader.NamespaceURI) ?
                                new XmlSerializer(objectType) :
                                new XmlSerializer(objectType, reader.NamespaceURI);

            //using (CultureUtility.SwitchThreadCultureToInvariant())
            //{
                try
                {
#if !SILVERLIGHT
                    if (throwIfInvalidItems)
                    {
                        xs.UnknownAttribute += xs_UnknownAttribute;
                        xs.UnknownElement += xs_UnknownElement;
                        xs.UnknownNode += xs_UnknownNode;
                        xs.UnreferencedObject += xs_UnreferencedObject;
                    }
#endif

                    int initDepth = reader.Depth;
                    string startElName = reader.Name;
                    object ret = xs.Deserialize(reader);

                    while ((!reader.EOF) && reader.Depth > initDepth)
                    {
                        Debug.WriteLine(String.Format("Ignoring xml not consumed by XmlSerializer.Deserialize: {0}", reader.NodeType));

                        reader.Read();
                        reader.MoveToContent();
                    }

                    if ((!reader.EOF)
                        && reader.Depth == initDepth
                        && reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name != startElName)
                        {
                            throw new Exception(String.Format("EndElement expected '{0}' but got '{1}'", startElName, reader.Name));
                        }

                        reader.ReadEndElement();
                        reader.MoveToContent();
                    }

                    return ret;
                }
                catch (Exception ex)
                {
                    throw Get2TopExMessages("Error deserializing", ex);
                }
                finally
                {
#if !SILVERLIGHT
                    xs.UnknownAttribute -= xs_UnknownAttribute;
                    xs.UnknownElement -= xs_UnknownElement;
                    xs.UnknownNode -= xs_UnknownNode;
                    xs.UnreferencedObject -= xs_UnreferencedObject;
#endif
                }
            //}
        }

#if !SILVERLIGHT
        private static void xs_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            throw new Exception("UnreferencedObject during deserialization: " + e.UnreferencedId); //xml
        }

        private static void xs_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            /*
            if (e.LocalName == "type"
                && e.NamespaceURI == RuleRepositoryConstants.XmlSchemaInstanceNamespace)
            {
                return;
            }

            if (e.LocalName == "Namespace"
                && e.NamespaceURI == RuleRepositoryConstants.IrXmlSchemaNamespace)
            {
                return;
            }*/

            throw new Exception("UnknownNode during deserialization: " +
                                             
                                                     String.Format("{0},{1} {2}: {3} xmlns='{4}'", e.LineNumber, e.LinePosition, e.ObjectBeingDeserialized.GetType().Name, e.LocalName, e.NamespaceURI)
                                            ,
                                             null); //xml
        }

        private static void xs_UnknownElement(object sender, XmlElementEventArgs e)
        {
            /*if (e.Element.LocalName == "Namespace"
                && e.Element.NamespaceURI == RuleRepositoryConstants.IrXmlSchemaNamespace)
            {
                return;
            }*/


            throw new Exception("UnknownElement during deserialization: " + 
                                             
                                                     String.Format("{0},{1} {2}: {3} xmlns='{4}' expected: {5}", e.LineNumber, e.LinePosition, e.ObjectBeingDeserialized.GetType().Name, e.Element.LocalName, e.Element.NamespaceURI, e.ExpectedElements)
                                                 ,
                                             null); //xml
        }

        private static void xs_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            /*if (e.Attr.LocalName == "type"
                && e.Attr.NamespaceURI == RuleRepositoryConstants.XmlSchemaInstanceNamespace)
            {
                return;
            }*/

            throw new Exception("UnknownAttribute during deserialization: " +
                                                                                                  String.Format("{0},{1}: {2} {3} xmlns='{4}' expected: {5}", e.LineNumber, e.LinePosition, e.ObjectBeingDeserialized.GetType().Name, e.Attr.LocalName, e.Attr.NamespaceURI, e.ExpectedAttributes)
                                                 ,
                                             null); //xml
        }
#endif

        /// <summary>
        /// Save xml reader stream to file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="xrdr"></param>
        public static void SaveXmlReaderToFile(string filename, XmlReader xrdr)
        {
            string toDir = Path.GetDirectoryName(filename);
            if (!String.IsNullOrEmpty(toDir) && !Directory.Exists(toDir))
            {
                Directory.CreateDirectory(toDir);
            }

            long length;
            // Changed FileShare settings - Read for reading, None for writing (Case 7819) - 2007/08/27 (DJ) 
            using (Stream fs = FileSystem.File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                const bool useFormatting = true;
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.Encoding = Encoding.UTF8; //Use UTF8 for writing out to file
                xws.Indent = useFormatting;
                xws.NewLineOnAttributes = useFormatting;
                XmlWriter xmlWr = XmlWriter.Create(fs, xws);

                try
                {
                    //Always write start doc for file output
                    xmlWr.WriteStartDocument();
                    while (!xrdr.EOF)
                    {
                        switch (xrdr.NodeType)
                        {
                            case XmlNodeType.ProcessingInstruction:
                                xmlWr.WriteProcessingInstruction(xrdr.Name, xrdr.Value);
                                xrdr.Read();
                                break;
                            case XmlNodeType.Element:
                                xmlWr.WriteNode(xrdr, true); //defAttr
                                break;
                            default:
                                Debug.WriteLine("Ignoring nodetype: " + xrdr.NodeType);
                                xrdr.Read();
                                break;
                        }
                    }

                    xmlWr.Flush();
                    length = fs.Length;
                }
                finally
                {
                    xmlWr.Close();
                }
            }

            VerifyFileWrittenToFileSystem(filename, length);
        }

        [Obfuscation]
        internal static void VerifyFileWrittenToFileSystem(string filename, long expectedLen)
        {
            DateTime tmr = DateTime.Now;
            const int saveWaitTimeMs = 20000;
            const int retrySleepMs = 100;
            while (true)
            {
                //Ensure file saved properly (since have seen at least one case where file not written all the way out)
                try
                {
                    long actualFileLen = new FileSystem.FileInfo(filename).Length;
                    if (actualFileLen < expectedLen)
                    {
                        throw new ApplicationException("File was not successfully written: Expected length " +
                                                       expectedLen
                                                       + " but file length is: " + actualFileLen);
                    }

                    break;
                }
                catch (ApplicationException)
                {
                    throw;
                }
                catch (Exception)
                {
                    Thread.Sleep(retrySleepMs);
                    //will loop till no throw or timeout
                }
                if ((DateTime.Now.Subtract(tmr)).TotalMilliseconds > saveWaitTimeMs)
                {
                    throw new ApplicationException("Saved file did not appear in filesystem after " + saveWaitTimeMs +
                                                   "ms: " + filename);
                }
            }
        }

        /// <summary>
        /// Save object instance to xml file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="obj"></param>
        public static void SaveObjectToFile(string filename, object obj)
        {
            //using (CultureUtility.SwitchThreadCultureToInvariant())
            //{
                try
                {
                    string toDir = Path.GetDirectoryName(filename);
                    if (!String.IsNullOrEmpty(toDir) && !Directory.Exists(toDir))
                    {
                        Directory.CreateDirectory(toDir);
                    }

                    long length;
                    XmlSerializer xs = new XmlSerializer(obj.GetType());

                    // Changed FileShare settings - Read for reading, None for writing (Case 7819) - 2007/08/27 (DJ) 
                    using (Stream fs = FileSystem.File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        const bool useFormatting = true;
                        XmlWriterSettings xws = new XmlWriterSettings();
                        xws.Encoding = Encoding.UTF8; //Use UTF8 for writing out to file
                        xws.Indent = useFormatting;
                        xws.NewLineOnAttributes = useFormatting;
                        using (XmlWriter xmlWr = XmlWriter.Create(fs, xws))
                        {
                            xs.Serialize(xmlWr, obj);
                            xmlWr.Flush();
                            length = fs.Length;
                        }
                    }

                    VerifyFileWrittenToFileSystem(filename, length);
                }
                catch (Exception ex)
                {
                    throw Get2TopExMessages("SaveObjectToFile error : " + ex.Message, ex);
                }
            //}
        }

        /// <summary>
        /// Save object instance to stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        public static void SaveObjectToStream(Stream stream, object obj)
        {
            //using (CultureUtility.SwitchThreadCultureToInvariant())
            //{
                try
                {
                    XmlSerializer xs = new XmlSerializer(obj.GetType());
                    const bool useFormatting = true;
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.Encoding = Encoding.UTF8; //Use UTF8 for writing out to file
                    xws.Indent = useFormatting;
                    xws.NewLineOnAttributes = useFormatting;
                    XmlWriter xmlWr = XmlWriter.Create(stream, xws);
                    xs.Serialize(xmlWr, obj);
                    xmlWr.Flush();
                }
                catch (Exception ex)
                {
                    throw Get2TopExMessages("SaveObjectToStream error : " + ex.Message, ex);
                }
            //}
        }

        /// <summary>
        /// Save xml string to xml file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="xml"></param>
        public static void SaveXmlStringToFile(string filename, string xml)
        {
            StringReader rdr = new StringReader(xml);
            var xrdr = XmlReader.Create(rdr, GetIgnoreWhitespaceReaderSettings());
            try
            {
                SaveXmlReaderToFile(filename, xrdr);
            }
            finally
            {
                xrdr.Close(); //Note no rdr.Close() req'd since it is closed by this.
            }
        }

        /// <summary>
        /// Get xml string from xml file, optionally specifying indented formatting
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="useFormatting"></param>
        /// <returns></returns>
        public static string FormatXmlStringFromFile(string filePath, bool useFormatting)
        {
            // Use FileShare.ReadWrite to allow reading files open for write but shareable
            // Changed FileShare settings: Read for reading, None for writing. (Case 7819) - 2007/08/27 (DJ) 
            using (Stream readStream = FileSystem.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var reader = XmlReader.Create(readStream);
                using (MemoryStream mem = new MemoryStream())
                {
                    WriteMemStreamFromReader(reader, mem);

                    mem.Position = 0;
                    XmlReader outRdr = XmlReader.Create(mem);

                    StringWriter stringWriter = new StringWriter();
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.Encoding = Encoding.Unicode; //Use Unicode for writing out to file
                    xws.Indent = useFormatting;
                    xws.NewLineOnAttributes = useFormatting;
                    XmlWriter outWriter = XmlWriter.Create(stringWriter, xws);

                    //Don't write start doc for string output
                    try
                    {
                        WriteThroughFirstElement(outRdr, outWriter);
                    }
                    finally
                    {
                        outWriter.Close();
                    }

                    return stringWriter.ToString();
                }
            }
        }

#if !SILVERLIGHT
        /// <summary>
        /// Persist an xml file as formatted or unformatted as specified, from another xml file
        /// </summary>
        /// <param name="fromFile"></param>
        /// <param name="toFile"></param>
        /// <param name="useFormatting"></param>
        public static void FormatXmlStringFromFileToFile(FileInfo fromFile, FileInfo toFile, bool useFormatting)
        {
            FormatXmlStringFromFileToFile(new FileSystem.FileInfo(fromFile), new FileSystem.FileInfo(toFile),
                                          useFormatting);
        }
#endif

        /// <summary>
        /// Persist an xml file as formatted or unformatted as specified, from another xml file
        /// </summary>
        /// <param name="fromFile"></param>
        /// <param name="toFile"></param>
        /// <param name="useFormatting"></param>
        internal static void FormatXmlStringFromFileToFile(FileSystem.FileInfo fromFile, FileSystem.FileInfo toFile, bool useFormatting)
        {
            // Use FileShare.ReadWrite to allow reading files open for write but shareable
            // Changed FileShare settings: Read for reading, None for writing. (Case 7819) - 2007/08/27 (DJ) 
            Stream readStream = FileSystem.File.Open(fromFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var reader = XmlReader.Create(readStream);
            try
            {
                XmlWriter writer = null;
                // Changed FileShare settings: Read for reading, None for writing. (Case 7819) - 2007/08/27 (DJ) 
                Stream outFs = FileSystem.File.Open(toFile.FullName, FileMode.Create, FileAccess.Write, FileShare.None);
                try
                {
#if !SILVERLIGHT
                    if (reader is XmlTextReader)
                    {
                        //Read past xml declaration (but not processing instructions) if present to set .Encoding
                        bool exitWhile = false;
                        while (!(reader.EOF | exitWhile))
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.XmlDeclaration:
                                    reader.Read();
                                    break;
                                case XmlNodeType.ProcessingInstruction:
                                case XmlNodeType.Element:
                                    exitWhile = true;
                                    break;
                                case XmlNodeType.None:
                                case XmlNodeType.Whitespace:
                                    reader.Read();
                                    break;
                                //case XmlNodeType.CDATA:
                                //case XmlNodeType.Comment:
                                default:
                                    Debug.WriteLine("Ignoring node type at doc start: " + reader.NodeType);
                                    reader.Read();
                                    break;
                            }
                        }

                        Encoding enc = ((XmlTextReader)reader).Encoding;
                        if (enc != null)
                        {
                            XmlWriterSettings xws = new XmlWriterSettings();
                            xws.Encoding = enc;
                            xws.Indent = useFormatting;
                            xws.NewLineOnAttributes = useFormatting;
                            writer = XmlWriter.Create(outFs, xws);
                        }
                    }
#endif

                    if (writer == null)
                    {
                        //ASSUME UTF8 from file if couldn't infer encoding above
                        XmlWriterSettings xws = new XmlWriterSettings();
                        xws.Encoding = Encoding.UTF8; //Use UTF8 for writing out to file
                        xws.Indent = useFormatting;
                        xws.NewLineOnAttributes = useFormatting;
                        writer = XmlWriter.Create(outFs, xws);
                    }

                    //Always write start doc for file output
                    writer.WriteStartDocument();
                    WriteThroughFirstElement(reader, writer);
                }

                finally
                {
                    if (writer != null)
                    {
                        writer.Close(); //closes underlying stream
                    }
                    else if (outFs != null)
                    {
                        outFs.Dispose();
                    }
                }
            }
            finally
            {
                reader.Close(); //closes underlying stream
            }
        }

        [Obfuscation]
        internal static void WriteMemStreamFromReader(XmlReader reader, MemoryStream outMem)
        {
#if !SILVERLIGHT
            if (reader is XmlTextReader)
            {
                //Read past xml declaration (but not processing instructions) if present to set .Encoding
                bool exitWhile = false;
                while (!(reader.EOF | exitWhile))
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.XmlDeclaration:
                            reader.Read();
                            break;
                        case XmlNodeType.ProcessingInstruction:
                        case XmlNodeType.Element:
                            exitWhile = true;
                            break;
                        case XmlNodeType.Whitespace:
                        case XmlNodeType.None:
                            reader.Read();
                            break;
                        //case XmlNodeType.CDATA:
                        //case XmlNodeType.Comment:
                        default:
                            Debug.WriteLine("Ignoring node type at doc start: " + reader.NodeType);
                            reader.Read();
                            break;
                    }
                }

                Encoding enc = ((XmlTextReader)reader).Encoding;
                if (enc != null)
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Encoding = enc };
                    XmlWriter wr = XmlWriter.Create(outMem, settings);
                    wr.WriteStartDocument();
                    wr.WriteNode(reader, true); //defAttr
                    wr.Flush();
                }
            }
#endif
            //If above not successful in inferring encoding, fall back to xmldocument read
            if (outMem.Length != 0) return;

            //Reader encoding unknown, load encoding-safely using xmldocument then write out
            var xdoc = XDocument.Load(reader);

            XmlWriter writer = XmlWriter.Create(outMem, new XmlWriterSettings { Encoding = Encoding.UTF8 });
            xdoc.WriteTo(writer);
            writer.Flush();
        }

        [Obfuscation]
        internal static void PersistObjectAsXml(object obj, Stream destination, Encoding encoding, bool omitXmlDeclaration, bool indent, bool useInvariantCulture, XmlSerializerNamespaces namespaces)
        {
            PersistObjectAsXml(obj, new StreamWriter(destination, encoding), encoding, omitXmlDeclaration, indent, useInvariantCulture, namespaces);
        }

        [Obfuscation]
        internal static void PersistObjectAsXml(object obj, TextWriter destination, Encoding encoding, bool omitXmlDeclaration, bool indent, bool useInvariantCulture, XmlSerializerNamespaces namespaces)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            CultureInfo initialCulture = Thread.CurrentThread.CurrentCulture;

            // Configure writer settings
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = indent;
            settings.Encoding = encoding;
            settings.OmitXmlDeclaration = omitXmlDeclaration;

            try
            {
                // Set up optional invariant culture
                if (useInvariantCulture) Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                // Serialize
                XmlWriter writer = XmlWriter.Create(destination, settings);
                if (namespaces != null)
                {
                    xs.Serialize(writer, obj, namespaces);
                }
                else
                {
                    xs.Serialize(writer, obj);
                }

                // Only flush underlying stream, don't close it - Calling method is responsible for this
                writer.Flush();
            }
            catch (Exception ex)
            {
                throw Get2TopExMessages(String.Format("Error serializing type '{0}': {1}", obj.GetType().Name, ex.Message), ex);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = initialCulture;
            }
        }

        private static XmlReaderSettings GetIgnoreWhitespaceReaderSettings()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            return settings;
        }

        [Obfuscation]
        internal static object DepersistObjectFromXml(Type type, TextReader source, bool useInvariantCulture, XmlSchemaSet validationSchemas)
        {
            XmlSerializer xs = new XmlSerializer(type);
            CultureInfo initialCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                // Set up optional invariant culture
                if (useInvariantCulture) Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                // Deserialize
                XmlReaderSettings settings = new XmlReaderSettings();
#if !SILVERLIGHT
                List<string> validationErrors = new List<string>();
                if (validationSchemas != null)
                {
                    settings.Schemas = validationSchemas;
                    settings.ValidationType = ValidationType.Schema;
                    settings.ValidationEventHandler += (sender, e) => validationErrors.Add(e.Message);
                }
#endif
                XmlReader reader = XmlReader.Create(source, settings);
                object obj = xs.Deserialize(reader);
#if !SILVERLIGHT
                if (validationErrors.Count > 0)
                {
                    throw new Exception(String.Format("XML for '{0}' does not validate against the schema provided. {1}", type.Name,validationErrors.ToArray() ));
                }
#endif
                return (obj);
            }
            catch (Exception ex)
            {
                throw Get2TopExMessages(String.Format("Error deserializing type '{0}': {1}", type.Name, ex.Message), ex);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = initialCulture;
            }
        }

#if !SILVERLIGHT
        //Not silverlight compatible...
        [Obfuscation]
        internal static XmlNameTable NameTable(this XDocument xdoc)
        {
            return xdoc.CreateNavigator().NameTable;
        }

        [Obfuscation]
        internal static XmlNamespaceManager NamespaceManager(this XDocument xdoc)
        {
            return new XmlNamespaceManager(xdoc.NameTable());
        }
#endif
    }
}

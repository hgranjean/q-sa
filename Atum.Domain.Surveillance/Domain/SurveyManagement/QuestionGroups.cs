using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class QuestionGroups : SortedList<int, QuestionGroup>, IXmlSerializable
    {   
        public QuestionGroups()
        {}
        
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(int));
            var valueSerializer = new XmlSerializer(typeof(QuestionGroup));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                var key = (int)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                var value = (QuestionGroup)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(int));
            var valueSerializer = new XmlSerializer(typeof(QuestionGroup));

            foreach (int key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                var value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion

        public QuestionGroup AddOrUpdate(int number, QuestionGroup questionGroup)
        {
            if (this.ContainsKey(number))
            {
                this[number] = questionGroup;
            }
            else
            {
                this.Add(number, questionGroup);
            }

            return questionGroup;
        }
    }
}

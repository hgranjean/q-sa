using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Atum.Domain.NLP.NaiveBayes
{
    /// <summary>
    /// Represent a Training Document in a Training Set
    /// </summary>
    [Serializable()]
    public class TrainingDocument
    {
        /// <summary>
        /// Empty Constructor for (De)Serialization
        /// </summary>
        public TrainingDocument()
        {
            //TODO: Add default Tokenizer
        }

        /// <summary>
        /// Initial Training Document Contsturctor
        /// </summary>
        /// <param name="docClass"></param>
        /// <param name="docText"></param>
        /// <param name="tokenizer"></param>
        public TrainingDocument(string docClass, string docText, Tokenizer tokenizer)
        {
            this.Class = docClass;
            this.Text = docText;
            this.Words = new List<string>(tokenizer.Tokenize(docText));

            HasWords = this.Words.Count > 0;

            init();
        }

        internal void init() 
        {
            WordFrequency = calucalteWordFrequency(Words); ;
        
        }

        private Dictionary<string, int> calucalteWordFrequency(List<string> words)
        {
            Dictionary<string, int> retVal = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (retVal.ContainsKey(word))
                {
                    retVal[word]++;
                }
                else
                {
                    retVal.Add(word, 1);
                }
            }

            return retVal;
        }
        //TODO: Investigate Serializable Dictionary. See Survey
        [XmlIgnore]
        public Dictionary<string, int> WordFrequency { get; set; }

        public SerializableKV<string, int>[] WordFrequencySerializable
        {
            get
            {
                var list = new List<SerializableKV<string, int>>();
                if (WordFrequency != null)
                {
                    list.AddRange(WordFrequency.Keys.Select(key => new SerializableKV<string, int>() { Key = key, Value = WordFrequency[key] }));
                }
                return list.ToArray();
            }
            set
            {
                WordFrequency = new Dictionary<string,int>();
                foreach (var item in value)
                {
                    WordFrequency.Add(item.Key, item.Value);
                }
            }
        }
        
        public string Class { get; set; }
        public string Text { get; set; }
        public List<string> Words { get; set; }
        public bool HasWords { get; set; }

        public string Id { get; set; }
    }
}

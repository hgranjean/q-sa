using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.NLP.Domain.NLP.NaiveBayes
{
    public class ClassDocument
    {
        public ClassDocument(string docText, Tokenizer tokenizer) : this("",docText,tokenizer)
        {
        }

        public ClassDocument(string docClass, string docText, Tokenizer tokenizer)
        {
            this.Class = docClass;
            this.Text = docText;
            this.Words = new List<string>(tokenizer.Tokenize(docText)) ;
            
            HasWords = this.Words.Count > 0;

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


        public Dictionary<string, int> WordFrequency { get; private set; }
        public string Class { get; set; }
        public string Text { get; private set; }
        public List<string> Words { get; private set; }
        public bool HasWords { get; set; }
    }
}

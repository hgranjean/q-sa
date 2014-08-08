using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atum.Domain.NLP.Domain.NLP;
using System.IO;

namespace Atum.Domain.NLP
{
    public class Tokenizer
    {
        private List<string> excludedWords;
        private string _modelPath;// = @"C:\Atum Technology Group\Projects\englishparsing_net2_0_src\ToolsExample\Models\";
        private OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer mTokenizer;

        private string[] TokenizeSentence(string sentence)
        {

            string modelPath = EnglishTokenizerModelPath;

            if (mTokenizer == null)
            {
                mTokenizer = new OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer(EnglishTokenizerModelPath);
            }

            return mTokenizer.Tokenize(sentence);
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="excludedWords"></param>
        public Tokenizer(List<string> excludedWords, string modelPath)
        {
            this._modelPath = modelPath;
            this.EnglishTokenizerModelPath = Path.Combine(_modelPath, "EnglishTok.nbin");
            this.excludedWords = excludedWords;
        }

        internal string[] Tokenize(string observation)
        {

            //remove numbers and punctuation
            observation = Regex.Replace(observation, "\\.|;|:|,|[0-9]|’", "");

            string[] observationTokens = TokenizeSentence(observation);
            string[] observationPOSTags = new WordPOS(this._modelPath).PosTagTokens(observationTokens);
            int length = observationTokens.Length;


            string[] retVal = null;
            List<string> words = new List<string>();

            for (int i = 0; i < length; i++)
            {
                string word = observationTokens[i];
                string posTag = observationPOSTags[i];

                PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
                if (ps.IsPlural(word))
                {
                    word = ps.Singularize(word);
                }
                bool excludedWord = isExcluded(word,posTag); ;

                if (!excludedWord) { words.Add(word); }

            }





            ////create collection of words
            //var wordCollection = Regex.Matches(observation, @"[\w]+");

            ////calculate word frequencies
            ////var dict = new Dictionary<string, int>();
            //for (int i = 0; i < wordCollection.Count; i++)
            //{
            //    string word = wordCollection[i].Value.ToLower();

            //    PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
            //    if (ps.IsPlural(word))
            //    {
            //        word = ps.Singularize(word);
            //    }

            //    bool excludedWord = isExcluded(word); ;

            //    if (!excludedWord) { words.Add(word); }

            //}
            retVal = words.ToArray();

            return retVal;
        }

        private bool isExcluded(string word, string posTag)
        {
            bool retVal = excludedWords.Contains(word);
            WordPOS.POS wPOS = WordPOS.WordPos(posTag);

            switch (wPOS)
            {
                case WordPOS.POS.Adjective:
                    break;
                case WordPOS.POS.Noun:
                    break;
                case WordPOS.POS.Adverb:
                    break;
                case WordPOS.POS.Verb:
                    break;
                case WordPOS.POS.Pronoun:
                    retVal |= true;
                    break;
                case WordPOS.POS.Conjunction:
                    retVal |= true;
                    break;
                case WordPOS.POS.Preposition:
                    retVal |= true;
                    break;
                //a word added to a sentence to convey an emotion or a sentiment 
                case WordPOS.POS.Interjection:
                    retVal |= true;
                    break;
                case WordPOS.POS.Idiom:
                    break;
                case WordPOS.POS.Other:
                    retVal |= true;
                    break;
                default:
                    break;
            }
            return retVal;
        }


        public string EnglishTokenizerModelPath { get; set; }
    }
}

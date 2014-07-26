using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atum.Domain.NLP.Domain.NLP;

namespace Atum.Domain.NLP
{
    public class Tokenizer
    {
        private List<string> excludedWords;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="excludedWords"></param>
        public Tokenizer(List<string> excludedWords)
        {
            this.excludedWords = excludedWords;
        }

        internal string[] Tokenize(string observation)
        {
            string[] retVal = null;
            List<string> words = new List<string>();

            //remove numbers and punctuation
            observation = Regex.Replace(observation, "\\.|;|:|,|[0-9]|’", "");

            //create collection of words
            var wordCollection = Regex.Matches(observation, @"[\w]+");

            //calculate word frequencies
            //var dict = new Dictionary<string, int>();
            for (int i = 0; i < wordCollection.Count; i++)
            {
                string word = wordCollection[i].Value.ToLower();

                PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
                if (ps.IsPlural(word))
                {
                    word = ps.Singularize(word);
                }

                bool excludedWord = isExcluded(word); ;

                if (!excludedWord) { words.Add(word); }

            }
            retVal = words.ToArray();

            return retVal;
        }

        private bool isExcluded(string word)
        {
            bool retVal = excludedWords.Contains(word);
            WordPOS.POS wPOS = WordPOS.WordPos(word);

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

    }
}

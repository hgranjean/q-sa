using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace Atum.Domain.NLP.Domain.NLP
{
    public class WordPOS : IDisposable
    {
        public enum POS
        {
            Adjective,
            Noun,
            Adverb,
            Verb,
            Pronoun,
            Conjunction,
            Preposition,
            Interjection,
            Idiom,
            Other

        }

        public WordPOS()
        {
        }


        static Word.Application WordApp = new Word.Application();

        static public WordPOS.POS WordPos(string word)
        {
            WordPOS.POS retVal = WordPOS.POS.Other;
            var synInfo = WordApp.SynonymInfo[word, Word.WdLanguageID.wdEnglishUS];

            if (synInfo.Found && synInfo.MeaningCount > 0)
            {
                var synInfoMeaningList = synInfo.MeaningList as Array;
                var synInfoPartsOfSpeechList = synInfo.PartOfSpeechList as Array;

                retVal = (synInfo.MeaningCount > 0) ? (WordPOS.POS)synInfoPartsOfSpeechList.GetValue(1) : WordPOS.POS.Other;
            }

            return retVal;
        }

        public void Dispose()
        {
            ((Word._Application)WordApp).Quit();
        }
    }
}

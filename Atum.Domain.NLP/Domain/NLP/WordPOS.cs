using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Word = Microsoft.Office.Interop.Word;

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

        public WordPOS(string modelPath)
        {
            _modelPath = modelPath;
        }


        //static Word.Application WordApp = new Word.Application();

        static public WordPOS.POS WordPos(string posTag)
        {
            WordPOS.POS retVal = WordPOS.POS.Other;
            if (posTag.StartsWith("NN"))
            {
                retVal = POS.Noun;
            }
            else if (posTag.StartsWith("JJ"))
            {
                retVal = POS.Adjective;
            }
            else if (posTag.StartsWith("VB"))
            {
                retVal = POS.Verb;
            }
            else if (posTag.StartsWith("RB"))
            {
                retVal = POS.Adverb;
            }
            return retVal;
        }


        private string _modelPath;
        private OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger mPosTagger;
        public string[] PosTagTokens(string[] tokens)
        {
            string EnglishPOSTaggerModelPath = System.IO.Path.Combine(_modelPath, "EnglishPOS.nbin");
            string tagDictPath = System.IO.Path.Combine(_modelPath, @"Parser\tagdict");

            if (mPosTagger == null)
            {
                mPosTagger = new OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger(EnglishPOSTaggerModelPath, tagDictPath);
            }

            return mPosTagger.Tag(tokens);
        }
        
        
        
        public void Dispose()
        {
            //((Word._Application)WordApp).Quit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.NLP.Domain.NLP.NaiveBayes
{
    public class TrainingSet
    {
        public TrainingSet()
        {
            this.Vocabulary = new List<string>();
            this.ClassWords = new Dictionary<string, Dictionary<string, int>>();
            this.ClassWordCount = new Dictionary<string, int>();
            this.ClassDocuments = new Dictionary<string, int>();
            this.ClassVocab = new Dictionary<string, List<string>>();
            this.TrainingDocuments = new List<TrainingDocument>();
            this.TSetClasses = new List<string>();
            this.ClassWordFrequency = new Dictionary<string, Dictionary<string, int>>();
            this.ClassPriors = new Dictionary<string, double>();

        }


        public decimal CalculateConditionalProbability(string word, string classId)
        {
            int wordCountInClass = this.GetCountOfWordInClass(word, classId);
            int totalWordsInClass = this.ClassWordCount[classId];
            int vocabularySize = this.Vocabulary.Count;

            decimal retVal = (decimal)(wordCountInClass + 1) / (decimal)(totalWordsInClass + vocabularySize);

            return retVal;
        }

        public void Add(TrainingDocument trainingDocument)
        {
            if (trainingDocument.HasWords)
            {
                TrainingDocuments.Add(trainingDocument);

                foreach (var word in trainingDocument.Words)
                {
                    AddWord(trainingDocument.Id, word, trainingDocument.Class);
                }

                AddToClassDocuments(trainingDocument);
            }
        }

        public decimal CalculatePrior(TrainingDocument trainingDocument)
        {
            int documentCount = this.TrainingDocuments.Count;
            int documentsInClass = this.ClassDocuments[trainingDocument.Class];

            decimal retVal = (decimal)documentsInClass / (documentCount);

            return retVal;
        }

        public int GetCountOfWordInClass(string word, string classId)
        {
            int retVal = 0;
            if (ClassWordFrequency[classId].ContainsKey(word))
            {
                retVal = ClassWordFrequency[classId][word];
            }
            return retVal;
        }

        internal void AddWord(string docId, string word, string classId)
        {
            //Vocabulary Maintenance
            AddToVocab(word);
            //WordInClass Maintenance
            AddWordToClassWords(docId, word, classId);
        }
        
        private void AddWordToClassWords(string docClass, string word, string classId)
        {
            if (ClassWords.ContainsKey(classId))
            {
                Dictionary<string, int> wordsInClass = ClassWords[classId];
                if (wordsInClass.ContainsKey(word))
                {
                    wordsInClass[word]++;
                }
                else
                {
                    wordsInClass.Add(word, 1);
                }
            }
            else
            {
                ClassWords.Add(classId, new Dictionary<string, int>());

            }

            AddToClassVocab(docClass, word, classId);

        }

        private void AddToClassDocuments(TrainingDocument doc)
        {
            string docClass = doc.Class;

            if (ClassDocuments.ContainsKey(docClass))
            {
                ClassDocuments[docClass]++;
            }
            else
            {
                ClassDocuments.Add(docClass, 1);
            };

            if (ClassWordCount.ContainsKey(docClass))
            {
                ClassWordCount[docClass] += doc.Words.Count;
            }
            else
            {
                ClassWordCount.Add(docClass, doc.Words.Count);
            }

            if (!TSetClasses.Contains(docClass))
            {
                TSetClasses.Add(docClass);
            }
        }

        private void AddToClassVocab(string docClass, string word, string classId)
        {
            if (!ClassVocab.ContainsKey(classId))
            {
                List<string> cVoc = new List<string>();
                cVoc.Add(word);
                ClassVocab.Add(classId, cVoc);
            }
            else
            {
                List<string> classVocab = ClassVocab[classId];
                if (!classVocab.Contains(word))
                {
                    classVocab.Add(word);
                }
            };
            //Class exists
            if (ClassWordFrequency.ContainsKey(classId))
            {
                //Word exists
                if (ClassWordFrequency[classId].ContainsKey(word))
                {
                    //increment WordFrequency
                    ClassWordFrequency[classId][word]++;
                }
                //Word doesn't exist
                else
                {
                    //Add Word and add to Class
                    ClassWordFrequency[classId].Add(word, 1);
                }
            }
            //Class dexists
            else
            {
                Dictionary<string, int> newWord = new Dictionary<string, int>();
                newWord.Add(word, 1);
                ClassWordFrequency.Add(classId, newWord);
            }

        }

        private void AddToVocab(string word)
        {
            if (!Vocabulary.Contains(word))
            {
                Vocabulary.Add(word);
            }
        }
        

        public List<string> Vocabulary { get; private set; }
        public Dictionary<string, Dictionary<string, int>> ClassWords { get; private set; }
        public Dictionary<string, List<string>> ClassVocab { get; private set; }
        public List<TrainingDocument> TrainingDocuments { get; private set; }
        public Dictionary<string, int> ClassDocuments { get; private set; }
        public Dictionary<string, int> ClassWordCount { get; private set; }
        public List<string> TSetClasses { get; private set; }
        public Dictionary<string, Dictionary<string, int>> ClassWordFrequency { get; private set; }
        public Dictionary<string, double> ClassPriors { get; private set; }
    }
}

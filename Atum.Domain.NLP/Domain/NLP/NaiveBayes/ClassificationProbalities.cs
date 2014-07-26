using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.NLP.NaiveBayes
{
    public class ClassificationProbalities
    {

        public ClassificationProbalities(string className, double prior)
        {
            this.Class = className;
            this.Prior = prior;
        }

        internal void AddLogProb(double logProb)
        {
            this.Sum += logProb;
            this.Probability = Prior * Math.Exp(Sum);
        }

        public double Sum { get; private set; }

        public double Prior { get; private set; }

        public double Probability { get; private set; }

        public string Class { get; private set; }
    }
}

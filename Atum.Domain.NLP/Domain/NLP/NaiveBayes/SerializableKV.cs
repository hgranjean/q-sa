using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Atum.Domain.NLP.NaiveBayes
{
    [Serializable]
    public class SerializableKV<T1, T2>
    {
        public T1 Key { get; set; }
        public T2 Value { get; set; }
    }
}

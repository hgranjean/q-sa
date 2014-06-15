using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Rules.Domain.Base;
using Rules.Domain.Vocabulary;

namespace Rules.Domain
{
    public class RuleApplicationSpec : RuleObjectBase
    {
	    public AuthoringSettings Settings { get; set; }
        public const int FeatureVersion = 1;
	    public String Imports { get; set; }
	    // public List<ClassNode> vocabulary;
	    // private List<UserFunctionLibrary> functionLibrary;
        public List<EntitySpec> Entities { get; set; } 
	    public List<RuleSpec> RuleSets { get; set; }
	    // public List<SchemaEndpoint> schemaEndpoints;
	    // private String strategyTemplate;
        public VocabularySpec Vocabulary { get; set; }
        public new string Name { get { return base.Name; } set { base.Name = value; } }

        public RuleApplicationSpec()
        {
            Settings = new AuthoringSettings();
            RuleSets = new List<RuleSpec>();
            Entities = new List<EntitySpec>();
            Vocabulary = new VocabularySpec();
        }

        public void Save(string fileName)
        {
            var serializer = new XmlSerializer(typeof(RuleApplicationSpec));
            var xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true});
            serializer.Serialize(xmlWriter, this);
        }

        public static RuleApplicationSpec Load(string fileName)
        {
            var serializer = new XmlSerializer(typeof(RuleApplicationSpec));
            return (RuleApplicationSpec)serializer.Deserialize(new XmlTextReader(fileName));
        }
    }
}

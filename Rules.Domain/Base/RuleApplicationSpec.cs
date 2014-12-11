using Rules.Domain.Base;
using Rules.Domain.EndPoints;
using Rules.Domain.Vocabulary;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Rules.Domain
{   
    [XmlInclude(typeof(SimpleRuleSet))]
    public class RuleApplicationSpec : RuleObjectBase
    {
	    public AuthoringSettings Settings { get; set; }
        public const int FeatureVersion = 1;
	    public String Imports { get; set; }
	    // public List<ClassNode> vocabulary;
	    // private List<UserFunctionLibrary> functionLibrary;
        public List<EntitySpec> Entities { get; set; } 
	    public List<RuleSpec> RuleSets { get; set; }
        public List<EndPointSpec> EndPoints { get; set; }
	    // public List<SchemaEndpoint> schemaEndpoints;
	    // private String strategyTemplate;
        public VocabularySpec Vocabulary { get; set; }
        public new string Name { get { return base.Name; } set { base.Name = value; } }

        public RuleApplicationSpec()
        {
            Settings = new AuthoringSettings();
            RuleSets = new List<RuleSpec>();
            Entities = new List<EntitySpec>();
            EndPoints = new List<EndPointSpec>();
            Vocabulary = new VocabularySpec();
        }

        public void Save(string fileName)
        {
            using (var xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings {Indent = true}))
            {
                var serializer = new XmlSerializer(typeof(RuleApplicationSpec));
                serializer.Serialize(xmlWriter, this);
            }

        }

        public static RuleApplicationSpec Load(string fileName)
        {
            using (var reader = new XmlTextReader(fileName))
            {
                var serializer = new XmlSerializer(typeof(RuleApplicationSpec));
                return (RuleApplicationSpec)serializer.Deserialize(reader);    
            }
            
        }
    }
}

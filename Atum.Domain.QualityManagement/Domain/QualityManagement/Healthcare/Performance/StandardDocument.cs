using Atum.Domain.Basis;
using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public enum Visibility
    {
        Public, Private
    }
    public class StandardDocument : DomainObject
    {
        Dictionary<string, Chapter> _luChapters = new Dictionary<string,Chapter>();

        public StandardDocument()
        {
        }

        public StandardDocument(string title)
        {
            this.Title = title;
            this.TableOfContents = new TableOfContents();
        }


        public string Title { get; set; }
        public List<Chapter> Chapters { get; set; }
        public int OwnerId { get; set; }
        public Visibility Visibility { get; set; }

        public TableOfContents TableOfContents { get; private set; }
        
        public Chapter AddChapter(string chapterKey, string chapterTitle)
        {
            //Check Key
            if (_luChapters.Keys.Contains(chapterKey))
            {
                throw new Exception("Key Exists");
            }
            else //Add to Chapter Lookup
            {
                Chapter chapter = new Chapter(chapterKey,chapterTitle);
                Chapters.Add(chapter);
                _luChapters.Add(chapterKey, chapter);
                //Update TOC
                TableOfContents.AddElement(chapter);
                return chapter;
            }
        }
    }
}

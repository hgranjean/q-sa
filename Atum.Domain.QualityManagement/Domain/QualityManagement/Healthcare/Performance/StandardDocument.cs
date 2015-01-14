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
        public StandardDocument()
        {
        }

        public StandardDocument(string title)
        {
            this.Title = title;
        }

        public string Title { get; set; }
        public DocumentElements Chapters { get; set; }
        public int OwnerId { get; set; }
        //public Visibility Visibility { get; set; }

        public Chapter AddChapter(string chapterKey, string chapterTitle)
        {
            if (Chapters==null)
            {
                Chapters = new DocumentElements();
            }
            Chapter chapter = new Chapter(chapterKey, chapterTitle);
            Chapters.Add(chapter);
            
            return chapter;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rules.WebEditor.Models
{
    public class JourneyViewModel
    {
        public List<BladeViewModel> Blades { get; set; }
        public BladeEditorViewModel BladeEditor { get; set; }

        public JourneyViewModel(List<BladeViewModel> blades, BladeEditorViewModel editor)
        {
            this.Blades = blades;
            this.BladeEditor = editor;
        }
    }
}
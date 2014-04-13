using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rules.WebEditor.Models
{
    public class JourneyViewModel
    {
        public List<BladeViewModel> Blades { get; set; }

        public JourneyViewModel(List<BladeViewModel> blades)
        {
            this.Blades = blades;
        }
    }
}
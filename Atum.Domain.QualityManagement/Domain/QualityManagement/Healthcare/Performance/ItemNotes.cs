using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public class ItemNotes : List<ItemNote>
    {

        public List<string> GetNotes()
        {
            List<string> retVal = new List<string>();

            foreach (var item in this)
            {
                retVal.Add(item.Text);
            }
            return retVal;
        }
    }
}

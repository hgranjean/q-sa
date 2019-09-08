using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atum.Domain.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Atum.Repository.Test
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            Address address1 = new Address { Street1 = "1818", City = "Evanston", State = "IL", Id = 2, Zip = "60201" };
            Address address2 = new Address { Street1 = "1817", City = "Evanston", State = "IL", Id = 1, Zip = "60201" };
            List<Address> addresses = new List<Address>();
            addresses.Add(address1);
            addresses.Add(address2);

            StringBuilder sb = new StringBuilder();

            foreach (var item in addresses)
            {
                sb.AppendLine(item.ToDelimitedstring("|"));
            }

            sb.Clear();
            foreach (var item in ToCsv(addresses, "|"))
            {
                sb.AppendLine(item);
            }

            var s = sb.ToString();
        }


        public static IEnumerable<string> ToCsv<T>(IEnumerable<T> objectlist, string separator = ",", bool header = true)
        {
            FieldInfo[] fields = typeof(T).GetFields();
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (header)
            {
                yield return String.Join(separator, fields.Select(f => f.Name).Concat(properties.Select(p => p.Name)).ToArray());
            }
            foreach (var o in objectlist)
            {
                yield return string.Join(separator, fields.Select(f => (f.GetValue(o) ?? "").ToString())
                    .Concat(properties.Select(p => (p.GetValue(o, null) ?? "").ToString())).ToArray());
            }
        }
    }
}
 
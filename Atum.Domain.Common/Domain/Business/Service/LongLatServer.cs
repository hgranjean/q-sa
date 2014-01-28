using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Atum.Utility;
using Atum.Utility.Calculator;

namespace Atum.Repository.Business
{
    public class LongLatServer
    {
        private Hashtable _htZipLongLats;
        private string _zip;

        private static readonly LongLatServer _instance = new LongLatServer();

        static LongLatServer() { }

        private LongLatServer() { }

        public static LongLatServer Instance { get { return _instance; } }

        private void load()
        {   
             
            //ZipGeoDataRepository zipGeoRepos = Atum.Repository.Business.ZipGeoDataRepository.Instance;

            //Atum.Domain.Business.ZipGeoData myzipData = (Atum.Domain.Business.ZipGeoData)zipGeoRepos.MatchingZipCode(_zip);
            //_htZipLongLats.Add(_zip, myzipData);
        }

        private Hashtable ZipLLP
        {
            get
            {
                if (_htZipLongLats == null)
                {
                    _htZipLongLats = new Hashtable();
                    load();
                }
                return _htZipLongLats;
            }
        }

        public LongLatPoint GetLongLatPointForZip(string Zip)
        {
            LongLatPoint retVal = (LongLatPoint)ZipLLP[Zip];
            if (retVal == null)
            { throw new ZipCodeNotFoundException(string.Format("No Data Found for {0}",Zip));
            }
            else
                {return retVal;}
        }
    }


        [global::System.Serializable]
        public class ZipCodeNotFoundException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public ZipCodeNotFoundException() { }
            public ZipCodeNotFoundException(string message) : base(message) { }
            public ZipCodeNotFoundException(string message, Exception inner) : base(message, inner) { }
            protected ZipCodeNotFoundException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }


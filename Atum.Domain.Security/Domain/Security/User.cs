using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Security
{
	/// <summary>
	/// Summary description for User.
	/// </summary>
    [Serializable]
    public class User //: DomainObject
	{
        //private long _loginID;
        //private long _tableID;
        private string _pwd_hash;
        //private long _appID;
        private string _userName;

        private Permission[] _permissions;
        private Group[] _groups;
        //private object _AttribecuteList;//New Collection

        //private object _Status;//Integer
        
        public User()
		{
		}

        public string PasswordHash { get { return _pwd_hash; } set{_pwd_hash=value; }}
        public string UserName { get { return _userName; } set { _userName = value; } }
        public Permission[] Permissions { get { return _permissions; } set { _permissions = value; } }
        public Group[] Groups { get { return _groups; } set { _groups = value; } }

    }
}

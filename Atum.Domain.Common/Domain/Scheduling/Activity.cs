using System;


namespace Atum.Domain.Scheduling
{
	/// <summary>
	/// Summary description for Activity.
	/// </summary>
	public class Activity
	{
		Resource _who;
		DateTime _when;
		TimeSlot _ts;
		string _description;
		string _party;
		

		public Activity(Resource who, TimeSlot ts, int day)
		{
			_who = who;
		}
		public Activity(Resource who, TimeSlot ts, DateTime day, string desc, string party)
		{
			_who = who;
			_ts = ts;
			_when = day;
			_description = desc;
			_party = party;
		}
		public TimeSlot TimeSlot {get{return _ts;}}
		public Resource Who{get{return _who;}}
		public string When{get{return _when.ToShortDateString();}}
		public string Description{get{return _description;}set{_description = value;}} 
		public string Party{get{return _party;}set{_party = value;}} 

		public override bool Equals(object obj)
		{
			return (obj.GetHashCode() == this.GetHashCode());
		}

		public override int GetHashCode()
		{
			string s = (_who.Name + _when + _ts.ToString());
			return s.GetHashCode ();
		}

	}
}

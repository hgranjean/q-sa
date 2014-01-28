using System;

namespace Atum.Domain.Scheduling
{
	/// <summary>
	/// Summary description for TimeSlot.
	/// </summary>
	public class TimeSlot
	{
		int _hourStart;
		int _minStart;
		int _hourEnd;
		int _minEnd;
		System.TimeSpan _ts;

		public TimeSlot(int startHour, int startMinute, int duration)
		{
			_ts = new TimeSpan(duration);
			DateTime dtEnd = new DateTime(1,1,1,startHour,startMinute,0).AddMinutes(duration);
			_hourStart = startHour;
			_minStart = startMinute;
			_hourEnd = dtEnd.Hour;
			_minEnd = dtEnd.Minute;
		}

		public TimeSlot(int startHour, int startMinute, int endHour, int endMinute)
		{
			_hourStart = startHour;
			_minStart = startMinute;
			_hourEnd = endHour;
			_minEnd = endMinute;
		}

		public int StartHour{get{return _hourStart;}}
		public int StartMin{get{return _minStart;}}
		public int EndHour{get{return _hourEnd;}}
		public int EndMin{get{return _minEnd;}}

		public override string ToString()
		{
			string s = string.Format("{0}:{1:00} to {2}:{3:00}",StartHour,StartMin,EndHour,EndMin);
			return s;
		}

		public override int GetHashCode()
		{
			string sKey = StartHour.ToString() + StartMin.ToString() + EndHour.ToString() + EndMin.ToString();
			return sKey.GetHashCode ();
		}

		public override bool Equals(object obj)
		{
			bool retVal = false;
			int toCheck = (int)obj;
			retVal =(this.GetHashCode()== toCheck);

//			retVal =(_hourStart == toCheck._hourStart);
//			retVal &=(_minStart == toCheck._minStart);
//			retVal &=(_hourEnd == toCheck._hourEnd);
//			retVal &=(_minEnd == toCheck._minEnd);
			
			return retVal;
		}

	}
}

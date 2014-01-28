using System;
using System.Collections;

namespace Atum.Domain.Scheduling
{
	/// <summary>
	/// Summary description for Schedule.
	/// </summary>
	public class Schedule
	{
		private Hashtable _activities = new Hashtable();
		private Hashtable _busySlots = new Hashtable();
		
		private Resource _res;
		private DateTime _startDay;
		private DateTime _endDay;
		private DateTime[] _days;

		public Schedule(Resource res, DateTime startDay, DateTime endDay)
		{
			_res = res;
			_startDay = startDay;
			_endDay = endDay;
			loadDays();
			loadActivitiesForResource(this._res);
		
		}
		public Schedule(Resource res, int day, int month, int year)
		{
		}
		public Schedule(Resource res, int month, int year)
		{
			//DateTime startDay;
			//DateTime endDay = new DateTime(month.Year,month.Month,
		}
		
		private void loadDays()
		{
			DateTime dayToAdd = _startDay;
			ArrayList days = new ArrayList();
			while(dayToAdd!=_endDay)
			{
				days.Add(dayToAdd);
				dayToAdd = dayToAdd.AddDays(1);
			}
			_days = (DateTime[])days.ToArray(typeof(System.DateTime));
		}
		private void loadActivitiesForResource(Resource res)
		{
			TimeSlot aTimeSlot = new TimeSlot(9,0,9,15);
			Activity act = new Activity(res,aTimeSlot,DateTime.Today,"Discuss Product","Customer");
			addActivity(act);

			aTimeSlot = new TimeSlot(10,0,11,45);
			act = new Activity(res,aTimeSlot,DateTime.Today.AddDays(3),"Discuss Product A","Customer B");
			addActivity(act);
		}

		private void addActivity(Activity act)
		{
			_activities.Add(act.GetHashCode(), act);
			_busySlots.Add(act.TimeSlot.GetHashCode(), act.TimeSlot);
		}

		private void removeActivity(Activity act)
		{
			_activities.Remove(act.GetHashCode());
			_busySlots.Remove(act.TimeSlot.GetHashCode());
		}

		public string ResourceName{get{return _res.Name;}}
		public DateTime[] Days{get{return _days;}}
		public bool IsBusyTimeSlot(TimeSlot ts)
		{
			return _busySlots.Contains(ts.GetHashCode());
		}

		public Activity[] ActivitiesForDay(DateTime day)
		{
			ArrayList alActs = new ArrayList();
			Activity[] retVal = new Activity[0];
			foreach(Activity act in this._activities.Values)
			{
				if(DateTime.Parse(act.When).Date==day.Date)
				{
					alActs.Add(act);
				}
			}
			if(alActs.Count>0)
			{retVal = (Activity[])alActs.ToArray(typeof(Activity));}
			return retVal;
		}


		public bool BookTimeSlot(TimeSlot ts, DateTime day,string description, string party)
		{
			if(!_busySlots.Contains(ts.GetHashCode()))
			{
				Activity act = new Activity(_res,ts,DateTime.Today,description,party);
				addActivity(act);

				Console.WriteLine("{0} has set an apptment with {1} on {2} from {3} for {4}",
					act.Who.Name,party, act.When,act.TimeSlot.ToString(),
					act.Description);
				
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool UnBookTimeSlot(TimeSlot ts, DateTime day)
		{
			
			if(_busySlots.Contains(ts.GetHashCode()))
			{
				Activity act =  new Activity(_res,ts,DateTime.Today,"","");
				act =  (Activity)this._activities[act.GetHashCode()];
				removeActivity(act);

				Console.WriteLine("{0}'s apptment with  {1} at {3} to {4} has been deleted",
					act.Who.Name, act.Party, act.When,act.TimeSlot.ToString(),
					act.Description);
				
				return true;
			}
			else
			{
				//No activity in that TimeSlot
				return false;
			}
		}
	}
}

using System;
namespace AmazonWareHouse
{
	public class WorkUnit
	{
		public string skill { get; set;}
		public int priority { get; set; }
		public int timeRequired { get; set; }
		public int jobID { get; set; }
		public bool isAllocated { get; set; }
	}
}

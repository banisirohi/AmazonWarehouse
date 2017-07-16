using System;
using System.Collections;
using System.Collections.Generic;

namespace AmazonWareHouse
{
	class MainClass
	{
		

		public static void Main(string[] args)
		{
			//worker: Designation#skill
			string[] empDesignationWithSkill = { ("w1#s1"),("w2#s2"),("w3#s3"),("w4#s1")};

			//skill#Priority#Time#jobID
			string[] skillWithTasks = { ("s1#40#10#101"), 
										("s2#10#5#102"), 
										("s3#90#15#103"),
										("s3#90#20#104"),
										("s2#20#5#105"),
										("s1#20#10#106"),
										("s1#90#15#107"),
										("s2#30#20#108"),
										("s3#40#5#109"),
										("s1#50#5#110")
									  };


			Process process = new Process();
			var workerAllocatedWithTasks = process.OutPut(empDesignationWithSkill, skillWithTasks);


			//outputs employee with jobs they workon
			foreach (var item in workerAllocatedWithTasks)
			{
				Console.Write(item.Key +" : ");
				item.Value.ForEach(y => Console.Write(" " + y));
				Console.WriteLine();
			}
		}
	}
}

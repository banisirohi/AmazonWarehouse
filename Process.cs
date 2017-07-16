using System;
using System.Collections.Generic;
using System.Linq;

namespace AmazonWareHouse
{
	public class Process
	{
		public List<WorkUnit> workUnits = new List<WorkUnit>();
		public List<Employee> employees = new List<Employee>();
		public List<string> skills = new List<string>();

		public Dictionary<string, List<int>> workerAllocatedWithTasks = new Dictionary<string, List<int>>();

		public void ParseInput(string[] empDesignationWithSkill, string[] skillWithTasks)
		{
			foreach (var item in empDesignationWithSkill)
			{
				string[] employee = item.Split('#');
				employees.Add(new Employee { designation = employee[0], skill = employee[1], timeAllocated = 0});
			}
			foreach (var item in skillWithTasks)
			{
				string[] skillWithTaskDetails = item.Split('#');
				WorkUnit unitOfWork = new WorkUnit
				{
					skill = skillWithTaskDetails[0],
					priority = Convert.ToInt32(skillWithTaskDetails[1]),
					timeRequired = Convert.ToInt32(skillWithTaskDetails[2]),
					jobID = Convert.ToInt32(skillWithTaskDetails[3]),
					isAllocated = false
										};
				workUnits.Add(unitOfWork);
			}
		skills = workUnits.Select(o => o.skill).ToList();
		}

		public Dictionary<string, List<int>> OutPut(string[] empDesignationWithSkill, string[] skillWithTasks)
		{
			ParseInput(empDesignationWithSkill, skillWithTasks);
			startAllocation();
			return workerAllocatedWithTasks;
		}

		public void startAllocation()
		{
			foreach (var skill in skills)
			{
				//pick job according to priority of jobs
				WorkUnit workUnit = workUnits.Where(x => x.skill == skill && !x.isAllocated).OrderByDescending(y => y.priority).ThenBy(m => m.timeRequired).FirstOrDefault();
				AllocateWorker(workUnit);			
			}
		}

		void AllocateWorker(WorkUnit workUnit)
		{
			List<Employee> workers = new List<Employee>();
			Employee worker = new Employee();
			var numberOfWorkersBySkill = employees.Where(v => v.skill == workUnit.skill).ToList();

			//Case 1.1: if multiple workes on same skill
			if (numberOfWorkersBySkill.Count() > 1)
			{
				var freeWorkers = employees.Where(b => b.skill == workUnit.skill && b.timeAllocated == 0).OrderBy(x => x.designation).ToList();
				//Case 2.1: if all workers engaged, then pick worker list order by time required to complete the job
				if (employees.Where(d => d.skill == workUnit.skill).All(d => d.timeAllocated > 0))
					workers = freeWorkers.Count() > 0 ? freeWorkers : employees.Where(c => c.skill == workUnit.skill).OrderBy(x => x.timeAllocated).ToList();

				//Case 2.2: if some workers engaged, then pick worker list order by designation
				else if (employees.Any(d => d.skill == workUnit.skill && d.timeAllocated > 0))
					workers = freeWorkers.Count() > 0 ? freeWorkers : employees.Where(c => c.skill == workUnit.skill && c.timeAllocated <= workUnit.timeRequired).OrderBy(x => x.designation).ToList();

				//Case 2.3: if all workers free
				else
					workers = employees.Where(c => c.skill == workUnit.skill).OrderBy(x => x.designation).ToList();
			}

			//Case 1.2: if one workes for particular skill
			else
				workers = numberOfWorkersBySkill;

			//Allocate worker from workers list allowed to work on that job
			 worker = workers.FirstOrDefault();

			var desig = worker.designation;
			if (!workerAllocatedWithTasks.ContainsKey(desig))
				workerAllocatedWithTasks.Add(desig , new List<int>());
			workerAllocatedWithTasks[desig].Add(workUnit.jobID);
			workUnits.Where(g => g.jobID == workUnit.jobID).FirstOrDefault().isAllocated = true;


			var durationOfTask = employees.Where(g => g.designation == worker.designation).FirstOrDefault().timeAllocated;
			employees.Where(g => g.designation == worker.designation).FirstOrDefault().timeAllocated = durationOfTask + workUnit.timeRequired;
		}
}
}

-- projectid = 102
-- reqId = 106

select COUNT() as completeCount,
requirement.Name as ReqName,
requirement.Id as Id 
from example.Requirements as requirement
full outer join example.Tasks as task on task.RequirementId = requirement.Id
full outer join example.Complete as complete on complete.TaskId = task.Id
where requirement.Id = 119
--group by requirement.Name, requirement.Id
order by requirement.Name

select COUNT(complete.IsComplete) as completeCount,
requirement.Name as ReqName,
requirement.Id as Id 
from example.Requirements as requirement
full outer join example.Tasks as task on task.RequirementId = requirement.Id
full outer join example.Complete as complete on complete.TaskId = task.Id
where complete.IsComplete = 1 and requirement.Id = 119
group by requirement.Name, requirement.Id
order by requirement.Name

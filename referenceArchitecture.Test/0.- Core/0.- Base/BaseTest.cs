using NoEstimates.Core.DTO;
using NoEstimates.repository._0.__Edmx;
using referenceArchitecture.repository.Edmx.Interfaces;
using referenceArchitecture.Test.Core.Factory;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NoEstimates.Test.Core.Base
{
    public abstract class BaseTest
    {
        //todo ARCHITECTURE include base test to inhirits and use common methods
        // Generics methods

        private Random r = new Random();
        public Stopwatch startTime()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            return stopWatch;
        }
        public long getMiliseconds(Stopwatch stopWatch)
        {
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }
        public long getSeconds(Stopwatch stopWatch)
        {
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds/1000;
        }
        public int getR(int maxValue)
        {
            return r.Next(maxValue);
        }
        public int getR(int minValue, int maxValue)
        {
            return r.Next(minValue, maxValue);
        }
        public int getRLong()
        {
            return r.Next(-1000000000, 1000000000);
        }
        public string getRString()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");

            return g.ToString();
        }
        public bool getRandomBool()
        {
            return r.Next(0, 1) == 1;
        }
        public DateTime getRandomDate()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(r.Next(range));
        }
        public bool datesAreEqual(DateTime date1, DateTime date2)
        {
            bool ret = false;

            ret = date1.Year == date2.Year;
            ret = date1.Month == date2.Month;
            ret = date1.Day == date2.Day;
            ret = date1.Hour == date2.Hour;
            ret = date1.Minute == date2.Minute;
            ret = date1.Second == date2.Second;

            return ret;
        }
        public void removeCollection<TEntity>(IEnumerable<TEntity> entities, IDbContext context) where TEntity : class
        {
            foreach (var entity in entities)
            {
                context.Set<TEntity>().Remove(entity);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool areSameObjets(object obj1, object obj2)
        {
            // Return false if any of both objects to compare are null
            if (obj1 == null || obj2 == null) return false;

            // Ret is initialized to false.
            bool ret = false;

            // Loop through properties of obj
            var properties1 = obj1.GetType().GetProperties();
            var properties2 = obj2.GetType().GetProperties();
            foreach (var property in properties1)
            {
                if (property.PropertyType.Name == nameof(Int32)
                    || property.PropertyType.Name == nameof(String)
                    || property.PropertyType.Name == nameof(Boolean)
                    || property.PropertyType.Name == nameof(Double)
                    || property.PropertyType.Name == nameof(Decimal)
                    || property.PropertyType.Name == nameof(TimeSpan)
                    || property.PropertyType.Name == nameof(Int64)
                    || property.PropertyType.Name == nameof(Int16)
                    || property.PropertyType.Name == nameof(Byte)
                    || property.PropertyType.Name == nameof(Enum))
                {
                    var prop1 = property.GetValue(obj1);
                    var prop2 = obj2.GetType().GetProperty(property.Name).GetValue(obj2);
                    ret = obj1.ToString() == obj2.ToString();
                }
                else if (property.PropertyType.Name == nameof(DateTime))
                {
                    var prop1 = property.GetValue(obj1) as DateTime?;
                    var prop2 = obj2.GetType().GetProperty(property.Name).GetValue(obj2) as DateTime?;
                    ret = datesAreEqual(prop1.Value, prop2.Value);   
                }
            }

            // true if both objects are the same base on their properties. Otherwise false.
            return ret;
        }
        public bool areSameObjectsCollection(List<object> objectList1, List<object> objectList2)
        {
            var ret = objectList1.Count() == objectList2.Count();
            if (!ret) return false;
            for (int i = 0; i < objectList1.Count(); i++)
            {
                ret = areSameObjets(objectList1[i], objectList2[i]);
            }

            return ret;

        }
        //todo ARCHITECTURE Include this equals in architecture to test  equality between objects and all the methods above

        // Remove Projects, Requirement, Tasks, Highlights, Complete, timer
        public void removeAll()
        {
            // Remove all projects, requirement, tasks and completes
            using (var db = Container.createIDbContext())
            {
                var highlights = db.Highlights.ToList(); removeCollection<Highlight>(highlights, db);
                var complete = db.Completes.ToList(); removeCollection<Complete>(complete, db);
                var timer = db.Timers.ToList(); removeCollection<Timer>(timer, db);

                var tasks = db.Tasks.ToList(); removeCollection<Task>(tasks, db);
                var allRequirements = db.Requirements.ToList(); removeCollection<Requirement>(allRequirements, db);
                var projects = db.Projects.ToList(); removeCollection<Project>(projects, db);
                db.SaveChanges();
            }

        }

        // Project
        public List<DTOProject> insertProjectsAndgetList(int listLength)
        {
            List<DTOProject> projectList = new List<DTOProject>();

            for (int i = 0; i < listLength; i++)
            {
                // Get individual task
                var projectToInsert = getRandomProject();

                // Insert task
                int insertedRecord = Container.createIProjectRepository().createProjectAndSaveChanges(Container.createIDbContext(), projectToInsert);
                projectToInsert.Id = insertedRecord;

                // Add task to list
                projectList.Add(projectToInsert);
            }

            return projectList;
        }
        public DTOProject getRandomProject()
        {
            string randonStr = getRString().ToString();
            var randonCreationDate = getRandomDate();
            var projectToInsert = new DTOProject
            {
                Description = "Test" + randonStr.ToString(),
                Name = randonStr,
                IsCompleted = true,
                CreationDate = randonCreationDate,
                FinalizationDate = (DateTime)SqlDateTime.MaxValue
            };

            return projectToInsert;
        }
        public DTOProject insertAndgetProject(DTOProject project)
        {

            int idOfInsertedRecord = Container.createIProjectRepository().createProjectAndSaveChanges(Container.createIDbContext(), project);
            project.Id = idOfInsertedRecord;

            return project;
        }

        // Requirement
        public List<DTORequirements> insertAndGetRequirementCollection(int numberOfRequirmentsToCreate, DTOProject project)
        {
            // Ge requirements collection
            List<DTORequirements> requirementCollection = new List<DTORequirements>();
            for (int i = 0; i < numberOfRequirmentsToCreate; i++)
            {
                var requirementToInsert = getRandomRequirement(project);
                
                requirementCollection.Add(requirementToInsert);
            }

            // Insert requirements
            using (var context = Container.createIDbContext())
            {
                foreach (var requirement in requirementCollection)
                {
                    int id = Container.createIRequirementsRepository().insertRequirementAndSaveChanges(context, requirement);
                    requirement.Id = id;
                }
            }

            return requirementCollection;

        }
        public DTORequirements getRandomRequirement(DTOProject project)
        {
            var randonName = "req-" + getRString().ToString();
            var randonDescription = "desp-" + getRString().ToString();
            var randonCreationDate = getRandomDate();
            var requirementToInsert = new DTORequirements
            {
                ProjectId = project.Id,
                Name = randonName,
                Description = randonDescription,
                CreationDate = randonCreationDate,
                FinalizationDate = (DateTime)SqlDateTime.MaxValue
            };

            return requirementToInsert;
        }
        public DTORequirements insertAndgetRequirement(DTORequirements DTORequirement)
        {
            int idOfInsertedRequirement = Container.createIRequirementsRepository().insertRequirementAndSaveChanges(Container.createIDbContext(), DTORequirement);
            DTORequirement.Id = idOfInsertedRequirement;

            return DTORequirement;
        }

        // Task
        public DTOTask getRandomTask(DTORequirements requirement)
        {
            var randonDescription = "descrp-" + getRString();
            var randonCreationDate = getRandomDate();
            var taskToInsert = new DTOTask
            {
                RequirementId = requirement.Id,
                Description = randonDescription,
                CreationDate = randonCreationDate
            };

            return taskToInsert;
        }
        public List<DTOTask> insertTasksAndgetList(DTORequirements requirement, int listLength)
        {
            List<DTOTask> taskList = new List<DTOTask>();

            for (int i = 0; i < listLength; i++)
            {
                // Get individual task
                var taskToInsert = getRandomTask(requirement);

                // Insert task
                int insertedRecord = Container.createITaskRepository().insertTaskAndSaveChanges(Container.createIDbContext(), taskToInsert);
                taskToInsert.Id = insertedRecord;

                // Add task to list
                taskList.Add(taskToInsert);
            }

            return taskList;
        }

        // Complete
        public DTOComplete getRandomCompleteWithIsCompleteTrue(DTOTask task)
        {
            var randonFinalizationDate = getRandomDate();
            DTOComplete complete = new DTOComplete
            {
                IsComplete = true,
                FinalizationDate = randonFinalizationDate,
                TaskId = task.Id
            };

            return complete;
        }
        public DTOComplete getRandomComplete(DTOTask task)
        {
            var randonComplete = getRandomBool();
            var randonFinalizationDate = getRandomDate();
            DTOComplete complete = new DTOComplete
            {
                IsComplete = randonComplete,
                FinalizationDate = randonFinalizationDate,
                TaskId = task.Id
            };

            return complete;
        }
        public DTOComplete insertAndGetComplete(DTOComplete complete)
        {
            int id = Container.createITaskRepository().insertCompleteAndSaveChanges(Container.createIDbContext(), complete);
            complete.Id = id;

            return complete;
        }

        // Timer
        public DTOTimer getRandomTimer(DTOTask task)
        {
            var randonSeconds = getRLong();
            DTOTimer timer = new DTOTimer
            {
                TimeInSeconds = randonSeconds,
                TaskId = task.Id
            };

            return timer;
        }
        public DTOTimer insertAndGetTimer(DTOTimer timer)
        {
            int id = Container.createITaskRepository().insertTimerAndSaveChanges(Container.createIDbContext(), timer);
            timer.Id = id;

            return timer;
        }

        // Highlight
        public DTOHighlightColor getRandomHighlight(DTOTask task)
        {
            var randonColor = r.Next(0, 4);
            DTOHighlightColor highlight = new DTOHighlightColor
            {
                Color = randonColor,
                TaskId = task.Id
            };

            return highlight;
        }

        public DTOHighlightColor insertAndGetHighlight(DTOHighlightColor highlight)
        {
            int id = Container.createITaskRepository().insertHighlightColorAndSaveChanges(Container.createIDbContext(), highlight);
            highlight.Id = id;

            return highlight;
        }
    }
}

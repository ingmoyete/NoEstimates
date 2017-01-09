using NoEstimates.Core.DTO;
using NoEstimates.repository._0.__Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.Configuration;

namespace NoEstimates.repository.Core.Mapper
{
    public class Mapper : IMapper
    {
        /// <summary>
        /// Initialize mappings.
        /// </summary>
        /// <returns>A IMapper with the configuration for the mappings.</returns>
        private static AutoMapper.IMapper initializeMappings()
        {
            var expressions = new MapperConfigurationExpression();
            //========================================================
            expressions.CreateMap<Project, DTOProject>().ReverseMap();
            expressions.CreateMap<Requirement, DTORequirements>().ReverseMap();
            expressions.CreateMap<Task, DTOTask>().ReverseMap();
            expressions.CreateMap<Highlight, DTOHighlightColor>().ReverseMap();
            expressions.CreateMap<Complete, DTOComplete>().ReverseMap();
            expressions.CreateMap<Timer, DTOTimer>().ReverseMap();
            

            //=========================================================
            return new MapperConfiguration(expressions).CreateMapper();
        }

        /// <summary>
        /// Map the source from a destination.
        /// </summary>
        /// <typeparam name="TSource">Source mapping.</typeparam>
        /// <param name="TDestination">Destination mapping.</param>
        /// <returns>A TDestination object.</returns>
        public TDestination map<TDestination>(object TSource)
        {
            var destination = MapperObject.Map<TDestination>(TSource);
            return destination;
        }


        /// <summary>
        /// Map project and DTO project entity to DTO.
        /// </summary>
        /// <param name="dtoProject">DTO project to map from.</param>
        /// <returns>A project entity.</returns>
        public Project mapProjectEntity(DTOProject dtoProject)
        {
            return map<Project>(dtoProject);
        }

        /// <summary>
        /// Map project DTO to Entity.
        /// </summary>
        /// <param name="dtoProject">Entity project to map from.</param>
        /// <returns>A DTO project.</returns>
        public DTOProject mapProjectDTO(Project entityProject)
        {
            return map<DTOProject>(entityProject);   
        }

        /// <summary>
        /// Map DTO requiremnets to entity requirement.
        /// </summary>
        /// <param name="entityRequirement">Entity requirement to map from.</param>
        /// <returns>A DTO requirement.</returns>
        public DTORequirements mapDTORequirements(Requirement entityRequirement)
        {
            return map<DTORequirements>(entityRequirement);
        }

        /// <summary>
        /// Map DTO requiremnets to entity requirement.
        /// </summary>
        /// <param name="dtoRequirement">Entity requirement to map from.</param>
        /// <returns>A DTO requirement.</returns>
        public Requirement mapEntityRequirements(DTORequirements dtoRequirement)
        {
            return map<Requirement>(dtoRequirement);
        }

        /// <summary>
        /// Map entity task to dto task.
        /// </summary>
        /// <param name="dtoTask">dto task to map from.</param>
        /// <returns>A entity task.</returns>
        public Task mapEntityTask(DTOTask dtoTask)
        {
            return map<Task>(dtoTask);
        }

        /// <summary>
        /// Map DTO task to entity task.
        /// </summary>
        /// <param name="entityTask">entity task to map from.</param>
        /// <returns>A DTO task.</returns>
        public DTOTask mapDTOTask(Task entityTask)
        {
            return map<DTOTask>(entityTask);
        }

        /// <summary>
        /// Map DTO Highlight to entity Highlight.
        /// </summary>
        /// <param name="entityHighlight">Highlight entity to map from.</param>
        /// <returns>A DTO Highlight.</returns>
        public DTOHighlightColor mapDTOHighlight(Highlight entityHighlight)
        {
            return map<DTOHighlightColor>(entityHighlight);
        }

        /// <summary>
        /// Map Entity Highlight to DTO Highlight.
        /// </summary>
        /// <param name="DTOHighlight">Highlight DTO to map from.</param>
        /// <returns>A Entity Highlight.</returns>
        public Highlight mapEntityHighlight(DTOHighlightColor DTOHighlight)
        {
            return map<Highlight>(DTOHighlight);
        }

        /// <summary>
        /// Map DTO Complete to entity Complete.
        /// </summary>
        /// <param name="Complete">Complete entity to map from.</param>
        /// <returns>A DTO Complete.</returns>
        public DTOComplete mapDTOComplete(Complete Complete)
        {
            return map<DTOComplete>(Complete);
        }

        /// <summary>
        /// Map Entity Complete to DTO Complete.
        /// </summary>
        /// <param name="Complete">Complete DTO to map from.</param>
        /// <returns>A Entity Complete.</returns>
        public Complete mapEntityComplete(DTOComplete Complete)
        {
            return map<Complete>(Complete);
        }

        /// <summary>
        /// Map DTO timer to entity timer.
        /// </summary>
        /// <param name="timer">timer entity to map from.</param>
        /// <returns>A DTO timer.</returns>
        public DTOTimer mapDTOTimer(Timer timer)
        {
            return map<DTOTimer>(timer);
        }

        /// <summary>
        /// Map Entity timer to DTO Complete.
        /// </summary>
        /// <param name="timer">timer DTO to map from.</param>
        /// <returns>A Entity timer.</returns>
        public Timer mapEntityTimer(DTOTimer timer)
        {
            return map<Timer>(timer);
        }

        #region Properties
        /// <summary>
        /// Property that contains the mapping configuration.
        /// </summary>
        private static AutoMapper.IMapper mapperObject;
        public static AutoMapper.IMapper MapperObject
        {
            get
            {
                if (mapperObject == null)
                {
                    mapperObject = initializeMappings();
                }
                return mapperObject;
            }
        }
        #endregion
    }
}

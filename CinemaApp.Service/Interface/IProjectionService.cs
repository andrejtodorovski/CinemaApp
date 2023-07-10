using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Service.Interface
{
    public interface IProjectionService
    {
        public ProjectionDTO GetDetailsForProjection(Guid? id);
        public List<ProjectionDTO> GetAllProjections();
        public Projection CreateNewProjection(Projection p);
        public Projection UpdateExistingProjection(Projection p);
        public Projection DeleteProjection(Guid id);
        public Projection GetProjection(Guid? id);

    }
}

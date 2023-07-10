using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaApp.Service.Implementation
{
    public class ProjectionService : IProjectionService
    {

        private readonly IRepository<Projection> _projectionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Movie> _movieRepository;


        public ProjectionService(IRepository<Projection> projectionRepository, IUserRepository userRepository, IRepository<Movie> movieRepository)
        {
            _projectionRepository = projectionRepository;
            _userRepository = userRepository;
            _movieRepository = movieRepository;
        }
        public Projection CreateNewProjection(Projection p)
        {
            p.Id = Guid.NewGuid();
            return _projectionRepository.Insert(p);

        }

        public Projection DeleteProjection(Guid id)
        {
            return _projectionRepository.Delete(_projectionRepository.Get(id));
        }

        public List<ProjectionDTO> GetAllProjections()
        {
            // return all projections but map them to projectionDTO
            List<Projection> projections = this._projectionRepository.GetAll().ToList();
            List<ProjectionDTO> result = new List<ProjectionDTO>();
            foreach (var item in projections)
            {
                var movie = this._movieRepository.Get(item.MovieId);
                result.Add(new ProjectionDTO
                {
                    Id = item.Id,
                    MovieName = movie.MovieTitle,
                    MovieId = item.MovieId,
                    TimeOfProjection = item.TimeOfProjection,
                    Price = item.Price,
                    TicketsAvailable = item.TicketsAvailable
                });
            }
            return result;
        }

        public ProjectionDTO GetDetailsForProjection(Guid? id)
        {
            var v = this._projectionRepository.Get(id);
            var movie = this._movieRepository.Get(v.MovieId);
            return new ProjectionDTO
            {
                Id = v.Id,
                MovieName = movie.MovieTitle,
                MovieId = v.Movie.Id,
                TimeOfProjection = v.TimeOfProjection,
                Price = v.Price,
                TicketsAvailable = v.TicketsAvailable
            };
        }

        public Projection GetProjection(Guid? id)
        {
            return _projectionRepository.Get(id);
        }

        public Projection UpdateExistingProjection(Projection p)
        {
            return _projectionRepository.Update(p);
        }
    }
}

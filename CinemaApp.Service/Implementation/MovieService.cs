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
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IUserRepository _userRepository;

        public MovieService(IRepository<Movie> movieRepository, IUserRepository userRepository)
        {
            _movieRepository = movieRepository;
            _userRepository = userRepository;
        }


        public Movie CreateNewMovie(Movie m)
        {
            m.Id = Guid.NewGuid();
            return _movieRepository.Insert(m);
        }

        public Movie DeleteMovie(Guid id)
        {
            return _movieRepository.Delete(_movieRepository.Get(id));
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            return _movieRepository.GetAll().ToList();
        }

        public Movie GetDetailsForMovie(Guid? id)
        {
            return _movieRepository.Get(id);
        }

        public Movie UpdeteExistingMovie(Movie m)
        {
            return _movieRepository.Update(m);  
        }
    }
}

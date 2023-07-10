using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Service.Interface
{
    public interface IMovieService
    {
        public IEnumerable<Movie> GetAllMovies();
        public Movie GetDetailsForMovie(Guid? id);
        public Movie CreateNewMovie(Movie m);
        public Movie UpdeteExistingMovie(Movie m);
        public Movie DeleteMovie(Guid id);
    }
}

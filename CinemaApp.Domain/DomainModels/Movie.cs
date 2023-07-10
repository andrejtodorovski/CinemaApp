using System.Collections.Generic;

namespace CinemaApp.Domain.DomainModels
{
    public class Movie : BaseEntity
    {
        public string MovieTitle { get; set; }
        public string MovieDescription { get; set; }
        public string MovieGenre { get; set; }
        public string MovieDirector { get; set; }
        public string MovieCast { get; set; }
        public int MovieDuration { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }

    }
}

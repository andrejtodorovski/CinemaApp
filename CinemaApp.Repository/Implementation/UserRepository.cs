using CinemaApp.Domain.Identity;
using CinemaApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaApp.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<CinemaAppUser> entities;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<CinemaAppUser>();
        }
        public CinemaAppUser Get(string id)
        {
            return entities
                .Include(z => z.UserShoppingCart)
                .Include("UserShoppingCart.Tickets")
                .Include("UserShoppingCart.Tickets.Projection")
                .Include("UserShoppingCart.Tickets.Projection.Movie")
                .SingleOrDefault(z => z.Id == id);

        }

        public IEnumerable<CinemaAppUser> GetAll()
        {
            return entities
                .Include(z => z.UserShoppingCart)
                .Include("UserShoppingCart.Tickets")
                .Include("UserShoppingCart.Tickets.Projection")
                .Include("UserShoppingCart.Tickets.Projection.Movie")
                .AsEnumerable();
        }
    }
}

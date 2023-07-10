using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Repository.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(Guid? id);
        T Insert(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}

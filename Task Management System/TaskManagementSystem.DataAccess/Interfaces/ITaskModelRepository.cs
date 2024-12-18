using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Domain;

namespace TaskManagementSystem.DataAccess.Interfaces
{
    //a generic interface when writing CRUD operations
    public interface ITaskModelRepository<T> where T : BaseEntity
    {
        List<T> GetAll(int pageNumber, int pageSize);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}

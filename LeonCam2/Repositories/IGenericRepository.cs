// IGenericRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task DeleteRowAsync(Guid id);

        Task<T> GetAsync(Guid id);

        Task<int> SaveRangeAsync(IEnumerable<T> list);

        Task UpdateAsync(T t);

        Task InsertAsync(T t);
    }
}

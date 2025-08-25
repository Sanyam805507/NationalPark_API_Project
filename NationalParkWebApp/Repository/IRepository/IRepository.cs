namespace NationalParkWebApp.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string Url, int Id);                              // FIND
        Task<IEnumerable<T>>GetAllAsync(string Url);                       //Display   
        Task<bool> CreateAsync(string Url, T ObjToCreate);                 //Create
        Task<bool> UpdateAsync(string Url, T ObjToUpdate);                 //Update
        Task<bool> DeleteAsync(string Url, int Id);                 //Delete
    }
}

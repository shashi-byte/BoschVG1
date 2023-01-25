using BoschVG1.Models;

namespace BoschVG1.DAL
{
    public interface IDataRepository
    {
        public Task<int?> GetJobIdDetails();
        public Task SaveData(List<ModelClass> vg);
    }
}

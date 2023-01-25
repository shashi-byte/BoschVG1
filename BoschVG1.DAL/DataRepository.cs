using BoschVG1.Models;

namespace BoschVG1.DAL
{
    public class DataRepository : DatabaseConnection, IDataRepository
    {
        public DataRepository(string connectionString) : base(connectionString)
        {
        }


        public async Task<int?> GetJobIdDetails()
        {
            using (var db = GetDatabase())
            {
                const string sql = $"select max(job_id) job_id from vg1";
                return await db.SingleOrDefaultAsync<int?>(sql);
            }
        }

        public Task SaveData(List<VisonGate1> vg)
        {
            using (var db = GetDatabase())
            {
                //db.BeginTransaction();
                //const string sql = $"insert into vg1 values (@box_id, @part_nu, @quantity, @job_id, @status)";
                //db.Execute(sql, new { vg.box_id, vg.part_nu, vg.quantity, vg.job_id, vg.status });
                //db.CompleteTransaction();
                db.InsertBulk<VisonGate1>(vg);
            }
            return Task.CompletedTask;
        }
    }
}

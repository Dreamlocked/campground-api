using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ms_billing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms_billing.Data.Repository
{
    public class BillingsRepository
    {
        private readonly IMongoCollection<Billing> _billingsCollection;

        public BillingsRepository(
            IOptions<BillingStoreDatabaseSettings> billingStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                billingStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                billingStoreDatabaseSettings.Value.DatabaseName);

            _billingsCollection = mongoDatabase.GetCollection<Billing>(
                billingStoreDatabaseSettings.Value.BillingsCollectionName);
        }

        public async Task<List<Billing>> GetAsync() =>
            await _billingsCollection.Find(_ => true).ToListAsync();

        public async Task<Billing?> GetAsync(string id) =>
            await _billingsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Billing?> GetAsyncByTenant(string id) =>
            await _billingsCollection.Find(x => x.TenantId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Billing newBilling) =>
            await _billingsCollection.InsertOneAsync(newBilling);

        public async Task UpdateAsync(string id, Billing updatedBilling) =>
            await _billingsCollection.ReplaceOneAsync(x => x.Id == id, updatedBilling);

        public async Task RemoveAsync(string id) =>
            await _billingsCollection.DeleteOneAsync(x => x.Id == id);
    }
}

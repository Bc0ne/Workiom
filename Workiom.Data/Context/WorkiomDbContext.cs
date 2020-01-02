namespace Workiom.Data.Context
{
    using MongoDB.Driver;
    using Workiom.Core.Company;
    using Workiom.Core.Contact;

    public class WorkiomDbContext
    {
        private readonly IMongoDatabase _database = null;

        public WorkiomDbContext(DatabaseSettings connectionStringSettings)
        {
            var client = new MongoClient(connectionStringSettings.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(connectionStringSettings.Database);
            }
        }

        public IMongoCollection<Company> Companies
        {
            get
            {
                return _database.GetCollection<Company>("Company");
            }
        }

        public IMongoCollection<Contact> Contacts
        {
            get
            {
                return _database.GetCollection<Contact>("Contact");
            }
        }
    }
}

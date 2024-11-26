using System;
using System.Reflection;
using Data.Services;

namespace Services
{
    public class DataServiceFactory : IDataServiceFactory
    {
       
        public DataServiceFactory()
        {
          
        }

        public IDataService CreateDataService()
        {
            string connectionString = "Data Source=LAPTOP-7HNFDGCO\\SQLEXPRESS;Initial Catalog=Reconciliation;User ID=vm;Password=Matrix@291;Pooling=False";

            return new SQLServerDataService(connectionString);
        }
    }
}

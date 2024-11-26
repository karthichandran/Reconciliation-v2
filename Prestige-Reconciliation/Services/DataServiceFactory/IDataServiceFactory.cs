
using Data.Services;

namespace Services
{
    public interface IDataServiceFactory
    {
        IDataService CreateDataService();
    }
}

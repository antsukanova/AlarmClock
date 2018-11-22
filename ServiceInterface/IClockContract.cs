using System;
using System.ServiceModel;
using AlarmClock.DBModels;

namespace AlarmClock.ServiceInterface
{
    [ServiceContract]
    public interface IClockContract
    {
        [OperationContract] bool UserExists(string login);
        [OperationContract] User GetUserByLogin(string login);
        [OperationContract] User GetUserByGuid(Guid guid);
        [OperationContract] void AddUser(User user);
        [OperationContract] void UpdateUser(User user);

        [OperationContract] Clock AddClock(Clock clock);
        [OperationContract] void SaveClock(Clock clock);
        [OperationContract] void DeleteClock(Clock clock);
    }
}

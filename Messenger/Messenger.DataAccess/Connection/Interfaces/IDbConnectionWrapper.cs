using System.Data;

namespace Messenger.DataAccess.Connection.Interfaces
{
    public interface IDbConnectionWrapper
    {
        IDbConnection Connection { get; }
    }
}
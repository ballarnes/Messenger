using Dapper;

namespace Messenger.DataAccess.Infrastructure.Interfaces
{
    public interface IStoredProcedureManager
    {
        public DynamicParameters GetParameters<T>(T dto) where T : class;
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace Product.API.DBConnection
{
    public class DataAccessManager<T> where T : DbContext
    {
        private T _dbContext;
        public Dictionary<string, string> outPutParamVaLue { get; set; } = new Dictionary<string, string>();
        private bool isTrasaction { get; set; } = false;
        private IDbContextTransaction _transaction { get; set; }
        private DbCommand _dbCommand { get; set; }

        public async Task GetDataAccess(T dbContext, bool _isTrasaction = false)
        {
            _dbContext = dbContext;
            isTrasaction = _isTrasaction;
            await createCommandAsync();
        }

        public async Task createCommandAsync()
        {
            _dbCommand = _dbContext.Database.GetDbConnection().CreateCommand();
            if (_dbCommand.Connection.State != ConnectionState.Open)
            {
                await _dbCommand.Connection.OpenAsync();
            }
            if (isTrasaction)
            {
                _transaction = await _dbContext.Database.BeginTransactionAsync();
                _dbCommand.Transaction = _transaction.GetDbTransaction();
            }
        }

        // param direction output
        public void AddOutputParam(string name, SqlDbType type)
        {
            var sqlParam = new SqlParameter
            {
                ParameterName = "@" + name,
                SqlDbType = type,
                Direction = ParameterDirection.Output,
                Size = 100
            };
            _dbCommand.Parameters.Add(sqlParam);
        }
        // for param direction input
        public void AddParam(string name, object value, SqlDbType type, ParameterDirection direction = ParameterDirection.Input)
        {
            var sqlParam = new SqlParameter
            {
                ParameterName = "@" + name,
                SqlDbType = type,
                Value = value,
                Direction = direction,
                Size = 100
            };
            _dbCommand.Parameters.Add(sqlParam);
        }

        public async Task executeNonQueryAsync(string storeProcedureName)
        {
            if (_dbCommand.Connection.State != ConnectionState.Open)
            {
                await _dbCommand.Connection.OpenAsync();
            }

            _dbCommand.CommandText = storeProcedureName;
            _dbCommand.CommandType = CommandType.StoredProcedure;

            await _dbCommand.ExecuteNonQueryAsync();

            foreach (SqlParameter param in _dbCommand.Parameters)
            {
                if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
                {
                    outPutParamVaLue[param.ParameterName.Replace("@", "")] = param.Value?.ToString();
                }
            }
        }

        public async Task<DbDataReader> executeAsReaderAsync(string storeProcedureName)
        {
            if (_dbCommand.Connection.State != ConnectionState.Open)
            {
                await _dbCommand.Connection.OpenAsync();
            }

            _dbCommand.CommandText = storeProcedureName;
            _dbCommand.CommandType = CommandType.StoredProcedure;

            var data = await _dbCommand.ExecuteReaderAsync();

            foreach (SqlParameter param in _dbCommand.Parameters)
            {
                if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
                {
                    outPutParamVaLue[param.ParameterName.Replace("@", "")] = param.Value?.ToString();
                }
            }

            return data;
        }

        public async Task commitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task rollbackAsync()
        {
            await _transaction.RollbackAsync();
        }
        public async Task disposeAsync()
        {
            try
            {
                await _transaction.DisposeAsync();
            }
            catch
            {

            }

            _dbCommand.Dispose();

        }
    }
}

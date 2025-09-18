using System.Data;
using Npgsql;

namespace CodeJanitor.Platform.GitLab.Infrastructure.Context;

public sealed class DatabaseContext(string connectionString)
{
    public async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> operation)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        return await operation(connection);
    }

    public async Task<T> TransactAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> operation)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var result = await operation(connection, transaction);
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
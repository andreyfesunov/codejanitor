using System.Data;
using CodeJanitor.Shared.Infrastructure.Factories;
using Npgsql;

namespace CodeJanitor.Shared.Infrastructure.Contexts;

public sealed class DatabaseContext(IDatabaseContextFactory factory)
{
    public async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> operation)
    {
        await using var connection = new NpgsqlConnection(await factory.GetConnectionString());
        await connection.OpenAsync();
        return await operation(connection);
    }

    public async Task<T> TransactAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> operation)
    {
        await using var connection = new NpgsqlConnection(await factory.GetConnectionString());
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
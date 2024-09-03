using Microsoft.Data.Sqlite;
using Dapper;
using XSplitter.Models;

public class DbConnection : IDisposable
{
    SqliteConnection _connection;
    public DbConnection() {
        _connection = new("Data Source=XSplitter.db");
        _connection.Open();
    }

    public async Task<IEnumerable<User>> GetUsersAsync() {
        return await _connection.QueryAsync<User>("SELECT * FROM Users");
    }

    public async Task<IEnumerable<Expense>> GetExpensesAsync() {
        return await _connection.QueryAsync<Expense>("SELECT * FROM Expenses");
    }

    public async Task<Expense?> GetExpenseAsync(int id) {
        return await _connection.QueryFirstOrDefaultAsync<Expense>("SELECT * FROM Expenses WHERE ID=@id", new {id=id});
    }

    public async Task<int> AddExpense(Expense expense) {
        return await _connection.ExecuteScalarAsync<int>("INSERT INTO Expenses (UserId, Description, Amount) VALUES (@UserId, @Description, @Amount); SELECT last_insert_rowid()",expense);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
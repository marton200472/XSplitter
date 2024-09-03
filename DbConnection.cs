using Microsoft.Data.Sqlite;
using Dapper;
using XSplitter.Models;

public class DbConnection : IDisposable
{
    SqliteConnection _connection;
    public DbConnection() {
        _connection = new("Data Source=data/XSplitter.db");
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

    public async Task AddOwings(IEnumerable<Owing> owings) {
        foreach (var item in owings)
        {
            await _connection.ExecuteAsync("INSERT INTO Owings (UserId, ExpenseId, Amount) VALUES (@UserId, @ExpenseId, @Amount);",item);
        }
    }

    public async Task<IEnumerable<OwingToUser>> GetOwingsForUser(int userId) {
        return await _connection.QueryAsync<Owing,Expense, User, OwingToUser>("SELECT SUM(Owings.Amount) AS Amount, GROUP_CONCAT(Expenses.Description, ', ') AS Description, Users.Id, Users.Name FROM Owings INNER JOIN Expenses ON Owings.ExpenseId = Expenses.ID INNER JOIN Users ON Expenses.UserId = Users.Id WHERE Owings.UserId=@id GROUP BY Expenses.UserId;",(x,z,y)=>new OwingToUser() {User = y, Amount = x.Amount, Expenses = z.Description},new {id=userId}, splitOn: "Description,Id");
    }

    public async Task ClearOwingsForUserAsync(int userId, int targetId) {
        await _connection.ExecuteAsync("DELETE FROM Owings WHERE Owings.UserId = @userId AND ExpenseId IN (SELECT Id FROM Expenses WHERE UserId = @targetId);", new{userId, targetId});
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
using FinanceApp.Models;

namespace FinanceApp.Data.Service
{
	public interface IExpensesService
	{
		Task<IEnumerable<Expense>> GetAll();
		Task Add(Expense expense);
		Task<Expense?> GetExpense(int id);
		Task Edit(Expense expense);
		Task Delete (Expense expense);
		IQueryable GetChartData();
	}
}

using FinanceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data.Service
{
	public class ExpensesService : IExpensesService
	{
		private readonly FinanceAppContext _context;
		public ExpensesService(FinanceAppContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Expense>> GetAll()
		{
			var expenses = await _context.Expenses.ToListAsync();
			return expenses;
		}

		public async Task Add(Expense expense)
		{
			_context.Expenses.Add(expense);
			await _context.SaveChangesAsync();
		}

		public async Task<Expense?> GetExpense(int id)
		{
			var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
			return expense;
		}

		public async Task Edit(Expense expense)
		{
			var expenseEdit = await _context.Expenses.FindAsync(expense.Id);
			if (expenseEdit != null)
			{
				expenseEdit.Description = expense.Description;
				expenseEdit.Amount = expense.Amount;
				expenseEdit.Category = expense.Category;

				await _context.SaveChangesAsync();
			}
		}

		public async Task Delete(Expense expense)
		{
			throw new NotImplementedException();
		}

		public IQueryable GetChartData()
		{
			var data = _context.Expenses
				.GroupBy(e => e.Category)
				.Select(g => new
				{
					Category = g.Key,
					Total = g.Sum(e => e.Amount)
				});
			return data;
		}
	}
}

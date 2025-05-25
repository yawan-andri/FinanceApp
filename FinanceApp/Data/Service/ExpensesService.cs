using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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
			var expenses = await _context.Expenses
				.Include(c => c.Category)
				.ToListAsync();
			return expenses;
		}
		public async Task<IEnumerable<ChoiceList>> GetAllCategories()
		{
			return await _context.ChoiceLists.ToListAsync();
		}

		public async Task Add(Expense expense)
		{
			_context.Expenses.Add(expense);
			await _context.SaveChangesAsync();
		}

		public async Task<Expense?> GetExpense(int id)
		{
			var expense = await _context.Expenses
				.Include(c => c.Category)
				.FirstOrDefaultAsync(e => e.Id == id);
			return expense;
		}

		public async Task Edit(Expense expense)
		{
			var expenseEdit = await _context.Expenses.FindAsync(expense.Id);
			if (expenseEdit != null)
			{
				expenseEdit.Description = expense.Description;
				expenseEdit.Amount = expense.Amount;
				expenseEdit.CategoryId = expense.CategoryId;

				await _context.SaveChangesAsync();
			}
		}

		public async Task Delete(Expense expense)
		{
			_context.Expenses.Remove(expense);
			await _context.SaveChangesAsync();
		}

		public IQueryable GetChartData()
		{
			var data = _context.Expenses
				.Include(c => c.Category)
				.GroupBy(e => e.Category!.Name)
				.Select(g => new
				{
					Category = g.Key,
					Total = g.Sum(e => e.Amount)
				});
			return data;
		}
	}
}

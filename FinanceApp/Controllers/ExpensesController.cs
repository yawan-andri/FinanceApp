using FinanceApp.Data;
using FinanceApp.Data.Service;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinanceApp.Controllers
{
	public class ExpensesController : Controller
	{
		private readonly IExpensesService _expenseService;
		public ExpensesController(IExpensesService expenseService)
		{
			_expenseService = expenseService;
		}

		private async Task PopulateCategories()
		{
			var categories = await _expenseService.GetAllCategories();
			ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			var categories = await _expenseService.GetAllCategories();
			return Json(categories);
		}

		public async Task<IActionResult> Index()	
		{
			var expenses = await _expenseService.GetAll();
			return View(expenses);
		}

		public async Task<IActionResult> Create()
		{
			await PopulateCategories();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Expense expense)
		{
			if (ModelState.IsValid)
			{
				await _expenseService.Add(expense);
				//return RedirectToAction("Index");
				return Json(new { success = true });
			}
			//return View(expense);
			return BadRequest(ModelState);
		}

		private async Task<bool> IsEditable(int id)
		{
			return await _expenseService.IsEditable(id);
		}

		private IActionResult GetEditFalse()
		{
			TempData["EditError"] = "Data can't be edited because it's has been more than 3 days.";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Edit(int id)
		{
			if (!await IsEditable(id)) 
			{
				return GetEditFalse();
			}

			await PopulateCategories();

			var expense = await _expenseService.GetExpense(id);
			if (expense == null) 
			{
				return NotFound();
			}
			return View(expense);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Expense expense)
		{
			if (id != expense.Id) 
			{
				return BadRequest();
			}

			if (ModelState.IsValid)
			{
				await _expenseService.Edit(expense);
				return RedirectToAction("Index");
			}
			return View(expense);
		}

		public async Task<IActionResult> Delete(int id)
		{
			if (!await IsEditable(id))
			{
				return GetEditFalse();
			}

			var expense = await _expenseService.GetExpense(id);
			if (expense == null)
			{
				return NotFound();
			}
			return View(expense);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteExpense(int id)
		{
			var expense = await _expenseService.GetExpense(id);
			if (expense == null)
			{
				return NotFound();
			}

			await _expenseService.Delete(expense);
			return RedirectToAction("Index");
		}

		public IActionResult GetChart()
		{
			var data = _expenseService.GetChartData();
			return Json(data);
		}
	}
}

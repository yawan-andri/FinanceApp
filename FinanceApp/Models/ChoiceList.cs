namespace FinanceApp.Models
{
	public class ChoiceList
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public List<Expense>? Expenses { get; set; }
	}
}

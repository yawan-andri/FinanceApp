using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Models
{
	public class Expense
	{
		public int Id { get; set; }
		[Required]
		public string Description { get; set; } = null!;
		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage ="Amount needs to be higher than 0")]
		public double Amount { get; set; }
		[Required]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public ChoiceList? Category { get; set; }
		public DateTime Date { get; set; } = DateTime.Now;
	}
}

using FinanceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data
{
	public class FinanceAppContext : DbContext
	{
		public FinanceAppContext(DbContextOptions<FinanceAppContext> options):base(options) { }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ChoiceList>().HasData(
					new ChoiceList { Id = 1, Name = "Makan"},
					new ChoiceList { Id = 2, Name = "Hiburan" },
					new ChoiceList { Id = 3, Name = "Transportasi" }
				);
			base.OnModelCreating(modelBuilder);
		}
		public DbSet<Expense> Expenses { get; set; }
		public DbSet<ChoiceList> ChoiceLists { get; set; }
	}
}

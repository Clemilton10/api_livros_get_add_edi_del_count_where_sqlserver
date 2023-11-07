using Microsoft.EntityFrameworkCore;

namespace Livros.Models;

public class livroContext : DbContext
{
	public DbSet<LivroObj> Livros { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(@"Server=localhost;Database=livros;Trusted_Connection=True;TrustServerCertificate=True;");
		// optionsBuilder.UseSqlServer(@"Server=localhost\MSSQLSERVER01;Database=livros;Trusted_Connection=True;TrustServerCertificate=True;");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<LivroObj>()
			.ToTable("Livro");

		modelBuilder.Entity<LivroObj>()
			.HasKey(p => p.LivroId);

		modelBuilder.Entity<LivroObj>()
			.Property(p => p.Titulo)
			.HasColumnType("varchar(50)")
			.IsRequired()
			.HasDefaultValue("");

		modelBuilder.Entity<LivroObj>()
			.Property(p => p.Autor)
			.HasColumnType("varchar(50)")
			.IsRequired()
			.HasDefaultValue("");

		modelBuilder.Entity<LivroObj>()
			.Property(p => p.AnoPublicacao)
			.HasColumnType("int")
			.IsRequired()
			.HasDefaultValue(0);
	}
}

public class LivroResponseDelete
{
	public int? LivroId { get; set; }
	public int? status_id { get; set; } = 0;
	public string? status { get; set; } = string.Empty;
}

public class LivroResponse
{
	public int? LivroId { get; set; }
	public string? Titulo { get; set; } = string.Empty;
	public string? Autor { get; set; } = string.Empty;
	public int? AnoPublicacao { get; set; } = 0;
	public int? status_id { get; set; } = 0;
	public string? status { get; set; } = string.Empty;
	public List<LivroObj>? rows { get; set; } = null;
}

public class LivroPost
{
	public string? Titulo { get; set; } = string.Empty;
	public string? Autor { get; set; } = string.Empty;
	public int? AnoPublicacao { get; set; } = 0;
}

public class LivroObj
{
	public int? LivroId { get; set; }
	public string? Titulo { get; set; } = string.Empty;
	public string? Autor { get; set; } = string.Empty;
	public int? AnoPublicacao { get; set; } = 0;
}

public class LivroFilter
{
	public string? where { get; set; } = string.Empty;
	public string? order { get; set; } = string.Empty;
	public string? meaning { get; set; } = string.Empty;
	public int? page { get; set; } = 0;
	public int? qtd { get; set; } = 0;
}

public class LivroCount
{
	public string? where { get; set; } = string.Empty;
}

public class LivroTable
{
	public string name { get; } = "livros.dbo.Livro";
}
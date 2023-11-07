using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;

namespace backend003.Controllers;
/*
[Route("/")]
public class teste : ControllerBase
{
	[HttpGet(Name = "/")]
	[SwaggerOperation(Summary = "Obter a página inicial", Tags = new[] { "Home" })]
	public string Get()
	{
		return "Home";
	}

	[HttpPost(Name = "Post")]
	[SwaggerOperation(Summary = "Criar um novo recurso", Tags = new[] { "Recursos" })]
	[SwaggerResponse(201, "Recurso criado com sucesso")]
	[SwaggerResponse(400, "Requisição inválida")]
	public IActionResult Post([FromBody] object data)
	{
		if (data == null)
		{
			return BadRequest("Dados inválidos");
		}
		return CreatedAtAction("Get", new { id = 1 }, data);
	}
}
*/
[Route("livros/form")]
public class livrosForm : ControllerBase
{
	[HttpPost(Name = "livros/form")]
	[SwaggerOperation(Summary = "Criar um novo Livro", Tags = new[] { "Livros" })]
	[SwaggerResponse(201, "Livro adicionado com sucesso")]
	[SwaggerResponse(400, "Requisição inválida")]
	public async Task<IActionResult> Post([FromForm] Livros.Models.LivroCount data)
	{
		return Ok($"{data.where}");
	}
}

[Route("livros/count")]
public class livrosCount : ControllerBase
{
	[HttpPost(Name = "livros/count")]
	[SwaggerOperation(Summary = "Criar um novo Livro", Tags = new[] { "Livros" })]
	[SwaggerResponse(201, "Livro adicionado com sucesso")]
	[SwaggerResponse(400, "Requisição inválida")]
	public async Task<IActionResult> Post([FromBody] Livros.Models.LivroCount data)
	{
		using (var db = new Livros.Models.livroContext())
		{
			var query = db.Livros.AsQueryable();
			if (
				data != null &&
				!string.IsNullOrEmpty(data.where) &&
				data.where != "string"
			)
			{
				query = query.Where(data.where);
			}
			int count = await query.CountAsync();
			return Ok(count);
		}
		return BadRequest("0");
	}
}

[Route("livros/filter")]
public class livrosFilter : ControllerBase
{
	[HttpPost(Name = "livros/filter")]
	[SwaggerOperation(Summary = "Criar um novo Livro", Tags = new[] { "Livros" })]
	[SwaggerResponse(201, "Livro adicionado com sucesso")]
	[SwaggerResponse(400, "Requisição inválida")]
	public async Task<IActionResult> Post([FromBody] Livros.Models.LivroFilter data)
	{
		var rp = new Livros.Models.LivroResponse { };
		using (var db = new Livros.Models.livroContext())
		{
			string table = new Livros.Models.LivroTable().name;

			string where = "";
			if (
				data != null &&
				!string.IsNullOrEmpty(data.where) &&
				data.where != "string"
			)
			{
				where = $"WHERE {data.where}";
			}

			string order = "ORDER BY LivroId ASC";
			if (
				data != null &&
				!string.IsNullOrEmpty(data.order) &&
				data.order != "string" &&
				!string.IsNullOrEmpty(data.meaning) &&
				data.meaning != "string"
			)
			{
				order = $"ORDER BY {data.order} {data.meaning}";
			}

			string limit = "";
			if (
				data != null &&
				!string.IsNullOrEmpty(data.order) &&
				data.order != "string" &&
				!string.IsNullOrEmpty(data.meaning) &&
				data.meaning != "string" &&
				data.page != null &&
				int.TryParse(data.page.ToString(), out var p) &&
				Convert.ToInt32(data.page.ToString()) > 0 &&
				int.TryParse(data.qtd.ToString(), out var q) &&
				Convert.ToInt32(data.qtd.ToString()) > 0
			)
			{
				int qtd = Convert.ToInt32(data.qtd.ToString());
				int page = Convert.ToInt32(data.page.ToString());
				int offset = (page - 1) * qtd;
				limit = $"OFFSET {offset} ROWS FETCH NEXT {qtd} ROWS ONLY";
			}

			var livros = await db.Livros
			.FromSqlRaw($"SELECT * FROM {table} {where} {order} {limit}")
			.ToListAsync();
			rp.rows = livros;
			return Ok(rp);
		}
		rp.status_id = 0;
		rp.status = "Alguma coisa aconteceu";
		return BadRequest(rp);
	}
}

[Route("livros")]
public class livros : ControllerBase
{
	[HttpGet(Name = "livrosGet")]
	public async Task<IActionResult> Get()
	{
		using (var db = new Livros.Models.livroContext())
		{
			// var livros = await db.Livros.ToArrayAsync();
			var livros = await db.Livros.ToListAsync();
			return Ok(livros);
		}
	}

	[HttpPost(Name = "livrosPost")]
	[SwaggerOperation(Summary = "Criar um novo Livro", Tags = new[] { "Livros" })]
	[SwaggerResponse(201, "Livro adicionado com sucesso")]
	[SwaggerResponse(400, "Requisição inválida")]
	public IActionResult Post([FromBody] Livros.Models.LivroPost data)
	{
		var rp = new Livros.Models.LivroResponse
		{
			Titulo = data.Titulo,
			Autor = data.Autor,
			AnoPublicacao = data.AnoPublicacao
		};
		var o = new Livros.Models.LivroObj
		{
			Titulo = data.Titulo,
			Autor = data.Autor,
			AnoPublicacao = data.AnoPublicacao
		};
		if (
			data == null ||
			string.IsNullOrEmpty(data.Titulo) ||
			string.IsNullOrEmpty(data.Autor) ||
			!int.TryParse(data.AnoPublicacao.ToString(), out var a)
		)
		{
			rp.status_id = 0;
			rp.status = "Dados inválidos no JSON.";
			return BadRequest(rp);
		}
		using (var db = new Livros.Models.livroContext())
		{
			db.Livros.Add(o);
			db.SaveChanges();
		}
		rp.LivroId = o.LivroId;
		rp.status_id = 1;
		rp.status = "success";
		return Ok(rp);
	}

	[HttpPut("{id}", Name = "livrosPut")]
	[SwaggerOperation(Summary = "Atualizar um Livro existente", Tags = new[] { "Livros" })]
	[SwaggerResponse(200, "Livro atualizado com sucesso")]
	[SwaggerResponse(400, "Requisição inválida")]
	[SwaggerResponse(404, "Livro não encontrado")]
	public IActionResult Put(int id, [FromBody] Livros.Models.LivroPost data)
	{
		var rp = new Livros.Models.LivroResponse
		{
			LivroId = id,
			Titulo = data.Titulo,
			Autor = data.Autor,
			AnoPublicacao = data.AnoPublicacao
		};
		if (
			data == null ||
			string.IsNullOrEmpty(data.Titulo) ||
			string.IsNullOrEmpty(data.Autor) ||
			!int.TryParse(data.AnoPublicacao.ToString(), out var a)
		)
		{
			rp.status_id = 0;
			rp.status = "Dados inválidos no JSON.";
			return BadRequest(rp);
		}
		using (var db = new Livros.Models.livroContext())
		{
			var rs = db.Livros.FirstOrDefault(l => l.LivroId == id);
			if (rs == null)
			{
				rp.status_id = -1;
				rp.status = "Dados não encontrados.";
				return NotFound(rp);
			}
			rs.Titulo = data.Titulo;
			rs.Autor = data.Autor;
			rs.AnoPublicacao = data.AnoPublicacao;
			db.SaveChanges();
		}
		rp.status_id = 1;
		rp.status = "success";
		return Ok(rp);
	}

	[HttpDelete("{id}", Name = "livrosDelete")]
	[SwaggerOperation(Summary = "Excluir um Livro existente", Tags = new[] { "Livros" })]
	[SwaggerResponse(204, "Livro excluído com sucesso")]
	[SwaggerResponse(404, "Livro não encontrado")]
	public IActionResult Delete(int id)
	{
		var rp = new Livros.Models.LivroResponseDelete
		{
			LivroId = id
		};
		using (var db = new Livros.Models.livroContext())
		{
			var existingLivro = db.Livros.FirstOrDefault(l => l.LivroId == id);

			if (existingLivro == null)
			{
				rp.status_id = -1;
				rp.status = "Dados não encontrados.";
				return NotFound(rp);
			}

			db.Livros.Remove(existingLivro);
			db.SaveChanges();
		}

		//return NoContent();
		rp.status_id = 1;
		rp.status = "success";
		return Ok(rp);
	}
}
/*
[Route("livro")]
public class LivroGet : ControllerBase
{
	[HttpGet(Name = "livro")]
	public async Task<List<Livros.Models.LivroObj>> Get()
	{
		using (var db = new Livros.Models.livroContext())
		{
			var l = await db.Livros.ToListAsync();
			return l;
		}
	}
}

[Route("livro2")]
public class Livro2Get : ControllerBase
{
	[HttpGet(Name = "livro2")]
	public async Task<IActionResult> Get()
	{
		using (var db = new Livros.Models.livroContext())
		{
			var livros = await db.Livros.ToArrayAsync();
			return Ok(livros);
		}
	}
}
*/
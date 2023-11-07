using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
[Route("livros")]
public class livros : ControllerBase
{
	[HttpGet(Name = "livrosGet")]
	public async Task<IActionResult> Get()
	{
		using (var db = new Livros.Models.livroContext())
		{
			var livros = await db.Livros.ToArrayAsync();
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
			string.IsNullOrWhiteSpace(data.Titulo) ||
			string.IsNullOrWhiteSpace(data.Autor) ||
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
			string.IsNullOrWhiteSpace(data.Titulo) ||
			string.IsNullOrWhiteSpace(data.Autor) ||
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
using Microsoft.AspNetCore.Mvc;
using ProjektApi.Models;
using ProjektApi.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjektApi.Controllers
{
	[EnableCors("_myAllowSpecificOrigins")]
	[Route("[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		// GET: api/<ProductsController>
		[HttpGet]
		public IEnumerable<Product> Get()
		{
			MySqlManager manager = new MySqlManager();
			if(manager.Open())
			{
				string[][] result = manager.Select("* from products");
				manager.Close();
				for (int i = 0; i < result.Length; i++)
				{
					yield return new Product()
					{
						Id=long.Parse(result[i][0]),
						Symbol=result[i][1],
						Name=result[i][2],
						Price=float.Parse(result[i][3]),
						Quantity=int.Parse(result[i][4])
					};
				}

			}else
			{
				yield return new Product() { Name="Problem z połączeniem z bazą danych"};
			}
		}

		[HttpGet("name={name}")]
		public IEnumerable<Product> Get(string name)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				string[][] result = manager.Select($"* from products where name like '%{name}%'");
				manager.Close();
				if (result.Length > 0)
				{
					for (int i = 0; i < result.Length; i++)
					{
						yield return new Product()
						{
							Id = long.Parse(result[i][0]),
							Symbol = result[i][1],
							Name = result[i][2],
							Price = float.Parse(result[i][3]),
							Quantity = int.Parse(result[i][4])
						};
					}
				}else
				{
					yield return new Product() { Name=$"Brak wyników dla '{name}'"};
				}

			}
			else
			{
				yield return new Product() { Name = "Problem z połączeniem z bazą danych" };
			}
		}
		// GET api/<ProductsController>/5
		[HttpGet("id={id}")]
		public Product Get(long id)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				string[][] result = manager.Select($"* from products where id like {id}");
				manager.Close();
				return new Product()
				{
					Id = long.Parse(result[0][0]),
					Symbol = result[0][1],
					Name = result[0][2],
					Price = float.Parse(result[0][3]),
					Quantity = int.Parse(result[0][4])
				};

			}
			else
			{
				return new Product() { Name = "Problem z połączeniem z bazą danych" };
			}
		}

		[HttpGet("priceRange={min}-{max}")]
		public IEnumerable<Product> GetWithPriceRange(float min,float max)
		{
			MySqlManager manager = new MySqlManager();
			if(manager.Open())
			{
				string[][] results = manager.Select($"* from products where price>={min} and price<={max}");
				manager.Close();

				for (int i = 0; i < results.Length; i++)
				{
					yield return new Product { 
						Id=long.Parse(results[i][0]),
						Symbol=results[i][1],
						Name=results[i][2],
						Price=float.Parse(results[i][3]),
						Quantity=int.Parse(results[i][4])
					};
				}

			}else
			{
				yield return new Product() { Name = "Problem z połączeniem z bazą danych" };
			}
		}

		[HttpGet("quantityRange={min}-{max}")]
		public IEnumerable<Product> GetWithQuantityRange(float min, float max)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				string[][] results = manager.Select($"* from products where quantity>={min} and quantity<={max}");
				manager.Close();

				for (int i = 0; i < results.Length; i++)
				{
					yield return new Product
					{
						Id = long.Parse(results[i][0]),
						Symbol = results[i][1],
						Name = results[i][2],
						Price = float.Parse(results[i][3]),
						Quantity = int.Parse(results[i][4])
					};
				}

			}
			else
			{
				yield return new Product() { Name = "Problem z połączeniem z bazą danych" };
			}
		}

		// POST api/<ProductsController>
		[HttpPost]
		public string Post([FromBody] Product value)
		{
			MySqlManager manager = new MySqlManager();
			if(manager.Open())
			{
				if(manager.Insert($"products(`Symbol`,`Name`,`Price`,`Quantity`) values('{value.Symbol}','{value.Name}','{value.Price.ToString().Replace(',','.')}','{value.Quantity}')"))
				{
					manager.Close();
					return $"Pomyślnie dodano produkt";
				}else
				{
					manager.Close();
					return $"Błąd przy dodawaniu produktu";
				}
			}else
			{
				return $"Problem z połączeniem z bazą";
			}
		}

		// PUT api/<ProductsController>/5
		[HttpPut]
		public string Put([FromBody] Product value)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				string set = "";
				if (value.Symbol != "")
					set += $"symbol='{value.Symbol}',";
				if (value.Name != "")
					set += $"name='{value.Name}',";
				if (value.Price >=0)
					set += $"price='{value.Price}',";
				if (value.Quantity >=0)
					set += $"quantity='{value.Quantity}',";


				if (manager.Update($"products {set.Remove(set.Length-1)} where id like {value.Id}')"))
				{
					manager.Close();
					return $"Pomyślnie zedytowano produkt/y";
				}
				else
				{
					manager.Close();
					return $"Błąd przy edycji";
				}
			}
			else
			{
				return $"Problem z połączeniem z bazą";
			}
		}

		// DELETE api/<ProductsController>/5
		[HttpDelete("id={id}")]
		public void Delete(long id)
		{
			MySqlManager manager = new MySqlManager();
			if(manager.Open())
			{
				manager.Delete($"products where id like {id}");
				manager.Close();
			}
		}
	}
}

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
	public class UsersController : ControllerBase
	{
		// GET: api/<UsersController>
		[HttpGet]
		public IEnumerable<User> Get()
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				string[][] queryResult = manager.Select("* from users");
				manager.Close();
				for (int i = 0; i < queryResult.Length; i++)
				{
					yield return new User()
					{
						Id = long.Parse(queryResult[i][0]),
						Login = queryResult[i][1],
						Password = queryResult[i][2],
						Permissions = int.Parse(queryResult[i][3])
					};
				}

			}
			else
			{
				yield return new User() { Login = "Błąd połączenia z bazą" };
			}

		}

		// GET api/<UsersController>/5
		[HttpGet("id={id}")]
		public User Get(long id)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				string[][] result = manager.Select($"* from users where id like {id}");
				manager.Close();
				if (result.Length > 0)
				{
					return new User()
					{
						Id = long.Parse(result[0][0]),
						Login = result[0][1],
						Password = result[0][2],
						Permissions = int.Parse(result[0][3])
					};
				}
				else
				{
					return new User() { Login = "Użytkownik nie istnieje" };
				}
			}
			else
			{
				return new User() { Login = "Błąd połączenia z bazą" };
			}
		}
		[HttpGet("login={login}")]
		public User Get(string login)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				string[][] result = manager.Select($"* from users where binary login = '{login}'");
				manager.Close();
				if (result.Length > 0)
				{
					return new User()
					{
						Id = long.Parse(result[0][0]),
						Login = result[0][1],
						Password = result[0][2],
						Permissions = int.Parse(result[0][3])
					};
				}
				else
				{
					return new User() { Login = "Użytkownik nie istnieje" };
				}
			}
			else
			{
				return new User() { Login = "Błąd połączenia z bazą" };
			}
		}

		// POST api/<UsersController>
		[HttpPost]
		public string Post([FromBody] User value)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				if (manager.Insert($"users(Login,Password,permissions) values('{value.Login}','{value.Password}','{value.Permissions}')"))
				{
					manager.Close();
					return $"Pomyślnie dodano użytkownika";
				}
				else
				{
					manager.Close();
					return $"Użytkownik z takim loginem już istnieje";
				}
			}
			else
			{
				return "Błąd połączenia z bazą";
			}
		}

		// PUT api/<UsersController>/5
		[HttpPut]
		public string Put([FromBody] User value)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				string set = "";
				if (value.Login != string.Empty)
					set += $"login ='{value.Login}',";
				if (value.Password != string.Empty)
					set += $"password ='{value.Password}',";
				if (value.Permissions > -1)
					set += $"permissions ='{value.Permissions}' ";

				if (manager.Update($"users set {set.Remove(set.Length - 1)} where id like '{value.Id}'"))
				{
					manager.Close();
					return $"Pomyślnie zedytowano dane użytkownika";
				}
				else
				{
					manager.Close();
					return $"Błąd edycji danych użytkownika";
				}
			}
			else
			{
				return "Błąd połączenia z bazą";
			}
		}

		// DELETE api/<UsersController>/5
		[HttpDelete("id={id}")]
		public string Delete(long id)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				if (manager.Delete($"users where id like '{id}'"))
				{
					manager.Close();
					return "Pomyślnie usunieto użytkownika";
				}
				else
				{
					manager.Close();
					return $"Brak rekordów do usunięcia";
				}
			}
			else
			{
				return "Błąd połączenia z bazą";
			}
		}

		[HttpDelete("login={login}")]
		public string Delete(string login)
		{
			MySqlManager manager = new MySqlManager();
			if (manager.Open())
			{
				if (manager.Delete($"users where binary login='{login}'"))
				{
					manager.Close();
					return "Pomyślnie usunieto użytkownika";
				}
				else
				{
					manager.Close();
					return $"Brak rekordów do usunięcia";
				}
			}
			else
			{
				return "Błąd połączenia z bazą";
			}
		}
	}
}

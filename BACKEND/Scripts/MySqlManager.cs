using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using System.IO;

namespace ProjektApi.Scripts
{
	public class MySqlManager
	{
		static string connectionString;
		MySqlConnection connector;

		public MySqlManager()
		{
			connector = new MySqlConnection(connectionString);
		}

		public static void Init(string server, string user, string password, string database)
		{
			connectionString = $"Database={database}; Data Source={server}; User Id={user}; Password={password};SslMode=none";
		}

		public bool Open()
		{
			try
			{
				connector.Open();
				return true;
			}
			catch (Exception e)
			{
				using (StreamWriter sw = new StreamWriter("log.txt"))
				{
					sw.WriteLine(e.Message);
				}
				return false;
			}
		}
		public void Close()
		{
			connector.Close();
		}

		public string[][] Select(string query)
		{
			List<string[]> result = new List<string[]>();
			try
			{
				MySqlCommand cmd = new MySqlCommand("Select "+query, connector);

				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string[] buf = new string[reader.FieldCount];

					for (int i = 0; i < reader.FieldCount; i++)
					{
						buf[i] = reader[i].ToString();
					}
					result.Add(buf);
				}
			}
			catch (Exception e)
			{
				using (StreamWriter sr = new StreamWriter("selectLog.txt"))
				{
					sr.WriteLine(query);
					sr.WriteLine(e.Message);
				}
			}
			return result.ToArray();
		}

		public bool Insert(string query)
		{
			try
			{
				MySqlCommand cmd = new MySqlCommand("Insert into "+query, connector);
				if (cmd.ExecuteNonQuery() > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception e)
			{
				using (StreamWriter sw = new StreamWriter("insertLog.txt"))
				{
					sw.WriteLine(query);
					sw.WriteLine(e.Message);
				}
				return false;
			}
		}

		public bool Update(string query)
		{
			try
			{
				MySqlCommand cmd = new MySqlCommand("Update " + query, connector);
				if (cmd.ExecuteNonQuery() > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception e)
			{
				using (StreamWriter sw = new StreamWriter("updateLog.txt"))
				{
					sw.WriteLine(query);
					sw.WriteLine(e.Message);
				}
				return false;
			}
		}

		public bool Delete(string query)
		{
			try
			{
				MySqlCommand cmd = new MySqlCommand("Delete from " + query, connector);
				if (cmd.ExecuteNonQuery() > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception e)
			{
				using (StreamWriter sw = new StreamWriter("deleteLog.txt"))
				{
					sw.WriteLine(query);
					sw.WriteLine(e.Message);
				}
				return false;
			}
		}

		
	}
}

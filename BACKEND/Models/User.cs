using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektApi.Models
{
	public class User
	{
		public long Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public int Permissions { get; set; }
	}
}

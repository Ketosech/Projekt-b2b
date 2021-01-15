using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektApi.Models
{
	public class Product
	{
		public long Id { get; set; }
		public string Symbol { get; set; }
		public string Name { get; set; }
		public float Price { get; set; }
		public int Quantity { get; set; }
	}
}

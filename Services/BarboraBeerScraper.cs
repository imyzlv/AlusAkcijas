using System;
using HtmlAgilityPack;
using AlusAkcijas.Models;

namespace AlusAkcijas.Services
{
	public class BarboraBeerScraper
	{
		public static async Task<IEnumerable<Beer>> GetBarboraBeers()
		{
            HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            List<Beer> beer = new List<Beer>();
			return beer.ToList();
        }
	}
}


using System;
using HtmlAgilityPack;

namespace AlusAkcijas.Services
{
    public class BeerScraper
    {
        public static async void GetRimiBeers()
        {
            HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            //int pageCount = await RimiPageCount();
            int totalBeers = 0;
            //for (int i = 1; i <= pageCount; i++)
            //{
            //    string url = "https://www.rimi.lv/e-veikals/lv/produkti/alkoholiskie-dzerieni/alus/c/SH-1-12?page=" + i.ToString() + "&pageSize=100&query=%3Arelevance%3AallCategories%3ASH-1-12%3AassortmentStatus%3AinAssortment";
            //    HtmlDocument doc = web.Load(url);
            //    foreach (var item in doc.DocumentNode.SelectNodes("//p[@class='card__name']"))
            //    {
            //        Console.WriteLine(item.InnerText);
            //        totalBeers++;
            //    }
            //}
            //Console.WriteLine("Total beers: {0}", totalBeers);

            string url = "https://www.rimi.lv/e-veikals/lv/produkti/alkoholiskie-dzerieni/alus/c/SH-1-12?page=1&pageSize=100&query=%3Arelevance%3AallCategories%3ASH-1-12%3AassortmentStatus%3AinAssortment";
            HtmlDocument doc = web.Load(url);
            foreach (var item in doc.DocumentNode.SelectNodes("//li[@class='product-grid__item']"))
            {
                //string title = item.SelectSingleNode(".//li[@class='product-grid__item']").InnerText.Trim();

                // load the beer names
                string beerName = item.SelectSingleNode(".//p[@class='card__name']").InnerText.Trim('\r', '\n', '\t');
                Console.Write(beerName);
                Console.Write(" ");

                // get pricing
                double beerCost = 0;
                var price = item.SelectSingleNode(".//div[@class='price-tag card__price']").InnerText.Trim('\n', '\t', '\r');
                {
                    // do some ugly cleanup of the pricing value
                    price = price.TrimStart().TrimEnd();
                    price = price.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
                    price = price.Remove(price.Length - 6);
                }
                beerCost = double.Parse(price) / 100;
                Console.WriteLine(beerCost);
            }
        }

        public static async Task<int> RimiPageCount()
        {
            int pageCount = 0;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://www.rimi.lv/e-veikals/lv/produkti/alkoholiskie-dzerieni/alus/c/SH-1-12?page=1&pageSize=100&query=%3Arelevance%3AallCategories%3ASH-1-12%3AassortmentStatus%3AinAssortment");
            foreach (var item in doc.DocumentNode.SelectNodes("//li[@class='pagination__item']"))
            {
                pageCount++;
            }
            return pageCount;
        }
    }
}


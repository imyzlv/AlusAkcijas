using System;
using HtmlAgilityPack;
using AlusAkcijas.Models;

namespace AlusAkcijas.Services
{
    public class RimiBeerScraper
    {
        public static async Task<IEnumerable<Beer>> GetRimiBeers()
        {
            HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            int pageCount = await RimiTotalBeersPageCount();
            //int pageCount = 1;
            List<Beer> beer = new List<Beer>();

            for (int i = 1; i <= pageCount; i++)
            {
                string url = "https://www.rimi.lv/e-veikals/lv/produkti/alkoholiskie-dzerieni/alus/c/SH-1-12?page=" + i.ToString() + "&pageSize=100&query=%3Arelevance%3AallCategories%3ASH-1-12%3AassortmentStatus%3AinAssortment";
                HtmlDocument doc = web.Load(url);
                foreach (var item in doc.DocumentNode.SelectNodes("//li[@class='product-grid__item']"))
                {
                    // load the beer names
                    string beerName = null;
                    if (item.SelectSingleNode(".//p[@class='card__name']") != null)
                    {
                        beerName = item.SelectSingleNode(".//p[@class='card__name']").InnerText.Trim('\r', '\n', '\t');
                    }

                    // load prices
                    double beerCost = 0;
                    if (item.SelectSingleNode(".//div[@class='price-tag card__price']") != null)
                    {
                        var price = item.SelectSingleNode(".//div[@class='price-tag card__price']").InnerText.Trim('\n', '\t', '\r');
                        // do some ugly cleanup of the pricing value
                        price = price.TrimStart().TrimEnd();
                        price = price.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
                        // remove misc symbols from the end of the string
                        price = price.Remove(price.Length - 6);
                        beerCost = double.Parse(price) / 100;
                    }

                    // load dicounted prices
                    double beerCostOld = 0;
                    if (item.SelectSingleNode(".//div[@class='old-price-tag card__old-price']") != null)
                    {
                        var oldPrice = item.SelectSingleNode(".//div[@class='old-price-tag card__old-price']").InnerText.Trim('\n', '\t', '\r');
                        oldPrice = oldPrice.TrimStart().TrimEnd();
                        oldPrice = oldPrice.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
                        // remove the comma from old price
                        oldPrice = oldPrice.Replace(",", "");
                        //remove the EUR mark from the end
                        oldPrice = oldPrice.Remove(oldPrice.Length - 1);
                        beerCostOld = double.Parse(oldPrice) / 100;
                    }

                    //fetch the picture
                    string imgUrl = null;
                    if(item.SelectSingleNode(".//div[@class='card__image-wrapper']")!=null)
                    {
                        var iUrl = item.SelectSingleNode(".//div[@class='card__image-wrapper']//div//img").GetAttributeValue("src", "");
                        imgUrl = iUrl.ToString();
                    }

                    //add to the list, if the beer is actually discounted
                    if (beerCostOld > 0)
                    {
                        beer.Add(new Beer
                        {
                            Name = beerName,
                            Price = beerCost,
                            OldPrice = beerCostOld,
                            ImageUrl = imgUrl
                        });
                    }
                }
            }
                return beer.ToList();
        }

        public static async Task<int> RimiTotalBeersPageCount()
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


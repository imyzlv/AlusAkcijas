using System;
using HtmlAgilityPack;
using AlusAkcijas.Models;

namespace AlusAkcijas.Services
{
    public class BarboraBeerScraper
    {
        //TODO
        //1) refactor the code to use config parameter for either Barbora or Rimi
        //2) get page count from Barbora
        //3) Figure out how to get only discounted beers and ommit others to speed up the process.

        public static async Task<IEnumerable<Beer>> GetBarboraBeers()
        {
            HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            List<Beer> beer = new List<Beer>();
            //light beer url
            string url = "https://www.barbora.lv/dzerieni/alus-sidri-un-kokteili/gaisais-alus";
            HtmlDocument doc = web.Load(url);
            foreach (var item in doc.DocumentNode.SelectNodes("//div[@class='b-product--wrap2 b-product--desktop-grid']"))
            {
                // load the beer names
                string beerName = null;
                if (item.SelectSingleNode(".//span[@itemprop='name']") != null)
                {
                    beerName = item.SelectSingleNode(".//span[@itemprop='name']").InnerText.Trim('\r', '\n', '\t');
                }


                // load prices
                double beerCost = 0;
                if (item.SelectSingleNode(".//span[@class='b-product-price-current-number']") != null)
                {
                    //var price = item.SelectSingleNode(".//div[@class='price-tag card__price']").InnerText.Trim('\n', '\t', '\r');
                    var price = item.SelectSingleNode(".//span[@class='b-product-price-current-number']").InnerText.Trim('\r', '\n', '\t');
                    // do some ugly cleanup of the pricing value
                    price = price.TrimStart().TrimEnd();
                    price = price.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "").Replace(",", "").Replace("€", "");
                    // remove misc symbols from the end of the string
                    beerCost = double.Parse(price) / 100;
                }

                // load dicounted prices

                double beerCostOld = 0;
                if (item.SelectSingleNode(".//del[@class='b-product-crossed-out-price']") != null)
                {
                    var oldPrice = item.SelectSingleNode(".//del[@class='b-product-crossed-out-price']").InnerText.Trim('\n', '\t', '\r');
                    oldPrice = oldPrice.TrimStart().TrimEnd();
                    //Perform a bit of misc char removal
                    oldPrice = oldPrice.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "").Replace(",", "").Replace("€", "");
                    beerCostOld = double.Parse(oldPrice) / 100;
                }
                
                //fetch the picture url
                string imgUrl = null;
                if (item.SelectSingleNode(".//div[@class='b-product-wrap-img']") != null)
                {
                    var iUrl = item.SelectSingleNode(".//div[@class='b-product-wrap-img']//a//img").GetAttributeValue("src", "");
                    imgUrl = iUrl.ToString();
                }

                //Check, if it's in fact discounted beer. Only then add it to the list
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
            return beer.ToList();
        }
    }
}


namespace AlusAkcijas;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		//SemanticScreenReader.Announce(CounterBtn.Text);

		var beerList = await AlusAkcijas.Services.RimiBeerScraper.GetRimiBeers();
		foreach(var beerItem in beerList)
        {
			Console.WriteLine(beerItem.Name);
        }
	}
}



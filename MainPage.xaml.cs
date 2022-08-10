namespace AlusAkcijas;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		//SemanticScreenReader.Announce(CounterBtn.Text);

		var beerList = await AlusAkcijas.Services.RimiBeerScraper.GetRimiBeers();
		foreach(var item in beerList)
        {
			Console.WriteLine(item.Name);
        }
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		var beers = await AlusAkcijas.Services.RimiBeerScraper.GetRimiBeers();
		beerView.ItemsSource = beers;
	}
}



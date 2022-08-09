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

		AlusAkcijas.Services.RimiBeerScraper.GetRimiBeers();
	}
}



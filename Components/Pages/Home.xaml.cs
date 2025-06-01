namespace QanooniRishta.Components.Pages;

public partial class Home : ContentPage
{
	public Home()
	{
		InitializeComponent();
	}

    private async void OnSearchRelationsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("matchedRelations");
    }
    private async void OnAddNewRelationClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("addEditRelations");
    }
}

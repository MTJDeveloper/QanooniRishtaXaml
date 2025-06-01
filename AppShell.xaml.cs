using QanooniRishta.Components.Pages;

namespace QanooniRishta;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("addEditRelations", typeof(AddEditRelations));
        Routing.RegisterRoute("matchedRelations", typeof(MatchedRelations)); // if used
    }



}
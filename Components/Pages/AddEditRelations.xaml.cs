using Microsoft.Maui.Controls;
using QanooniRishta.Models;
using QanooniRishta.Services;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace QanooniRishta.Components.Pages;

public partial class AddEditRelations : ContentPage
{
    private readonly SqlLiteDatabaseService _dbService;
    private MatchRealtion _model = new();
    private bool _isViewMode;

    public AddEditRelations()
    {
        InitializeComponent();
        _dbService = App.Services.GetService<SqlLiteDatabaseService>();
        this.BindingContext = _model;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Extract query parameters (e.g., ?id=5&view=true)
        var uri = new Uri(Shell.Current.CurrentState.Location.ToString());
        var query = HttpUtility.ParseQueryString(uri.Query);

        if (query.AllKeys.Contains("view"))
            _isViewMode = query["view"]?.ToLower() == "true";

        if (query.AllKeys.Contains("id") && int.TryParse(query["id"], out int id))
        {
            var result = await _dbService.GetByIdAsync<MatchRealtion>(id);
            if (result != null)
            {
                _model = result;
                BindingContext = _model;
            }
        }

        // If _isViewMode is true, disable all input controls (example shown later)
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var validationContext = new ValidationContext(_model);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(_model, validationContext, validationResults, true);

        if (!isValid)
        {
            string errors = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
            await DisplayAlert("Validation Failed", errors, "OK");
            return;
        }

        string message;
        if (_model.Id > 0)
        {
            await _dbService.UpdateAsync(_model);
            message = "Record updated successfully!";
        }
        else
        {
            await _dbService.AddAsync(_model);
            message = "Record added successfully!";
        }

        await DisplayAlert("Success", message, "OK");
        await Shell.Current.GoToAsync(".."); // or "matchedRelations" if it's a registered route
    }
}

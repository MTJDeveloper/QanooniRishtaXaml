using Microsoft.Maui.Controls;
using QanooniRishta.Models;
using QanooniRishta.Services;
using System.ComponentModel.DataAnnotations;

namespace QanooniRishta.Components.Pages;

[QueryProperty(nameof(NavigationId), "id")]
[QueryProperty(nameof(NavigationView), "view")]
public partial class AddEditRelations : ContentPage
{
    private readonly SqlLiteDatabaseService _dbService;
    private MatchRealtion _model = new();

    private string _navId;
    private string _navView;

    public string PageIcon { get; set; } = "";
    public string PageTitle { get; set; } = "Add Match Relation ❤️";
    public bool IsFormEditable { get; set; } = true;

    public AddEditRelations()
    {
        InitializeComponent();
        _dbService = App.Services.GetService<SqlLiteDatabaseService>();
        BindingContext = _model;
    }

    public string NavigationId
    {
        set
        {
            _navId = value;
            TryInitialize();
        }
    }

    public string NavigationView
    {
        set
        {
            _navView = value;
            TryInitialize();
        }
    }

    private async void TryInitialize()
    {
        if (_navId == null || _navView == null)
            return;

        IsFormEditable = _navView.ToLower() != "true";
        int.TryParse(_navId, out int id);

        if (id > 0)
        {
            var result = await _dbService.GetByIdAsync<MatchRealtion>(id);
            if (result != null)
            {
                _model = result;
                BindingContext = _model;
            }
        }

        if (!IsFormEditable)
        {
            PageTitle = "View Match Relation ❤️";
            PageIcon = "👁️";
        }
        else if (_model.Id > 0)
        {
            PageTitle = "Edit Match Relation ❤️";
            PageIcon = "✏️";
        }
        else
        {
            PageTitle = "Add Match Relation ❤️";
            PageIcon = "";
        }

        OnPropertyChanged(nameof(PageTitle));
        OnPropertyChanged(nameof(PageIcon));
        OnPropertyChanged(nameof(IsFormEditable));
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
        await Shell.Current.GoToAsync("..");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//matchedRelations");
    }
}

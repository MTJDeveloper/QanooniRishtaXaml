using System.Collections.ObjectModel;
using ClosedXML.Excel;
using Microsoft.Maui.Controls;
using QanooniRishta.Models;
using QanooniRishta.Services;

namespace QanooniRishta.Components.Pages;

public partial class MatchedRelations : ContentPage
{
    private readonly SqlLiteDatabaseService _dbService;
    public ObservableCollection<MatchRealtion> Relations { get; set; } = new();
    public ObservableCollection<MatchRealtion> FilteredRelations { get; set; } = new();

    public MatchedRelations()
    {
        InitializeComponent();
        _dbService = App.Services.GetService<SqlLiteDatabaseService>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _dbService.InitAsync<MatchRealtion>();
        var data = await _dbService.GetAllAsync<MatchRealtion>();

        Relations.Clear();
        foreach (var item in data)
            Relations.Add(item);

        ApplyFilter(string.Empty);

        for (int i = 0; i < Relations.Count; i++)
        {
            Relations[i].Index = (i + 1).ToString();
        }

    }

    private void ApplyFilter(string search)
    {
        FilteredRelations.Clear();

        var filtered = string.IsNullOrWhiteSpace(search)
            ? Relations
            : Relations.Where(r =>
                (r.FirstName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (r.LastName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (r.Address?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                r.Gender.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Age.ToString().Contains(search));

        foreach (var item in filtered)
            FilteredRelations.Add(item);

  
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        // Navigate back or to home page
        await Shell.Current.GoToAsync("//home");
    }

    private async void OnAddRelationClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("/addEditRelations");
    }

    private async void OnExportClicked(object sender, EventArgs e)
    {
        try
        {
        #if ANDROID
                var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                    if (status != PermissionStatus.Granted)
                    {
                        await DisplayAlert("Permission Denied", "Storage permission is required to export the Excel file.", "OK");
                        return;
                    }
                }
        #endif

            var data = await _dbService.GetAllAsync<MatchRealtion>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Matched Relations");

            worksheet.Cell(1, 1).Value = "First Name";
            worksheet.Cell(1, 2).Value = "Last Name";
            worksheet.Cell(1, 3).Value = "Age";
            worksheet.Cell(1, 4).Value = "Gender";
            worksheet.Cell(1, 5).Value = "Address";
            worksheet.Range("A1:E1").Style.Font.Bold = true;

            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = data[i].FirstName;
                worksheet.Cell(i + 2, 2).Value = data[i].LastName;
                worksheet.Cell(i + 2, 3).Value = data[i].Age.ToString();
                worksheet.Cell(i + 2, 4).Value = data[i].Gender.ToString();
                worksheet.Cell(i + 2, 5).Value = data[i].Address;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var bytes = stream.ToArray();

            string filePath;

        #if ANDROID
                var downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)?.AbsolutePath;
                filePath = Path.Combine(downloadsPath, "matched_relations.xlsx");
        #elif WINDOWS
                    var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
                    filePath = Path.Combine(downloadsPath, "matched_relations.xlsx");
        #else
                filePath = Path.Combine(FileSystem.AppDataDirectory, "matched_relations.xlsx");
        #endif

            await File.WriteAllBytesAsync(filePath, bytes);

            await DisplayAlert("Success", "Exported Excel file successfully.", "OK");

            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnViewClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is MatchRealtion item)
        {
            await Shell.Current.GoToAsync($"/addEditRelations?id={item.Id}&view=true");
        }
    }


    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is MatchRealtion item)
        {
            await Shell.Current.GoToAsync($"/addEditRelations?id={item.Id}&view=false");

        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is MatchRealtion item)
        {
            bool confirmed = await DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete {item.FirstName} {item.LastName}?",
                "Delete", "Cancel");

            if (confirmed)
            {
                await _dbService.DeleteAsync(item);
                Relations.Remove(item);
                ApplyFilter(string.Empty);
                OnAppearing();
                await DisplayAlert("Success", "Deleted successfully!", "OK");
            }
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // You can handle item selection if needed
    }
}

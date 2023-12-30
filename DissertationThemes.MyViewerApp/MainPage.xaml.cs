using DissertationThemes.SharedLibrary.Api;
using DissertationThemes.SharedLibrary.DataModels;
using DissertationThemes.SharedLibrary.DTOs;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Application = Microsoft.Maui.Controls.Application;

namespace DissertationThemes.MyViewerApp
{
    public partial class MainPage : ContentPage
    {
        public ApiConnector Api { get; set; }
        public List<ThemeModel> Themes { get; set; }
        public List<StProgramModel> StPrograms { get; set; }
        public List<int> Years { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            Api = new ApiConnector();
            Themes = Api.GetThemes().Result ?? new List<ThemeModel>();
            StPrograms = Api.GetPrograms().Result ?? new List<StProgramModel>();
            Years = Api.GetYears().Result ?? new List<int>();
            ThemesListView.ItemsSource = Themes;
            StProgramComboBox.ItemsSource = StPrograms;
            YearsComboBox.ItemsSource = Years;

        }

        private void YearsComboBox_SelectionChanged(object sender, EventArgs e)
        {
            FilterListView();
        }

        private void StProgramComboBox_SelectionChanged(object sender, EventArgs e)
        {
            FilterListView();
        }

        private void FilterListView()
        {
            var stProgramId = -1;
            var year = -1;
            if (StProgramComboBox.SelectedItem is StProgramModel program)
            {
                stProgramId = program.Id;
            }
            if (YearsComboBox.SelectedItem is not null)
            {
                int.TryParse(YearsComboBox.SelectedItem.ToString(), out year);
            }
            ThemesListView.ItemsSource = Themes.Where(theme => (year > 0 ? theme.Created.Year == year : true) &&
                                                               (stProgramId > 0 ? theme.StProgramId == stProgramId : true))
                                               .ToList();
        }

        private void ClearFiltersButtonClick(object sender, EventArgs e)
        {
            StProgramComboBox.SelectedItem = null;
            YearsComboBox.SelectedItem = null;
            FilterListView();
        }

        private void ShowDetailsButtonClick(object sender, EventArgs e)
        {
            if (ThemesListView.SelectedItem is ThemeModel selectedTheme)
            {
                var stProgram = StPrograms.FirstOrDefault(program => program.Id == selectedTheme.StProgramId);
                if (stProgram is not null)
                {
                    var detailWindow = new ThemeInfoPage(selectedTheme, stProgram);
                    Navigation.PushAsync(detailWindow);
                }
            }
        }

        private void GenerateDocxButtonClicked(object sender, EventArgs e)
        {
            if (ThemesListView.SelectedItem is ThemeModel selectedTheme)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = $"DisertationTheme_{selectedTheme.Id}";
                saveFileDialog.DefaultExt = ".docx";
                saveFileDialog.Filter = "Word Documents|*.docx"; ;
                var result = saveFileDialog.ShowDialog();
                if (true)
                {
                    string filePath = saveFileDialog.FileName;
                    var content = Api.GetDocx(selectedTheme.Id).Result;
                    if (content is not null)
                    {
                        try
                        {
                            // Save the file
                            System.IO.File.WriteAllBytes(filePath, content);
                            // Start the default application associated with the file extension
                            Process p = new Process();
                            p.StartInfo.FileName = filePath;
                            p.StartInfo.UseShellExecute = true;
                            p.Start();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error saving file: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void ExitClick(object sender, EventArgs e)
        {
            if (Application.Current is not null)
            {
                Application.Current.Quit();
            }
        }

        private void ExportCsvClicked(object sender, EventArgs e)
        {
            var stProgramId = -1;
            var year = -1;
            if (StProgramComboBox.SelectedItem is StProgramModel program)
            {
                stProgramId = program.Id;
            }
            if (YearsComboBox.SelectedItem is not null)
            {
                int.TryParse(YearsComboBox.SelectedItem.ToString(), out year);
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = $"DisertationThemes";
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            var result = saveFileDialog.ShowDialog();
            if (true)
            {
                string filePath = saveFileDialog.FileName;
                var content = Api.GetCsv(year, stProgramId).Result;
                if (content is not null)
                {
                    try
                    {
                        // Save the file
                        System.IO.File.WriteAllBytes(filePath, content);
                        // Start the default application associated with the file extension
                        Process p = new Process();
                        p.StartInfo.FileName = filePath;
                        p.StartInfo.UseShellExecute = true;
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving file: {ex.Message}");
                    }
                }
            }
        }

        private void AboutClick(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }

        private void ThemesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DetailsButton.IsEnabled = true;
            DocxButton.IsEnabled = true;
        }
    }

}

using DissertationThemes.SharedLibrary;
using DissertationThemes.SharedLibrary.Api;
using DissertationThemes.SharedLibrary.DTOs;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DissertationThemes.ViewerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApiConnector Api { get; set; }
        public List<ThemeModel> Themes { get; set; }
        public List<StProgramModel> StPrograms { get; set; }
        public List<int> Years { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Api = new ApiConnector();
            Themes = Api.GetThemes().Result ?? new List<ThemeModel>();
            StPrograms = Api.GetPrograms().Result ?? new List<StProgramModel>();
            Years = Api.GetYears().Result ?? new List<int>();
        }

        private void YearsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterListView();
        }

        private void StProgramComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            if (YearsComboBox.SelectedValue is not null)
            {
                int.TryParse(YearsComboBox.SelectedValue.ToString(), out year);
            }
            ThemesListView.ItemsSource = Themes.Where(theme => (year > 0 ? theme.Created.Year == year : true) &&
                                                               (stProgramId > 0 ? theme.StProgramId == stProgramId : true));
        }

        private void ClearFiltersButtonClick(object sender, RoutedEventArgs e)
        {
            StProgramComboBox.SelectedItem = null;
            YearsComboBox.SelectedItem = null;
            FilterListView();
        }

        private void ShowDetailsButtonClick(object sender, RoutedEventArgs e)
        {
            if (ThemesListView.SelectedItem is ThemeModel selectedTheme)
            {
                var stProgram = StPrograms.FirstOrDefault(program => program.Id == selectedTheme.StProgramId);
                if (stProgram is not null)
                {
                    var detailWindow = new DisertationThemeDetailWindow(selectedTheme, stProgram);
                    detailWindow.Show();
                }
            }
        }

        private void GenerateDocxButtonClicked(object sender, RoutedEventArgs e)
        {
            if (ThemesListView.SelectedItem is ThemeModel selectedTheme)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = $"DisertationTheme_{selectedTheme.Id}";
                saveFileDialog.DefaultExt = ".docx";
                saveFileDialog.Filter = "Word Documents|*.docx"; ;
                var result = saveFileDialog.ShowDialog();
                if (result == true)
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

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ExportCsvClicked(object sender, RoutedEventArgs e)
        {
            var stProgramId = -1;
            var year = -1;
            if (StProgramComboBox.SelectedItem is StProgramModel program)
            {
                stProgramId = program.Id;
            }
            if (YearsComboBox.SelectedValue is not null)
            {
                int.TryParse(YearsComboBox.SelectedValue.ToString(), out year);
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = $"DisertationThemes";
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            var result = saveFileDialog.ShowDialog();
            if (result == true)
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

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Version: 1.0 \nAuthor: Michal Murin", "Disertation Theme Viewer", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
using DissertationThemes.SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DissertationThemes.ViewerApp
{
    /// <summary>
    /// Interaction logic for DisertationThemeDetailWindow.xaml
    /// </summary>
    public partial class DisertationThemeDetailWindow : Window
    {
        public ThemeModel Theme { get; set; }
        public StProgramModel StProgram { get; set; }
        public DisertationThemeDetailWindow(ThemeModel theme, StProgramModel stProgram)
        {
            Theme = theme;
            StProgram = stProgram;
            DataContext = this;
            InitializeComponent();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

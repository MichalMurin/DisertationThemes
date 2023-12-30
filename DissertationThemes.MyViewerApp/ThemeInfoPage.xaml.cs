using DissertationThemes.SharedLibrary.DTOs;

namespace DissertationThemes.MyViewerApp;

public partial class ThemeInfoPage : ContentPage
{
    public ThemeModel Theme { get; set; }
    public StProgramModel StProgram { get; set; }
    public ThemeInfoPage(ThemeModel theme, StProgramModel stProgram)
    {
        Theme = theme;
        StProgram = stProgram;
        BindingContext = this;
        InitializeComponent();
    }

    private void OkButtonClick(object sender, EventArgs e)
    {
        Navigation.RemovePage(this);
    }
}
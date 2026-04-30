using AtestareTicket.Services;

namespace AtestareTicket;

public partial class MainPage : ContentPage
{
    private readonly ExcelService _excelService = new();
    private readonly WordTicketService _wordTicketService = new();
    private readonly Random _random = new();

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadGroups();
    }

    private void LoadGroups()
    {
        try
        {
            var groups = _excelService.GetGroupNames();
            GroupPicker.ItemsSource = groups;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Could not load Excel file:\n{ex.Message}", "OK");
        }
    }

    private void OnGroupSelected(object sender, EventArgs e)
    {
        if (GroupPicker.SelectedIndex < 0)
            return;

        var selectedGroup = GroupPicker.SelectedItem as string;

        // Reset name picker
        NamePicker.ItemsSource = null;
        NamePicker.SelectedIndex = -1;
        GenerateButton.IsEnabled = false;
        HintLabel.Text = "Теперь выберите имя";

        try
        {
            var names = _excelService.GetStudentNames(selectedGroup!);
            NamePicker.ItemsSource = names;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Could not load students:\n{ex.Message}", "OK");
        }
    }

    private void OnNameSelected(object sender, EventArgs e)
    {
        bool bothSelected = GroupPicker.SelectedIndex >= 0 && NamePicker.SelectedIndex >= 0;
        GenerateButton.IsEnabled = bothSelected;
        HintLabel.Text = bothSelected
            ? "✅ Готово! Нажмите кнопку для создания билета."
            : "Теперь выберите имя";
    }

    private async void OnGenerateTicketClicked(object sender, EventArgs e)
    {
        var name = NamePicker.SelectedItem as string;
        int ticketNumber = _random.Next(1, _wordTicketService.TicketCount + 1); // tickets 1–N

        var group = GroupPicker.SelectedItem as string;

        // Save to Excel before showing popup
        try
        {
            _excelService.SaveTicketRecord(group!, name!, ticketNumber);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Warning", $"Could not save record:\n{ex.Message}", "OK");
        }

        // Disable button while popup is shown
        GenerateButton.IsEnabled = false;

        // Show custom popup with questions
        var questions = _wordTicketService.GetQuestions(ticketNumber);
        var popup = new TicketPopupPage(ticketNumber, name!, questions);
        await Navigation.PushModalAsync(popup, animated: true);

        // Reset everything for the next user
        GroupPicker.SelectedIndex = -1;
        NamePicker.ItemsSource = null;
        NamePicker.SelectedIndex = -1;
        GenerateButton.IsEnabled = false;
        HintLabel.Text = "Пожалуйста, выберите группу и имя";
    }
}

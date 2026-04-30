namespace AtestareTicket;

public partial class TicketPopupPage : ContentPage
{
    public TicketPopupPage(int ticketNumber, string studentName, List<string> questions)
    {
        InitializeComponent();

        TicketTitle.Text = $"🎟  Билет {ticketNumber}";
        StudentName.Text = studentName;

        foreach (var q in questions)
        {
            QuestionsLayout.Add(new Label
            {
                Text = q,
                FontSize = 17,
                TextColor = Color.FromArgb("#1A2B3C"),
                LineBreakMode = LineBreakMode.WordWrap
            });
        }
    }

    private async void OnCloseClicked(object sender, EventArgs e) =>
        await Navigation.PopModalAsync();

    private void OnOverlayTapped(object sender, TappedEventArgs e) { }
    private void OnCardTapped(object sender, TappedEventArgs e) { }
}

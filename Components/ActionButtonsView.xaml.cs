namespace OwlReadingRoom.Components;

public partial class ActionButtonsView : ContentView
{
    public event EventHandler<EventArgs> EditClicked;
    public event EventHandler<EventArgs> EyeClicked;

    public ActionButtonsView()
    {
        InitializeComponent();
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        EditClicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnEyeClicked(object sender, EventArgs e)
    {
        EyeClicked?.Invoke(this, EventArgs.Empty);
    }
}

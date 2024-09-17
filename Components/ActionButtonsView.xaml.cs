using Microsoft.Maui.Controls;

namespace OwlReadingRoom.Components;

public partial class ActionButtonsView : ContentView
{
    public event EventHandler<EventArgs> EditClicked;
    public event EventHandler<EventArgs> EyeClicked;

    public static readonly BindableProperty IsEditVisibleProperty =
        BindableProperty.Create(nameof(IsEditVisible), typeof(bool), typeof(ActionButtonsView), true);

    public static readonly BindableProperty IsEyeVisibleProperty =
        BindableProperty.Create(nameof(IsEyeVisible), typeof(bool), typeof(ActionButtonsView), true);

    public bool IsEditVisible
    {
        get => (bool)GetValue(IsEditVisibleProperty);
        set => SetValue(IsEditVisibleProperty, value);
    }

    public bool IsEyeVisible
    {
        get => (bool)GetValue(IsEyeVisibleProperty);
        set => SetValue(IsEyeVisibleProperty, value);
    }

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
namespace OwlReadingRoom.Components;

public partial class AvailableDeskView : ContentView
{
    public static readonly BindableProperty AvailableDesksProperty =
        BindableProperty.Create(nameof(AvailableDesks), typeof(int), typeof(AvailableDeskView), 0, propertyChanged: OnPropertyChanged);

    public static readonly BindableProperty TotalDesksProperty =
        BindableProperty.Create(nameof(TotalDesks), typeof(int), typeof(AvailableDeskView), 1, propertyChanged: OnPropertyChanged);

    public int AvailableDesks
    {
        get => (int)GetValue(AvailableDesksProperty);
        set => SetValue(AvailableDesksProperty, value);
    }

    public int TotalDesks
    {
        get => (int)GetValue(TotalDesksProperty);
        set => SetValue(TotalDesksProperty, value);
    }

    public AvailableDeskView()
    {
        InitializeComponent();
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        UpdateView();
    }

    private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((AvailableDeskView)bindable).UpdateView();
    }

    private void UpdateView()
    {
        if (AvailabilityLabel != null && ProgressBar != null)
        {
            AvailabilityLabel.Text = $"{AvailableDesks}/{TotalDesks}";
            double percentage = TotalDesks > 0 ? (double)AvailableDesks / TotalDesks : 0;
            ProgressBar.WidthRequest = percentage * Width;
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        UpdateView();
    }
}

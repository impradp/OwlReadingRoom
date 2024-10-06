namespace OwlReadingRoom.Components;

public partial class ActionButtonsView : ContentView
{
    public event EventHandler<EventArgs> EditClicked;
    public event EventHandler<EventArgs> EyeClicked;
    public event EventHandler<EventArgs> TrashClicked;

    public static readonly BindableProperty IsEditVisibleProperty =
        BindableProperty.Create(nameof(IsEditVisible), typeof(bool), typeof(ActionButtonsView), false, propertyChanged: OnIsEditVisibleChanged);

    public static readonly BindableProperty IsEyeVisibleProperty =
        BindableProperty.Create(nameof(IsEyeVisible), typeof(bool), typeof(ActionButtonsView), false, propertyChanged: OnIsEyeVisibleChanged);

    public static readonly BindableProperty IsTrashVisibleProperty =
        BindableProperty.Create(nameof(IsTrashVisible), typeof(bool), typeof(ActionButtonsView), false, propertyChanged: OnIsTrashVisibleChanged);

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

    public bool IsTrashVisible
    {
        get => (bool)GetValue(IsTrashVisibleProperty);
        set => SetValue(IsTrashVisibleProperty, value);
    }

    public ActionButtonsView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the property change for edit flag and reflects in the existing view.
    /// </summary>
    /// <param name="bindable">The ActionButtonsView object that consumes the binding attributes.</param>
    /// <param name="oldValue">The old value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    private static void OnIsEditVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (ActionButtonsView)bindable;
        view.EditButton.IsVisible = (bool)newValue;
    }

    /// <summary>
    /// Handles the property change for eye flag and reflects in the existing view.
    /// </summary>
    /// <param name="bindable">The ActionButtonsView object that consumes the binding attributes.</param>
    /// <param name="oldValue">The old value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    private static void OnIsEyeVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (ActionButtonsView)bindable;
        view.EyeButton.IsVisible = (bool)newValue;
    }

    /// <summary>
    /// Handles the property change for trash flag and reflects in the existing view.
    /// </summary>
    /// <param name="bindable">The ActionButtonsView object that consumes the binding attributes.</param>
    /// <param name="oldValue">The old value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    private static void OnIsTrashVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (ActionButtonsView)bindable;
        view.TrashButton.IsVisible = (bool)newValue;
    }


    /// <summary>
    /// Handles the edit click event.
    /// </summary>
    /// <param name="sender">The edit button that will trigger the main function in parent page.</param>
    /// <param name="e">The argument passed on that triggered event.</param>
    private void OnEditClicked(object sender, EventArgs e)
    {
        EditClicked?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles the eye click event.
    /// </summary>
    /// <param name="sender">The eye button that will trigger the main function in parent page.</param>
    /// <param name="e">The argument passed on that triggered event.</param>
    private void OnEyeClicked(object sender, EventArgs e)
    {
        EyeClicked?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles the trash click event.
    /// </summary>
    /// <param name="sender">The trash button that will trigger the main function in parent page.</param>
    /// <param name="e">The argument passed on that triggered event.</param>
    private void OnTrashClicked(object sender, EventArgs e)
    {
        TrashClicked?.Invoke(this, EventArgs.Empty);
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.Components;

public partial class Loader : ContentView, INotifyPropertyChanged
{
    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
    }

    public Loader()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

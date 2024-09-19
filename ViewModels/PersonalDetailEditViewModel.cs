using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OwlReadingRoom.ViewModels;

public class PersonalDetailEditViewModel : INotifyPropertyChanged
{
    private int _customerId;
    private string _fullName;
    private string _gender;
    private string _faculty;
    private DateTime? _dateOfBirth;
    private string _mobileNumber;
    private string _currentAddress;
    private string _permanantAddress;
    private string _disease;
    private string _allergies;
    private string _status;

    public int CustomerId
    {
        get => _customerId;
        set
        {
            if (_customerId != value)
            {
                _customerId = value;
                OnPropertyChanged();
            }
        }
    }

    public string FullName
    {
        get => _fullName;
        set
        {
            if (_fullName != value)
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }
    }

    public string Gender
    {
        get => _gender;
        set
        {
            if (_gender != value)
            {
                _gender = value;
                OnPropertyChanged();
            }
        }
    }

    public string Faculty
    {
        get => _faculty;
        set
        {
            if (_faculty != value)
            {
                _faculty = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime? DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (_dateOfBirth != value)
            {
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }
    }

    public string MobileNumber
    {
        get => _mobileNumber;
        set
        {
            if (_mobileNumber != value)
            {
                _mobileNumber = value;
                OnPropertyChanged();
            }
        }
    }

    public string CurrentAddress
    {
        get => _currentAddress;
        set
        {
            if (_currentAddress != value)
            {
                _currentAddress = value;
                OnPropertyChanged();
            }
        }
    }

    public string PermanantAddress
    {
        get => _permanantAddress;
        set
        {
            if (_permanantAddress != value)
            {
                _permanantAddress = value;
                OnPropertyChanged();
            }
        }
    }

    public string Disease
    {
        get => _disease;
        set
        {
            if (_disease != value)
            {
                _disease = value;
                OnPropertyChanged();
            }
        }
    }

    public string Allergies
    {
        get => _allergies;
        set
        {
            if (_allergies != value)
            {
                _allergies = value;
                OnPropertyChanged();
            }
        }
    }

    public string Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

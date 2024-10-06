using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.Devices.Sensors;
using OwlReadingRoom.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.ViewModels;

public class DocumentEditViewModel : INotifyPropertyChanged
{
    private int _customerId;
    private int? _documentInformationId;
    private string _documentNumber;
    private DateTime? _issueDate;
    private DocumentType? _documentType;
    private string _placeOfIssue;
    private ObservableCollection<DocumentImageViewModel>? _selectedFiles;
    public bool HasDocuments => ActiveImages is not null && !ActiveImages.IsNullOrEmpty();
    public ObservableCollection<DocumentType> DocumentTypes { get; set; }
    public DocumentEditViewModel()
    {
        SelectedFiles = new ObservableCollection<DocumentImageViewModel>();
        DocumentTypes = new ObservableCollection<DocumentType>(Enum.GetValues(typeof(DocumentType)).Cast<DocumentType>());
    }

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

    public int? DocumentInformationId
    {
        get => _documentInformationId;
        set
        {
            if (_documentInformationId != value)
            {
                _documentInformationId = value;
                OnPropertyChanged();
            }
        }
    }

    public string DocumentNumber
    {
        get => _documentNumber;
        set
        {
            if (_documentNumber != value)
            {
                _documentNumber = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime? IssueDate
    {
        get => _issueDate;
        set
        {
            if (_issueDate != value)
            {
                _issueDate = value;
                OnPropertyChanged();
            }
        }
    }

    public DocumentType? DocumentType
    {
        get => _documentType;
        set
        {
            if (_documentType != value)
            {
                _documentType = value;
                OnPropertyChanged();
            }
        }
    }

    public string PlaceOfIssue
    {
        get => _placeOfIssue;
        set
        {
            if (_placeOfIssue != value)
            {
                _placeOfIssue = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<DocumentImageViewModel> ActiveImages
    {
        get => new ObservableCollection<DocumentImageViewModel>(SelectedFiles.Where(img => !img.IsDeleted));
    }

    public void NotifyActiveImageList()
    {
        OnPropertyChanged(nameof(ActiveImages));
        OnPropertyChanged(nameof(HasDocuments));
    }
       
    public ObservableCollection<DocumentImageViewModel> SelectedFiles
    {
        get => _selectedFiles;
        set
        {
            if (_selectedFiles != value)
            {
                _selectedFiles = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasDocuments));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}

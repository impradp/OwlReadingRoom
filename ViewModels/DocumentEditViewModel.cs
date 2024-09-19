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
    private string _issueDate;
    private DocumentType? _documentType;
    private string _placeOfIssue;
    private ObservableCollection<DocumentImageViewModel>? _selectedFiles;
    public bool HasDocuments => _selectedFiles is not null;
    public DocumentEditViewModel()
    {
        SelectedFiles = new ObservableCollection<DocumentImageViewModel>();
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

    public string IssueDate
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

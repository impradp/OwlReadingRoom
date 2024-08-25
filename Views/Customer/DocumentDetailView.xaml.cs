using OwlReadingRoom.Events;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.Views.Customer;

public partial class DocumentDetailView : ContentView, INotifyPropertyChanged
{
    public event EventHandler<DocumentDetailSavedEventArgs> DocumentDetailSaved;
    private ObservableCollection<FileItem> _selectedFiles = new ObservableCollection<FileItem>();
    public ObservableCollection<FileItem> SelectedFiles
    {
        get => _selectedFiles;
        set
        {
            if (_selectedFiles != value)
            {
                _selectedFiles = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedFiles));
            }
        }
    }

    public bool HasSelectedFiles => SelectedFiles.Any();
    public DocumentDetailView()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        //TODO: Update any necessary info from form 
        //TODO: Update CustomerPackageViewModel accordingly.
        CustomerPackageViewModel updatedModel = new CustomerPackageViewModel();
        DocumentDetailSaved?.Invoke(this, new DocumentDetailSavedEventArgs(updatedModel));
    }

    private async void OnBrowseClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Please select image files",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                foreach (var file in result)
                {
                    SelectedFiles.Add(new FileItem { FileName = file.FileName, FilePath = file.FullPath });
                }
                OnPropertyChanged(nameof(HasSelectedFiles));
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Uploading documents.", ex);
        }
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is FileItem fileItem)
        {
            SelectedFiles.Remove(fileItem);
            OnPropertyChanged(nameof(HasSelectedFiles));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.Views.Customer;

public partial class DocumentDetailView : ContentView, INotifyPropertyChanged
{
    private DocumentEditViewModel _customerEditViewModel;
    public DocumentDetailView(DocumentEditViewModel viewModel)
    {
        InitializeComponent();
        _customerEditViewModel = viewModel;
        BindingContext = _customerEditViewModel;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        //TODO: Update any necessary info from form 
        //TODO: Update CustomerPackageViewModel accordingly.
        AlertService.Instance.ShowAlert("Success", "Documents saved successfully.");
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
                    _customerEditViewModel.SelectedFiles.Add(new DocumentImageViewModel { ImageName = file.FileName, ImagePath = file.FullPath });

                }
                /*_customerEditViewModel.NotifyCustomerUpdated();*/
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("Uploading documents.", ex);
        }
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is DocumentImageViewModel fileItem)
        {
            _customerEditViewModel.SelectedFiles.Remove(fileItem);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

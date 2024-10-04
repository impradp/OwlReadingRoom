using OwlReadingRoom.Components.AlertDialog;
using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlReadingRoom.Views.Customer;

public partial class DocumentDetailView : ContentView, INotifyPropertyChanged
{
    private readonly DocumentEditViewModel _customerEditViewModel;
    private readonly ICustomerService _customerService;
    public DocumentDetailView(DocumentEditViewModel viewModel, ICustomerService customerService)
    {
        InitializeComponent();
        _customerEditViewModel = viewModel;
        BindingContext = _customerEditViewModel;
        _customerService = customerService;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        try
        {
            SaveButton.IsEnabled = false;
            _customerService.UpdateCustomerDocument(_customerEditViewModel);
            AlertService.Instance.ShowAlert("Success", "Documents saved successfully.");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException("updating documents", ex);
        }
        finally
        {
            SaveButton.IsEnabled = true;
        }
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
                    if (Validator.IsValidDocument(file))
                    {
                        _customerEditViewModel.SelectedFiles.Add(new DocumentImageViewModel { ImageName = file.FileName, ImagePath = file.FullPath, IsDeleted = false });
                        _customerEditViewModel.NotifyActiveImageList();
                    }
                }
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
            if (fileItem.Id.HasValue)
            {
                fileItem.IsDeleted = true;
            }
            else
            {
                _customerEditViewModel.SelectedFiles.Remove(fileItem);
            }
            _customerEditViewModel.NotifyActiveImageList();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

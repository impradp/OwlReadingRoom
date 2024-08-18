using OwlReadingRoom.Events;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views;

public partial class PersonalDetailView : ContentView
{
    public event EventHandler<PersonalDetailSavedEventArgs> PersonalDetailSaved;
    public PersonalDetailView()
	{
		InitializeComponent();
	}

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        //TODO: Update any necessary info from form 
        //TODO: Update CustomerPackageViewModel accordingly.
        CustomerPackageViewModel updatedModel = new CustomerPackageViewModel();
        PersonalDetailSaved?.Invoke(this, new PersonalDetailSavedEventArgs(updatedModel));
    }

}

using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Resources.Rooms.Plans;

public partial class ACRoomPlan : ContentView
{

    public List<DeskInfoViewModel> Desks { get; set; }
    public ACRoomPlan(List<DeskInfoViewModel> deskInfoViewModels)
	{
		InitializeComponent();
        Desks = deskInfoViewModels;
        BindingContext = this;
    }
}

using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Resources.Rooms.Plans;

public partial class NonACRoomPlan : ContentView
{
    public List<DeskInfoViewModel> Desks { get; set; }
    public NonACRoomPlan(List<DeskInfoViewModel> deskInfoViewModels)
    {
        InitializeComponent();
        Desks = deskInfoViewModels;
        BindingContext = this;
    }
}

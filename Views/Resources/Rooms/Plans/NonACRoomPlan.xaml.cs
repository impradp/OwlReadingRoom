using OwlReadingRoom.Models.Enums;
using OwlReadingRoom.Utils;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Resources.Rooms.Plans;

public partial class NonACRoomPlan : ContentView
{
    public List<DeskInfoViewModel> Desks { get; set; }
    public Boolean IsSelectable { get; set; }
    public NonACRoomPlan(List<DeskInfoViewModel> deskInfoViewModels, Boolean _isSelectable)
    {
        InitializeComponent();
        Desks = deskInfoViewModels;
        IsSelectable = _isSelectable;
        BindingContext = this;
    }

    private void OnDeskSelected(object sender, EventArgs e)
    {
        // Cast EventArgs to TappedEventArgs to access the Parameter property
        if (e is TappedEventArgs tappedEventArgs && tappedEventArgs.Parameter is string deskName && IsSelectable)
        {
            var selectedDesk = Desks.FirstOrDefault(d => d.Name == deskName);
            if (selectedDesk != null && selectedDesk.Status == DeskStatus.Available)
            {
                // Update the status of the selected desk
                selectedDesk.Status = DeskStatus.Selected;
                selectedDesk.Color = Utility.GetBackGroundColorByDeskStatus(DeskStatus.Selected);
                selectedDesk.Message = "Selected";

                // Deselect other desks
                foreach (var desk in Desks.Where(d => d.Name != deskName && d.Status == DeskStatus.Selected))
                {
                    desk.Status = DeskStatus.Available;
                    desk.Color = Utility.GetBackGroundColorByDeskStatus(DeskStatus.Available);
                    selectedDesk.Message = "Available";
                }

                // Notify that the Desks collection has been updated
                OnPropertyChanged(nameof(Desks));
            }
        }
    }
}

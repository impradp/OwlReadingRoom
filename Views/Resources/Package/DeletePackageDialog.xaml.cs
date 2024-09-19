using CommunityToolkit.Maui.Views;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Views.Resources.Package;

public partial class DeletePackageDialog : Popup
{
    private readonly PackageListViewModel _package;

    public event EventHandler<PackageListViewModel> DeleteConfirmed;

    public DeletePackageDialog(PackageListViewModel package)
    {
        InitializeComponent();
        _package = package;
    }

    /// <summary>
    /// Closes the modal poup for confirmation of package deletion.
    /// </summary>
    /// <param name="sender">The button that triggered this click event.</param>
    /// <param name="e">The event parameters passed on to invoke this click event.</param>
    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Closes the modal poup for confirmation of package deletion upon clicking No button.
    /// </summary>
    /// <param name="sender">The button that triggered this click event.</param>
    /// <param name="e">The event parameters passed on to invoke this click event.</param>
    private void OnNoButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Handles the delete confirmed status and triggers the deletion of package in the parent class.
    /// </summary>
    /// <param name="sender">The button that triggered this click event.</param>
    /// <param name="e">The event parameters passed on to invoke this click event.</param>
    private void OnYesButtonClicked(object sender, EventArgs e)
    {
        DeleteConfirmed?.Invoke(this, _package);
        Close();
    }
}

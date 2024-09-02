using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Printing;
using OwlReadingRoom.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing;

namespace OwlReadingRoom.Platforms.Windows.Services
{
    public class WindowsPrintService : IPrintService
    {
        public async Task PrintAsync(object content)
        {
            try
            {
                var printManager = PrintManager.GetForCurrentView();
                printManager.PrintTaskRequested += PrintManager_PrintTaskRequested;

                await PrintManager.ShowPrintUIAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Print Error", $"An error occurred while printing: {ex.Message}", "OK");
            }
        }

        private void PrintManager_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            var printTask = args.Request.CreatePrintTask("Receipt", (printTaskSourceRequestedArgs) =>
            {
                printTaskSourceRequestedArgs.SetSource(GetPrintDocumentSource());
            });

            printTask.Completed += PrintTask_Completed;
        }

        private void PrintTask_Completed(PrintTask sender, PrintTaskCompletedEventArgs args)
        {
            if (args.Completion == PrintTaskCompletion.Failed)
            {
                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Print Error", "Printing failed.", "OK");
                });
            }
        }

        private IPrintDocumentSource GetPrintDocumentSource()
        {
            var printDocument = new PrintDocument();
            printDocument.AddPages += PrintDocument_AddPages;
            return printDocument.DocumentSource;
        }

        private void PrintDocument_AddPages(object sender, AddPagesEventArgs e)
        {
            var printDocument = (PrintDocument)sender;
            var page = new Microsoft.UI.Xaml.Controls.Page();
            var grid = new Microsoft.UI.Xaml.Controls.Grid();

            // Here you would add your receipt content to the grid
            // This is a placeholder - you'll need to implement the actual content transfer
            grid.Children.Add(new TextBlock { Text = "Receipt Content" });

            page.Content = grid;
            printDocument.AddPage(page);
            printDocument.AddPagesComplete();
        }
    }
}

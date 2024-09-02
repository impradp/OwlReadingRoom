using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Services
{
    public class DefaultPrintService : IPrintService
    {
        public async Task PrintAsync(object content)
        {
            await Application.Current.MainPage.DisplayAlert("Print Error", "Printing is not supported on this platform.", "OK");
        }
    }
}

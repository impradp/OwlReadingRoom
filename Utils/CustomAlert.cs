namespace OwlReadingRoom.Utils
{
    public class CustomAlert
    {
        private CustomAlert() { }
        public static Task ShowAlert(string title, string message, string cancel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}

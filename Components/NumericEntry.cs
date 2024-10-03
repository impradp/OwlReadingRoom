namespace OwlReadingRoom.Components;

public class NumericEntry : Entry
{
    public NumericEntry()
    {
        this.Keyboard = Keyboard.Numeric;
        this.BackgroundColor = Color.FromArgb("#FFFFFF");
        this.TextColor = Color.FromArgb("#000000");
        this.HeightRequest = 48;
    }

    protected override void OnTextChanged(string oldValue, string newValue)
    {
        base.OnTextChanged(oldValue, newValue);

        if (!string.IsNullOrEmpty(newValue) && !int.TryParse(newValue, out _))
        {
            this.Text = oldValue; // Revert to old value if new value is not a valid integer
        }
    }
}

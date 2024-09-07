namespace OwlReadingRoom.Components;

class DoubleEntry : Entry
{
    public DoubleEntry()
    {
        this.Keyboard = Keyboard.Numeric;
    }

    protected override void OnTextChanged(string oldValue, string newValue)
    {
        base.OnTextChanged(oldValue, newValue);

        if (!string.IsNullOrEmpty(newValue) && !double.TryParse(newValue, out _))
        {
            this.Text = oldValue; // Revert to old value if new value is not a valid integer
        }
    }
}
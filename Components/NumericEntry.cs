namespace OwlReadingRoom.Components;

public class NumericEntry : Entry
{
    public NumericEntry()
    {
        this.Keyboard = Keyboard.Numeric;
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

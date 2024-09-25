using System.Windows.Input;

namespace OwlReadingRoom.Utils
{
    /// <summary>
    /// Manages the tapped behaviour for the selected frame.
    /// </summary>
    public class FrameTappedBehavior : Behavior<Frame>
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FrameTappedBehavior), null);

        public static readonly BindableProperty IsPressedProperty =
            BindableProperty.CreateAttached("IsPressed", typeof(bool), typeof(FrameTappedBehavior), false, propertyChanged: OnIsPressedChanged);

        private Frame associatedObject;

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets the bindable object's pressed property.
        /// </summary>
        /// <param name="view"></param>
        /// <returns>Boolean value for the property</returns>
        public static bool GetIsPressed(BindableObject view)
        {
            return (bool)view.GetValue(IsPressedProperty);
        }

        /// <summary>
        /// Sets the bindable object's pressed property.
        /// </summary>
        /// <param name="view">The selected object for which the pressed property value is to be set.</param>
        /// <param name="value">The boolean value to be set as a pressed property for the selected object.</param>
        public static void SetIsPressed(BindableObject view, bool value)
        {
            view.SetValue(IsPressedProperty, value);
        }

        /// <summary>
        /// Manages the visual state for the pressed object
        /// </summary>
        /// <param name="view">The selected object for which the visual state is changed</param>
        /// <param name="oldValue">The old property that represents the visual state of the selected object.</param>
        /// <param name="newValue">The new property that represents the visual state of the selected object.</param>
        public static void OnIsPressedChanged(BindableObject view, object oldValue, object newValue)
        {
            if (view is Frame frame)
            {
                VisualStateManager.GoToState(frame, (bool)newValue ? "Pressed" : "Normal");
            }
        }

        /// <summary>
        /// Detaches the visual state manager for the selected object
        /// </summary>
        /// <param name="bindable">The selected frame</param>
        protected override void OnDetachingFrom(Frame bindable)
        {
            base.OnDetachingFrom(bindable);
            associatedObject = null;
            bindable.GestureRecognizers.Clear();
        }

        /// <summary>
        /// Manages the tap activity for the selected frame.
        /// </summary>
        /// <param name="frame">The selected frame</param>
        public async void OnFrameTapped(Frame frame)
        {
            associatedObject = frame;
            if (associatedObject != null)
            {
                SetIsPressed(associatedObject, true);
                await associatedObject.ScaleTo(0.95, 50, Easing.CubicOut);
                await Task.Delay(100);
                await associatedObject.ScaleTo(1, 50, Easing.CubicIn);
                SetIsPressed(associatedObject, false);
                Command?.Execute(null);
            }
        }
    }
}

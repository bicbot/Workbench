namespace SchoolsApp.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Windows.Input;

    [Serializable]
    public abstract class HotKeyCommandBase : CommandBase
    {
        public KeyGesture Gesture { get; set; }

        public string GestureText => this.Gesture.GetDisplayStringForCulture(CultureInfo.CurrentUICulture);

        public string Header { get; set; }
    }
}
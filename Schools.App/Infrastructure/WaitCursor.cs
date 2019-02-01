namespace SchoolsApp.Infrastructure
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class WaitCursor : IDisposable
    {
        private Cursor previousCursor;

        public WaitCursor()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => this.previousCursor = Mouse.OverrideCursor));
            Application.Current.Dispatcher.Invoke(new Action(() => Mouse.OverrideCursor = Cursors.Wait));
        }

        public void Dispose()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => Mouse.OverrideCursor = this.previousCursor));
        }
    }
}
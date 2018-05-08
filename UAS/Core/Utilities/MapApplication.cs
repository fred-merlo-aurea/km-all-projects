using System;
using MapPoint;

namespace Core_AMS.Utilities
{
    public class MapApplication : IDisposable
    {
        private bool _isDisposed;

        public Application Application { get; private set; }

        public MapApplication(Application application)
        {
            Application = application;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (Application != null)
                {
                    Application.ActiveMap.Saved = true;
                    Application.Quit();
                    Application = null;
                }
            }

            _isDisposed = true;
        }
    }
}

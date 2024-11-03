using Godot;
using System;

namespace JumpvalleyApp.Display
{
    /// <summary>
    /// Class responsible for handling cases where a user is running the Jumpvalley app on a HiDPI display,
    /// affecting window width and height
    /// </summary>
    public partial class HiDpiAdapter : IDisposable
    {
        public Window HandledWindow { get; private set; }

        private bool _enabled;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;

                if (value)
                {
                    ResizeUi();
                    respondsToWindowResize = true;
                }
                else
                {
                    respondsToWindowResize = false;
                    ResizeUi();
                }
            }
        }

        private float _baseDpi;
        public float BaseDpi
        {
            get => _baseDpi;
            set
            {
                if (value > 0)
                {
                    _baseDpi = value;
                    ResizeUi();
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Attempted to set Base DPI to be less than or equal to zero.");
                }
            }
        }

        private bool _respondsToWindowResize;
        private bool respondsToWindowResize
        {
            get => _respondsToWindowResize;
            set
            {
                if (_respondsToWindowResize == value) return;
                _respondsToWindowResize = value;

                if (value)
                {
                    HandledWindow.SizeChanged += ResizeUi;
                }
                else
                {
                    HandledWindow.SizeChanged -= ResizeUi;
                }
            }
        }

        /// <summary>
        /// Construct a HiDpiAdapter.
        /// </summary>
        /// <param name="window">The window to handle HiDPI for</param>
        public HiDpiAdapter(Window window, float baseDpi = 96f)
        {
            if (window == null) throw new ArgumentNullException(nameof(window), "Attempted to pass a Window that doesn't exist.");
            HandledWindow = window;
            BaseDpi = baseDpi;
        }

        private void ResizeUi()
        {
            if (Enabled)
            {
                float baseDpi = BaseDpi;
                float dpi = DisplayServer.ScreenGetDpi();
                if (dpi > baseDpi)
                {
                    HandledWindow.ContentScaleFactor = dpi / baseDpi;
                }
            }

            HandledWindow.ContentScaleFactor = 1f;
        }

        public void Dispose()
        {
            respondsToWindowResize = false;
        }
    }
}

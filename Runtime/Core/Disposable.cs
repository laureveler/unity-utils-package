using System;

namespace Utilities
{
    public abstract class Disposable : IDisposable
    {
        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                DisposeManagedResources();
            }

            DisposeUnmanagedResources();

            _disposed = true;
        }

        /// <summary>
        /// Managed resources are those handled by the GC.
        /// These include C# objects, arrays, strings, and other types that are allocated by the CLR.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {

        }

        /// <summary>
        /// Unmanaged resources include native memory, file handles, network connections, graphics and sound resources.
        /// Unity-specific resources would include textures, meshes, shaders, audioclips, etc.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
            
        }

        // Destructor ensures resources are released if Dispose is not called.
        ~Disposable()
        {
            Dispose(false);
        }
    }
}



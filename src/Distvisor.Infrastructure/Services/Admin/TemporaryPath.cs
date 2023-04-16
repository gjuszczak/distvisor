using System;
using System.IO;

namespace Distvisor.Infrastructure.Services.Admin
{
    public sealed class TemporaryPath : IDisposable
    {
        private readonly bool _isDirectory;
        private bool _isDisposed;

        private TemporaryPath(string path, bool isDirectory)
        {
            Path = path;
            _isDirectory = isDirectory;
            _isDisposed = false;
        }

        public string Path { get; init; }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_isDirectory && Directory.Exists(Path))
            {
                Directory.Delete(Path, true);
            }

            if (!_isDirectory && File.Exists(Path))
            {
                File.Delete(Path);
            }

            _isDisposed = true;
        }

        public static TemporaryPath ForDirectory()
            => new(GetRandomFilePath(), true);

        public static TemporaryPath ForFile()
            => new(GetRandomFilePath(), false);

        private static string GetRandomFilePath()
            => System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
    }
}

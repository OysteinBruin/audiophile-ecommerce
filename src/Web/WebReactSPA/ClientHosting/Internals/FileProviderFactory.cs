using Microsoft.Extensions.FileProviders;

namespace WebReactSpa.Internals
{
    internal class FileProviderFactory
    {
        public virtual IFileProvider CreateFileProvider(string physicalRoot) => new PhysicalFileProvider(physicalRoot);
    }
}

namespace SpaceLogistic.Persistence
{
    using System.IO;

    public static class FileInfoExtensions
    {
        public static string Extension(this FileInfo file)
        {
            return Path.GetExtension(file.Name);
        }

        public static string NameWithoutExtension(this FileInfo file)
        {
            return Path.GetFileNameWithoutExtension(file.Name);
        }
    }
}

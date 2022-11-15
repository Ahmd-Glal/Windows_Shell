using System.IO;

namespace OS_Project
{
    internal class StreamReade
    {
        private FileStream virtual_disk_text;

        public StreamReade(FileStream virtual_disk_text)
        {
            this.virtual_disk_text = virtual_disk_text;
        }
    }
}
using System.IO;

namespace ByteDev.Testing.Builders
{
    internal static class TextWriterExtensions
    {
        public static void WriteFillerText(this TextWriter source, long size)
        {
            for (long l = 0; l < size; l++)
            {
                source.Write("A");
            }
        }
    }
}
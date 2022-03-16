using System;
using System.Runtime.Serialization;

namespace BandcampDownloaderLib
{
    [Serializable]
    public class BandcampDownloaderException : Exception
    {
        public BandcampDownloaderException()
        {
        }

        public BandcampDownloaderException(string message) : base(message)
        {
        }

        public BandcampDownloaderException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BandcampDownloaderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
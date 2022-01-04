using System;
using System.Collections.Generic;
using System.IO;

namespace Metalama.Open.DependencyEmbedder.Weaver
{
    /// <summary>
    /// Multiple streams rolled into one. Read-only. Comes from https://stackoverflow.com/a/3879231/1580088.
    /// </summary>
    internal class ConcatenatedStream : Stream
    {
        private readonly Queue<Stream> _streams;
        private readonly Stream[] _allStreams;

        public ConcatenatedStream(Stream[] streams)
        {
            _allStreams = streams;
            _streams = new Queue<Stream>(streams);
        }

        public override bool CanRead => true;

        public void ResetAllToZero()
        {
            foreach (var stream in _allStreams) stream.Position = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var totalBytesRead = 0;

            while (count > 0 && _streams.Count > 0)
            {
                var bytesRead = _streams.Peek().Read(buffer, offset, count);
                if (bytesRead == 0)
                {
                    _streams.Dequeue();
                    continue;
                }

                totalBytesRead += bytesRead;
                offset += bytesRead;
                count -= bytesRead;
            }

            return totalBytesRead;
        }

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length => throw new NotImplementedException();

        public override long Position
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
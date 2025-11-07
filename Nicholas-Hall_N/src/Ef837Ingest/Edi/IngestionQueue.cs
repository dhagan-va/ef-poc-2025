using System.Threading.Channels;

namespace Ef837Ingest.Edi
{

    using System.Threading.Channels;

    public sealed class IngestionQueue
    {
        private readonly Channel<string> _channel = Channel.CreateUnbounded<string>();

        public ChannelReader<string> Reader => _channel.Reader;

        public ValueTask EnqueueAsync(string s3Uri, CancellationToken ct = default)
            => _channel.Writer.WriteAsync(s3Uri, ct);
    }
}

using System.Threading.Channels;

namespace Ef837Ingest.Edi
{

    public class IngestionQueue : IIngestionQueue
    {
        private readonly Channel<string> _channel;
        public IngestionQueue(Channel<string> channel) => _channel = channel;
        public ValueTask EnqueueAsync(string s3Uri, CancellationToken ct = default)
            => _channel.Writer.WriteAsync(s3Uri, ct);
        public ChannelReader<string> Reader => _channel.Reader; // for the worker
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace X12EDI.Core.Services
{
    public static class ChannelProcessor
    {
        public static async IAsyncEnumerable<TResult> ParallelMapAsync<TInput, TResult>(
            IEnumerable<TInput> source,
            Func<TInput, CancellationToken, Task<TResult>> transform,
            int maxDegreeOfParallelism = 4,
            int boundCapacity = 100,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default, 
            BoundedChannelFullMode boundedChannelFullMode = BoundedChannelFullMode.Wait,
            bool singleReader = false,
            bool singleWriter = true)
        {
            // Setup Channels
            var workChannel = Channel.CreateBounded<TInput>(new BoundedChannelOptions(boundCapacity)
            {
                FullMode = boundedChannelFullMode,
                SingleReader = singleReader,
                SingleWriter = singleWriter
            });

            var resultsChannel = Channel.CreateUnbounded<TResult>();

            // Start Producer (Background Task)
            var producer = Task.Run(async () =>
            {
                try
                {
                    foreach (var item in source)
                    {
                        await workChannel.Writer.WriteAsync(item, ct);
                    }
                }
                catch (OperationCanceledException) { /* Normal exit */ }
                finally
                {
                    workChannel.Writer.TryComplete();
                }
            }, ct);

            // Start Consumers (Worker Pool)
            var workers = Enumerable.Range(0, maxDegreeOfParallelism)
                .Select(async _ =>
                {
                    try
                    {
                        await foreach (var item in workChannel.Reader.ReadAllAsync(ct))
                        {
                            var result = await transform(item, ct);
                            await resultsChannel.Writer.WriteAsync(result, ct);
                        }
                    }
                    catch (OperationCanceledException) { /* Normal exit */ }
                })
                .ToArray();

            // Monitor Completion
            // Once all workers finish, we close the results channel
            _ = Task.WhenAll(workers).ContinueWith(_ => resultsChannel.Writer.TryComplete(), CancellationToken.None);

            // Yield Results to the Caller
            await foreach (var result in resultsChannel.Reader.ReadAllAsync(ct))
            {
                yield return result;
            }

            // Ensure producer exceptions are observed if the stream finishes normally
            await producer;
        }
    }
}
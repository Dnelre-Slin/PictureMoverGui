using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PictureMoverGui.Helpers
{

    public static class Enumerables
    {
        public static IEnumerable<T> CatchUnauthorizedAccessExceptions<T>(this IEnumerable<T> src, Action<UnauthorizedAccessException> action = null)
        {
            using (IEnumerator<T> enumerator = src.GetEnumerator())
            {
                bool next = true;
                while (next)
                {
                    try
                    {
                        next = enumerator.MoveNext();
                    }
                    catch (UnauthorizedAccessException obj)
                    {
                        action?.Invoke(obj);
                        continue;
                    }

                    if (next)
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        public static IEnumerable<T> CancelWorker<T>(this IEnumerable<T> src, BackgroundWorker worker)
        {
            using (IEnumerator<T> enumerator = src.GetEnumerator())
            {
                bool next = true;
                while (next)
                {
                    if (worker.CancellationPending)
                    {
                        break;
                    }
                    next = enumerator.MoveNext();

                    if (next)
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }
    }
}

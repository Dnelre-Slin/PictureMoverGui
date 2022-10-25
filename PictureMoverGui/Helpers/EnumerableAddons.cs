using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Helpers
{
    public class EnumerableCancelArgs
    {
        public bool Cancel { get; set; } = false;
    }

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

        public static IEnumerable<T> Cancel<T>(this IEnumerable<T> src, EnumerableCancelArgs e)
        {
            using (IEnumerator<T> enumerator = src.GetEnumerator())
            {
                bool next = true;
                while (next)
                {
                    if (e.Cancel)
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

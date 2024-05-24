namespace Pulsar.Utils;

public static class CollectionExtensions
{
    public static void Add<T1, T2>(this ICollection<(T1, T2)> collection, T1 t1, T2 t2)
    {
        collection.Add((t1, t2));
    }

    public static void Add<T1, T2, T3>(this ICollection<(T1, T2, T3)> collection, T1 t1, T2 t2, T3 t3)
    {
        collection.Add((t1, t2, t3));
    }

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, T target) where T : struct
    {
        List<T> list = new();
        foreach (var v in source)
        {
            if (v.Equals(target))
            {
                yield return list;
                list = new();
            }
            else
            {
                list.Add(v);
            }
        }

        if (list.Any())
        {
            yield return list;
        }
    }

    public static IEnumerable<List<T>> Split<T>(this ICollection<T> source, T[] target) where T : struct
    {
        List<T> list = new();
        var i = 0;
        var done = false;
        using var enumerator = source.GetEnumerator();
        while ((i + target.Length) < source.Count)
        {
            if (!enumerator.MoveNext())
            {
                done = true;
                break;
            }

            list.Add(enumerator.Current);

            if (list.Count >= target.Length)
            {
                if (list.TakeLast(target.Length).SequenceEqual(target))
                {
                    yield return list;
                    list = new();
                }
            }

            i++;
        }

        while (!done && enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
        }

        if (list.Any())
        {
            yield return list;
        }
    }

    public static bool Contains<T>(this IEnumerable<T> source, T[] target) where T : struct
    {
        List<T> list = new();
        var i = 0;
        var done = false;
        using var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);

            if (list.Count >= target.Length)
            {
                if (list.TakeLast(target.Length).SequenceEqual(target))
                {
                    return true;
                }
            }

            i++;
        }

        return false;
    }

    public static List<byte> Replace(this IEnumerable<byte> source, ReadOnlySpan<byte> target, ReadOnlySpan<byte> replacement) 
    {
        List<byte> result = new();
        List<byte> buffer = new (20);
        var done = false;
        using var enumerator = source.GetEnumerator();
        var targetArray = target.ToArray();
        while (enumerator.MoveNext())
        {
            buffer.Add(enumerator.Current);

            if (buffer.Count < target.Length) continue;
            
            if (buffer.TakeLast(target.Length).SequenceEqual(targetArray))
            {
                result.AddRange(replacement.ToArray());

                buffer.Clear();
                done = true;
                break;
            }

            if (buffer.Count < target.Length) continue;
            result.Add(buffer.First());
            buffer.RemoveAt(0);
        }

        result.AddRange(buffer);

        while (done && enumerator.MoveNext())
        {
            result.Add(enumerator.Current);
        }

        return result;
    }
}
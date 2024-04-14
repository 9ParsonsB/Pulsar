namespace Pulsar.Utils;

public static class CollectionExtensions
{
    public static void Add<T1, T2>(this ICollection<(T1,T2)> collection, T1 t1, T2 t2)
    {
        collection.Add((t1, t2));
    }
    
    public static void Add<T1, T2, T3>(this ICollection<(T1,T2,T3)> collection, T1 t1, T2 t2, T3 t3)
    {
        collection.Add((t1, t2, t3));
    }
}
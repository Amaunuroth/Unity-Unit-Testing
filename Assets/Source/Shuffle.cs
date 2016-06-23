using System.Collections;

public static class Extensions
{
    static System.Random random = new System.Random();
        
    public static IList Shuffle(this IList collection)
    {
        for (int i = collection.Count - 1; i > 1; --i)
        {
            int swap = random.Next(0, i);
            object temp = collection[swap];
            collection[swap] = collection[i];
            collection[i] = temp;
        }
        return collection;
    }

    public static T[] Shuffle<T>(this T[] collection)
    {
        for (int i = collection.Length - 1; i > 1; --i)
        {
            int swap = random.Next(0, i);
            T temp = collection[swap];
            collection[swap] = collection[i];
            collection[i] = temp;
        }
        return collection;
    }
}

namespace ChemodartsWebApp.ModelHelper
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Rotate<T>(this IEnumerable<T> source, int count)
        {
            List<T> list = source.ToList();

            if (list.Count == 0)
            {
                return list;
            }

            count %= list.Count;

            if (count == 0)
            {
                return list;
            }

            if (count > 0)
            {
                // Rotate elements up (to the left)
                return list.Skip(count).Concat(list.Take(count));
            }
            else
            {
                // Rotate elements down (to the right)
                return list.Take(list.Count + count).Concat(list.Skip(list.Count + count));
            }
        }
    }
}

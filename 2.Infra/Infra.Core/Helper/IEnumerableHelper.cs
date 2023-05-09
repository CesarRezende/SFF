namespace SFF.Infra.Core.Helper
{
    public static class IEnumerableHelper
    {
        public static bool IsNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null;
        }

        public static bool IsNotNull<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.IsNull();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.IsNull() || (!enumerable.Any());
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.IsEmpty();
        }

        public static IEnumerable<T2> SelectOrDefault<T, T2>(this IEnumerable<T> enumerable, Func<T, T2> func)
        {
            return enumerable.IsEmpty() ? new List<T2>() : enumerable.Select(func);
        }

        public static T2 SelectOneOrDefault<T, T2>(this IEnumerable<T> enumerable, Func<T, T2> func)
        {
            return enumerable.IsEmpty() ? default : enumerable.Select(func).DefaultIfEmpty().First();
        }

        public static IEnumerable<T> ToEmptyIfNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.IsEmpty() ? new List<T>() : enumerable;
        }
    }
}

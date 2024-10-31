namespace Domain.Extensions
{
    public static class ListExtensions
    {
        public static string JoinElements(this IEnumerable<string> list)
            => list.Aggregate((current, next) => current + ", " + next);
    }
}

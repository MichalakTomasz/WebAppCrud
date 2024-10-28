namespace Domain.Models
{
    public class Roles
    {
        public static IReadOnlyList<string> List => new List<string> { CommonConsts.Admin, CommonConsts.Guest };
        public static string StringRoles => List.Aggregate((current, next) => current + ", " + next);
    }
}

namespace WebApplication1.Data
{
    public class Role
    {
        public const string Customer = "customer";

        public const string Seller = "selller";

        public static List<string> ToList() => new() { Customer, Seller };
    }
}

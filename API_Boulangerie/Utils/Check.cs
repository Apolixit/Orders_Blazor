namespace API_Orders.Utils
{
    public class Check<T>
    {
        public virtual bool AllNotNull(T first, T second)
        {
            return first != null && second != null;
        }
    }

    public class CheckString : Check<string?>
    {
        public override bool AllNotNull(string? first, string? second) => !string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(second);

        public bool Contains(string? first, string? second) => AllNotNull(first, second) && first.Contains(second);
    }
}

namespace TendalProject.Common.Utils
{
    public static class StringUtils
    {
        public static bool IsNullOrWhiteSpace(params string[] valores)
        {
            return valores.All(v => !string.IsNullOrWhiteSpace(v));
        }
    }
}

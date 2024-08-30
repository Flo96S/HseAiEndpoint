namespace hseaiendpoint.Src
{
    public static class CleanOutput
    {
        public static string Clean(string input)
        {
            return input.Replace("System: ", "").Replace("User:", "");
        }
    }
}

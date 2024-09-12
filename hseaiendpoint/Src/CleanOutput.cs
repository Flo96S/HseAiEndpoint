namespace hseaiendpoint.Src
{
    public static class CleanOutput
    {
        public static string Clean(string input)
        {
            return input
                .Replace("System: ", "")
                .Replace("User:", "")
                .Replace("Assistant:", "")
                .Replace("Answer:", "")
                .Replace("Answer in german.", "")
                .Replace("Only answer with as much information as needed.", "")
                .Replace("No english!", "")
                .Replace("```", "");
        }
    }
}

namespace hseaiendpoint.Src
{
    public class SystemPromptGetter
    {
        public string GetSystemPrompt(int num = -1)
        {
            switch(num)
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    string content = File.ReadAllText("Files/Praxis.txt");
                    if(content.Length > 0)
                    {
                        return content;
                    }
                    break;
            }
            return "";
        }
    }
}

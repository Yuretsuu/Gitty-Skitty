namespace GittySkitty.Command
{
    public class SummaryObject
    {
        public string CommandName { get; }
        public string CommandDesc { get; }

        // Default constructor with hardcoded values
        public SummaryObject()
        {
            CommandName = "summary";
            CommandDesc = "This command displays recent git activity";
        }
    }
}
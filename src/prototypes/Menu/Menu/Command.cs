namespace Menu
{
    public class Command
    {
        public string Description { get; set; }
        public string KeyboardShortcut { get; set; }
        public string Title { get; set; }

        public Command(string title, string keyboardShortcut = null, string description = null)
        {
            Description = description;
            KeyboardShortcut = keyboardShortcut;
            Title = title;
        }
    }
}
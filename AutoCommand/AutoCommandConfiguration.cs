using Rocket.API;

namespace AutoCommand
{
    public class AutoCommandConfiguration : IRocketPluginConfiguration
    {
        public string CommandToExecute;
        public int MinInterval;
        public int MaxInterval;
        public string Webhook;
        public long rolid;

        public void LoadDefaults()
        {
            CommandToExecute = "padm start 60"; 
            MinInterval = 18000; 
            MaxInterval = 28800;
            Webhook = "";
            rolid = 1224393353374728315;
        }
    }
}

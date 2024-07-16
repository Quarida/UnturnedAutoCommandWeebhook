using System;
using System.Net.Http;
using System.Text;
using System.Timers;
using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using Rocket.Core.Plugins;
using Rocket.Core;

namespace AutoCommand
{
    public class AutoCommandPlugin : RocketPlugin<AutoCommandConfiguration>
    {
        private Timer timer;
        private Random random;
        private readonly HttpClient httpClient = new HttpClient();

        protected override void Load()
        {
            timer = new Timer();
            random = new Random();
            timer.Elapsed += OnTimerElapsed;
            SetTimerInterval();
            timer.Start();
        }

        protected override void Unload()
        {
            timer.Stop();
            timer.Dispose();
        }

        private  void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteCommand();
            SetTimerInterval();
        }

        private void ExecuteCommand()
        {
            string commandToExecute = Configuration.Instance.CommandToExecute;
            Rocket.Core.Logging.Logger.Log($"Executing command: {commandToExecute}");
            R.Commands.Execute(new ConsolePlayer(), commandToExecute);
            SendWebhookMessage("60 Seconds to the Start of the Event (Event Location Sign has been registered on the Map)");
        }

        private void SetTimerInterval()
        {
            int interval = random.Next(Configuration.Instance.MinInterval, Configuration.Instance.MaxInterval + 1) * 1000; // saniye cinsinden milisaniye olarak dönüştürme
            timer.Interval = interval;
        }

        private async void SendWebhookMessage(string message)
        {
            var webhookUrl = base.Configuration.Instance.Webhook;
            var roleId = base.Configuration.Instance.rolid;
            var json = $"{{\"content\":\"<@&{roleId}>\", \"embeds\": [{{\"description\":\"**{message}**\", \"color\":16711680}}]}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(webhookUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    Rocket.Core.Logging.Logger.LogError($"Failed to send webhook message. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Rocket.Core.Logging.Logger.LogError($"Failed to send webhook message: {ex.Message}");
            }
        }

    }
}

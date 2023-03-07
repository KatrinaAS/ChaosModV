using Serilog;
using Shared;
using System.IO;

namespace ChatVotingProxy.Config
{
    class Config : IConfig
    {
        // General Settings
        public static readonly string KEY_OVERLAY_SERVER_PORT = "OverlayServerPort";
        public static readonly string KEY_CHAT_OVERLAY_MODE = "VotingOverlayMode";
        public static readonly string KEY_CHAT_RETAIN_INITIAL_VOTES = "VotingChanceSystemRetainChance";
        public static readonly string KEY_CHAT_VOTING_CHANCE_SYSTEM = "VotingChanceSystem";
        public static readonly string KEY_CHAT_TYPE = "ChatType";

        // Twitch Settings
        public static readonly string KEY_TWITCH_CHANNEL_NAME = "TwitchChannelName"; 
        public static readonly string KEY_TWITCH_CHANNEL_OAUTH = "TwitchChannelOAuth";
        public static readonly string KEY_TWITCH_CHANNEL_USER_NAME = "TwitchUserName";
        public static readonly string KEY_TWITCH_PERMITTED_USERNAMES = "TwitchPermittedUsernames";

        // Old Settings
        // Here for backwards compatibility
        public static readonly string KEY_TWITCH_VOTING_CHANCE_SYSTEM = "TwitchVotingChanceSystem";

        public EOverlayMode? OverlayMode { get; set; }
        public int? OverlayServerPort { get; set; }
        public bool RetainInitalVotes { get; set; }
        public EVotingMode? VotingMode { get; set; }
        public string TwitchChannelName { get; set; }
        public string TwitchOAuth { get; set; }
        public string TwitchUserName { get; set; }
        public string[] PermittedChatUsernames { get; set; }
        public EChatType? ChatType { get; set; }

        private ILogger logger = Log.Logger.ForContext<Config>();
        private OptionsFile optionsFile;
        
        public Config(string file)
        {
            if (!File.Exists(file))
            {
                logger.Warning($"chat config file \"{file}\" not found");
                
            } else
            {
                // If the file does exist, read its content
                optionsFile = new OptionsFile(file);
                optionsFile.ReadFile();

                OverlayServerPort = optionsFile.ReadValueInt(KEY_OVERLAY_SERVER_PORT, -1);
                if (OverlayServerPort == -1) OverlayServerPort = null;
                RetainInitalVotes = optionsFile.ReadValueBool(KEY_CHAT_RETAIN_INITIAL_VOTES, false);
                TwitchChannelName = optionsFile.ReadValue(KEY_TWITCH_CHANNEL_NAME);
                TwitchOAuth = optionsFile.ReadValue(KEY_TWITCH_CHANNEL_OAUTH);
                TwitchUserName = optionsFile.ReadValue(KEY_TWITCH_CHANNEL_USER_NAME);
                VotingMode = optionsFile.ReadValueInt(KEY_CHAT_VOTING_CHANCE_SYSTEM, 0) == 0 ? EVotingMode.MAJORITY : EVotingMode.PERCENTAGE;
                OverlayMode = (EOverlayMode)optionsFile.ReadValueInt(KEY_CHAT_OVERLAY_MODE, 0);
                ChatType = (EChatType)optionsFile.ReadValueInt(KEY_CHAT_TYPE, 0);

                string tmpPermittedUsernames = optionsFile.ReadValue(KEY_TWITCH_PERMITTED_USERNAMES, "").Trim().ToLower();  // lower case the username to allow case-insensitive comparisons

                if (tmpPermittedUsernames.Length > 0)
                {
                    PermittedChatUsernames = tmpPermittedUsernames.Split(',');

                    for (var i = 0; i < PermittedChatUsernames.Length; i++)
                    {
                        // remove any potential whitespaces around the usernames
                        PermittedChatUsernames[i] = PermittedChatUsernames[i].Trim();
                    }
                }
                else
                {
                    PermittedChatUsernames = new string[0];
                }
            }
        }
    }
}

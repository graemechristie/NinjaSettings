using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaSettings.Repositories
{
    public class CommandLineSettingsRepository : ISettingsRepository
    {
        private readonly string[] _args;
        private readonly KeyValuePair<string, string>?[] _parsedArgs;

        public CommandLineSettingsRepository(string[] args)
        {
            _args = args;
            _parsedArgs = args.Select(ParseCommandline).ToArray();
        }

        private static KeyValuePair<string, string>? ParseCommandline(string arg)
        {
            if (arg.StartsWith("-"))
            {
                // switch/bool
                return new KeyValuePair<string, string>(arg.TrimStart('-'), "true");
            }

            var split = arg.Split(new[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 2)
            {
                // value
                return new KeyValuePair<string, string>(split[0], split[1]);
            }

            return null;
        }

        public string Get(string settingName)
        {
            var setting = _parsedArgs.FirstOrDefault(kvp => kvp.HasValue && kvp.Value.Key.Equals(settingName));
            return setting?.Value;
        }

        public override string ToString()
        {
            return $"Raw arguments: {string.Join("", _args)}";
        }
    }
}
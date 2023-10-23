using System;
using DotNetEnv;
using Octokit;

namespace GittySkitty.Authentication
{
    public class Authenticate
    {
        private readonly GitHubClient _gitHubClient;
        private readonly string _discordToken;

        public Authenticate(string envFilePath = null)
        {
            LoadEnvironmentVariables(envFilePath);
            
            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            _discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(_discordToken))
            {
                Console.WriteLine("Failed to retrieve tokens from environment variables.");
                // Optionally throw an exception or handle the error in some way
                // throw new Exception("Failed to retrieve tokens from environment variables.");
            }
            
            _gitHubClient = new GitHubClient(new ProductHeaderValue("GittySkitty"))
            {
                Credentials = new Credentials(token)
            };
        }

        private void LoadEnvironmentVariables(string envFilePath = null)
        {
            if (string.IsNullOrEmpty(envFilePath))
            {
                envFilePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), ".env");
            }
            Console.WriteLine($"Loading environment variables from: {envFilePath}");  // Debug statement
            DotNetEnv.Env.Load(envFilePath);
        }
        public GitHubClient GitHubClient => _gitHubClient;

        public string DiscordToken => _discordToken;
    }
}
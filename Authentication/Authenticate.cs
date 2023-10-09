using System;
using DotNetEnv;
using Octokit;

namespace GittySkitty.Authentication
{
    public class Authenticate
    {
        private readonly GitHubClient _gitHubClient;
        private readonly string _discordToken;

        public Authenticate() // Constructor
        {
            LoadEnvironmentVariables();
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

        private void LoadEnvironmentVariables()
        {
            var envFilePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), ".env");
            Console.WriteLine($"Loading environment variables from: {envFilePath}");  // Debug statement
            DotNetEnv.Env.Load(envFilePath);
        }

        public GitHubClient GetGitHubClient()
        {
            return _gitHubClient;
        }

        public string GetDiscordToken()
        {
            return _discordToken;
        }
    }
}
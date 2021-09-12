using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using GithubBrowser.Model;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubBrowser
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private static readonly char[] tokenBytes = new char[] { 'g', 'h', 'p', '_', 'B', '4', 'i', 'n', 'k', 'k', 'w', 'X', 'q', 'a', '6', 'I', 't', 'u', 'r', 'F', '2', '6', 'v', 'W', 'A', 'I', 'e', 'I', 'G', 'l', 'r', 'f', 'U', '5', '1', 'h', 'Q', 'w', 'k', 'O' };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var buttonShowCommits = FindViewById<Button>(Resource.Id.buttonShowCommits);
            buttonShowCommits.Click += ButtonShowCommits_Click;
        }

        private async void ButtonShowCommits_Click(object sender, EventArgs e)
        {
            string repoOwner = FindViewById<EditText>(Resource.Id.editTextOwner).Text;
            string repoName = FindViewById<EditText>(Resource.Id.editTextName).Text;

            var commits = await GetMostRecentCommits(repoOwner, repoName, 14);

            var commitModels = commits.Select(o => new CommitModel(o.Commit.Author.Name, o.Sha, o.Commit.Message));
            var intent = new Intent(this, typeof(CommitsActivity));
            intent.PutExtra("commits", JsonConvert.SerializeObject(commitModels));
            StartActivity(intent);
        }

        private async Task<List<GitHubCommit>> GetMostRecentCommits(string repoOwner, string repoName, int days)
        {
            var client = new GitHubClient(new ProductHeaderValue(repoOwner));

            string token = new string(tokenBytes);
            client.Credentials = new Credentials(token);

            var commitRequest = new CommitRequest()
            {
                Since = DateTime.Now.Subtract(new TimeSpan(days, 0, 0, 0))
            };

            var repository = await client.Repository.Commit.GetAll(repoOwner, repoName, commitRequest);
            var getCommitDetail = repository.Select(async (o) =>
            {
                return await client.Repository.Commit.Get(repoOwner, repoName, o.Sha);
            }).ToList();
            var commits = await Task.WhenAll(getCommitDetail);

            OutputGithubApiRateInfo(client);

            return commits.ToList();
        }

        private void OutputGithubApiRateInfo(GitHubClient client)
        {
            var apiInfo = client.GetLastApiInfo();
            var rateLimit = apiInfo?.RateLimit;

            Console.WriteLine($"Github requests per hour: {rateLimit?.Limit}");
            Console.WriteLine($"Github requests remaining: {rateLimit?.Remaining}");
            Console.WriteLine($"Github request resets at: {rateLimit?.Reset.ToLocalTime()}");
        }
	}
}

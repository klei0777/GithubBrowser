using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
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
        /// <summary>
        /// Specifies a token that can read commit status and access public repositories. The token was
        /// created on 9/11/21 and will expire on 10/11/21. I wouldn't normally put tokens in the code
        /// and commit them to source control, but for this test application I'll make an exception.
        /// </summary>
        //private const string Token = "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";
        private const string Token = "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";

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

            StartActivity(typeof(CommitsActivity));
        }

        private async Task<List<GitHubCommit>> GetMostRecentCommits(string repoOwner, string repoName, int days)
        {
            var client = new GitHubClient(new ProductHeaderValue(repoOwner));

            client.Credentials = new Credentials(Token);

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

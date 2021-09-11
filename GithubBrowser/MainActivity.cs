using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using Octokit;
using System;
using System.Collections.Generic;
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

            var commits = await GetMostRecentCommits(repoOwner, repoName);
        }

        private async Task<List<GitHubCommit>> GetMostRecentCommits(string repoOwner, string repoName)
        {
            var client = new GitHubClient(new ProductHeaderValue(repoOwner));

            return new List<GitHubCommit>();
        }
	}
}

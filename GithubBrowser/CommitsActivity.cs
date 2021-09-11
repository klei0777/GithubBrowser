using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using GithubBrowser.Model;
using Newtonsoft.Json;
using Octokit;
using System.Collections.Generic;

namespace GithubBrowser
{
    [Activity(Label = "CommitsActivity")]
    public class CommitsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.commits);

            List<CommitModel> commits = JsonConvert.DeserializeObject<List<CommitModel>>(Intent.GetStringExtra("commits"));
        }
    }
}
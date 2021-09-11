using Android.App;
using Android.OS;

namespace GithubBrowser
{
    [Activity(Label = "CommitsActivity")]
    public class CommitsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.commits);
        }
    }
}
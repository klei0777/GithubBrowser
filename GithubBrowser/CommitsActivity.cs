using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
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

            string owner = Intent.GetStringExtra("owner");
            string name = Intent.GetStringExtra("name");

            var textViewHeader = FindViewById<TextView>(Resource.Id.textViewCommitHeader);
            textViewHeader.Text = $"Commits for github.com/{owner}/{name}";

            List<CommitModel> commits = JsonConvert.DeserializeObject<List<CommitModel>>(Intent.GetStringExtra("commits"));

            var listView = FindViewById<ListView>(Resource.Id.listViewCommits);
            listView.Adapter = new CommitModelAdapter(this, commits);
        }

        private class CommitModelAdapter : BaseAdapter<CommitModel>
        {
            List<CommitModel> items;
            Android.App.Activity context;

            public CommitModelAdapter(Android.App.Activity context, List<CommitModel> items)
                : base()
            {
                this.context = context;
                this.items = items;
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override CommitModel this[int position]
            {
                get { return items[position]; }
            }

            public override int Count
            {
                get { return items.Count; }
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                View view = convertView;
                if (view == null)
                {
                    view = context.LayoutInflater.Inflate(Resource.Layout.commit_list_item, null);
                }
                view.FindViewById<TextView>(Resource.Id.textViewMessage).Text = items[position].Message;
                view.FindViewById<TextView>(Resource.Id.textViewAuthor).Text = items[position].Author;
                view.FindViewById<TextView>(Resource.Id.textViewHash).Text = items[position].Sha;
                return view;
            }
        }
    }
}
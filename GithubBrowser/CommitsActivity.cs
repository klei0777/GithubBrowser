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
                    view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
                }
                view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position].Message;
                return view;
            }
        }
    }
}
namespace GithubBrowser.Model
{
    public class CommitModel
    {
        public CommitModel()
        {
        }

        public CommitModel(string author, string sha, string message)
        {
            Author = author;
            Sha = sha;
            Message = message;
        }

        public string Author { get; set; }
        public string Sha { get; set; }
        public string Message { get; set; }
    }
}
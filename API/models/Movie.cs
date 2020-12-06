namespace API.models
{
    public class Movie
    {
        public int MovieNo { get; set; }
        public string Title { get; set; }
        public int RelYear { get; set; }
        public int Runtime { get; set; }

        public Movie()
        {
            this.MovieNo = 0;
            this.Title = "";
            this.RelYear = 0;
            this.Runtime = 0;
        }

        public Movie(int movieno, string title, int relyear, int runtime)
        {
            this.MovieNo = movieno;
            this.Title = title;
            this.RelYear = relyear;
            this.Runtime = runtime;
        }

        public int NumActors(int total)
        {
            
        }

        public int GetAge()
        {
            
        }
    }
}
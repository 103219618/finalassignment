using System;
using System.Data.SqlClient;

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

        public int NumActors(int movieno)
        {
            //connect to an sql server database
            string connectionString = @"Data Source=csharp.czit4bgdokjy.us-east-1.rds.amazonaws.com;Initial Catalog=Movies;User ID=admin;Password=databaseconnection";
            SqlConnection conn = new SqlConnection(connectionString);

            //creating query
            string queryString = @"SELECT COUNT(MOVIENO) FROM CASTING WHERE MOVIENO ='" + movieno + "' GROUP BY MOVIENO";

            //sending queries via the connection
            SqlCommand command = new SqlCommand (queryString, conn);
            //opening the connection
            conn.Open();
            
            int actorCount = 0;
            using(SqlDataReader reader = command.ExecuteReader())
            {   
                while (reader.Read())
                {
                    actorCount = Convert.ToInt32(reader[0]);                
                }
            }

            return actorCount;

        }

        public int GetAge(int movieno)
        {
            string connectionString = @"Data Source=csharp.czit4bgdokjy.us-east-1.rds.amazonaws.com;Initial Catalog=Movies;User ID=admin;Password=databaseconnection";
            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = @"SELECT (YEAR(GETDATE()) - RELYEAR) AS GETAGE
                                    FROM MOVIE
                                    WHERE MOVIENO ='" + movieno + "'";

            SqlCommand command = new SqlCommand (queryString, conn);
            conn.Open();

            int movieAge = 0;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    movieAge = Convert.ToInt32(reader[0]);
                }
            }
            return movieAge;
        }
    }
}
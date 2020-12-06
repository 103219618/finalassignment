using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using API.models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        //string connectionString = @"Data Source=bikestoresdb.c3raologixkl.us-east-1.rds.amazonaws.com;Initial Catalog=SampleDB;User ID=admin;Password=abcd1234";
        SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();
        IConfiguration configuration; //private and not yet initialised
        string connectionString = "";

        //constructor
        public MoviesController(IConfiguration iConfig)
        {
            this.configuration = iConfig;

            // use configuration to retrieve connection string from appsettings.json
            // this.connectionString = this.configuration.GetSection("DbConnectionString").Value;

            //use the SqlConnectionStringBuilder to create our connection string
            this.stringBuilder.DataSource = this.configuration.GetSection("DbConnectionString").GetSection("Url").Value;
            this.stringBuilder.InitialCatalog = this.configuration.GetSection("DbConnectionString").GetSection("Database").Value;
            this.stringBuilder.UserID = this.configuration.GetSection("DbConnectionString").GetSection("User").Value;
            this.stringBuilder.Password = this.configuration.GetSection("DbConnectionString").GetSection("Password").Value;

            this.connectionString = stringBuilder.ConnectionString;
        }

        /*
        [HttpGet("CountActor/{movieno}")] //testing countactor

        public int CountActor(int movieno)
        {
            Movie m = new Movie();
            return m.NumActors(movieno);
        }
        */

        //READ TASK - 1
        [HttpGet("listallmovies")] //testing to list all movies
        public List<Movie> ListMovies()
        {

            List<Movie> Movies = new List<Movie>();

            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = "Select * From Movie";

            SqlCommand command = new SqlCommand( queryString, conn);
            conn.Open();
        
            string result = "";
            using(SqlDataReader reader = command.ExecuteReader())
            {   
                while (reader.Read())
                {
                    result += reader[0] + " | " + reader[1] + reader[2] + reader[3] + "\n";
                    
                    // ORM - Object Relation Mapping
                    Movies.Add(
                        new Movie() { MovieNo = Convert.ToInt32(reader[0]), Title = reader[1].ToString(), RelYear = Convert.ToInt32(reader[2]), Runtime = Convert.ToInt32(reader[3])});                
                }
            }
            return Movies;
        }

        //READ TASK - 2
        // movies that begin with the word 'The'
        [HttpGet("themovies")]
        public List<Movie> TheMovies()
        {

            List<Movie> Movies = new List<Movie>();

            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = "SELECT * FROM MOVIE WHERE TITLE LIKE 'The%'";

            SqlCommand command = new SqlCommand( queryString, conn);
            conn.Open();
        
            string result = "";
            using(SqlDataReader reader = command.ExecuteReader())
            {   
                while (reader.Read())
                {
                    result += reader[0] + " | " + reader[1] + reader[2] + reader[3] + "\n";
                    
                    // ORM - Object Relation Mapping
                    Movies.Add(
                        new Movie() { MovieNo = Convert.ToInt32(reader[0]), Title = reader[1].ToString(), RelYear = Convert.ToInt32(reader[2]), Runtime = Convert.ToInt32(reader[3])});                
                }
            }
            return Movies;
        }

        //READ TASK - 3
        // display Luke Wilson movies
        [HttpGet("lukewilson")]
        public List<Movie> LukeWilson()
        {

            List<Movie> Movies = new List<Movie>();

            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = @" SELECT * 
                                    FROM MOVIE M
                                    INNER JOIN CASTING C
                                    ON M.MOVIENO = C.MOVIENO
                                    WHERE ACTORNO = (SELECT ACTORNO FROM ACTOR WHERE FULLNAME = 'Luke Wilson');";

            SqlCommand command = new SqlCommand( queryString, conn);
            conn.Open();
        
            string result = "";
            using(SqlDataReader reader = command.ExecuteReader())
            {   
                while (reader.Read())
                {
                    result += reader[0] + " | " + reader[1] + reader[2] + reader[3] + "\n";
                    
                    // ORM - Object Relation Mapping
                    Movies.Add(
                        new Movie() { MovieNo = Convert.ToInt32(reader[0]), Title = reader[1].ToString(), RelYear = Convert.ToInt32(reader[2]), Runtime = Convert.ToInt32(reader[3])});                
                }
            }
            return Movies;
        }

        //READ TASK - 4
        // total runtime of all the movies
        [HttpGet("totalruntime")]
        public List<Movie> TotalRuntime()
        {

            List<Movie> Movies = new List<Movie>();

            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = @"SELECT (SUM(RUNTIME)) AS 'TOTAL RUNTIME OF ALL MOVIES IN THE LIST!' FROM MOVIE";

            SqlCommand command = new SqlCommand( queryString, conn);
            conn.Open();
        
            string result = "";

            using(SqlDataReader reader = command.ExecuteReader())
            {   
                while (reader.Read())
                {
                    result = "The Total Runtime Of All The Movies In The List is: "+(reader[0]).ToString();
                }
            }
            return Movies;
        }
    }
}
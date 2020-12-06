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
        public string TotalRuntime(){

            string Movielist = "";

            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = @" SELECT (SUM(RUNTIME)) AS 'TOTAL RUNTIME OF ALL THE MOVIES IN THE LIST'
                                    FROM MOVIE";

            SqlCommand command = new SqlCommand( queryString, conn);
            conn.Open();
    
            using(SqlDataReader reader = command.ExecuteReader())
            {   
                while (reader.Read())
                {
                    Movielist = "The Total Run Time Of All The Movies is: " + (reader[0]).ToString();
                }
            }

            return Movielist;

        }

        //UPDATE TASK - 1
        // update the runtime of a movie by title
        [HttpPost("updateruntime/{movieTitle}/{newRuntime}")]
        public List<Movie> UpdateRuntime(string movieTitle, int newRuntime){

            List<Movie> Movies = new List<Movie>();

            SqlConnection conn = new SqlConnection(connectionString);

            string runtimeUpdater = @" UPDATE MOVIE 
                                    SET RUNTIME = " + newRuntime + @"   
                                    WHERE TITLE = '" + movieTitle + "';";            

            string displayResult = @" SELECT *
                                        FROM MOVIE
                                        WHERE TITLE = '" + movieTitle + "';";

            // SQL Command to update
            SqlCommand command1 = new SqlCommand( runtimeUpdater, conn);
            conn.Open();
            command1.ExecuteNonQuery();

            // To display the results
            SqlCommand command2 = new SqlCommand( displayResult,conn);

            string result = "";
            using(SqlDataReader reader = command2.ExecuteReader())
            {   
                while (reader.Read())
                {
                    result += reader[0] + " | " + reader[1] + reader[2] + reader[3] + "\n";
                    
                    // ORM - Object Relation Mapping
                    Movies.Add(
                        new Movie() { MovieNo = Convert.ToInt32(reader[0]), Title = reader[1].ToString(), RelYear = Convert.ToInt32(reader[2]), Runtime = Convert.ToInt32(reader[3])});                
                }
            }
            conn.Close();
            return Movies;
        }

        //UPDATE TASK - 2
        // changing actors surname and fullname
        [HttpPost("changename/{givenname}/{surname}/{newsurname}")]
        public List<Actor> ChangeName(string givenname, string surname, string newsurname){
            List<Actor> actor = new List<Actor>();

            string space = " ";

            SqlConnection conn = new SqlConnection(connectionString);
            string changesurname = " UPDATE ACTOR SET SURNAME = '" + newsurname +"', FULLNAME = '"+ givenname + space + newsurname+"' WHERE GIVENNAME ='" + givenname + "' AND SURNAME = '" + surname + "'";            
            string displayResult = @" SELECT *
                                        FROM ACTOR
                                        WHERE GIVENNAME = '"+ givenname +"' AND SURNAME='"+ newsurname +"'";
        
            // SQL Command to update
            SqlCommand command1 = new SqlCommand(changesurname, conn);
            // making connection open
            conn.Open();

            command1.ExecuteNonQuery();

            // To display the results
            SqlCommand command2 = new SqlCommand( displayResult,conn);
            string result = "";
            using(SqlDataReader reader = command2.ExecuteReader())
            {   
                while (reader.Read())
                {
                    result += reader[0] + " | " + reader[1] + reader[2] + reader[3] + "\n";
                    // ORM - Object Relation Mapping
                    actor.Add(
                        new Actor() {ActorNo = Convert.ToInt32(reader[0]), FullName = reader[1].ToString(), GivenName = reader[2].ToString(), SurName = reader[3].ToString()});                
                }
            }
            conn.Close();
            return actor;
        }

        //CREATE TASK - 1
        // add a movie to the list of movies
        [HttpPost("addmovie")]
        public string AddMovie(Movie Mov)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = "INSERT INTO MOVIE (MOVIENO, TITLE, RELYEAR, RUNTIME) VALUES (@movNo, @movTitle , @movRelYear ,@movRuntime)";

            SqlCommand command = new SqlCommand(queryString, conn);
            
            command.Parameters.AddWithValue("@movNo", Mov.MovieNo);
            command.Parameters.AddWithValue("@movTitle", Mov.Title);
            command.Parameters.AddWithValue("@movRelYear", Mov.RelYear);
            command.Parameters.AddWithValue("@movRuntime", Mov.Runtime);

            conn.Open();
            
            var result = command.ExecuteNonQuery();
            
            return "Movie Added. " + result + " Row is Added to the Table!";
        }

        /* tested by adding this
        {
            "MovieNo": 20,
            "Title": "TEST",
            "RelYear": 2020,
            "Runtime": 200
        }
        */

        //CREATE TASK - 2
        // add an actor to the list of actors
        [HttpPost("addactor")]
        public string AddActor(Actor Act)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = "INSERT INTO ACTOR (ACTORNO, FULLNAME, GIVENNAME, SURNAME) VALUES (@actNo, @actFname , @actGname ,@actSname)";

            SqlCommand command = new SqlCommand(queryString, conn);
            
            command.Parameters.AddWithValue("@actNo", Act.ActorNo);
            command.Parameters.AddWithValue("@actFname", Act.FullName);
            command.Parameters.AddWithValue("@actGname", Act.GivenName);
            command.Parameters.AddWithValue("@actSname", Act.SurName);

            conn.Open();
            
            var result = command.ExecuteNonQuery();
            
            return "Actor Added. " + result + " Row is Added to the Actor Table!";
        }
        /* tested by adding this
        {
            "ActorNo": 101,
            "FullName": "MISTER TEST",
            "GivenName": "MISTER",
            "Surname": "TEST"
        }
        */

        //CREATE TASK - 3
        // casting an actor to a movie
        
    }
}
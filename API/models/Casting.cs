using System;
using System.Data.SqlClient;

namespace API.models
{
    public class Casting
    {
        public int CastID { get; set; }
        public int ActorNo { get; set; }
        public int MovieNo { get; set; }

        public Casting()
        {
            this.CastID = 0;
            this.ActorNo = 0;
            this.MovieNo = 0;
        }

        public Casting(int castid, int actorno, int movieno)
        {
            this.CastID = castid;
            this.ActorNo = actorno;
            this.MovieNo = movieno;
        }


    }


}
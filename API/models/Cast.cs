using System;
using System.Data.SqlClient;

namespace API.models
{
    public class Cast
    {
        public int CastID { get; set; }

        public Cast()
        {
            this.CastID = 0;
        }

        public Cast(int castid)
        {
            this.CastID = castid;
        }


    }


}
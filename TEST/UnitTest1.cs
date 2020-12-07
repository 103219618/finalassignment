using System;
using API.models;
using Xunit;
using System.Data.SqlClient;

namespace TEST
{
    public class UnitTest1
    {
        //TEST TASK

        Movie x = new Movie();

        // Task a, checking output for num actors method
        [Fact]
        public void NumActorsTest1()
        {
            int movieno = 20;
            Assert.Equal(1, x.NumActors(movieno));
        }

        [Theory]
        [InlineData(4,22)]
        [InlineData(6,24)]
        [InlineData(4,70)]
        [InlineData(1,77)]
        public void NumActorsTest2(int expected, int movieno)
        {
            Assert.Equal(expected, x.NumActors(movieno));
        }

        //Task b, checking correct output for GetAge method to check how old movie
        [Fact]
        public void GetAgeTest1()
        {
            int movieno = 20;
            Assert.Equal(0, x.GetAge(movieno));
        }

        [Theory]
        [InlineData(15,22)]
        [InlineData(16,24)]
        [InlineData(20,70)]
        [InlineData(17,77)]
        public void GetAgeTest2(int expected, int movieno)
        {
            Assert.Equal(expected, x.GetAge(movieno));
        }

    }
}

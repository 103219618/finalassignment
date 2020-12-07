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
        [InlineData(1,21)]
        [InlineData(1,22)]
        [InlineData(1,23)]
        [InlineData(1,24)]
        public void NumActorsTest2(int expected, int movieno)
        {
            Assert.Equal(expected, x.NumActors(movieno));
        }

        //Task b, checking correct output for GetAge method
        [Fact]
        public void GetAgeTest1()
        {
            int movieno = 20;
            Assert.Equal(20, x.GetAge(movieno));
        }

        [Theory]
        [InlineData(15,21)]
        [InlineData(16,22)]
        [InlineData(20,23)]
        [InlineData(21,24)]
        public void GetAgeTest2(int expected, int movieno)
        {
            Assert.Equal(expected, x.GetAge(movieno));
        }
    }
}

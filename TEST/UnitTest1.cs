using System;
using API.models;
using Xunit;

namespace TEST
{
    public class UnitTest1
    {
        //TEST TASK

        Movie m = new Movie();

        // Task a, checking output for num actors method
        [Fact]
        public void NumActorsTest1()
        {
            int movieno = 20;
            Assert.Equal(1, m.NumActors(movieno));
        }

        [Theory]
        [InlineData(1,21)]
        [InlineData(1,22)]
        [InlineData(1,23)]
        [InlineData(1,24)]
        public void NumActorsTest2(int expected, int movieno)
        {
            Assert.Equal(expected, m.NumActors(movieno));
        }
    }
}

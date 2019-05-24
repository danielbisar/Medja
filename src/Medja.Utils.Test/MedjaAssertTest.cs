using System;
using Xunit;

namespace Medja.Utils.Test
{

    public class MedjaAssertTest
    {        
        [Fact]
        public void EqualComparesArrayLength()
        {
            var expected = new int[1,2];

            // 1nd dimension differs
            var actual = new int[2,2]; 

            Assert.Throws<Exception>(() => MedjaAssert.Equal(expected, actual));
        
            // 2nd dimension differs
            actual = new int[1,1];

            Assert.Throws<Exception>(() => MedjaAssert.Equal(expected, actual));

            // inverted x,y to make sure not .Length is used to compare array bounds
            actual = new int[2,1]; 

            Assert.Throws<Exception>(() => MedjaAssert.Equal(expected, actual));
        }

        [Fact]
        public void EqualCompare2DArrayValues()
        {
            var expectedValues = new [,]
            {
                { 1, 2 },
                { 3, 4 }
            };

            var comparedValues = new [,]
            {
                { 1, 2 },
                { 3, 4 }
            };

            MedjaAssert.Equal(comparedValues, expectedValues);
        }

        [Fact]
        public void Compare2DArraysFails()
        {
            var expectedValues = new [,]
            {
                { 1, 2 },
                { 3, 4 }
            };

            var comparedValues = new [,]
            {
                { 1, 3 },
                { 3, 4 }
            };

            Assert.Throws<Exception>(() => MedjaAssert.Equal(comparedValues, expectedValues));
        }
    }
}
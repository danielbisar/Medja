using Xunit;

namespace Medja.Utils.Test
{
    public class MedjaMathTest
    {
        [Fact]
        public void IsPowerOfTwo()
        {
            uint x = 1;
            
            for(int i = 0; i < 30; i++)
            {
                Assert.True(x.IsPowerOfTwo());
                
                if(x > 1)
                    Assert.False((x + 1).IsPowerOfTwo());
                
                x *= 2;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Medja.Performance
{
    public class PropertyPerformanceReport
    {
        private const int CreateIterations = 100000;
        private const int PropertyIterations = 10000000;
        private readonly string[] Strings = new[] {"12466487 4", "bklajsd ", "asdvlcj  sdf", "adfklj", "alsdfjk", "llsdk"};

        public IEnumerable<string> CreateReport(Func<ITestObject> testObjFactory)
        {
            // warmup
            for (int i = 0; i < 1000; i++)
            {
                Apply(testObjFactory(), i);
            }

            var testObj = testObjFactory();
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < CreateIterations; i++)
            {
                testObj = testObjFactory();
            }

            sw.Stop();

            GetTestObjAsString(testObj);
            yield return string.Format("Object type: {0}", testObj.GetType());
            yield return string.Format("{0} create iterations took {1}", CreateIterations, sw.Elapsed);

            sw = Stopwatch.StartNew();
            
            for (int i = 0; i < PropertyIterations; i++)
            {
                Apply(testObj, i);
            }

            sw.Stop();

            yield return GetTestObjAsString(testObj);
            yield return string.Format("{0} property iterations took {1}", PropertyIterations, sw.Elapsed);
        }

        private string GetTestObjAsString(ITestObject testObj)
        {
            return string.Join(",", new string[]
            {
                testObj.TestString,
                testObj.TestInt0.ToString(),
                testObj.TestInt1.ToString(),
                testObj.TestInt2.ToString(),
                testObj.TestInt3.ToString(),
                testObj.TestInt4.ToString(),
                testObj.TestInt5.ToString(),
                testObj.TestInt6.ToString(),
                testObj.TestInt7.ToString(),
                testObj.TestInt8.ToString(),
                testObj.TestInt9.ToString(),
                testObj.TestInt10.ToString(),
                testObj.TestInt11.ToString(),
                testObj.TestInt12.ToString(),
                testObj.TestInt13.ToString(),
                testObj.TestInt14.ToString(),
            });
        }

        private void Apply(ITestObject testObj, int i)
        {
            testObj.TestInt0 = i;
            testObj.TestInt1 = testObj.TestInt0 + 1;
            testObj.TestInt2 = testObj.TestInt1 + 1;
            testObj.TestInt3 = testObj.TestInt2 + 1;
            testObj.TestInt4 = testObj.TestInt3 + 1;
            testObj.TestInt5 = testObj.TestInt4 + 1;
            testObj.TestInt6 = testObj.TestInt5 + 1;
            testObj.TestInt7 = testObj.TestInt6 + 1;
            testObj.TestInt8 = testObj.TestInt7 + 1;
            testObj.TestInt9 = testObj.TestInt8 + 1;
            testObj.TestInt10 = testObj.TestInt9 + 1;
            testObj.TestInt11 = testObj.TestInt10 + 1;
            testObj.TestInt12 = testObj.TestInt11 + 1;
            testObj.TestInt13 = testObj.TestInt12 + 1;
            testObj.TestInt14 = testObj.TestInt13 + 1;

            testObj.TestString = Strings[testObj.TestInt14 % 6];
            testObj.TestInt0 = testObj.TestString.Length;
        }
    }
}

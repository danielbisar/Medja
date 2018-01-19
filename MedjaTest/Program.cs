using System;
using Medja;
using Medja.Performance;
using MedjaTest.Performance;

namespace MedjaTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var bp = new BindingPerformance();
            //bp.Run().Print();

            //ExecuteTest(() => new NativeTestObject());
            //ExecuteTest(() => new TPropertyTestObject());
            //ExecuteTest(() => new MTestObject());
            //ExecuteTest(() => new DefaultPropertyChangedTestObject());

            //ExecuteCreationOnlyTest(() => new MTestObject());

            //Console.ReadLine();

            using (var wnd = new MainWindow())
            {
                wnd.Run(1 / 30.0);
            }
        }

        private static void ExecuteCreationOnlyTest(Func<MTestObject> testObjFactory)
        {
            var report = new PropertyPerformanceReport();
            report.TestProperties = false;
            var result = report.CreateReport(testObjFactory);            

            foreach (var item in result)
                Console.WriteLine(item);

            Console.WriteLine();
            Console.WriteLine();
        }

        private static void ExecuteTest(Func<ITestObject> testObjFactory)
        {
            var report = new PropertyPerformanceReport();
            var result = report.CreateReport(testObjFactory);

            foreach (var item in result)
                Console.WriteLine(item);

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}

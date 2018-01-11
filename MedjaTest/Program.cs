using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medja;
using Medja.Performance;

namespace MedjaTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //ExecuteTest(() => new NativeTestObject());
            //ExecuteTest(() => new TPropertyTestObject());

            //Console.ReadLine();

            using (var wnd = new MainWindow())
            {
                wnd.Run(1 / 30.0);
            }
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

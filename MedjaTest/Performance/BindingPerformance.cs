using System.Collections.Generic;
using System.Diagnostics;
using Medja;
using Medja.Performance;

namespace MedjaTest.Performance
{
    /// <summary>
    /// Known result from 01/19/2018 - omitted object values, were correct
    /// 
    /// no binding -> base line
    /// 10000000 * 15 property iterations took 00:00:01.5919813
    /// 
    /// 
    /// created 15 bindings
    /// 10000000 * 15 property iterations took 00:00:02.6035644 --> factor: ~1.6
    /// 
    /// 
    /// INotifyPropertyChanged without binding
    /// 10000000 * 15 property iterations took 00:00:02.7329731
    /// 
    /// 
    /// INotifyPropertyChanged with 15 bindings
    /// 10000000 * 15 property iterations took 00:00:37.0693801 --> factor: ~13.6 (vs without binding), ~14.2 (vs binding with MTestObject)
    /// 
    /// Note: the tests do not yet include creation times for the bindings.
    ///     
    /// </summary>
    public class BindingPerformance
    {
        public BindingPerformance()
        {
            
        }

        public IEnumerable<string> Run()
        {
            //var report = new PropertyPerformanceReport();

            //var obj1 = new MTestObject();
            //var obj2 = new MTestObject();

            //report.TestCreate = false;

            //yield return "no binding -> base line";
            
            //foreach (var line in report.CreateReport(() => obj1))
            //    yield return line;



            //// with binding
            //obj2.AddBinding(obj2._testStringProperty, obj1._testStringProperty);
            //obj2.AddBinding(obj2._testInt0, obj1._testInt0);
            //obj2.AddBinding(obj2._testInt1, obj1._testInt1);
            //obj2.AddBinding(obj2._testInt2, obj1._testInt2);
            //obj2.AddBinding(obj2._testInt3, obj1._testInt3);
            //obj2.AddBinding(obj2._testInt4, obj1._testInt4);
            //obj2.AddBinding(obj2._testInt5, obj1._testInt5);
            //obj2.AddBinding(obj2._testInt6, obj1._testInt6);
            //obj2.AddBinding(obj2._testInt7, obj1._testInt7);
            //obj2.AddBinding(obj2._testInt8, obj1._testInt8);
            //obj2.AddBinding(obj2._testInt9, obj1._testInt9);
            //obj2.AddBinding(obj2._testInt10, obj1._testInt10);
            //obj2.AddBinding(obj2._testInt11, obj1._testInt11);
            //obj2.AddBinding(obj2._testInt12, obj1._testInt12);
            //obj2.AddBinding(obj2._testInt13, obj1._testInt13);
            //obj2.AddBinding(obj2._testInt14, obj1._testInt14);

            //yield return "";
            //yield return "";
            //yield return "created 15 bindings";

            //foreach (var line in report.CreateReport(() => obj1))
            //    yield return line;

            //yield return "bound objects values: ";
            //yield return PropertyPerformanceReport.GetTestObjAsString(obj2);

            //yield return "";
            //yield return "";



            //var defaultPropertyChanged1 = new DefaultPropertyChangedTestObject();
            //var defaultPropertyChanged2 = new DefaultPropertyChangedTestObject();

            //yield return "INotifyPropertyChanged without binding";

            //foreach (var line in report.CreateReport(() => defaultPropertyChanged1))
            //    yield return line;


            //yield return "";
            //yield return "";


            //var bindings = new List<Binding>();
            //bindings.Add(new Binding(defaultPropertyChanged1, "TestString", defaultPropertyChanged2, "TestString", BindingMode.OneWay));

            //for(int i = 0; i < 15; i++)
            //    bindings.Add(new Binding(defaultPropertyChanged1, "TestInt" + i, defaultPropertyChanged2, "TestInt" + i, BindingMode.OneWay));

            //yield return "INotifyPropertyChanged with 15 bindings";

            //foreach (var line in report.CreateReport(() => defaultPropertyChanged1))
            //    yield return line;

            //yield return "bound objects values: ";
            //yield return PropertyPerformanceReport.GetTestObjAsString(defaultPropertyChanged2);

            //yield return "";


            // create binding performance - note since the results of the previous tests 
            // we focus on different implementations for MObject/MTestObject

            const int iterations = PropertyPerformanceReport.CreateIterations / 100;
            var rootObj = new MTestObject();
            var boundObjects = new MTestObject[iterations];

            for (int i = 0; i < iterations; i++)
                boundObjects[i] = new MTestObject();

            var sw = Stopwatch.StartNew();

            for(int i = 0; i < iterations; i++)
            {
                var boundObject = boundObjects[i];

                // 15 x 50000 properties took 00:00:00.662
                // update values 1.72s (see below)
                //new Binding<string, string>(boundObject._testStringProperty, rootObj._testStringProperty, p => p);
                //new Binding<int, int>(boundObject._testInt0, rootObj._testInt0, p => p);
                //new Binding<int, int>(boundObject._testInt1, rootObj._testInt1, p => p);
                //new Binding<int, int>(boundObject._testInt2, rootObj._testInt2, p => p);
                //new Binding<int, int>(boundObject._testInt3, rootObj._testInt3, p => p);
                //new Binding<int, int>(boundObject._testInt4, rootObj._testInt4, p => p);
                //new Binding<int, int>(boundObject._testInt5, rootObj._testInt5, p => p);
                //new Binding<int, int>(boundObject._testInt6, rootObj._testInt6, p => p);
                //new Binding<int, int>(boundObject._testInt7, rootObj._testInt7, p => p);
                //new Binding<int, int>(boundObject._testInt8, rootObj._testInt8, p => p);
                //new Binding<int, int>(boundObject._testInt9, rootObj._testInt9, p => p);
                //new Binding<int, int>(boundObject._testInt10, rootObj._testInt10, p => p);
                //new Binding<int, int>(boundObject._testInt11, rootObj._testInt11, p => p);
                //new Binding<int, int>(boundObject._testInt12, rootObj._testInt12, p => p);
                //new Binding<int, int>(boundObject._testInt13, rootObj._testInt13, p => p);
                //new Binding<int, int>(boundObject._testInt14, rootObj._testInt14, p => p);

                // 15 x 50000 properties took 00:00:00.6701315
                // update values 00:00:01.7089224
                BindingFactory.Create(boundObject._testStringProperty, rootObj._testStringProperty);
                BindingFactory.Create(boundObject._testInt0, rootObj._testInt0);
                BindingFactory.Create(boundObject._testInt1, rootObj._testInt1);
                BindingFactory.Create(boundObject._testInt2, rootObj._testInt2);
                BindingFactory.Create(boundObject._testInt3, rootObj._testInt3);
                BindingFactory.Create(boundObject._testInt4, rootObj._testInt4);
                BindingFactory.Create(boundObject._testInt5, rootObj._testInt5);
                BindingFactory.Create(boundObject._testInt6, rootObj._testInt6);
                BindingFactory.Create(boundObject._testInt7, rootObj._testInt7);
                BindingFactory.Create(boundObject._testInt8, rootObj._testInt8);
                BindingFactory.Create(boundObject._testInt9, rootObj._testInt9);
                BindingFactory.Create(boundObject._testInt10, rootObj._testInt10);
                BindingFactory.Create(boundObject._testInt11, rootObj._testInt11);
                BindingFactory.Create(boundObject._testInt12, rootObj._testInt12);
                BindingFactory.Create(boundObject._testInt13, rootObj._testInt13);
                BindingFactory.Create(boundObject._testInt14, rootObj._testInt14);

                // 15 x 50000 properties took 00:00:00.6120575
                // update values 1.87s (see below)
                //boundObject.AddBinding(boundObject._testStringProperty, rootObj._testStringProperty);
                //boundObject.AddBinding(boundObject._testInt0, rootObj._testInt0);
                //boundObject.AddBinding(boundObject._testInt1, rootObj._testInt1);
                //boundObject.AddBinding(boundObject._testInt2, rootObj._testInt2);
                //boundObject.AddBinding(boundObject._testInt3, rootObj._testInt3);
                //boundObject.AddBinding(boundObject._testInt4, rootObj._testInt4);
                //boundObject.AddBinding(boundObject._testInt5, rootObj._testInt5);
                //boundObject.AddBinding(boundObject._testInt6, rootObj._testInt6);
                //boundObject.AddBinding(boundObject._testInt7, rootObj._testInt7);
                //boundObject.AddBinding(boundObject._testInt8, rootObj._testInt8);
                //boundObject.AddBinding(boundObject._testInt9, rootObj._testInt9);
                //boundObject.AddBinding(boundObject._testInt10, rootObj._testInt10);
                //boundObject.AddBinding(boundObject._testInt11, rootObj._testInt11);
                //boundObject.AddBinding(boundObject._testInt12, rootObj._testInt12);
                //boundObject.AddBinding(boundObject._testInt13, rootObj._testInt13);
                //boundObject.AddBinding(boundObject._testInt14, rootObj._testInt14);

                // 15 x 50000 properties took 00:00:00.6475101
                // update values 1.89s (see below)
                //ExtendMObject.AddBinding(boundObject, p => p._testStringProperty, rootObj, p => p._testStringProperty);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt0, rootObj, p => p._testInt0);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt1, rootObj, p => p._testInt1);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt2, rootObj, p => p._testInt2);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt3, rootObj, p => p._testInt3);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt4, rootObj, p => p._testInt4);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt5, rootObj, p => p._testInt5);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt6, rootObj, p => p._testInt6);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt7, rootObj, p => p._testInt7);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt8, rootObj, p => p._testInt8);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt9, rootObj, p => p._testInt9);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt10, rootObj, p => p._testInt10);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt11, rootObj, p => p._testInt11);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt12, rootObj, p => p._testInt12);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt13, rootObj, p => p._testInt13);
                //ExtendMObject.AddBinding(boundObject, p => p._testInt14, rootObj, p => p._testInt14);
            }

            sw.Stop();
            yield return string.Format("15 x {0} properties took {1}", iterations, sw.Elapsed);

            sw.Reset();
            sw.Start();

            for(int i = 0; i < iterations / 100; i++)
                rootObj.TestInt0 = i;

            sw.Stop();

            yield return "Property value = " + boundObjects[100].TestInt0;
            yield return string.Format("Set 1 property with " + iterations + " objects bound to " + iterations / 100 + " times took {0}", sw.Elapsed);
        }
    }
}

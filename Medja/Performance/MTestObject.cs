using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Performance
{
    public class MTestObject : MObject, ITestObject
    {
        private readonly Property<string> _testStringProperty;
        private readonly Property<int> _testInt0;
        private readonly Property<int> _testInt1;
        private readonly Property<int> _testInt2;
        private readonly Property<int> _testInt3;
        private readonly Property<int> _testInt4;
        private readonly Property<int> _testInt5;
        private readonly Property<int> _testInt6;
        private readonly Property<int> _testInt7;
        private readonly Property<int> _testInt8;
        private readonly Property<int> _testInt9;
        private readonly Property<int> _testInt10;
        private readonly Property<int> _testInt11;
        private readonly Property<int> _testInt12;
        private readonly Property<int> _testInt13;
        private readonly Property<int> _testInt14;

        public string TestString
        {
            get { return _testStringProperty.Get(); }
            set { _testStringProperty.Set(value); }
        }

        public int TestInt0 { get { return _testInt0.Get(); } set { _testInt0.Set(value); } }
        public int TestInt1 { get { return _testInt1.Get(); } set { _testInt1.Set(value); } }
        public int TestInt2 { get { return _testInt2.Get(); } set { _testInt2.Set(value); } }
        public int TestInt3 { get { return _testInt3.Get(); } set { _testInt3.Set(value); } }
        public int TestInt4 { get { return _testInt4.Get(); } set { _testInt4.Set(value); } }
        public int TestInt5 { get { return _testInt5.Get(); } set { _testInt5.Set(value); } }
        public int TestInt6 { get { return _testInt6.Get(); } set { _testInt6.Set(value); } }
        public int TestInt7 { get { return _testInt7.Get(); } set { _testInt7.Set(value); } }
        public int TestInt8 { get { return _testInt8.Get(); } set { _testInt8.Set(value); } }
        public int TestInt9 { get { return _testInt9.Get(); } set { _testInt9.Set(value); } }
        public int TestInt10 { get { return _testInt10.Get(); } set { _testInt10.Set(value); } }
        public int TestInt11 { get { return _testInt11.Get(); } set { _testInt11.Set(value); } }
        public int TestInt12 { get { return _testInt12.Get(); } set { _testInt12.Set(value); } }
        public int TestInt13 { get { return _testInt13.Get(); } set { _testInt13.Set(value); } }
        public int TestInt14 { get { return _testInt14.Get(); } set { _testInt14.Set(value); } }


        public MTestObject()
        {
            _testStringProperty = AddProperty<string>();
            _testInt0 = AddProperty<int>();
            _testInt1 = AddProperty<int>();
            _testInt2 = AddProperty<int>();
            _testInt3 = AddProperty<int>();
            _testInt4 = AddProperty<int>();
            _testInt5 = AddProperty<int>();
            _testInt6 = AddProperty<int>();
            _testInt7 = AddProperty<int>();
            _testInt8 = AddProperty<int>();
            _testInt9 = AddProperty<int>();
            _testInt10 = AddProperty<int>();
            _testInt11 = AddProperty<int>();
            _testInt12 = AddProperty<int>();
            _testInt13 = AddProperty<int>();
            _testInt14 = AddProperty<int>();
        }
    }
}

namespace Medja.Performance
{
    public class TPropertyTestObject : ITestObject
    {
        private readonly Property<string> _strProp;
        private readonly Property<int> _int0Prop;
        private readonly Property<int> _int1Prop;
        private readonly Property<int> _int2Prop;
        private readonly Property<int> _int3Prop;
        private readonly Property<int> _int4Prop;
        private readonly Property<int> _int5Prop;
        private readonly Property<int> _int6Prop;
        private readonly Property<int> _int7Prop;
        private readonly Property<int> _int8Prop;
        private readonly Property<int> _int9Prop;
        private readonly Property<int> _int10Prop;
        private readonly Property<int> _int11Prop;
        private readonly Property<int> _int12Prop;
        private readonly Property<int> _int13Prop;
        private readonly Property<int> _int14Prop;

        public string TestString
        {
            get { return _strProp.Get(); }
            set { _strProp.Set(value); }
        }

        public int TestInt0
        {
            get { return _int0Prop.Get(); }
            set { _int0Prop.Set(value); }
        }

        public int TestInt1
        {
            get { return _int1Prop.Get(); }
            set { _int1Prop.Set(value); }
        }
        public int TestInt2
        {
            get { return _int2Prop.Get(); }
            set { _int2Prop.Set(value); }
        }
        public int TestInt3
        {
            get { return _int3Prop.Get(); }
            set { _int3Prop.Set(value); }
        }
        public int TestInt4
        {
            get { return _int4Prop.Get(); }
            set { _int4Prop.Set(value); }
        }
        public int TestInt5
        {
            get { return _int5Prop.Get(); }
            set { _int5Prop.Set(value); }
        }
        public int TestInt6
        {
            get { return _int6Prop.Get(); }
            set { _int6Prop.Set(value); }
        }
        public int TestInt7
        {
            get { return _int7Prop.Get(); }
            set { _int7Prop.Set(value); }
        }
        public int TestInt8
        {
            get { return _int8Prop.Get(); }
            set { _int8Prop.Set(value); }
        }
        public int TestInt9
        {
            get { return _int9Prop.Get(); }
            set { _int9Prop.Set(value); }
        }
        public int TestInt10
        {
            get { return _int10Prop.Get(); }
            set { _int10Prop.Set(value); }
        }
        public int TestInt11
        {
            get { return _int11Prop.Get(); }
            set { _int11Prop.Set(value); }
        }
        public int TestInt12
        {
            get { return _int12Prop.Get(); }
            set { _int12Prop.Set(value); }
        }
        public int TestInt13
        {
            get { return _int13Prop.Get(); }
            set { _int13Prop.Set(value); }
        }
        public int TestInt14
        {
            get { return _int14Prop.Get(); }
            set { _int14Prop.Set(value); }
        }

        public TPropertyTestObject()
        {
            _strProp = new Property<string>();
            _int0Prop = new Property<int>();
            _int1Prop = new Property<int>();
            _int2Prop = new Property<int>();
            _int3Prop = new Property<int>();
            _int4Prop = new Property<int>();
            _int5Prop = new Property<int>();
            _int6Prop = new Property<int>();
            _int7Prop = new Property<int>();
            _int8Prop = new Property<int>();
            _int9Prop = new Property<int>();
            _int10Prop = new Property<int>();
            _int11Prop = new Property<int>();
            _int12Prop = new Property<int>();
            _int13Prop = new Property<int>();
            _int14Prop = new Property<int>();
        }
    }
}

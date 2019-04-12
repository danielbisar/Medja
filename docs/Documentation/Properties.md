# Requirements

- Binding
  - Property Changed notification? or some other way to track changes

nice to have
  - easy serialization
-

Each operation system implements some kind of message loop and a usual program needs
to have something like this implemented (getting position and state of mouse/keyboard etc.)
So it might be helpful to stage change notifcations something like

Frame 1   Frame2
stateA    stateB  --> notify classes that depend on changes (could leed to further changes)
						--> update UI that depends on changes
							--> draw UI

Example 1

Frame 1
Button normal

Frame 2
Mouse over --> change visual

Frame 3 
Mouse click --> change visual  
		    --> handle click
			ORDER of this two?
			-> click before visual


Example 2 Slider controls ProgressBar via connected ViewModel

Mouse down --> change visual
Mouse move --> change visual --> change backend (via binding?)
Mouse up   --> change visual

The changes could also be send only if mouse is up (depends how the user wants it)







Performance tests

Object type: Medja.Performance.NativeTestObject
100000 create iterations took 00:00:00.0025227
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:01.4678248
Object type: Medja.Performance.DataTestObject
100000 create iterations took 00:00:00.0525998
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:15.9025107
Object type: Medja.Performance.TPropertyTestObject
100000 create iterations took 00:00:00.0226397
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:05.2523026


Object type: Medja.Performance.NativeTestObject
100000 create iterations took 00:00:00.0025551
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:01.4538194
Object type: Medja.Performance.DataTestObject
100000 create iterations took 00:00:00.0530835
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:15.7758104
Object type: Medja.Performance.TPropertyTestObject
100000 create iterations took 00:00:00.0228007
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:07.8823856


Debug, equality comparer as local var per property

Object type: Medja.Performance.NativeTestObject
100000 create iterations took 00:00:00.0033763
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:01.4638628
Object type: Medja.Performance.DataTestObject
100000 create iterations took 00:00:00.0532731
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:15.6551447
Object type: Medja.Performance.TPropertyTestObject
100000 create iterations took 00:00:00.0341555
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:07.0542460

Release

Object type: Medja.Performance.NativeTestObject
100000 create iterations took 00:00:00.0018172
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:00.7046572
Object type: Medja.Performance.DataTestObject
100000 create iterations took 00:00:00.0398304
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:09.2250837
Object type: Medja.Performance.TPropertyTestObject
100000 create iterations took 00:00:00.0196122
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:01.3129321



Decision: Equality comparer local, data object not used but property<T> class
TODO creation time optimization





DataTest object was:

namespace Medja.Performance
{
    public class DataTestObject : ITestObject
    {
        private readonly Data _data;
        private readonly IProperty _strProp;
        private readonly IProperty _int0Prop;
        private readonly IProperty _int1Prop;
        private readonly IProperty _int2Prop;
        private readonly IProperty _int3Prop;
        private readonly IProperty _int4Prop;
        private readonly IProperty _int5Prop;
        private readonly IProperty _int6Prop;
        private readonly IProperty _int7Prop;
        private readonly IProperty _int8Prop;
        private readonly IProperty _int9Prop;
        private readonly IProperty _int10Prop;
        private readonly IProperty _int11Prop;
        private readonly IProperty _int12Prop;
        private readonly IProperty _int13Prop;
        private readonly IProperty _int14Prop;

        public string TestString
        {
            get { return _data.Get<string>(_strProp); }
            set { _data.Set(_strProp, value); }
        }

        public int TestInt0
        {
            get { return _data.Get<int>(_int0Prop); }
            set { _data.Set(_int0Prop, value); }
        }

        public int TestInt1
        {
            get { return _data.Get<int>(_int1Prop); }
            set { _data.Set(_int1Prop, value); }
        }
        public int TestInt2
        {
            get { return _data.Get<int>(_int2Prop); }
            set { _data.Set(_int2Prop, value); }
        }
        public int TestInt3
        {
            get { return _data.Get<int>(_int3Prop); }
            set { _data.Set(_int3Prop, value); }
        }
        public int TestInt4
        {
            get { return _data.Get<int>(_int4Prop); }
            set { _data.Set(_int4Prop, value); }
        }
        public int TestInt5
        {
            get { return _data.Get<int>(_int5Prop); }
            set { _data.Set(_int5Prop, value); }
        }
        public int TestInt6
        {
            get { return _data.Get<int>(_int6Prop); }
            set { _data.Set(_int6Prop, value); }
        }
        public int TestInt7
        {
            get { return _data.Get<int>(_int7Prop); }
            set { _data.Set(_int7Prop, value); }
        }
        public int TestInt8
        {
            get { return _data.Get<int>(_int8Prop); }
            set { _data.Set(_int8Prop, value); }
        }
        public int TestInt9
        {
            get { return _data.Get<int>(_int9Prop); }
            set { _data.Set(_int9Prop, value); }
        }
        public int TestInt10
        {
            get { return _data.Get<int>(_int10Prop); }
            set { _data.Set(_int10Prop, value); }
        }
        public int TestInt11
        {
            get { return _data.Get<int>(_int11Prop); }
            set { _data.Set(_int11Prop, value); }
        }
        public int TestInt12
        {
            get { return _data.Get<int>(_int12Prop); }
            set { _data.Set(_int12Prop, value); }
        }
        public int TestInt13
        {
            get { return _data.Get<int>(_int13Prop); }
            set { _data.Set(_int13Prop, value); }
        }
        public int TestInt14
        {
            get { return _data.Get<int>(_int14Prop); }
            set { _data.Set(_int14Prop, value); }
        }


        public DataTestObject()
        {
            _strProp = new Property { Index = 0 };
            _int0Prop = new Property { Index = 1 };
            _int1Prop = new Property { Index = 2 };
            _int2Prop = new Property { Index = 3 };
            _int3Prop = new Property { Index = 4 };
            _int4Prop = new Property { Index = 5 };
            _int5Prop = new Property { Index = 6 };
            _int6Prop = new Property { Index = 7 };
            _int7Prop = new Property { Index = 8 };
            _int8Prop = new Property { Index = 9 };
            _int9Prop = new Property { Index = 10 };
            _int10Prop = new Property { Index = 11 };
            _int11Prop = new Property { Index = 12 };
            _int12Prop = new Property { Index = 13 };
            _int13Prop = new Property { Index = 14 };
            _int14Prop = new Property { Index = 15 };

            _data = new Data(new[]
            {
                _strProp,
                _int0Prop,
                _int1Prop,
                _int2Prop,
                _int3Prop,
                _int4Prop,
                _int5Prop,
                _int6Prop,
                _int7Prop,
                _int8Prop,
                _int9Prop,
                _int10Prop,
                _int11Prop,
                _int12Prop,
                _int13Prop,
                _int14Prop,
            });
        }
    }
}

class Data
    {
        private readonly object[] _data;
        private readonly IReadOnlyList<IProperty> _properties;

        public Data(IEnumerable<IProperty> properties)
        {
            _properties = properties.ToList();
            _data = new object[_properties.Count];
        }

        public T Get<T>(IProperty property)
        {
            var value = _data[property.Index];

            if(typeof(T).IsValueType)
                return (T) (value != null ? value : default(T));

            return (T)value;
        }

        public void Set<T>(IProperty property, T value)
        {
            _data[property.Index] = value;
        }
    }

	public class Property : IProperty
    {
        /// <summary>
        /// Local index of the property.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Global unique id of the property.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The properties name.
        /// </summary>
        public string Name { get; private set; }

        internal static Property Create(string name)
        {
            return new Property
            {
                Id = GeneratePropertyId(),
                Name = name
            };
        }

        private static int GeneratePropertyId()
        {
            // TODO some id generation, questions: same over different runs?, fast, thread-safe, unique
            return 0;
        }
    }





	Release, simplified property object

Object type: Medja.Performance.NativeTestObject
100000 create iterations took 00:00:00.0018445
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:00.6973240
Object type: Medja.Performance.TPropertyTestObject
100000 create iterations took 00:00:00.0187684
llsdk,5,10000000,10000001,10000002,10000003,10000004,10000005,10000006,10000007,10000008,10000009,10000010,10000011,10000012,10000013
10000000 property iterations took 00:00:01.3981172

public class Property<T> : IProperty<T>
    {
        private readonly EqualityComparer<T> _comparer;
        private T _value;

        public Property()
        {
            _comparer = EqualityComparer<T>.Default;
        }

        // would allow properties with and without change notification
        public void Set(T value)
        {
            if (_comparer.Equals(_value, value))
                return;

            _value = value;
        }

        public T Get()
        {
            return _value;
        }
    }
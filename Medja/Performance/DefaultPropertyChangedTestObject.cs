using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;
using System.Text;

namespace Medja.Performance
{
    public class DefaultPropertyChangedTestObject : ITestObject, INotifyPropertyChanged
    {
        private int _testInt0;
        private int _testInt1;
        private int _testInt2;
        private int _testInt3;
        private int _testInt4;
        private int _testInt5;
        private int _testInt6;
        private int _testInt7;
        private int _testInt8;
        private int _testInt9;
        private int _testInt10;
        private int _testInt11;
        private int _testInt12;
        private int _testInt13;
        private int _testInt14;

        private string _testString;
        public string TestString
        {
            get { return _testString; }
            set { SetValue(ref _testString, value); }
        }

        public int TestInt0 { get { return _testInt0; } set { SetValue(ref _testInt0, value); } }
        public int TestInt1 { get { return _testInt1; } set { SetValue(ref _testInt1, value); } }
        public int TestInt2 { get { return _testInt2; } set { SetValue(ref _testInt2, value); } }
        public int TestInt3 { get { return _testInt3; } set { SetValue(ref _testInt3, value); } }
        public int TestInt4 { get { return _testInt4; } set { SetValue(ref _testInt4, value); } }
        public int TestInt5 { get { return _testInt5; } set { SetValue(ref _testInt5, value); } }
        public int TestInt6 { get { return _testInt6; } set { SetValue(ref _testInt6, value); } }
        public int TestInt7 { get { return _testInt7; } set { SetValue(ref _testInt7, value); } }
        public int TestInt8 { get { return _testInt8; } set { SetValue(ref _testInt8, value); } }
        public int TestInt9 { get { return _testInt9; } set { SetValue(ref _testInt9, value); } }
        public int TestInt10 { get { return _testInt10; } set { SetValue(ref _testInt10, value); } }
        public int TestInt11 { get { return _testInt11; } set { SetValue(ref _testInt11, value); } }
        public int TestInt12 { get { return _testInt12; } set { SetValue(ref _testInt12, value); } }
        public int TestInt13 { get { return _testInt13; } set { SetValue(ref _testInt13, value); } }
        public int TestInt14 { get { return _testInt14; } set { SetValue(ref _testInt14, value); } }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void SetValue<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

namespace Medja.Properties.Test.Helpers
{
    public class DemoObj
    {
        public readonly Property<bool> PropertyIsA;
        public bool IsA
        {
            get => PropertyIsA.Get();
            set => PropertyIsA.Set(value);
        }

        public readonly Property<bool> PropertyIsB;
        public bool IsB
        {
            get => PropertyIsB.Get();
            set => PropertyIsB.Set(value);
        }

        public readonly Property<bool> PropertyIsC;

        public bool IsC
        {
            get => PropertyIsC.Get();
            set => PropertyIsC.Set(value);
        }

        public readonly Property<bool> PropertyIsD;

        public bool IsD
        {
            get => PropertyIsD.Get();
            set => PropertyIsD.Set(value);
        }

        public DemoObj()
        {
            PropertyIsA = new Property<bool>();
            PropertyIsB = new Property<bool>();
            PropertyIsC = new Property<bool>();
            PropertyIsD = new Property<bool>();
        }
    }
}
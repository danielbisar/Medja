using System;
using Medja.Properties.ChangeTracking;

namespace Medja.Properties.Test.ChangeTracking
{
    [Serializable]
    public class IUndoableTestObj : IUndoable
    {
        [field: NonSerialized]
        public PropertyRegistry PropertyRegistry { get; }

        public readonly Property<string> PropertyText;

        public string Text
        {
            get { return PropertyText.Get(); }
            set { PropertyText.Set(value); }
        }

        public readonly Property<int> PropertyNumber;

        public int Number
        {
            get { return PropertyNumber.Get(); }
            set { PropertyNumber.Set(value); }
        }

        public readonly Property<MedjaObservableCollection<IUndoableTestObj>> PropertyChildren;

        public MedjaObservableCollection<IUndoableTestObj> Children
        {
            get { return PropertyChildren.Get(); }
            set { PropertyChildren.Set(value); }
        }
        
        public IUndoableTestObj()
        {
            PropertyRegistry = new PropertyRegistry();
            PropertyRegistry.Add(nameof(Number), PropertyNumber = new Property<int>());
            PropertyRegistry.Add(nameof(Text), PropertyText = new Property<string>());

            PropertyChildren = new Property<MedjaObservableCollection<IUndoableTestObj>>();
            PropertyChildren.SetSilent(new MedjaObservableCollection<IUndoableTestObj>());
            PropertyRegistry.Add(nameof(Children), PropertyChildren);
        }
    }
}

using System.Collections.Specialized;
using Medja.Properties.ChangeTracking;
using Xunit;

namespace Medja.Properties.Test
{
    public class PropertyRegistryTest
    {
        [Fact]
        public void RecognizesValueChanged()
        {
            var registry = new PropertyRegistry();
            var property = new Property<string>();
            
            registry.Add("prop", property);
            
            property.Set("sf");

            Assert.True(registry.IsChanged);
            Assert.Collection(registry.Changes, p =>
            {
                Assert.Equal("prop", p.Name);
                Assert.Equal("sf", ((ValuePropertyChange)p).NewValue);
            });
        }

        [Fact]
        public void UnregistersOnDispose()
        {
            var registry = new PropertyRegistry();
            var property = new Property<string>();
            
            registry.Add("prop", property);
            registry.Dispose();
            
            property.Set("sf");

            Assert.False(registry.IsChanged);
        }

        [Fact]
        public void UndoDoesNotFailOnEmptyChanges()
        {
            var registry = new PropertyRegistry();

            Assert.Empty(registry.Changes);

            registry.UndoLastChange();
        }

        [Fact]
        public void UndoLastChange()
        {
            var registry = new PropertyRegistry(); 
            var property = new Property<string>();
            
            registry.Add("property", property);
            
            property.Set("123");
            
            registry.UndoLastChange();
            
            Assert.Empty(registry.Changes);
            Assert.False(registry.IsChanged);
            Assert.Null(property.Get());
        }
        
        [Fact]
        public void UndoMultipleChanges()
        {
            var registry = new PropertyRegistry(); 
            var property = new Property<string>();
            
            registry.Add("property", property);
            
            property.Set("123");
            property.Set("456");
            
            registry.UndoLastChange();
            
            Assert.Collection(registry.Changes, p =>
            {
                Assert.Null(((ValuePropertyChange)p).OldValue);
                Assert.Equal("123", ((ValuePropertyChange)p).NewValue);
                Assert.Equal("property", p.Name);
            });
            Assert.Equal("123", property.Get());

            registry.UndoLastChange();

            Assert.Empty(registry.Changes);
            Assert.False(registry.IsChanged);
            Assert.Null(property.Get());
        }

        [Fact]
        public void CommitChanges()
        {
            var registry = new PropertyRegistry(); 
            var property = new Property<string>();
            
            registry.Add("property", property);
            
            property.Set("123");
            property.Set("456");

            registry.CommitChanges();
            
            Assert.Empty(registry.Changes);
            Assert.False(registry.IsChanged);
            Assert.Equal("456", property.Get());
        }

        [Fact]
        public void UndoCollectionChanges()
        {
            var registry = new PropertyRegistry(); 
            var property = new Property<MedjaObservableCollection<string>>();
            
            var collection = new MedjaObservableCollection<string>();
            property.Set(collection);
            
            registry.Add("property", property);
            
            collection.Add("123");
            collection.Add("456");
            collection.RemoveAt(0);
            collection.Remove("456");
            
            Assert.Collection(registry.Changes, 
                p =>
                {
                    Assert.Equal(NotifyCollectionChangedAction.Add, ((CollectionPropertyChange)p).Action);
                    Assert.Equal("123", ((CollectionPropertyChange)p).Item);
                    Assert.Equal(0, ((CollectionPropertyChange)p).Index);
                },
                p =>
                {
                    Assert.Equal(NotifyCollectionChangedAction.Add, ((CollectionPropertyChange)p).Action);
                    Assert.Equal("456", ((CollectionPropertyChange)p).Item);
                    Assert.Equal(1, ((CollectionPropertyChange)p).Index);
                },
                p =>
                {
                    Assert.Equal(NotifyCollectionChangedAction.Remove, ((CollectionPropertyChange)p).Action);
                    Assert.Equal("123", ((CollectionPropertyChange)p).Item);
                    Assert.Equal(0, ((CollectionPropertyChange)p).Index);
                },
                p =>
                {
                    Assert.Equal(NotifyCollectionChangedAction.Remove, ((CollectionPropertyChange)p).Action);
                    Assert.Equal("456", ((CollectionPropertyChange)p).Item);
                    Assert.Equal(0, ((CollectionPropertyChange)p).Index);
                });
            
            registry.UndoLastChange();

            Assert.Collection(registry.Changes, 
                p =>
                {
                    Assert.Equal(NotifyCollectionChangedAction.Add, ((CollectionPropertyChange)p).Action);
                    Assert.Equal("123", ((CollectionPropertyChange)p).Item);
                    Assert.Equal(0, ((CollectionPropertyChange)p).Index);
                },
                p =>
                {
                    Assert.Equal(NotifyCollectionChangedAction.Add, ((CollectionPropertyChange)p).Action);
                    Assert.Equal("456", ((CollectionPropertyChange)p).Item);
                    Assert.Equal(1, ((CollectionPropertyChange)p).Index);
                },
                p =>
                {
                    Assert.Equal(NotifyCollectionChangedAction.Remove, ((CollectionPropertyChange)p).Action);
                    Assert.Equal("123", ((CollectionPropertyChange)p).Item);
                    Assert.Equal(0, ((CollectionPropertyChange)p).Index);
                });
            
            Assert.Collection(collection, p => Assert.Equal("456", p));
            
            registry.UndoLastChange();
            
            Assert.Collection(collection, p => Assert.Equal("123", p),
                p => Assert.Equal("456", p));
            
            registry.UndoChanges();
            
            Assert.Empty(registry.Changes);
            Assert.False(registry.IsChanged);
        }
        
        [Fact]
        public void UndoUnorderedCollectionChanges()
        {
            var registry = new PropertyRegistry(); 
            var property = new Property<MedjaObservableCollection<string>>();
            
            var collection = new MedjaObservableCollection<string>();
            property.Set(collection);
            
            registry.Add("property", property);
            
            collection.Add("123");
            collection.Add("456");
            collection.Insert(1, "5");
            collection.RemoveAt(2); // 456
            collection.Clear();
            
            registry.UndoLastChange();
            registry.UndoLastChange();

            Assert.Collection(collection, 
                p => Assert.Equal("123", p), 
                p => Assert.Equal("5", p), 
                p => Assert.Equal("456", p));
        }

        [Fact]
        public void UnregistersCollectionChangedOnDispose()
        {
            var registry = new PropertyRegistry(); 
            var property = new Property<MedjaObservableCollection<string>>();
            
            var collection = new MedjaObservableCollection<string>();
            property.Set(collection);
            
            registry.Add("property", property);
            registry.Dispose();
            
            collection.Add("123");
            
            Assert.False(registry.IsChanged);
            Assert.Empty(registry.Changes);
        }

        [Fact]
        public void GetChangesTreeSnapshot()
        {
            var registry = new PropertyRegistry();
            var property = new Property<IUndoableTestObj>();
            
            registry.Add("property", property);
            
            var obj = new IUndoableTestObj();
            
            property.Set(obj);
            
            obj.Number = 1;
            obj.Text = "tt";
            
            var obj2 = new IUndoableTestObj();
            obj2.Number = 2;
            obj2.Text = "some t";
            
            property.Set(obj2);
            
            var snapshot = registry.GetChangesTreeSnapshot();
            
            Assert.Collection(snapshot,
                p =>
                {
                    var valueChange = Assert.IsType<ValuePropertyChange>(p);
                    Assert.Equal("property", p.Name);
                    Assert.Equal(obj, valueChange.OldValue);
                    Assert.Equal(obj2, valueChange.NewValue);
                },
                p =>
                {
                    var subPropertyChanges = Assert.IsType<SubPropertyChanges>(p);
                    Assert.Equal("property", p.Name);
                    
                    Assert.Collection(subPropertyChanges.Changes, s =>
                        {
                            var valueChange = Assert.IsType<ValuePropertyChange>(s);
                            Assert.Equal("Number", s.Name);
                            Assert.Equal(0, valueChange.OldValue);
                            Assert.Equal(2, valueChange.NewValue);
                        },
                        s =>
                        {
                            var valueChange = Assert.IsType<ValuePropertyChange>(s);
                            Assert.Equal("Text", s.Name);
                            Assert.Null(valueChange.OldValue);
                            Assert.Equal("some t", valueChange.NewValue);
                        });
                });
        }

        [Fact]
        public void GetChangesTreeSnapshotFromSubLists()
        {
            var registry = new PropertyRegistry();
            var property = new Property<MedjaObservableCollection<IUndoableTestObj>>();
            
            registry.Add("property", property);
            
            var list = new MedjaObservableCollection<IUndoableTestObj>();
            property.Set(list);

            var item0 = new IUndoableTestObj {Text = "Item1"};
            var item1 = new IUndoableTestObj {Text = "Item2"};
            list.Add(item0);
            list.Add(item1);

            list[1].Number = 1;

            var snapshot = registry.GetChangesTreeSnapshot();

            Assert.Collection(snapshot,
                p =>
                {
                    var valueChange = Assert.IsType<ValuePropertyChange>(p);
                    Assert.Equal("property", p.Name);
                    Assert.Null(valueChange.OldValue);
                    Assert.Equal(list, valueChange.NewValue);
                },
                p =>
                {
                    var colChange = Assert.IsType<CollectionPropertyChange>(p);
                    Assert.Equal("property", p.Name);
                    Assert.Equal(NotifyCollectionChangedAction.Add, colChange.Action);
                    Assert.Equal(item0, colChange.Item);
                },
                p =>
                {
                    var colChange = Assert.IsType<CollectionPropertyChange>(p);
                    Assert.Equal("property", p.Name);
                    Assert.Equal(NotifyCollectionChangedAction.Add, colChange.Action);
                    Assert.Equal(item1, colChange.Item);
                },
                p =>
                {
                    var subChange = Assert.IsType<SubListItemChanges>(p);
                    Assert.Equal("property", p.Name);
                    Assert.Equal(0, subChange.Index);
                    Assert.Collection(subChange.Changes,
                        c =>
                        {
                            var valueChange = Assert.IsType<ValuePropertyChange>(c);
                            Assert.Equal("Text", c.Name);
                            Assert.Null(valueChange.OldValue);
                            Assert.Equal("Item1", valueChange.NewValue);
                        });
                },
                p =>
                {
                    var subChange = Assert.IsType<SubListItemChanges>(p);
                    Assert.Equal("property", p.Name);
                    Assert.Equal(1, subChange.Index);
                    Assert.Collection(subChange.Changes,
                        c =>
                        {
                            var valueChange = Assert.IsType<ValuePropertyChange>(c);
                            Assert.Equal("Text", c.Name);
                            Assert.Null(valueChange.OldValue);
                            Assert.Equal("Item2", valueChange.NewValue);
                        },
                        c =>
                        {
                            var valueChange = Assert.IsType<ValuePropertyChange>(c);
                            Assert.Equal("Number", c.Name);
                            Assert.Equal(1, valueChange.NewValue);
                        });
                });
        }

        [Fact]
        public void GetChangesTreeWithSubListWhenItemsAlreadyAdded()
        {
            var registry = new PropertyRegistry();
            var property = new Property<MedjaObservableCollection<IUndoableTestObj>>();
            
            var list = new MedjaObservableCollection<IUndoableTestObj>();
            list.Add(new IUndoableTestObj());

            property.Set(list);
            registry.Add("property", property);
            
            list[0].Number = 2;
            
            var snapshot = registry.GetChangesTreeSnapshot();
            
            Assert.Equal(1, snapshot.Count);
            var item = Assert.IsType<SubListItemChanges>(snapshot[0]);
            
            Assert.Equal(0, item.Index);
            Assert.Equal(1, item.Changes.Count);
            var valuePropertyChange = Assert.IsType<ValuePropertyChange>(item.Changes[0]);
            
            Assert.Equal("Number", valuePropertyChange.Name);
            Assert.Equal(2, valuePropertyChange.NewValue);
            Assert.Equal(0, valuePropertyChange.OldValue);
        }

        [Fact]
        public void CommitChangesTree()
        {
            var registry = new PropertyRegistry();
            var property = new Property<IUndoableTestObj>();

            registry.Add("property", property);

            var obj = new IUndoableTestObj();

            property.Set(obj);

            obj.Number = 1;
            obj.Text = "tt";
            
            Assert.True(obj.PropertyRegistry.IsChanged);
            Assert.True(registry.IsChanged);
            
            registry.CommitChangesTree();
            
            Assert.False(obj.PropertyRegistry.IsChanged);
            Assert.Empty(obj.PropertyRegistry.Changes);
            Assert.False(registry.IsChanged);
            Assert.Empty(registry.Changes);
            
        }

        [Fact]
        public void ApplySnapshot()
        {
            var tObj1 = new IUndoableTestObj();
            var tObj2 = new IUndoableTestObj();

            tObj1.Number = 1;
            tObj1.Text = "Hey";
            tObj1.Children.Add(new IUndoableTestObj {Text = "Sub"});

            var snapshot = tObj1.PropertyRegistry.GetChangesTreeSnapshot();
            tObj2.PropertyRegistry.ApplySnapshot(snapshot);

            Assert.Equal(1, tObj2.Number);
            Assert.Equal("Hey", tObj2.Text);
            Assert.Collection(tObj2.Children, p => { Assert.Equal("Sub", p.Text); });

            tObj1.PropertyRegistry.CommitChanges();
            tObj1.Children[0].Text = "Sub 1";

            var snapshot2 = tObj1.PropertyRegistry.GetChangesTreeSnapshot();
            tObj2.PropertyRegistry.ApplySnapshot(snapshot2);

            Assert.Collection(tObj2.Children, p => { Assert.Equal("Sub 1", p.Text); });
        }
    }
}
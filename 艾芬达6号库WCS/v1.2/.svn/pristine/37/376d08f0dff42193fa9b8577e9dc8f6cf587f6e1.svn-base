using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs.Framework.MessageBoard;

namespace Wcs.App.Plugins.MessageBoard
{
    public delegate void FilterHandler(object Sender, ref bool Include);
    public class MessageBindingListView: BindingList<Wcs.Framework.MessageBoard.AbstractMessage>, IBindingListView, ITypedList
    {
        public Int32 Size { get; set; }
        private List<PropertyComparer<Wcs.Framework.MessageBoard.AbstractMessage>> comparers;
        private FilterHandler mFilterHandler;
        private string mFilterString = string.Empty;

        [NonSerialized]
        private PropertyDescriptorCollection properties;

        private ArrayList unfilteredItems = new ArrayList();

        protected override void OnAddingNew(AddingNewEventArgs e)
        {
            base.OnAddingNew(e);

            while (this.Size < this.Count)
            {
                this.Remove(this.OrderBy(x => x.Id).First());
            }
        }

        protected override void InsertItem(int index, AbstractMessage item)
        {
            try
            {
                if (this.FilterHandler != null)
                {
                    bool include = true;
                    this.FilterHandler(item, ref include);
                    if (include == false)
                    {
                        return;
                    }
                }

                base.InsertItem(index, item);

                while (this.Size < this.Count)
                {
                    this.Remove(this.OrderBy(x => x.Id).First());
                }
            }
            catch
            {

            }
        }

        public MessageBindingListView()
        {
            Size = Int32.MaxValue;
            // Get the 'shape' of the list. 
            // Only get the public properties marked with Browsable = true.
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(
                typeof(Wcs.Framework.MessageBoard.AbstractMessage),
                new Attribute[]
                    {
                        new BrowsableAttribute(true)
                    });

            // Sort the properties.
            properties = pdc.Sort();
        }

        public MessageBindingListView(IList<Wcs.Framework.MessageBoard.AbstractMessage> list)
            : base(list)
        {
            Size = Int32.MaxValue;
            // Get the 'shape' of the list. 
            // Only get the public properties marked with Browsable = true.
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(typeof(Wcs.Framework.MessageBoard.AbstractMessage),
                new Attribute[]
                    {
                        new BrowsableAttribute(true)
                    });

            // Sort the properties.
            properties = pdc.Sort();
        }

        #region Sorting

        private bool isSorted;
        private ListSortDirection sortDirection;
        private PropertyDescriptor sortProperty;

        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }
        
        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            var items = Items as List<Wcs.Framework.MessageBoard.AbstractMessage>;

            if (items != null)
            {
                var pc = new PropertyComparer<Wcs.Framework.MessageBoard.AbstractMessage>(property, direction);
                items.Sort(pc);
                isSorted = true;
            }
            else
            {
                isSorted = false;
            }

            sortProperty = property;
            sortDirection = direction;

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        //public void Sort(PropertyDescriptor property, ListSortDirection direction)
        //{
        //    ApplySortCore(property, direction);
        //}

        #endregion

        #region Searching

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        protected override int FindCore(PropertyDescriptor property, object key)
        {
            // Specify search columns
            if (property == null) return -1;

            // Get list to search
            var items = Items as List<Wcs.Framework.MessageBoard.AbstractMessage>;
            // Traverse list for value
            if (items != null)
            {
                foreach (Wcs.Framework.MessageBoard.AbstractMessage item in items)
                {
                    // Test column search value
                    string value = property.GetValue(item).ToString();

                    // If value is the search value, return the 
                    // index of the data item
                    if (key.ToString() == value) return IndexOf(item);
                }
            }
            return -1;
        }

        #endregion

        #region IBindingListView 成员

        public void ApplySort(ListSortDescriptionCollection sorts)
        {
            // Get list to sort
            // Note: this.Items is a non-sortable ICollection<Wcs.Framework.MessageBoard.AbstractMessage>
            var items = Items as List<Wcs.Framework.MessageBoard.AbstractMessage>;

            // Apply and set the sort, if items to sort
            if (items != null)
            {
                SortDescriptions = sorts;
                comparers = new List<PropertyComparer<Wcs.Framework.MessageBoard.AbstractMessage>>();
                foreach (ListSortDescription sort in sorts)
                    comparers.Add(new PropertyComparer<Wcs.Framework.MessageBoard.AbstractMessage>(sort.PropertyDescriptor, sort.SortDirection));
                items.Sort(CompareValuesByProperties);
                //_isSorted = true;
            }

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        public string Filter
        {
            get { return mFilterString; }
            set
            {
                bool Include = false;

                while (Items.Count > Size)
                {
                    Items.Remove(Items.OrderBy(x => x.Id).First());
                }

                foreach (var item in Items)
                {
                    if (!unfilteredItems.Contains(item))
                    {
                        unfilteredItems.Add(item);
                    }
                }
                //if (string.IsNullOrEmpty(mFilterString))
                //{
                //    unfilteredItems.AddRange((ICollection)Items);
                //}
                Clear();
                foreach (Wcs.Framework.MessageBoard.AbstractMessage item in unfilteredItems)
                {
                    if (mFilterHandler != null)
                    {
                        Include = true;
                        mFilterHandler.Invoke(item, ref Include);
                        if (Include) Add(item);
                    }
                    else
                    {
                        Add(item);
                    }
                }
                mFilterString = value;
            }
        }

        public void RemoveFilter()
        {
            Clear();

            foreach (Wcs.Framework.MessageBoard.AbstractMessage item in unfilteredItems)
            {
                Add(item);
            }
            unfilteredItems.Clear();
        }

        public ListSortDescriptionCollection SortDescriptions { get; set; }

        public bool SupportsAdvancedSorting
        {
            get { return true; }
        }

        public bool SupportsFiltering
        {
            //get { return true; }
            get { return true; }
        }

        #endregion

        public FilterHandler FilterHandler
        {
            private get
            {
                return mFilterHandler;
            }
            set
            {
                mFilterHandler = value;
                if (mFilterHandler == null)
                {
                    Filter = "";
                }
                else
                {
                    Filter = "any no zero length string";
                }
            }
        }

        private int CompareValuesByProperties(Wcs.Framework.MessageBoard.AbstractMessage x, Wcs.Framework.MessageBoard.AbstractMessage y)
        {
            if (x == null)
                return (y == null) ? 0 : -1;
            if (y == null)
                return 1;
            foreach (var comparer in comparers)
            {
                int retval = comparer.Compare(x, y);
                if (retval != 0)
                    return retval;
            }
            return 0;
        }

        #region ITypedList 成员

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            PropertyDescriptorCollection pdc;

            if (null == listAccessors)
            {
                // Return properties in sort order.
                pdc = properties;
            }
            else
            {
                // Return child list shape.
                pdc = ListBindingHelper.GetListItemProperties(listAccessors[0].PropertyType);
            }

            return pdc;
        }

        // This method is only used in the design-time framework 
        // and by the obsolete DataGrid control.
        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return typeof(Wcs.Framework.MessageBoard.AbstractMessage).Name;
        }

        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows;

namespace Lab_10
{
    public class SortableBindingList<T> : BindingList<T>
    {
        protected override bool SupportsSortingCore => true;
        protected override bool SupportsSearchingCore => true;
        private PropertyDescriptor _sortProperty;
        private ListSortDirection _sortDirection;
        private List<T> originalList;

        public SortableBindingList() : base()
        {
            originalList = new List<T>();
        }
        public SortableBindingList(IList<T> list) : base(list)
        {
            originalList = list.ToList();
        }

        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            for (int i = 0; i < Count; i++)
            {
                if (prop.GetValue(this[i]).Equals(key))
                    return i;
            }
            return -1;
        }

        public List<T> FindAllCore(PropertyDescriptor prop, object key)
        {
            List<T> foundItems = new List<T>();
            for (int i = 0; i < Count; i++)
            {
                if (prop.GetValue(this[i])?.Equals(key) ?? false)
                {
                    foundItems.Add(this[i]);
                }
            }
            return foundItems;
        }

        public int Find(string propertyName, object key)
        {
            PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(T)).Find(propertyName, true);
            if (prop == null)
                throw new ArgumentException("Invalid property name", nameof(propertyName));
            return FindCore(prop, key);
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            var items = Items as List<T>;
            if (items == null) return;

            items.Sort((x, y) =>
            {
                var propValue1 = prop.GetValue(x);
                var propValue2 = prop.GetValue(y);
                return direction == ListSortDirection.Ascending
                    ? Comparer.Default.Compare(propValue1, propValue2)
                    : Comparer.Default.Compare(propValue2, propValue1);
            });

            _sortProperty = prop;
            _sortDirection = direction;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override PropertyDescriptor SortPropertyCore => _sortProperty;
        protected override ListSortDirection SortDirectionCore => _sortDirection;

        public void SortByProperty(string propertyName, ListSortDirection direction)
        {
            string[] propertyNames = propertyName.Split('.');
            PropertyDescriptor prop = null;

            for (int i = 0; i < propertyNames.Length; i++)
            {
                if (i == 0)
                {
                    prop = TypeDescriptor.GetProperties(typeof(T)).Find(propertyNames[i], false);
                }
                else
                {
                    if (prop != null)
                    {
                        prop = prop.GetChildProperties().Find(propertyNames[i], false);
                    }
                }
            }

            if (prop != null)
            {
                ApplySortCore(prop, direction);
            }
        }

        public void SortByModel(ListSortDirection direction)
        {
            SortByProperty("Model", direction);
        }

        public void SortByYear(ListSortDirection direction)
        {
            SortByProperty("Year", direction);
        }

        public void SortByMotorHorsePower(ListSortDirection direction)
        {
            SortByProperty("Motor.HorsePower", direction);
        }
    }
}

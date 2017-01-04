using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VsTrello.UI
{
    public static class GridViewColumnCollection
    {
        public static readonly DependencyProperty ColumnCollectionBehaviourProperty =
            DependencyProperty.RegisterAttached("ColumnCollectionBehaviour", typeof(GridViewColumnCollectionBehaviour), typeof(GridViewColumnCollection), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ColumnsSourceProperty =
            DependencyProperty.RegisterAttached("ColumnsSource", typeof(object), typeof(GridViewColumnCollection), new UIPropertyMetadata(null, GridViewColumnCollection.ColumnsSourceChanged));

        public static readonly DependencyProperty DisplayMemberFormatMemberProperty =
            DependencyProperty.RegisterAttached("DisplayMemberFormatMember", typeof(string), typeof(GridViewColumnCollection), new UIPropertyMetadata(null, GridViewColumnCollection.DisplayMemberFormatMemberChanged));

        public static readonly DependencyProperty DisplayMemberMemberProperty =
            DependencyProperty.RegisterAttached("DisplayMemberMember", typeof(string), typeof(GridViewColumnCollection), new UIPropertyMetadata(null, GridViewColumnCollection.DisplayMemberMemberChanged));

        public static readonly DependencyProperty HeaderTextMemberProperty =
            DependencyProperty.RegisterAttached("HeaderTextMember", typeof(string), typeof(GridViewColumnCollection), new UIPropertyMetadata(null, GridViewColumnCollection.HeaderTextMemberChanged));

        public static readonly DependencyProperty WidthMemberProperty =
            DependencyProperty.RegisterAttached("WidthMember", typeof(string), typeof(GridViewColumnCollection), new UIPropertyMetadata(null, GridViewColumnCollection.WidthMemberChanged));

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static GridViewColumnCollectionBehaviour GetColumnCollectionBehaviour(DependencyObject obj)
        {
            return (GridViewColumnCollectionBehaviour)obj.GetValue(ColumnCollectionBehaviourProperty);
        }

        public static void SetColumnCollectionBehaviour(DependencyObject obj, GridViewColumnCollectionBehaviour value)
        {
            obj.SetValue(ColumnCollectionBehaviourProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static object GetColumnsSource(DependencyObject obj)
        {
            return (object)obj.GetValue(ColumnsSourceProperty);
        }

        public static void SetColumnsSource(DependencyObject obj, object value)
        {
            obj.SetValue(ColumnsSourceProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static string GetDisplayMemberFormatMember(DependencyObject obj)
        {
            return (string)obj.GetValue(DisplayMemberFormatMemberProperty);
        }

        public static void SetDisplayMemberFormatMember(DependencyObject obj, string value)
        {
            obj.SetValue(DisplayMemberFormatMemberProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static string GetDisplayMemberMember(DependencyObject obj)
        {
            return (string)obj.GetValue(DisplayMemberMemberProperty);
        }

        public static void SetDisplayMemberMember(DependencyObject obj, string value)
        {
            obj.SetValue(DisplayMemberMemberProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static string GetHeaderTextMember(DependencyObject obj)
        {
            return (string)obj.GetValue(HeaderTextMemberProperty);
        }

        public static void SetHeaderTextMember(DependencyObject obj, string value)
        {
            obj.SetValue(HeaderTextMemberProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static string GetWidthMember(DependencyObject obj)
        {
            return (string)obj.GetValue(WidthMemberProperty);
        }

        public static void SetWidthMember(DependencyObject obj, string value)
        {
            obj.SetValue(WidthMemberProperty, value);
        }

        private static void ColumnsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GridViewColumnCollection.GetOrCreateColumnCollectionBehaviour(sender).ColumnsSource = e.NewValue;
        }

        private static void DisplayMemberFormatMemberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GridViewColumnCollection.GetOrCreateColumnCollectionBehaviour(sender).DisplayMemberFormatMember = e.NewValue as string;
        }

        private static void DisplayMemberMemberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GridViewColumnCollection.GetOrCreateColumnCollectionBehaviour(sender).DisplayMemberMember = e.NewValue as string;
        }

        private static void HeaderTextMemberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GridViewColumnCollection.GetOrCreateColumnCollectionBehaviour(sender).HeaderTextMember = e.NewValue as string;
        }

        private static void WidthMemberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GridViewColumnCollection.GetOrCreateColumnCollectionBehaviour(sender).WidthMember = e.NewValue as string;
        }

        private static GridViewColumnCollectionBehaviour GetOrCreateColumnCollectionBehaviour(DependencyObject source)
        {
            GridViewColumnCollectionBehaviour behaviour = GetColumnCollectionBehaviour(source);

            if (behaviour == null)
            {
                GridView typedSource = source as GridView;

                if (typedSource == null)
                {
                    throw new Exception("This property can only be set on controls deriving GridView");
                }

                behaviour = new GridViewColumnCollectionBehaviour(typedSource);

                SetColumnCollectionBehaviour(typedSource, behaviour);
            }

            return behaviour;
        }
    }

    public class GridViewColumnCollectionBehaviour
    {
        private object columnsSource;
        private GridView gridView;

        public GridViewColumnCollectionBehaviour(GridView gridView)
        {
            this.gridView = gridView;
        }

        public object ColumnsSource
        {
            get { return this.columnsSource; }
            set
            {
                object oldValue = this.columnsSource;
                this.columnsSource = value;
                this.ColumnsSourceChanged(oldValue, this.columnsSource);
            }
        }

        public string DisplayMemberFormatMember { get; set; }

        public string DisplayMemberMember { get; set; }

        public string HeaderTextMember { get; set; }

        public string WidthMember { get; set; }

        private void AddHandlers(ICollectionView collectionView)
        {
            collectionView.CollectionChanged += this.ColumnsSource_CollectionChanged;
        }

        private void ColumnsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ICollectionView view = sender as ICollectionView;

            if (this.gridView == null)
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        GridViewColumn column = CreateColumn(e.NewItems[i]);
                        gridView.Columns.Insert(e.NewStartingIndex + i, column);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    List<GridViewColumn> columns = new List<GridViewColumn>();

                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        GridViewColumn column = gridView.Columns[e.OldStartingIndex + i];
                        columns.Add(column);
                    }

                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        GridViewColumn column = columns[i];
                        gridView.Columns.Insert(e.NewStartingIndex + i, column);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        gridView.Columns.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        GridViewColumn column = CreateColumn(e.NewItems[i]);

                        gridView.Columns[e.NewStartingIndex + i] = column;
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    gridView.Columns.Clear();
                    CreateColumns(sender as ICollectionView);
                    break;
                default:
                    break;
            }
        }

        private void ColumnsSourceChanged(object oldValue, object newValue)
        {
            if (this.gridView != null)
            {
                gridView.Columns.Clear();

                if (oldValue != null)
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(oldValue);

                    if (view != null)
                    {
                        this.RemoveHandlers(view);
                    }
                }

                if (newValue != null)
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(newValue);

                    if (view != null)
                    {
                        this.AddHandlers(view);

                        this.CreateColumns(view);
                    }
                }
            }
        }

        private GridViewColumn CreateColumn(object columnSource)
        {
            GridViewColumn column = new GridViewColumn();

            if (!string.IsNullOrEmpty(this.HeaderTextMember))
            {
                column.Header = GetPropertyValue(columnSource, this.HeaderTextMember);
            }

            if (!string.IsNullOrEmpty(this.DisplayMemberMember))
            {
                string propertyName = GetPropertyValue(columnSource, this.DisplayMemberMember) as string;

                string format = null;

                if (!string.IsNullOrEmpty(this.DisplayMemberFormatMember))
                {
                    format = GetPropertyValue(columnSource, this.DisplayMemberFormatMember) as string;
                }

                if (string.IsNullOrEmpty(format))
                {
                    format = "{0}";
                }

                column.DisplayMemberBinding = new Binding(propertyName) { StringFormat = format };
            }

            if (!string.IsNullOrEmpty(this.WidthMember))
            {
                double width = (double)GetPropertyValue(columnSource, this.WidthMember);
                column.Width = width;
            }

            return column;
        }

        private void CreateColumns(ICollectionView collectionView)
        {
            foreach (object item in collectionView)
            {
                GridViewColumn column = this.CreateColumn(item);

                this.gridView.Columns.Add(column);
            }
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            object returnVal = null;

            if (obj != null)
            {
                PropertyInfo prop = obj.GetType().GetProperty(propertyName);

                if (prop != null)
                {
                    returnVal = prop.GetValue(obj, null);
                }
            }

            return returnVal;
        }

        private void RemoveHandlers(ICollectionView collectionView)
        {
            collectionView.CollectionChanged -= this.ColumnsSource_CollectionChanged;
        }
    }
}
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;

/// <summary>
/// https://www.codeproject.com/Articles/35416/Use-of-MarkupExtension-with-Converters-in-WPF
/// </summary>
namespace LinkageComboBox
{
    [ContentProperty("Selector")]
    public class SelectorViewAdapter : ContentControl
    {
        public SelectorViewAdapter()
        {
            IsTabStop = false;
        }

        protected ICollectionView CollectionView { get; set; }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SelectorViewAdapter), new PropertyMetadata(0));

        public static readonly DependencyProperty SelectorProperty = DependencyProperty.Register(
            "Selector", typeof(Selector), typeof(SelectorViewAdapter),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(Selector_Changed)));

        static void Selector_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SelectorViewAdapter adapter = (SelectorViewAdapter)sender;
            adapter.Content = e.NewValue;
            var selector = (Selector)e.OldValue;
            selector = (Selector)e.NewValue;
            if (selector != null)
                selector.IsSynchronizedWithCurrentItem = true;

            adapter.Adapt();
        }

        public Selector Selector
        {
            get { return (Selector)GetValue(SelectorProperty); }
            set { SetValue(SelectorProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(SelectorViewAdapter),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsSource_Changed)));

        static void ItemsSource_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SelectorViewAdapter adapter = (SelectorViewAdapter)sender;
            adapter.Adapt();
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        protected void Adapt()
        {
            if (CollectionView != null)
            {
                CollectionView.CurrentChanged -= CollectionView_CurrentChanged;
                CollectionView = null;
            }
            if (Selector != null && ItemsSource != null)
            {
                CompositeCollection comp = new CompositeCollection();
                comp.Add(new CollectionContainer { Collection = ItemsSource });

                CollectionView = CollectionViewSource.GetDefaultView(comp);
                if (CollectionView != null)
                    CollectionView.CurrentChanged += CollectionView_CurrentChanged;

                Selector.ItemsSource = comp;
            }
        }

        void CollectionView_CurrentChanged(object sender, EventArgs e)
        {
            if (Selector != null && ((ICollectionView)sender).CurrentItem == null && Selector.Items.Count != 0)
            {
                Selector.SelectedIndex = 0;
            }
            else
            {
                SelectedIndex = Selector.SelectedIndex;
            }
        }
    }
}

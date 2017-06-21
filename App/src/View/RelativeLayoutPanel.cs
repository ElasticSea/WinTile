using System;
using System.Windows;
using System.Windows.Controls;

namespace App.View
{
    public class RelativeLayoutPanel : Panel
    {
        // TODO WHy do I have to use affetsParentaArange ?
        public static readonly DependencyProperty RelativeXProperty = DependencyProperty.RegisterAttached(
            "RelativeX", typeof(double), typeof(RelativeLayoutPanel),
            new FrameworkPropertyMetadata(0d,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                FrameworkPropertyMetadataOptions.AffectsParentArrange |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty RelativeYProperty = DependencyProperty.RegisterAttached(
            "RelativeY", typeof(double), typeof(RelativeLayoutPanel),
            new FrameworkPropertyMetadata(0d,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                FrameworkPropertyMetadataOptions.AffectsParentArrange |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty RelativeWidthProperty = DependencyProperty.RegisterAttached(
            "RelativeWidth", typeof(double), typeof(RelativeLayoutPanel),
            new FrameworkPropertyMetadata(0d,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                FrameworkPropertyMetadataOptions.AffectsParentArrange |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty RelativeHeightProperty = DependencyProperty.RegisterAttached(
            "RelativeHeight", typeof(double), typeof(RelativeLayoutPanel),
            new FrameworkPropertyMetadata(0d,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                FrameworkPropertyMetadataOptions.AffectsParentArrange |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static double GetRelativeX(UIElement element)
        {
            return (double) element.GetValue(RelativeXProperty);
        }

        public static void SetRelativeX(UIElement element, double value)
        {
            element.SetValue(RelativeXProperty, value);
        }

        public static double GetRelativeY(UIElement element)
        {
            return (double) element.GetValue(RelativeYProperty);
        }

        public static void SetRelativeY(UIElement element, double value)
        {
            element.SetValue(RelativeYProperty, value);
        }

        public static double GetRelativeWidth(UIElement element)
        {
            return (double) element.GetValue(RelativeWidthProperty);
        }

        public static void SetRelativeWidth(UIElement element, double value)
        {
            element.SetValue(RelativeWidthProperty, value);
        }

        public static double GetRelativeHeight(UIElement element)
        {
            return (double) element.GetValue(RelativeHeightProperty);
        }

        public static void SetRelativeHeight(UIElement element, double value)
        {
            element.SetValue(RelativeHeightProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (UIElement element in InternalChildren)
                element.Measure(availableSize);

            return new Size();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement element in InternalChildren)
            {
                var x = GetRelativeX(element) * finalSize.Width;
                var y = GetRelativeY(element) * finalSize.Height;
                var width = GetRelativeWidth(element) * finalSize.Width;
                var height = GetRelativeHeight(element) * finalSize.Height;

                width = Math.Max(0, width);
                height = Math.Max(0, height);

                element.Arrange(new Rect(x, y, width, height));
            }

            return finalSize;
        }
    }
}
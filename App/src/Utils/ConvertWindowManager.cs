using static System.Windows.SystemParameters;
using Rect = App.Model.Rect;

namespace App
{
    public class ConvertWindowManager : IWindowManager
    {
        private readonly IWindowManager wrapped;

        public ConvertWindowManager(IWindowManager wrapped)
        {
            this.wrapped = wrapped;
        }

        public Rect CurrentWindowRect
        {
            get => PxtoPercent(wrapped.CurrentWindowRect);
            set => wrapped.CurrentWindowRect = PercentToPx(value);
        }

        private Rect MonitorRect => new Rect(0, 0, (int)WorkArea.Width, (int)WorkArea.Height);

        private Rect PxtoPercent(Rect rect)
        {
            return (rect * 100).shrink(MonitorRect.Width, MonitorRect.Height);
        }

        private Rect PercentToPx(Rect rect)
        {
            return rect.extend(MonitorRect.Width, MonitorRect.Height) / 100;
        }
    }
}
using System.Windows.Input;
using static System.Windows.SystemParameters;

namespace App.Model.Managers
{
    public class PositionWIndowManager
    {
        public PositionWIndowManager(TileManager tileManager)
        {
            new HotKeyUtils(
                Key.Right,
                KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt,
                k => {
                    tileManager.MoveNext();
                    if(tileManager.Selected!=null) PositionWindow(tileManager.Selected);
                });

            new HotKeyUtils(
                Key.Left,
                KeyModifier.Win | KeyModifier.Shift | KeyModifier.Alt,
                k => {
                    tileManager.MovePrev();
                    if (tileManager.Selected != null) PositionWindow(tileManager.Selected);
                });
        }

        private void PositionWindow(Tile tile)
        {
            var pxRect = tile.Rect.extend(WorkArea.Width, WorkArea.Height);
            User32Utils.SetCurrentWindowPos((int)pxRect.Left, (int)pxRect.Top, (int)pxRect.Width,
                (int)pxRect.Height);
        }
    }
}
using System.Collections.ObjectModel;
using App;
using App.Model;
using App.Model.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rect = App.Model.Rect;

namespace Tests.Model.Managers
{
    [TestClass]
    public class TileManagerTests
    {

        [TestMethod]
        public void CycleTiles()
        {
            var tileA = new Tile(new Rect(0,0,100,200));
            var tileB = new Tile(new Rect(100,200,200,400));

            var observableCollection = new ObservableCollection<Tile>();

            var user32 = new EditorWindowManager { MonitorRect = new Rect(0, 0, 3840, 2160) };
            var positionSystem = new PositioningSystem(observableCollection, user32);
            var tileManager = new TileManager(observableCollection, positionSystem);
            observableCollection.Add(tileA);
            observableCollection.Add(tileB);

            positionSystem.Selected = tileB;

            tileManager.PositionNext();
            Assert.AreEqual(positionSystem.Selected, tileA);
            tileManager.PositionPrev();
            Assert.AreEqual(positionSystem.Selected, tileB);
        }
    }
}
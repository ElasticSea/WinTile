using System.Collections.ObjectModel;
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
            var tileA = new Tile(new Rect(), new Hotkey());
            var tileB = new Tile(new Rect(), new Hotkey());

            var observableCollection = new ObservableCollection<Tile>();
            var tileManager = new TileManager(observableCollection);
            observableCollection.Add(tileA);
            observableCollection.Add(tileB);

            tileManager.PositionNext();
            Assert.AreEqual(tileManager.Selected, tileA);
            tileManager.PositionPrev();
            Assert.AreEqual(tileManager.Selected, tileB);
        }
    }
}
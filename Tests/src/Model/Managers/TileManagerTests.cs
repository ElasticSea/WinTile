using System.Collections.ObjectModel;
using System.Linq;
using App.Model;
using App.Model.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Model.Managers
{
    [TestClass]
    public class TileManagerTests
    {
        [TestMethod]
        public void AddTile()
        {
            var tile = new Tile(new Rect(), new Hotkey());

            var tileManager = new TileManager(new ObservableCollection<Tile>());
            tileManager.Add(tile);

            Assert.AreEqual(tileManager.Selected, tile);
        }

        [TestMethod]
        public void AddAndRemoveTile()
        {
            var tile = new Tile(new Rect(), new Hotkey());

            var tileManager = new TileManager(new ObservableCollection<Tile>());
            tileManager.Add(tile);
            tileManager.Remove(tile);

            Assert.AreEqual(tileManager.Selected, null);
        }

        [TestMethod]
        public void CycleTiles()
        {
            var tileA = new Tile(new Rect(), new Hotkey());
            var tileB = new Tile(new Rect(), new Hotkey());

            var tileManager = new TileManager(new ObservableCollection<Tile>());
            tileManager.Add(tileA);
            tileManager.Add(tileB);

            tileManager.MoveNext();
            Assert.AreEqual(tileManager.Selected, tileA);
            tileManager.MovePrev();
            Assert.AreEqual(tileManager.Selected, tileB);
        }

        [TestMethod]
        public void ClearTiles()
        {
            var tileA = new Tile(new Rect(), new Hotkey());

            var collection = new ObservableCollection<Tile>();

            var tileManager = new TileManager(collection);
            tileManager.Add(tileA);
            tileManager.Clear();

            Assert.AreEqual(tileManager.Selected, null);
            Assert.AreEqual(collection.Count(), 0);
        }
    }
}
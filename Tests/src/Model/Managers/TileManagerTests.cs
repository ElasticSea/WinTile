using System.Collections.ObjectModel;
using System.Windows;
using App;
using App.Model;
using App.Model.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Rect = App.Model.Rect;

namespace Tests.Model.Managers
{
    [TestClass]
    public class TileManagerTests
    {
        private readonly TilePositionManager mocked = Substitute.For<TilePositionManager>();

        [TestMethod]
        public void CycleTiles()
        {
            var tileA = new Tile(new Rect(), new Hotkey());
            var tileB = new Tile(new Rect(), new Hotkey());

            var observableCollection = new ObservableCollection<Tile>();
            var tileManager = new TileManager(observableCollection, mocked);
            observableCollection.Add(tileA);
            observableCollection.Add(tileB);

            tileManager.PositionNext();
            Assert.AreEqual(tileManager.Selected, tileA);
            tileManager.PositionPrev();
            Assert.AreEqual(tileManager.Selected, tileB);
        }

        [TestMethod]
        public void HeurTest()
        {
            var direction = new Vector(1, 0);
            var l = 100;
            var tileA = new Tile(new Rect(0,0,l,l), new Hotkey());
            var tileB = new Tile(new Rect(l,0,2*l,l), new Hotkey());

            var observableCollection = new ObservableCollection<Tile>();
            var tileManager = new TileManager(observableCollection, mocked);
            observableCollection.Add(tileA);
            observableCollection.Add(tileB);

            Assert.AreEqual(tileManager.TitleCloserating(direction, tileA, tileB), l);
        }
    }
}
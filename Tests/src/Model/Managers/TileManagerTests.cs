using System;
using System.Collections.Generic;
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
        private static readonly Tile tileA = new Tile(new Rect(0, 0, 60, 100), new Hotkey());
        private static readonly Tile tileB = new Tile(new Rect(60, 0, 84, 40), new Hotkey());
        private static readonly Tile tileC = new Tile(new Rect(84, 0, 100, 40), new Hotkey());
        private static readonly Tile tileD = new Tile(new Rect(60, 0, 100, 40), new Hotkey());
        private static readonly Tile tileE = new Tile(new Rect(60, 40, 84, 100), new Hotkey());
        private static readonly Tile tileF = new Tile(new Rect(84, 40, 100, 100), new Hotkey());
        private static readonly Tile tileG = new Tile(new Rect(60, 40, 100, 100), new Hotkey());
        private static readonly Tile tileH = new Tile(new Rect(60, 0, 84, 100), new Hotkey());
        private static readonly Tile tileI = new Tile(new Rect(84, 0, 100, 100), new Hotkey());

        private readonly List<Tile> tiles = new List<Tile>(){ tileA , tileB , tileC , tileD , tileE , tileF , tileG , tileH, tileI };

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
        public void NextTo()
        {
            var direction = new Vector(1, 0);
            Assert.AreEqual(TileManager.TilePenalty(direction, tileB, tileC), 1);
        }

        [TestMethod]
        public void OpositeTo()
        {
            var direction = new Vector(-1, 0);
            Assert.AreEqual(TileManager.TilePenalty(direction, tileB, tileB), Double.NaN);
        }

        [TestMethod]
        public void Corner()
        {
            var direction = new Vector(1, 0);
            var tileA = new Tile(new Rect(0, 0, 100, 100), new Hotkey());
            var tileB = new Tile(new Rect(100, 100, 200, 200), new Hotkey());
            Assert.AreEqual(TileManager.TilePenalty(direction, tileA, tileB), .5f);
        }

        [TestMethod]
        public void Above()
        {
            var direction = new Vector(1, 0);
            Assert.AreEqual(TileManager.TilePenalty(direction, tileE, tileB), 0);
        }

        [TestMethod]
        public void PerpendicularDirection()
        {
            var direction = new Vector(0, -1);
            Assert.AreEqual(TileManager.TilePenalty(direction, tileE, tileF), 0);
        }

        [TestMethod]
        public void CloseUp()
        {
            var observableCollection = new ObservableCollection<Tile>(tiles);
            var tileManager = new TileManager(observableCollection, mocked);

            tileManager.Selected = tileE;
            tileManager.PositionClosestUp();

            Assert.AreEqual(tileManager.Selected, tileB);
        }
    }
}
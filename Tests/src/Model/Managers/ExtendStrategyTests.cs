using System.Collections.Generic;
using App.Model;
using App.Model.Managers;
using App.Model.Managers.Strategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rect = App.Model.Rect;

namespace Tests.Model.Managers
{
    [TestClass]
    public class ExtendStrategyTests
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

        private static readonly List<Tile> tiles = new List<Tile> { tileA , tileB , tileC , tileD , tileE , tileF , tileG , tileH, tileI };
        private readonly ExtendStrategy ext = new ExtendStrategy(tiles);

        [TestMethod]
        public void BCMergeToD()
        {
            Assert.AreEqual(ext.Right(tileB), tileD);
        }

        [TestMethod]
        public void CBMergeToD()
        {
            Assert.AreEqual(ext.Left(tileC), tileD);
        }

        [TestMethod]
        public void EFMergeToG()
        {
            Assert.AreEqual(ext.Right(tileE), tileG);
        }

        [TestMethod]
        public void FEMergeToG()
        {
            Assert.AreEqual(ext.Left(tileF), tileG);
        }

        [TestMethod]
        public void EBMergeToH()
        {
            Assert.AreEqual(ext.Up(tileE), tileH);
        }

        [TestMethod]
        public void BEMergeToH()
        {
            Assert.AreEqual(ext.Down(tileB), tileH);
        }

        [TestMethod]
        public void ARightMergeNew()
        {
            var newRect = new Rect(0,0,84,100);
            Assert.AreEqual(ext.Right(tileA).Rect, newRect);
        }

        [TestMethod]
        public void CRightSame()
        {
            Assert.AreEqual(ext.Right(tileC), tileC);
        }
    }
}
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
    public class ClosestStrategyTests
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
        private readonly ClosestStrategy ext = new ClosestStrategy(tiles);

        [TestMethod]
        public void NextTo()
        {
            Assert.AreEqual(ext.Right(tileB), tileC);
        }

        [TestMethod]
        public void OpositeTo()
        {
            Assert.AreEqual(ext.Right(tileC), null);
        }

        [TestMethod]
        public void Above()
        {
            Assert.AreEqual(ext.Up(tileE), tileB);
        }
    }
}
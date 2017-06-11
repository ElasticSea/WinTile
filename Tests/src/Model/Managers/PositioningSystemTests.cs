using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using App;
using App.Model;
using App.Model.Managers;
using App.Model.Managers.Strategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Rect = App.Model.Rect;

namespace Tests.Model.Managers
{
    [TestClass]
    public class PositioningSystemTests
    {
        private static readonly Tile tileA = new Tile(new Rect(0, 0, 60, 100));
        private static readonly Tile tileB = new Tile(new Rect(60, 0, 84, 40));
        private static readonly Tile tileC = new Tile(new Rect(84, 0, 100, 40));
        private static readonly Tile tileD = new Tile(new Rect(60, 0, 100, 40));
        private static readonly Tile tileE = new Tile(new Rect(60, 40, 84, 100));
        private static readonly Tile tileF = new Tile(new Rect(84, 40, 100, 100));
        private static readonly Tile tileG = new Tile(new Rect(60, 40, 100, 100));
        private static readonly Tile tileH = new Tile(new Rect(60, 0, 84, 100));
        private static readonly Tile tileI = new Tile(new Rect(84, 0, 100, 100));

        private static readonly List<Tile> tiles = new List<Tile> { tileA , tileB , tileC , tileD , tileE , tileF , tileG , tileH, tileI };
        private readonly ClosestStrategy ext = new ClosestStrategy(tiles);

        [TestMethod]
        public void SetWindow()
        {
            var user32 = new WindowManagerDummy { MonitorRect = new Rect(0, 0, 3840, 2160) };
            var pos = new PositioningSystem(tiles, user32);
            pos.Selected = tileA;
            Assert.AreEqual(new Rect(0, 0, 2304, 2160), user32.CurrentWindowRect);
        }
        [TestMethod]
        public void GetWIndow()
        {
            var user32 = new WindowManagerDummy { MonitorRect = new Rect(0, 0, 3840, 2160) };
            var pos = new PositioningSystem(tiles, user32);

            user32.CurrentWindowRect = new Rect(100,100,2000,2000);
            Assert.AreEqual(tileA, pos.Selected);
        }
    }
}
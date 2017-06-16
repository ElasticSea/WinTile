using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using App;
using App.Model;
using App.Model.Managers.Strategies;
using App.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rect = App.Model.Rect;

namespace Tests.Model.Managers
{
    [TestClass]
    public class ClosestStrategyTests
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
        private static readonly SelectedHolder holder = new SelectedHolder();
        private readonly AbstractClosestStrategy ext = new AbstractClosestStrategy(holder, tiles, new WindowManagerDummy());

        [TestMethod]
        public void NextTo()
        {
            holder.Selected = tileB;
            ext.Right();
            Assert.AreEqual(holder.Selected, tileC);
        }

        [TestMethod]
        public void OpositeTo()
        {
            holder.Selected = tileC;
            ext.Right();
            Assert.AreEqual(holder.Selected, null);
        }

        [TestMethod]
        public void Above()
        {
            holder.Selected = tileE;
            ext.Up();
            Assert.AreEqual(holder.Selected, tileB);
        }
    }
}
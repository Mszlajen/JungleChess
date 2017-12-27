using System;
using NUnit.Framework;
using JungleChess;

namespace JungleChessTests
{
    [TestFixture]
    public class Tests
    {
        [OneTimeSetUp]
        public void InitialSetup()
        {
            
        }
        [OneTimeTearDown]
        public void FinalTearDown(){}
        [SetUp]
        public void TestSetup(){}
        [TearDown]
        public void TestTearDown(){}

        [Test]
        public void Co3_1IsWater ()
        {
            Coordinate zeroFour = new Coordinate(3, 1);

            Assert.IsTrue(Table.IsWater(zeroFour));
        }
    }
}

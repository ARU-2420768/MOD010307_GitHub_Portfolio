
using UnityEngine.TestTools;
using NUnit.Framework;
using MicrogameCourse.ProgressionFramework;
using System.Collections;

public class MedalTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void Gold_Should_Be_Greater_Than_All_Other_Medals()
    {
        Assert.Greater((int)MedalType.Gold, (int)MedalType.Silver);
        Assert.Greater((int)MedalType.Gold, (int)MedalType.Bronze);
        Assert.Greater((int)MedalType.Gold, (int)MedalType.Unranked);
    }

    [Test]
    public void Silver_Should_Be_Greater_Than_Bronze_And_Unranked()
    {
        Assert.Greater((int)MedalType.Silver, (int)MedalType.Bronze);
        Assert.Greater((int)MedalType.Silver, (int)MedalType.Unranked);
    }

    [Test]
    public void Bronze_Should_Be_Greater_Than_Unranked()
    {
        Assert.Greater((int)MedalType.Bronze, (int)MedalType.Unranked);
    }

    [Test]
    public void Gold_Should_Be_Greater_Than_Silver()
    {
        Assert.Greater(
            (int)MedalType.Gold,
            (int)MedalType.Silver);
    }

    [Test]
    public void Gold_Should_Be_Greater_Than_Bronze()
    {
        Assert.Greater(
            (int)MedalType.Gold,
            (int)MedalType.Bronze);
    }

    [Test]
    public void Gold_Should_Be_Greater_Than_Unranked()
    {
        Assert.Greater(
            (int)MedalType.Gold,
            (int)MedalType.Unranked);
    }

    [Test]
    public void Silver_Should_Be_Greater_Than_Bronze()
    {
        Assert.Greater(
            (int)MedalType.Silver,
            (int)MedalType.Bronze);
    }

    [Test]
    public void Silver_Should_Be_Greater_Than_Unranked()
    {
        Assert.Greater(
            (int)MedalType.Silver,
            (int)MedalType.Unranked);
    }

    [Test]
    public void Gold_Should_Equal_Gold()
    {
        Assert.AreEqual(
            MedalType.Gold,
            MedalType.Gold);
    }

    [Test]
    public void Silver_Should_Equal_Silver()
    {
        Assert.AreEqual(
            MedalType.Silver,
            MedalType.Silver);
    }

    [Test]
    public void Bronze_Should_Equal_Bronze()
    {
        Assert.AreEqual(
            MedalType.Bronze,
            MedalType.Bronze);
    }

    [Test]
    public void Unranked_Should_Equal_Unranked()
    {
        Assert.AreEqual(
            MedalType.Unranked,
            MedalType.Unranked);
    }

    [Test]
    public void Gold_Should_Not_Equal_Silver()
    {
        Assert.AreNotEqual(
            MedalType.Gold,
            MedalType.Silver);
    }

    [Test]
    public void Gold_Should_Not_Equal_Bronze()
    {
        Assert.AreNotEqual(
            MedalType.Gold,
            MedalType.Bronze);
    }

    [Test]
    public void Silver_Should_Not_Equal_Bronze()
    {
        Assert.AreNotEqual(
            MedalType.Silver,
            MedalType.Bronze);
    }

    [Test]
    public void Medal_Count_Should_Be_Four()
    {
        Assert.AreEqual(
            4,
            System.Enum.GetValues(typeof(MedalType)).Length);
    }

    [Test]
    public void Medal_Enum_Contains_Gold()
    {
        CollectionAssert.Contains(
            System.Enum.GetValues(typeof(MedalType)),
            MedalType.Gold);
    }

    [Test]
    public void Medal_Enum_Contains_Silver()
    {
        CollectionAssert.Contains(
            System.Enum.GetValues(typeof(MedalType)),
            MedalType.Silver);
    }

    [Test]
    public void Medal_Enum_Contains_Bronze()
    {
        CollectionAssert.Contains(
            System.Enum.GetValues(typeof(MedalType)),
            MedalType.Bronze);
    }

    [Test]
    public void Medal_Enum_Contains_Unranked()
    {
        CollectionAssert.Contains(
            System.Enum.GetValues(typeof(MedalType)),
            MedalType.Unranked);
    }
}


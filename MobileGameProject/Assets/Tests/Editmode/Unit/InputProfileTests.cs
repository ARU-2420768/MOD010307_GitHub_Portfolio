using NUnit.Framework;

public class InputProfileTests
{
    [Test]
    public void None_Flag_Can_Be_Detected()
    {
        InputMode inputs = InputMode.None;

        Assert.IsTrue(
            inputs.HasFlag(InputMode.None));
    }

    [Test]
    public void Click_Flag_Can_Be_Detected()
    {
        InputMode inputs = InputMode.Click;

        Assert.IsTrue(
            inputs.HasFlag(InputMode.Click));
    }

    [Test]
    public void DoubleClick_Flag_Can_Be_Detected()
    {
        InputMode inputs = InputMode.DoubleClick;

        Assert.IsTrue(
            inputs.HasFlag(InputMode.DoubleClick));
    }

    [Test]
    public void Drag_Flag_Can_Be_Detected()
    {
        InputMode inputs = InputMode.Drag;

        Assert.IsTrue(
            inputs.HasFlag(InputMode.Drag));
    }

    [Test]
    public void DragRelease_Flag_Can_Be_Detected()
    {
        InputMode inputs = InputMode.DragRelease;

        Assert.IsTrue(
            inputs.HasFlag(InputMode.DragRelease));
    }

    [Test]
    public void TwoFingerDrag_Flag_Can_Be_Detected()
    {
        InputMode inputs = InputMode.TwoFingerDrag;

        Assert.IsTrue(
            inputs.HasFlag(InputMode.TwoFingerDrag));
    }

    [Test]
    public void TwoFingerTap_Flag_Can_Be_Detected()
    {
        InputMode inputs = InputMode.TwoFingerTap;

        Assert.IsTrue(
            inputs.HasFlag(InputMode.TwoFingerTap));
    }

    [Test]
    public void Click_And_Drag_Can_Be_Combined()
    {
        InputMode inputs =
            InputMode.Click |
            InputMode.Drag;

        Assert.IsTrue(inputs.HasFlag(InputMode.Click));
        Assert.IsTrue(inputs.HasFlag(InputMode.Drag));
    }

    [Test]
    public void Click_Does_Not_Contain_Drag()
    {
        InputMode inputs = InputMode.Click;

        Assert.IsFalse(
            inputs.HasFlag(InputMode.Drag));
    }

    [Test]
    public void Drag_Does_Not_Contain_Click()
    {
        InputMode inputs = InputMode.Drag;

        Assert.IsFalse(
            inputs.HasFlag(InputMode.Click));
    }

    [Test]
    public void DoubleClick_Does_Not_Contain_Click()
    {
        InputMode inputs = InputMode.DoubleClick;

        Assert.IsFalse(
            inputs.HasFlag(InputMode.Click));
    }

    [Test]
    public void None_Does_Not_Contain_Click()
    {
        InputMode inputs = InputMode.None;

        Assert.IsFalse(
            inputs.HasFlag(InputMode.Click));
    }

    [Test]
    public void None_Does_Not_Contain_Drag()
    {
        InputMode inputs = InputMode.None;

        Assert.IsFalse(
            inputs.HasFlag(InputMode.Drag));
    }

    [Test]
    public void All_Flags_Can_Be_Combined()
    {
        InputMode inputs =
            InputMode.Click |
            InputMode.DoubleClick |
            InputMode.Drag |
            InputMode.DragRelease |
            InputMode.TwoFingerDrag |
            InputMode.TwoFingerTap;

        Assert.IsTrue(inputs.HasFlag(InputMode.Click));
        Assert.IsTrue(inputs.HasFlag(InputMode.DoubleClick));
        Assert.IsTrue(inputs.HasFlag(InputMode.Drag));
        Assert.IsTrue(inputs.HasFlag(InputMode.DragRelease));
        Assert.IsTrue(inputs.HasFlag(InputMode.TwoFingerDrag));
        Assert.IsTrue(inputs.HasFlag(InputMode.TwoFingerTap));
    }

    [Test]
    public void Slalom_Profile_Supports_Drag_Only()
    {
        InputMode inputs =
            InputMode.Drag;

        Assert.IsTrue(inputs.HasFlag(InputMode.Drag));

        Assert.IsFalse(inputs.HasFlag(InputMode.Click));
        Assert.IsFalse(inputs.HasFlag(InputMode.DoubleClick));
        Assert.IsFalse(inputs.HasFlag(InputMode.DragRelease));
        Assert.IsFalse(inputs.HasFlag(InputMode.TwoFingerDrag));
        Assert.IsFalse(inputs.HasFlag(InputMode.TwoFingerTap));
    }

    [Test]
    public void Shooting_Profile_Supports_Click_And_Drag()
    {
        InputMode inputs =
            InputMode.Click |
            InputMode.Drag;

        Assert.IsTrue(inputs.HasFlag(InputMode.Click));
        Assert.IsTrue(inputs.HasFlag(InputMode.Drag));

        Assert.IsFalse(inputs.HasFlag(InputMode.DoubleClick));
        Assert.IsFalse(inputs.HasFlag(InputMode.DragRelease));
        Assert.IsFalse(inputs.HasFlag(InputMode.TwoFingerDrag));
        Assert.IsFalse(inputs.HasFlag(InputMode.TwoFingerTap));
    }

    [Test]
    public void Curling_Profile_Supports_Drag_And_DragRelease()
    {
        InputMode inputs =
            InputMode.Drag |
            InputMode.DragRelease;

        Assert.IsTrue(inputs.HasFlag(InputMode.Drag));
        Assert.IsTrue(inputs.HasFlag(InputMode.DragRelease));

        Assert.IsFalse(inputs.HasFlag(InputMode.Click));
        Assert.IsFalse(inputs.HasFlag(InputMode.DoubleClick));
        Assert.IsFalse(inputs.HasFlag(InputMode.TwoFingerDrag));
        Assert.IsFalse(inputs.HasFlag(InputMode.TwoFingerTap));
    }

    [Test]
    public void Mobile_Profile_Supports_TwoFinger_Inputs()
    {
        InputMode inputs =
            InputMode.TwoFingerDrag |
            InputMode.TwoFingerTap;

        Assert.IsTrue(inputs.HasFlag(InputMode.TwoFingerDrag));
        Assert.IsTrue(inputs.HasFlag(InputMode.TwoFingerTap));

        Assert.IsFalse(inputs.HasFlag(InputMode.Click));
        Assert.IsFalse(inputs.HasFlag(InputMode.Drag));
    }

    [Test]
    public void Click_And_DragRelease_Can_Be_Combined()
    {
        InputMode inputs =
            InputMode.Click |
            InputMode.DragRelease;

        Assert.IsTrue(inputs.HasFlag(InputMode.Click));
        Assert.IsTrue(inputs.HasFlag(InputMode.DragRelease));
    }

    [Test]
    public void Combined_Flags_Are_Not_Equal_To_A_Single_Flag()
    {
        InputMode inputs =
            InputMode.Click |
            InputMode.Drag;

        Assert.AreNotEqual(
            InputMode.Click,
            inputs);
    }
}

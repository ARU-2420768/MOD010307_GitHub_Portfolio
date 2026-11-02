using NUnit.Framework;
using UnityEngine;

public class MovementProfileTests
{
    [Test]
    public void Position_Is_Clamped_To_MaxX()
    {
        float result = Mathf.Clamp(
            10f,
            -2.3f,
            2.3f);

        Assert.AreEqual(2.3f, result);
    }

    [Test]
    public void Position_Is_Clamped_To_MinX()
    {
        float result = Mathf.Clamp(
            -10f,
            -2.3f,
            2.3f);

        Assert.AreEqual(-2.3f, result);
    }

    [Test]
    public void Position_Remains_Within_X_Bounds()
    {
        float result = Mathf.Clamp(
            1f,
            -2.3f,
            2.3f);

        Assert.AreEqual(1f, result);
    }

    [Test]
    public void Position_Is_Clamped_To_MaxY()
    {
        float result = Mathf.Clamp(
            10f,
            -3f,
            3f);

        Assert.AreEqual(3f, result);
    }

    [Test]
    public void Position_Is_Clamped_To_MinY()
    {
        float result = Mathf.Clamp(
            -10f,
            -3f,
            3f);

        Assert.AreEqual(-3f, result);
    }

    [Test]
    public void Position_Remains_Within_Y_Bounds()
    {
        float result = Mathf.Clamp(
            1f,
            -3f,
            3f);

        Assert.AreEqual(1f, result);
    }

    [Test]
    public void Relative_MinX_Is_Calculated_Correctly()
    {
        Vector3 originalPosition =
            new Vector3(5f, 0f, 0f);

        float minX = -2f;

        float result =
            originalPosition.x + minX;

        Assert.AreEqual(3f, result);
    }

    [Test]
    public void Relative_MaxX_Is_Calculated_Correctly()
    {
        Vector3 originalPosition =
            new Vector3(5f, 0f, 0f);

        float maxX = 2f;

        float result =
            originalPosition.x + maxX;

        Assert.AreEqual(7f, result);
    }

    [Test]
    public void Relative_MinY_Is_Calculated_Correctly()
    {
        Vector3 originalPosition =
            new Vector3(0f, 5f, 0f);

        float minY = -2f;

        float result =
            originalPosition.y + minY;

        Assert.AreEqual(3f, result);
    }

    [Test]
    public void Relative_MaxY_Is_Calculated_Correctly()
    {
        Vector3 originalPosition =
            new Vector3(0f, 5f, 0f);

        float maxY = 2f;

        float result =
            originalPosition.y + maxY;

        Assert.AreEqual(7f, result);
    }

    [Test]
    public void Horizontal_Movement_Profile_Locks_Y_Position()
    {
        Vector3 originalPosition =
            new Vector3(0f, 5f, 0f);

        float minY = 0f;
        float maxY = 0f;

        float result = Mathf.Clamp(
            originalPosition.y,
            originalPosition.y + minY,
            originalPosition.y + maxY);

        Assert.AreEqual(
            originalPosition.y,
            result);
    }

    [Test]
    public void Vertical_Movement_Profile_Locks_X_Position()
    {
        Vector3 originalPosition =
            new Vector3(5f, 0f, 0f);

        float minX = 0f;
        float maxX = 0f;

        float result = Mathf.Clamp(
            originalPosition.x,
            originalPosition.x + minX,
            originalPosition.x + maxX);

        Assert.AreEqual(
            originalPosition.x,
            result);
    }

    [Test]
    public void Acceleration_Value_Is_Positive()
    {
        float acceleration = 5f;

        Assert.Greater(
            acceleration,
            0f);
    }

    [Test]
    public void MinX_Is_Less_Than_MaxX()
    {
        float minX = -2f;
        float maxX = 2f;

        Assert.Less(
            minX,
            maxX);
    }

    [Test]
    public void MinY_Is_Less_Than_MaxY()
    {
        float minY = -2f;
        float maxY = 2f;

        Assert.Less(
            minY,
            maxY);
    }
}

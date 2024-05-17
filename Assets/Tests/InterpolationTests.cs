using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InterpolationTests
{
    /*
    [Test]
    public void BasicInterpolationTests()
    {
        HermiteInterpolation linear = HermiteInterpolation.CreateLinear(0, 1, 0, 1);
        for (float i = 0; i < 10; i++)
            AssertAlmostEqual(linear.ValueAt(i / 10), i / 10);

        HermiteInterpolation linear2 = HermiteInterpolation.CreateLinear(-10, 10, 10, 20);
        Assert.AreEqual(linear2.ValueAt(5), 17.5);
        Assert.AreEqual(linear2.ValueAt(6), 18);


        HermiteInterpolation easeInEaseOut = HermiteInterpolation.CreateEaseInEaseOut(4, 8, 10, 20);
        Assert.True(easeInEaseOut.ValueAt(5) < 12.5f);
        Assert.True(easeInEaseOut.ValueAt(7) > 17.5f);
        Assert.AreEqual(easeInEaseOut.ValueAt(6), 15f);
    }


    [Test]
    public void SplineInterpolationTests()
    {
        HermiteSplineInterpolation i = new HermiteSplineInterpolation();
        i.AddKeyFrame(0, 0).AddKeyFrame(10, 100).AddKeyFrame(4, 80);


        Assert.AreEqual(-1, i.GetInterpolationAt(-1));
        Assert.AreEqual(0, i.GetInterpolationAt(0));
        Assert.AreEqual(0, i.GetInterpolationAt(2));
        Assert.AreEqual(0, i.GetInterpolationAt(3.99f));
        Assert.AreEqual(1, i.GetInterpolationAt(4.01f));
        Assert.AreEqual(1, i.GetInterpolationAt(6));
        Assert.AreEqual(2, i.GetInterpolationAt(10));
        Assert.AreEqual(2, i.GetInterpolationAt(12));

        Assert.AreEqual(0, i.ValueAt(-10));
        Assert.AreEqual(0, i.ValueAt(0));
        Assert.AreEqual(100, i.ValueAt(10));
        Assert.AreEqual(100, i.ValueAt(99));
        Assert.AreEqual(80, i.ValueAt(4));
        Assert.AreEqual(40, i.ValueAt(2));
        Assert.AreEqual(90, i.ValueAt(7));

        i.AddKeyFrame(-10, 1000).AddKeyFrame(100, -1000);
        Assert.AreEqual(1000, i.ValueAt(-100));
        Assert.AreEqual(1000, i.ValueAt(-10));
        Assert.AreEqual(-1000, i.ValueAt(1000));
        Assert.AreEqual(-1000, i.ValueAt(100));
    }

    private void AssertAlmostEqual(float a, float b, float threshold = 1e-6f)
    {
        Assert.True((a - b) < threshold);
    }


    [UnityTest]
    public IEnumerator InterpolationTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}

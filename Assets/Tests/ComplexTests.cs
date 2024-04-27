using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ComplexTests
{
    [Test]
    public void UnitTestingSimplePasses()
    {
        Complex test = Activator.CreateInstance(Complex.ZERO.GetType()) as Complex;
        Assert.True(test.Equals(Complex.ZERO));
        Complex a = new Complex(-1, 3);
        Assert.True(a.Equals(new Complex(-1, 3)));

        Complex b = new Complex(2, -4);
        Assert.AreNotEqual(a, b);
        Assert.AreEqual(a + b, new Complex(1, -1));
        Assert.AreEqual(a - b, new Complex(-3, 7));
        Assert.AreEqual(a * b, new Complex(10, 10));
        Assert.AreEqual(a / b, new Complex(-7f/10, 1f/10));
        Assert.AreEqual(a ^ 3, new Complex(26, -18));

        Assert.AreEqual((a ^ 2).abs(), 10);
        assertAlmostEqual(Complex.Polar(3, Mathf.PI), new Complex(-3, 0));
        assertAlmostEqual(Complex.I ^ Complex.I, new Complex(Mathf.Exp(-Mathf.PI/2), 0));

        object x = 54f;
        object y = Complex.ZERO;

        Assert.IsTrue(x is float);
        Assert.IsTrue(y is Complex);
    }

    private void assertAlmostEqual(float a, float b, float threshold = 1e-6f)
    {
        Assert.True((a - b) < threshold);
    }
    private void assertAlmostEqual(Complex a, Complex b, float threshold = 1e-6f)
    {
        Assert.True((a - b).abs() < threshold);
    }











    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator UnitTestingWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

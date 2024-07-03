using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ParameterHandlerTests
{
    [Test]
    public void UnitTestingSimplePasses()
    {

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

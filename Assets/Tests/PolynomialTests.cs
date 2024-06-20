using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static Complex;

public class PolynomialTests
{
    [Test]
    public void UnitTestingSimplePasses()
    {
        
        // (1+i)x^3 + 4x + i
        ClassicPolynomial a = new ClassicPolynomial(I , ONE*4 , ZERO, ONE + I);
        // 3x^2 + 2x + 1
        ClassicPolynomial b = new ClassicPolynomial(1, 2, 3);

        Assert.AreEqual(a, new ClassicPolynomial(I, ONE * 4, ZERO, ONE + I));
        Assert.AreEqual(a.degree(), 3);
        Assert.AreEqual(b << 2, new ClassicPolynomial(0, 0, 1, 2, 3));
        Assert.AreEqual(b * new Complex(1, 1), new ClassicPolynomial(new Complex(1, 1), new Complex(2, 2), new Complex(3, 3)));
        Assert.AreEqual(a.derivative(), new ClassicPolynomial(ONE * 4, ZERO, new Complex(3, 3)));
        Assert.AreEqual(a.evaluate(ONE), new Complex(5, 2));
        Assert.AreEqual(a.evaluate(-ONE), new Complex(-5, 0));
        Assert.AreEqual(a.evaluate(I), new Complex(1, 4));

        Complex c = new Complex(12, -4);
        Assert.AreEqual(a.addRoot(c).evaluate(c), ZERO);

        Complex[] roots = new Complex[4] { ONE, I, -ONE, -I };
        ClassicPolynomial p = ClassicPolynomial.fromRoots(roots);
        Assert.AreEqual(p, new ClassicPolynomial(-1, 0, 0, 0, 1));     //x^4 - 1
        foreach(Complex root in roots)
            Assert.AreEqual(p.evaluate(root), ZERO);

        Assert.AreEqual(p.derivative().evaluate(I), -4*I);
        Complex x = new Complex(3, 6);
        Assert.AreEqual(rootsEvaluate(x, roots), p.evaluate(x));

        ClassicPolynomial sum = new ClassicPolynomial(new Complex(1, 1), new Complex(6, 0), new Complex(3, 0), new Complex(1, 1));
        ClassicPolynomial sub = new ClassicPolynomial(new Complex(-1, 1), new Complex(2, 0), new Complex(-3, 0), new Complex(1, 1));
        ClassicPolynomial prod = new ClassicPolynomial(new Complex(0, 1), new Complex(4, 2), new Complex(8, 3), new Complex(13, 1), new Complex(2, 2), new Complex(3, 3));

        Assert.AreEqual(sum, a + b);
        Assert.AreEqual(sub, a - b);
        Assert.AreEqual(prod, a * b);
    }

    private Complex rootsEvaluate(Complex x, Complex[] roots)
    {
        Complex answer = ONE;
        foreach (Complex root in roots)
            answer *= x - root;
        return answer;
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

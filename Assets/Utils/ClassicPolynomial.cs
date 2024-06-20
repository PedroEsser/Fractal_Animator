using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicPolynomial
{
    public Complex[] Coefficients { get; }

    public ClassicPolynomial(params Complex[] coefficients)
    {
        this.Coefficients = coefficients;
    }

    public ClassicPolynomial(params float[] coefficients)
    {
        Complex[] complexCoefficients = new Complex[coefficients.Length];
        for (int i = 0; i < coefficients.Length; i++)
            complexCoefficients[i] = new Complex(coefficients[i], 0);
        this.Coefficients = complexCoefficients;
    }

    public ClassicPolynomial(int degree)
    {
        this.Coefficients = new Complex[degree + 1];
        for (int i = 0; i < Coefficients.Length; i++)
            Coefficients[i] = Complex.ZERO;
    }

    public int degree() { return Coefficients.Length - 1; }

    public ClassicPolynomial addPolynomial(ClassicPolynomial p)
    {
        ClassicPolynomial higherDegree = this;
        if (this.degree() < p.degree())
        {
            higherDegree = p;
            p = this;
        }
        ClassicPolynomial sum = higherDegree.clone();
        for (int i = 0; i < p.Coefficients.Length; i++)
            sum.Coefficients[i] += p.Coefficients[i];

        return sum;
    }

    public ClassicPolynomial minus()
    {
        ClassicPolynomial result = this.clone();
        for (int i = 0; i < result.Coefficients.Length; i++)
            result.Coefficients[i] *= -1;

        return result;
    }

    public ClassicPolynomial timesComplex(Complex c)
    {
        ClassicPolynomial result = this.clone();
        for (int i = 0; i < result.Coefficients.Length; i++)
            result.Coefficients[i] *= c;

        return result;
    }

    public ClassicPolynomial timesPowerX(int power = 1)
    {
        ClassicPolynomial result = new ClassicPolynomial(this.degree() + power);
        for (int i = power; i < result.Coefficients.Length; i++)
            result.Coefficients[i] = this.Coefficients[i - power];
        return result;
    }

    public ClassicPolynomial timesPolynomial(ClassicPolynomial p)
    {
        ClassicPolynomial result = new ClassicPolynomial(this.degree() + p.degree());
        for (int i = 0; i < this.Coefficients.Length; i++)
            result += (p << i) * this.Coefficients[i];

        return result;
    }

    public Complex coefficientAt(int powerX) { return Coefficients[powerX]; }

    public Complex evaluate(Complex x)
    {
        Complex result = Complex.ZERO;
        Complex acc = Complex.ONE;

        for (int i = 0; i < Coefficients.Length; i++)
        {
            result += Coefficients[i] * acc;
            acc *= x;
        }

        return result;
    }

    public ClassicPolynomial derivative()
    {
        if (this.degree() == 0)
            return ClassicPolynomial.ZERO;
        ClassicPolynomial derivative = new ClassicPolynomial(this.degree() - 1);
        for (int i = 0; i < derivative.Coefficients.Length; i++)
            derivative.Coefficients[i] = this.Coefficients[i + 1] * (i + 1);
        return derivative;
    }

    public ClassicPolynomial addRoot(Complex root) { return (this << 1) - this * root; }

    public ClassicPolynomial clone()
    {
        Complex[] clonedCoefficients = new Complex[Coefficients.Length];
        for (int i = 0; i < Coefficients.Length; i++)
            clonedCoefficients[i] = Coefficients[i].clone();

        return new ClassicPolynomial(clonedCoefficients);
    }
    public static ClassicPolynomial fromRoots(IEnumerable<Complex> roots)
    {
        ClassicPolynomial p = ClassicPolynomial.ONE;
        foreach (Complex c in roots)
            p = p.addRoot(c);

        return p;
    }

    public static ClassicPolynomial operator +(ClassicPolynomial a, ClassicPolynomial b) { return a.addPolynomial(b); }

    public static ClassicPolynomial operator -(ClassicPolynomial a) { return a.minus(); }
    public static ClassicPolynomial operator -(ClassicPolynomial a, ClassicPolynomial b) { return a + -b; }
    public static ClassicPolynomial operator -(ClassicPolynomial a, Complex c) { return a - new ClassicPolynomial(c); }

    public static ClassicPolynomial operator *(ClassicPolynomial a, Complex c) { return a.timesComplex(c); }
    public static ClassicPolynomial operator *(Complex c, ClassicPolynomial a) { return a * c; }
    public static ClassicPolynomial operator *(ClassicPolynomial a, ClassicPolynomial b) { return a.timesPolynomial(b); }

    public static ClassicPolynomial operator <<(ClassicPolynomial a, int powerX) { return a.timesPowerX(powerX); }

    public static ClassicPolynomial ZERO = new ClassicPolynomial(new Complex[1] { Complex.ZERO });
    public static ClassicPolynomial ONE = new ClassicPolynomial(new Complex[1] { Complex.ONE });

    public static ClassicPolynomial rootsOfUnity(int rootsCount) { return (ClassicPolynomial.ONE << rootsCount) - Complex.ONE; }

    public override string ToString()
    {
        string s = "";
        for(int i = Coefficients.Length-1; i > 0; i--)
        {
            s += "(" + Coefficients[i] + ")*x^" + i + " + ";
        }
        return s + Coefficients[0];
    }

    public override bool Equals(object obj)
    {
        if (!(obj is ClassicPolynomial p))
            return false;
        if (this.degree() != p.degree())
            return false;

        for (int i = 0; i < Coefficients.Length; i++)
            if (!Coefficients[i].Equals(p.Coefficients[i]))
                return false;

        return true;
    }

}

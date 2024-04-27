using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polynomial
{
    public Complex[] coefficients { get; }

    public Polynomial(params Complex[] coefficients)
    {
        this.coefficients = coefficients;
    }

    public Polynomial(params float[] coefficients)
    {
        Complex[] complexCoefficients = new Complex[coefficients.Length];
        for (int i = 0; i < coefficients.Length; i++)
            complexCoefficients[i] = new Complex(coefficients[i], 0);
        this.coefficients = complexCoefficients;
    }

    public Polynomial(int degree)
    {
        this.coefficients = new Complex[degree + 1];
        for (int i = 0; i < coefficients.Length; i++)
            coefficients[i] = Complex.ZERO;
    }

    public int degree() { return coefficients.Length - 1; }

    public Polynomial addPolynomial(Polynomial p)
    {
        Polynomial higherDegree = this;
        if (this.degree() < p.degree())
        {
            higherDegree = p;
            p = this;
        }
        Polynomial sum = higherDegree.clone();
        for (int i = 0; i < p.coefficients.Length; i++)
            sum.coefficients[i] += p.coefficients[i];

        return sum;
    }

    public Polynomial minus()
    {
        Polynomial result = this.clone();
        for (int i = 0; i < result.coefficients.Length; i++)
            result.coefficients[i] *= -1;

        return result;
    }

    public Polynomial timesComplex(Complex c)
    {
        Polynomial result = this.clone();
        for (int i = 0; i < result.coefficients.Length; i++)
            result.coefficients[i] *= c;

        return result;
    }

    public Polynomial timesPowerX(int power = 1)
    {
        Polynomial result = new Polynomial(this.degree() + power);
        for (int i = power; i < result.coefficients.Length; i++)
            result.coefficients[i] = this.coefficients[i - power];
        return result;
    }

    public Polynomial timesPolynomial(Polynomial p)
    {
        Polynomial result = new Polynomial(this.degree() + p.degree());
        for (int i = 0; i < this.coefficients.Length; i++)
            result += (p << i) * this.coefficients[i];

        return result;
    }

    public Complex coefficientAt(int powerX) { return coefficients[powerX]; }

    public Complex evaluate(Complex x)
    {
        Complex result = Complex.ZERO;
        Complex acc = Complex.ONE;

        for (int i = 0; i < coefficients.Length; i++)
        {
            result += coefficients[i] * acc;
            acc *= x;
        }

        return result;
    }

    public Polynomial derivative()
    {
        if (this.degree() == 0)
            return Polynomial.ZERO;
        Polynomial derivative = new Polynomial(this.degree() - 1);
        for (int i = 0; i < derivative.coefficients.Length; i++)
            derivative.coefficients[i] = this.coefficients[i + 1] * (i + 1);
        return derivative;
    }

    public Polynomial addRoot(Complex root) { return (this << 1) - this * root; }

    public Polynomial clone()
    {
        Complex[] clonedCoefficients = new Complex[coefficients.Length];
        for (int i = 0; i < coefficients.Length; i++)
            clonedCoefficients[i] = coefficients[i].clone();

        return new Polynomial(clonedCoefficients);
    }
    public static Polynomial fromRoots(IEnumerable<Complex> roots)
    {
        Polynomial p = Polynomial.ONE;
        foreach (Complex c in roots)
            p = p.addRoot(c);

        return p;
    }

    public static Polynomial operator +(Polynomial a, Polynomial b) { return a.addPolynomial(b); }

    public static Polynomial operator -(Polynomial a) { return a.minus(); }
    public static Polynomial operator -(Polynomial a, Polynomial b) { return a + -b; }
    public static Polynomial operator -(Polynomial a, Complex c) { return a - new Polynomial(c); }

    public static Polynomial operator *(Polynomial a, Complex c) { return a.timesComplex(c); }
    public static Polynomial operator *(Complex c, Polynomial a) { return a * c; }
    public static Polynomial operator *(Polynomial a, Polynomial b) { return a.timesPolynomial(b); }

    public static Polynomial operator <<(Polynomial a, int powerX) { return a.timesPowerX(powerX); }

    public static Polynomial ZERO = new Polynomial(new Complex[1] { Complex.ZERO });
    public static Polynomial ONE = new Polynomial(new Complex[1] { Complex.ONE });

    public static Polynomial rootsOfUnity(int rootsCount) { return (Polynomial.ONE << rootsCount) - Complex.ONE; }

    public override string ToString()
    {
        string s = "";
        for(int i = coefficients.Length-1; i > 0; i--)
        {
            s += "(" + coefficients[i] + ")*x^" + i + " + ";
        }
        return s + coefficients[0];
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Polynomial p))
            return false;
        if (this.degree() != p.degree())
            return false;

        for (int i = 0; i < coefficients.Length; i++)
            if (!coefficients[i].Equals(p.coefficients[i]))
                return false;

        return true;
    }

}

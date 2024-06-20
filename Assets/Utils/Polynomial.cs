using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polynomial : MonoBehaviour
{

    public static readonly int MAX_TERMS = 16;
    public Term[] Terms;
    private int TermCount = 0;

    public Polynomial()
    {
        Terms = new Term[MAX_TERMS];
    }

    public void AddTerm(Complex coeficcient, Complex power) { Terms[TermCount++] = new Term(coeficcient, power); }

    public Complex Evaluate(Complex z)
    {
        Complex answer = new Complex();
        for (int i = 0; i < TermCount; i++)
            answer += Terms[i].evaluate(z);
        return answer;
    }

    public class Term
    {
        public Complex Coeficcient;
        public Complex Power;

        public Term(Complex coeficcient, Complex power)
        {
            Coeficcient = coeficcient;
            Power = power;
        }

        public Complex evaluate(Complex z) { return Coeficcient * (z ^ Power); }

        public static implicit operator Vector4(Term t) { return new Vector4(t.Coeficcient.re, t.Coeficcient.im, t.Power.re, t.Power.im); }

    }

}

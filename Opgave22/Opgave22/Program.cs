using System;
using System.Collections.Generic;
using System.Linq;

namespace Opgave22
{

    internal class Program
    {
        private static void Main(string[] args)
        {
            var fiveLetterRomans = new List<RomanNumber>();
            
            var primes = new List<RomanNumber>();
            var oddAndFirstTwoAreEqual = new List<RomanNumber>();

            for (int i = 1; i < 3601; i++)
            {
                var numberRomanPair = new RomanNumber { Number = i, Roman = ToRoman(i) };

                if (numberRomanPair.Roman.Length == 5)
                {
                    fiveLetterRomans.Add(numberRomanPair);

                    if (IsPrime(i))
                    {
                        primes.Add(numberRomanPair);
                    }

                    if(i % 2 == 1 && numberRomanPair.Roman[0] == numberRomanPair.Roman[1])
                    {
                        oddAndFirstTwoAreEqual.Add(numberRomanPair);
                    }
                }
            }

            var divisableByOtherNumber = new List<DividerPair>();
            var sumOfTwoPairs = new List<SumOfTwoPair>();

            divisableByOtherNumber = getDividerPairs(fiveLetterRomans);
            sumOfTwoPairs = getPairsWithEqualSum(fiveLetterRomans);

            Console.WriteLine("Primes");
            foreach (var num in primes)
            {
                Console.WriteLine(num.Number + "\t" + num.Roman);
            }
            Console.WriteLine(" ");
            Console.WriteLine("Odd and starts with two equals");
            foreach (var num in oddAndFirstTwoAreEqual)
            {
                Console.WriteLine(num.Number + "\t" + num.Roman);
            }
            Console.WriteLine(" ");
            Console.WriteLine("Divider pair");
            foreach (var pair in divisableByOtherNumber)
            {
                Console.WriteLine(pair.Numerator.Number + "\t" + pair.Numerator.Roman + "\t" +
                    pair.Denominator.Number + "\t" + pair.Denominator.Roman);
            }
            Console.WriteLine(" ");
            Console.WriteLine("Sum of two pairs");
            foreach (var equation in sumOfTwoPairs)
            {
                Console.WriteLine(equation.LargestLeft.Number + " + " + equation.SmallestLeft.Number + " = " +
                    equation.LargestRight.Number + " + " + equation.SmallestRight.Number);
                Console.WriteLine(equation.LargestLeft.Roman + " + " + equation.SmallestLeft.Roman + " = " +
                    equation.LargestRight.Roman + " + " + equation.SmallestRight.Roman);
            }

            var E1 = sumOfTwoPairs.Select(c => c.LargestLeft);
            var E = divisableByOtherNumber.Select(c => c.Denominator).Intersect(E1, new RomanNumberComparer());

            var sp = sumOfTwoPairs.Where(s => s.LargestLeft.Number == 390 || s.LargestLeft.Number == 225 || s.LargestLeft.Number == 360 || s.LargestLeft.Number == 230 || s.LargestLeft.Number == 270 || s.LargestLeft.Number == 320 || s.LargestLeft.Number == 850);

            var ra = sp.Select(s => s.SmallestLeft.Roman).ToArray();

            Console.ReadKey();
        }

        class RomanNumberComparer : IEqualityComparer<RomanNumber>
        {
            public bool Equals(RomanNumber x, RomanNumber y)
            {
                return x.Number == y.Number;
            }

            public int GetHashCode(RomanNumber obj)
            {
                return obj.GetHashCode();
            }
        }

        private static List<SumOfTwoPair> getPairsWithEqualSum(List<RomanNumber> fiveLetterRomans)
        {
            var sumOfTwoPairs = new List<SumOfTwoPair>();

            for (var i = 0; i < fiveLetterRomans.Count; i++)
            {
                for (var j = i+1; j < fiveLetterRomans.Count; j++)
                {
                    for (var k = j + 1; k < fiveLetterRomans.Count; k++)
                    {
                        for (var l = k + 1; l < fiveLetterRomans.Count; l++)
                        {
                            if (DoesEquationHold(fiveLetterRomans, i, j, k, l) &&
                                IsE0EqualToG1orH1(fiveLetterRomans, j, k, l) &&
                                DoNumbersFit(fiveLetterRomans, i, j, k, l) )
                            {
                                var sumOfTwoPair = new SumOfTwoPair();
                                sumOfTwoPair.SmallestLeft = fiveLetterRomans[i];
                                sumOfTwoPair.SmallestRight = fiveLetterRomans[j];
                                sumOfTwoPair.LargestLeft = fiveLetterRomans[l];
                                sumOfTwoPair.LargestRight = fiveLetterRomans[k];

                                sumOfTwoPairs.Add(sumOfTwoPair);
                            }
                        }
                    }
                }
            }

            return sumOfTwoPairs;
        }

        private static bool IsE0EqualToG1orH1(List<RomanNumber> fiveLetterRomans, int j, int k, int l)
        {
            var E = fiveLetterRomans[l].Roman;
            var GorH = fiveLetterRomans[j].Roman;
            var HorG = fiveLetterRomans[k].Roman;
            return (E[0] == GorH[1] || E[0] == HorG[1]);
        }

        private static bool DoNumbersFit(List<RomanNumber> fiveLetterRomans, int i, int j, int k, int l)
        {
            var E = fiveLetterRomans[l].Roman;
            var B = fiveLetterRomans[i].Roman;
            var GorH = fiveLetterRomans[j].Roman;
            var HorG = fiveLetterRomans[k].Roman;

            var result =  (E[1] == GorH[0] &&
                E[2] == HorG[0] &&
                B[1] == GorH[3] &&
                B[2] == HorG[3]) ||
                (E[1] == HorG[0] &&
                E[2] == GorH[0] &&
                B[1] == HorG[3] &&
                B[2] == GorH[3]);

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool DoesEquationHold(List<RomanNumber> fiveLetterRomans, int i, int j, int k, int l)
        {
            return fiveLetterRomans[i].Number + fiveLetterRomans[l].Number ==
                                            fiveLetterRomans[j].Number + fiveLetterRomans[k].Number;
        }

        private static List<DividerPair> getDividerPairs(List<RomanNumber> fiveLetterRomans)
        {
            var dividerPairs = new List<DividerPair>();
            foreach (var fiveLetterRoman in fiveLetterRomans)
            {
                var number = fiveLetterRoman.Number;
                foreach(var potentialDivisor in fiveLetterRomans)
                {
                    if(number % potentialDivisor.Number == 0 && fiveLetterRoman.Roman[4] == potentialDivisor.Roman[0])
                    {
                        var dividerPair = new DividerPair();
                        dividerPair.Denominator = potentialDivisor;
                        dividerPair.Numerator = fiveLetterRoman;

                        dividerPairs.Add(dividerPair);
                    }

                }
            }

            return dividerPairs;
        }

        public static bool IsPrime(int number)
        {
            if (number <= 1)
            {
                return false;
            }

            if (number == 2)
            {
                return true;
            }

            if (number % 2 == 0)
            {
                return false;
            }

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999))
            {
                throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            }

            if (number < 1)
            {
                return string.Empty;
            }

            if (number >= 1000)
            {
                return "M" + ToRoman(number - 1000);
            }

            if (number >= 900)
            {
                return "CM" + ToRoman(number - 900);
            }

            if (number >= 500)
            {
                return "D" + ToRoman(number - 500);
            }

            if (number >= 400)
            {
                return "CD" + ToRoman(number - 400);
            }

            if (number >= 100)
            {
                return "C" + ToRoman(number - 100);
            }

            if (number >= 90)
            {
                return "XC" + ToRoman(number - 90);
            }

            if (number >= 50)
            {
                return "L" + ToRoman(number - 50);
            }

            if (number >= 40)
            {
                return "XL" + ToRoman(number - 40);
            }

            if (number >= 10)
            {
                return "X" + ToRoman(number - 10);
            }

            if (number >= 9)
            {
                return "IX" + ToRoman(number - 9);
            }

            if (number >= 5)
            {
                return "V" + ToRoman(number - 5);
            }

            if (number >= 4)
            {
                return "IV" + ToRoman(number - 4);
            }

            if (number >= 1)
            {
                return "I" + ToRoman(number - 1);
            }

            throw new ArgumentOutOfRangeException("something bad happened");
        }
    }

    internal class RomanNumber
    {
        public int Number { get; set; }
        public string Roman { get; set; }
    }

    internal class DividerPair
    {
        public RomanNumber Numerator { get; set; }
        public RomanNumber Denominator { get; set; }
    }

    internal class SumOfTwoPair
    {
        public RomanNumber LargestLeft { get; set; }
        public RomanNumber LargestRight { get; set; }
        public RomanNumber SmallestLeft { get; set; }
        public RomanNumber SmallestRight { get; set; }
    }
}

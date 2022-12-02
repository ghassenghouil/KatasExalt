/****************************************************************************************************************************************************************************************************
 * Description of the solution :
 * 
 * Getting the Nth root of a real number is essentialy numercial equation resolving
 * The idea being starting from an estimated guess (or a couple of values for a bracket) we iterate and compute a deviation error till we reach a desired precision
 * These are some of the methods ordered from the slowest converging to the fastest :
 *   - Bisection (Binary Search) : uses a bracketing technique of the root value and has a linear convergence (Order = 1)
 *   - False Position : uses also a bracketing technique but enhances the search interval splitting, 
 *     usually faster then the bisection method but can be as slow and still has an order of 1
 *   - Secant method : similar to False Position but uses a succession of secant lines and can be thought of as a slower version to the Newton Raphson and has an order of approximately 1.6
 *     Can also be useful when we cannot compute a derivative function for the Newton Raphson method.
 *   - Newton Raphson : uses a derivative of the function in order to compute the deviation, invented by Newton but applied initially to polynomials equations only.
 *     It's the fastest one but does not guarantee coonvergence. It has an order of 2
 * 
 *  In a more realistic scenario and depending on the requirements, peoples may usually use a combination of these methods.
 *  For example start with a linear one and once it starts converging switch to the faster Newton method for a determenistic result.
 *  Or speculatively start with the Newton method and handle the non-convergence case linearly or fallback to the secant method
 * 
 * In this file i'll implement the Bisection and the Newton Raphson methods to resolve the Nth root of a real number
 * The equation for root solving is : f(x)  = x^(N) – A
 * I'll use a default precision of .001
 *
 * Some of the resources i used :
 *  - https://youtube.com/playlist?list=PLkZjai-2Jcxn35XnijUtqqEg0Wi5Sn8ab
 *  - https://www.geeksforgeeks.org/ 
 * 
 * ***************************************************************************************************************************************************************************************************/


namespace NthRoot;

internal class Program
{
    private const double PRECISION = 0.001;

    static void Main(string[] args)
    {
        //Examples: 
        Console.WriteLine($"2nd root of 2 using binary search is {GetNthRootBinary(2, 2)}");
        Console.WriteLine($"2nd root of 2 using newton method is {GetNthRootNewton(2, 2)}");

        Console.WriteLine($"3rd root of 9 using binary search is {GetNthRootBinary(9, 3)}");
        Console.WriteLine($"3rd root of 9 using newton method is {GetNthRootNewton(9, 3)}");

        Console.WriteLine("Press any key to exit!");
        Console.ReadKey();
    }

    //1st solution : Binary search
    public static double GetNthRootBinary(double number, int n)
    {
        //Bracket boundary
        double low = 1, high = number;
        //interval splitting
        double guess = (low + high) / 2;
        //iteration
        while (ABS((Power(guess, n)) - number) >= PRECISION)
        {
            if (Power(guess, n) > number)
            {
                high = guess;
            }
            else
            {
                low = guess;
            }
            guess = (low + high) / 2;
        }
        return guess;
    }

    /*2nd solution : Newton Raphson
     * Our function is : f(x)  = x^(N) – A
     * according to newton : 
     * x(K+1) = x(K) – f(x) / f’(x)        
     * the derivative :  f’(x) = N*x^(N - 1)
     * K : denotes the iteration index, so :
     * x(K + 1) = (1 / N) * ((N - 1) * x(K) + A / x(K) ^ (N - 1))
     */
    public static double GetNthRootNewton(double number, int n)
    {
        //x0, being the initial guess
        double x0 = 1;
        // xK for current interation sampling
        double xK = 1;
        // max value for the difference between two sampling iterations
        double delta = int.MaxValue;
        // looping until we reach desired accuracy
        while (delta > PRECISION)
        {
            xK = ((n - 1.0) * x0 + number / Power(x0, n - 1)) / n;
            delta = ABS(xK - x0);
            x0 = xK;
        }
        return xK;
    }

    #region Tools functions

    //does the same as Math.Pow but in a very simple way (supposes y >=1 )
    public static double Power(double x, int y)
    {
        double result = 1;
        for (int i = 1; i <= y; i++)
        {
            result *= x;
        }
        return result;
    }

    //does the same as Math.ABS
    public static double ABS(double x)
    {
        return x > 0 ? x : -x;
    }

    #endregion
}

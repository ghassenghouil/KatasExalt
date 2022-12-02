/*******************************************************************************************************************************************
 * 
 * Solution description : 
 * 
 * Generating 3 different strings with the same hash code is equivalent to detecting 'collisions' within a hash function algorithm.
 * The C# string.GetHashCode method generates a 32bit signed integer so there is 2^31 available hashes.
 * Given a total of at least (2^31 + 1) input strings there will be a non zero probability of a collision.
 * The implementation of C# string.GetHashCode function is Framework version/Plateform (x86-x64/Arm) dependent and even app domain 
 * dependent as per :  https://learn.microsoft.com/en-us/dotnet/api/system.string.gethashcode?view=net-7.0
 * And it is optimized for uniform distrubution of hashes to minimize collisions.
 * 
 * The most efficient way of solving this requires an understanding of the underlying hashing function, which we cannot.
 * So the second best is to bruteforce our way into it and we have probability in our side somewhat : 
 *  https://en.wikipedia.org/wiki/Birthday_problem
 * So the problem becomes : whats is the most efficient brute force algorithm.
 * 
 * I've tried below running all combinations of a 4 character strings and detecting all collisions.
 * I've used a hash map and a linked list as the backbone of the storage to keep a linear access latency.
 * 
 * The program resolves 7 collisions at around 15 seconds on my machine (CPU Ryzen 2700x,DDR4 3200) and consumes 2.7GB of memory targeting 
 * the hash function of .net7 on x64.
 * 
 * *****************************************************************************************************************************************
 */


using System;

namespace StringHashCollisions
{
    internal class Program
    {
        private const int NCHAR = 4;
        private static char[] chars = new char[62] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private const int ALPHAS = 62;

        static void Main(string[] args)
        {
            Detect3WayCollisions();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void Detect3WayCollisions()
        {
            var map = new Dictionary<int, LinkedList<string>>();
            int[] index = new int[NCHAR];
            char[] buf = new char[NCHAR];
            while (true)
            {
                for (int i = 0; i < NCHAR; ++i)
                {
                    buf[i] = chars[index[i]];
                }
                var str = new string(buf);
                var hash = str.GetHashCode();
                LinkedList<string> strings;
                if (map.TryGetValue(hash, out strings) == false)
                {
                    strings = new LinkedList<string>();
                    strings.AddFirst(str);
                    map[hash] = strings;
                }
                else
                {
                    strings.AddLast(str);
                }
                var carry = 1;
                for (int i = 0; i < NCHAR; ++i)
                {
                    index[i] = index[i] + carry;
                    carry = (int)(index[i] / ALPHAS);
                    index[i] %= ALPHAS;
                }
                if (carry > 0)
                {
                    break;
                }
            }
            foreach (var kvp in map)
            {
                if (kvp.Value.Count >= 3)
                {
                    Console.WriteLine("detected 3 collisions for hash : " + kvp.Key);
                    foreach (var str in kvp.Value)
                    {
                        Console.WriteLine("\t" + str);
                    }
                }
            }
        }
    }
}
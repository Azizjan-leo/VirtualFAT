using System;

namespace VirtualFAT
{
    public static class IntToBase
    {
        /// <summary>
        /// To return char for a value. For  
        /// example '2' is returned for 2.  
        /// 'A' is returned for 10. 'B' for 11 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        static char ReVal(int num)
        {
            if (num >= 0 && num <= 9)
                return (char)(num + 48);
            else
                return (char)(num - 10 + 65);
        }

        /// <summary>
        /// Converts a given decimal number to a base
        /// </summary>
        /// <param name="base1">The base</param>
        /// <param name="inputNum">The number</param>
        /// <returns>Given number in given base</returns>
        public static string DoIt(int base1, int inputNum)
        {
            string s = "";

            // Convert input number is given  
            // base by repeatedly dividing it 
            // by base and taking remainder 
            while (inputNum > 0)
            {
                s += ReVal(inputNum % base1);
                inputNum /= base1;
            }
            char[] res = s.ToCharArray();

            // Reverse the result 
            Array.Reverse(res);
            return new String(res);
        }
    }
}

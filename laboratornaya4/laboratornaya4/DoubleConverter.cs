using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace laboratornaya4
{
    public class DoubleConverter
    {
        public static string ToExactString(double d)
        {
            if (double.IsPositiveInfinity(d))
                return "+Infinity";
            if (double.IsNegativeInfinity(d))
                return "-Infinity";
            if (double.IsNaN(d))
                return "NaN";
            
            long bits = BitConverter.DoubleToInt64Bits(d);
            bool negative = (bits < 0);
            int exponent = (int) ((bits >> 52) & 0x7ffL);
            long mantissa = bits & 0xfffffffffffffL;
            
            if (exponent == 0)
            {
                exponent++;
            }
            else
            {
                mantissa = mantissa | (1L << 52);
            }
            
            exponent -= 1075;

            if (mantissa == 0)
            {
                return "0";
            }
            
            while ((mantissa & 1) == 0)
            {
                mantissa >>= 1;
                exponent++;
            }
            
            ArbitraryDecimal ad = new ArbitraryDecimal(mantissa);
            
            if (exponent < 0)
            {
                for (int i = 0; i < -exponent; i++)
                    ad.MultiplyBy(5);
                ad.Shift(-exponent);
            }

            else
            {
                for (int i = 0; i < exponent; i++)
                    ad.MultiplyBy(2);
            }
            
            if (negative)
                return "-" + ad.ToString();
            else
                return ad.ToString();
        }
        
        class ArbitraryDecimal
        {
            byte[] digits;


            int decimalPoint = 0;


            internal ArbitraryDecimal(long x)
            {
                string tmp = x.ToString(CultureInfo.InvariantCulture);
                digits = new byte[tmp.Length];
                for (int i = 0; i < tmp.Length; i++)
                    digits[i] = (byte) (tmp[i] - '0');
                Normalize();
            }


            internal void MultiplyBy(int amount)
            {
                byte[] result = new byte[digits.Length + 1];
                for (int i = digits.Length - 1; i >= 0; i--)
                {
                    int resultDigit = digits[i] * amount + result[i + 1];
                    result[i] = (byte) (resultDigit / 10);
                    result[i + 1] = (byte) (resultDigit % 10);
                }
                if (result[0] != 0)
                {
                    digits = result;
                }
                else
                {
                    Array.Copy(result, 1, digits, 0, digits.Length);
                }
                Normalize();
            }
            
            internal void Shift(int amount)
            {
                decimalPoint += amount;
            }
            
            internal void Normalize()
            {
                int first;
                for (first = 0; first < digits.Length; first++)
                    if (digits[first] != 0)
                        break;
                int last;
                for (last = digits.Length - 1; last >= 0; last--)
                    if (digits[last] != 0)
                        break;

                if (first == 0 && last == digits.Length - 1)
                    return;

                byte[] tmp = new byte[last - first + 1];
                for (int i = 0; i < tmp.Length; i++)
                    tmp[i] = digits[i + first];

                decimalPoint -= digits.Length - (last + 1);
                digits = tmp;
            }
            
            public override String ToString()
            {
                char[] digitString = new char[digits.Length];
                for (int i = 0; i < digits.Length; i++)
                    digitString[i] = (char) (digits[i] + '0');
                
                if (decimalPoint == 0)
                {
                    return new string(digitString);
                }
                if (decimalPoint < 0)
                {
                    return new string(digitString) +
                           new string('0', -decimalPoint);
                }
                
                if (decimalPoint >= digitString.Length)
                {
                    return "0." +
                           new string('0', (decimalPoint - digitString.Length)) +
                           new string(digitString);
                }
                
                return new string(digitString, 0,
                           digitString.Length - decimalPoint) +
                       "." +
                       new string(digitString,
                           digitString.Length - decimalPoint,
                           decimalPoint);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace laboratornaya4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void txtfrDectoBindec_TextChanged(object sender, EventArgs e)
        {
        }

        public void FloatBin(string decimal_number)
        {
            if (decimal_number[0] == '-')
            {
                decimal_number = decimal_number.Substring(1);
                textBox2.Text = "1";
                textBox9.Text = "1";
                textBox13.Text = "1";
            }
            else
            {
                textBox2.Text = "0";
                textBox9.Text = "0";
                textBox13.Text = "0";
            }
            string[] tokens = decimal_number.Split(',');
            string int_part = tokens[0];
            string float_part = "0,";
            if (tokens.Length > 1)
            {
                float_part += tokens[1];
            }
            else
            {
                float_part += "0";
            }

            string bin_float_part = ToBinDouble(Convert.ToDouble(float_part), 52);

            //textBox1.Text += "";
        }

        public static string ToBinInt(double integ)
        {
            string str = "";
            if (integ == 0)
            {
                return integ.ToString();
            }
            while (integ > 0)
            {
                str = String.Concat(Convert.ToString(integ % 2), str);
                integ = Math.Truncate(integ / 2);
            }

            return str;
        }

        public static string ToBinDouble(double frac, int len)
        {
            string str = "";
            int c;
            int n = 0;
            while (n < len)
            {
                frac *= 2;
                c = Convert.ToInt32(Math.Truncate(frac));
                str = String.Concat(str, Convert.ToString(c));
                frac -= c;
                n++;
            }
            return str;
        }

        //public static string ToHexNew(string number)
        //{

        //}

        public static string ToHexDouble(int num, int length)
        {
            string
                a = "0123456789ABCDEF"; //массив символов для хранения цифр(для более быстрого перевода букв в числа) 
            string b = "";
            int ost = 0;
            int res = 0;

            double d_num = Convert.ToDouble("0," + num.ToString());

            string result = "";

            for (int i = 0; i < length; i = i + 1) //умножаем цифры на 16 и отбрасываем последнюю полученную цифру 
            {
                d_num = d_num * 16;

                string[] parts = d_num.ToString().Split(',');

                if (parts.Length == 2 && d_num != 0)
                {
                    result = result + a[Convert.ToInt32(parts[0])];
                    d_num = Convert.ToDouble("0," + parts[1]);
                }
                else
                {
                    result = result + a[Convert.ToInt32(d_num)];
                    break;
                }
            }
            return result;
        }


        public static string ToHexInt(int number)
        {
            if (number == 0)
            {
                return number.ToString();
            }
            string massiv_abc = "0123456789ABCDEF";
            string result = "";
            int[] parts = new int[50];
            int res = 27;
            int k = 0;
            while (number != 0)
            {
                res = number % 16;
                number = number / 16;
                parts[k] = res;
                k = k + 1;
            }
            for (int i = k - 1; i >= 0; i = i - 1)
            {
                result = result + massiv_abc[parts[i]];
            }
            return result;
        }

        public List<string> ToBinNew(double number, int bias, int exponenta, int mantissa)
        {
            string exp;
            string mant;
            int flag_for_zero = 0;

            if (number == 0)
            {
                flag_for_zero = 1;
            }

            //if (number)

            if (number < 0)
            {
                number = Math.Abs(number);
                textBox2.Text = "1";
                textBox9.Text = "1";
                textBox13.Text = "1";
                textBox14.Text = "-";
            }
            else
            {
                textBox2.Text = "0";
                textBox9.Text = "0";
                textBox13.Text = "0";
                textBox14.Text = "+";
            }

            string[] tokens = number.ToString().Split(',');
            string int_part = tokens[0];
            string float_part = "0,";
            if (tokens.Length > 1)
            {
                float_part += tokens[1];
            }
            else
            {
                float_part += "0";
            }


            string bin_int_part = ToBinInt(Convert.ToDouble(int_part));
            string bin_float_part = ToBinDouble(Convert.ToDouble(float_part), mantissa - bin_int_part.Length);

            string whole_bin_num = (bin_int_part + ',' + bin_float_part);

            if (bin_int_part == "0")
            {
                int index = bin_float_part.IndexOf("1") + 1;
                exp = Convert.ToString(-index + bias, 2);
                mant = bin_float_part.Substring(1);
            }
            else
            {
                exp = Convert.ToString(((bin_int_part.Length - 1) + bias), 2);
                mant = bin_int_part.Substring(1) + bin_float_part;
            }

            if (flag_for_zero == 1)
            {
                exp = "0";
            }

            if (exp.Length < exponenta)
            {
                while (exp.Length < exponenta)
                {
                    exp = "0" + exp;
                }
            }
            else if (exp.Length > exponenta)
            {
                exp = exp.Substring(exp.Length - exponenta);
            }

            if (mant.Length > mantissa)
            {
                mant = mant.Substring(0, mantissa);
            }
            else if (mant.Length < mantissa)
            {
                while (mant.Length < mantissa)
                {
                    mant = mant + "0";
                }
            }

            List<string> ret_ans = new List<string>();

            ret_ans.Add(exp);
            ret_ans.Add(mant);
            return ret_ans;


            //MessageBox.Show(mant + " " + exp);
            //MessageBox.Show(ToHexInt(Convert.ToDouble(int_part)) + "," + ToHexDouble(Convert.ToDouble(float_part)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox15.Text = TestHex(txtfrDectoBindec.Text);
            List<String> ans1 = ToBinNew(Convert.ToDouble(txtfrDectoBindec.Text), 127, 8, 23);
            List<String> ans2 = ToBinNew(Convert.ToDouble(txtfrDectoBindec.Text), 1023, 11, 52);
            List<String> ans3 = ToBinNew(Convert.ToDouble(txtfrDectoBindec.Text), 16383, 15, 63);

            textBox3.Text = ans1[0].ToString();
            textBox5.Text = ans1[1].ToString();
            textBox8.Text = ans2[0].ToString();
            textBox6.Text = ans2[1].ToString();
            textBox12.Text = ans3[0].ToString();
            textBox10.Text = ans3[1].ToString();
        }

        //public string NormalizePoryadok(int Dec_Poryadok, int len)
        //{
        //    //int norm_por = Dec_Poryadok - len;
        //    //return ToBinInt(norm_por.ToString());
        //}

        public string Poryadok(string bin_string)
        {
            int first1 = bin_string.IndexOf('1');
            if (first1.ToString() == "-1")
            {
                //return ToBinInt("0");
            }
            int first_dot = bin_string.IndexOf(',');
            MessageBox.Show(first_dot.ToString());
            return (first_dot - first1 - 1).ToString();
        }

        public string Normalize(string bin_string)
        {
            string[] tokens = bin_string.Split(',');
            string whole_num = tokens[0] + tokens[1];
            return whole_num;
        }

        public string TestHex(string str)
        {
            double text_num = Convert.ToDouble(str);
            if (text_num < 0)
            {
                text_num = Math.Abs(text_num);
            }
            string[] tokens = text_num.ToString().Split(',');
            string int_part = tokens[0];
            string float_part = "0,";
            if (tokens.Length > 1)
            {
                float_part += tokens[1];
            }
            else
            {
                float_part += "0";
            }

            if (float_part.Length > 5)
            {
                return ToHexInt(Convert.ToInt32(int_part)) + "," +
                       ToHexDouble(Convert.ToInt32(float_part.Substring(2, 5)), 5).ToString();
            }
            else
            {
                return ToHexInt(Convert.ToInt32(int_part)) + "," +
                       ToHexDouble(Convert.ToInt32(float_part.Substring(2)), 5).ToString();
            }
        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double dec_number;
            if (textBox28.Text.Contains("1") || textBox26.Text.Contains("1"))
            {
                dec_number = Math.Pow(-1, Convert.ToUInt64(textBox29.Text, 2)) *
                             Math.Pow(2, Convert.ToUInt64(textBox28.Text, 2) - 127) *
                             (1 + Convert.ToUInt64(textBox26.Text, 2) / Math.Pow(2, 23));
            }
            else
            {
                dec_number = 0;
            }
            textBox30.Text = dec_number.ToString();
            if (Math.Pow(-1, Convert.ToUInt64(textBox29.Text, 2)) < 0)
            {
                textBox17.Text = "-";
            }
            else
            {
                textBox17.Text = "+";
            }

            //textBox16.Text = BinaryStringToHexString(textBox28.Text+textBox26.Text);

            textBox16.Text = TestHex(textBox30.Text);
        }

        public static string BinaryStringToHexString(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double dec_number;
            if (textBox24.Text.Contains("1") || textBox22.Text.Contains("1"))
            {
                dec_number = Math.Pow(-1, Convert.ToUInt64(textBox25.Text, 2)) *
                             Math.Pow(2, Convert.ToUInt64(textBox24.Text, 2) - 1023) *
                             (1 + Convert.ToUInt64(textBox22.Text, 2) / Math.Pow(2, 52));
            }
            else
            {
                dec_number = 0;
            }
            textBox30.Text = dec_number.ToString();
            if (Math.Pow(-1, Convert.ToUInt64(textBox25.Text, 2)) < 0)
            {
                textBox17.Text = "-";
            }
            else
            {
                textBox17.Text = "+";
            }

            //textBox16.Text = BinaryStringToHexString(textBox24.Text + textBox22.Text);
            textBox16.Text = TestHex(textBox30.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double dec_number;
            if (textBox20.Text.Contains("1") || textBox18.Text.Contains("1"))
            {
                dec_number = Math.Pow(-1, Convert.ToUInt64(textBox21.Text, 2)) *
                             Math.Pow(2, Convert.ToUInt64(textBox20.Text, 2) - 16383) *
                             (1 + Convert.ToUInt64(textBox18.Text, 2) / Math.Pow(2, 63));
            }
            else
            {
                dec_number = 0;
            }
            textBox30.Text = dec_number.ToString();
            if (Math.Pow(-1, Convert.ToUInt64(textBox21.Text, 2)) < 0)
            {
                textBox17.Text = "-";
            }
            else
            {
                textBox17.Text = "+";
            }

            //textBox16.Text = BinaryStringToHexString(textBox20.Text + textBox18.Text);
            textBox16.Text = TestHex(textBox30.Text);
        }
    }
}
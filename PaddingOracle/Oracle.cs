using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddingOracle
{
    class Oracle
    {
        public Oracle(string cipher)
        {
            cipherText = new List<byte>();
            plainText = new List<byte>();

            for(int i = 0; i < cipher.Length; i+=2)
            {
                string val = cipher.Substring(i, 2);
                cipherText.Add(Byte.Parse(val, System.Globalization.NumberStyles.HexNumber));
            }
        }

        public string getCipherText()
        {
            string hex = "";

            foreach (byte b in cipherText)
                hex += String.Format("{0:x2}", b);
            //hex += '\n';

            return hex;
        }

        public int getCipherTextSize()
        {
            return cipherText.Count;
        }

        public string getPlainText()
        {
            string hex = "";
            foreach (byte b in plainText)
                hex += String.Format("{0:x2} ", b);
            //hex += '\n';
            return hex;
        }

        public string getPlainTextInChar()
        {
            string hex = "";
            foreach (byte b in plainText)
                hex += Convert.ToChar(b);
            //hex += '\n';
            return hex;
        }

        public void guessByte(int guess, int length)
        {
            int delta;
            if (guess == 0)
                delta = guess ^ length;
            else
                delta = (guess - 1) ^ guess;
            cipherText[cipherText.Count - length] ^= (byte)delta;

            if (guess == 0)
            {
                for (int i = 1; i < length; i++)
                {
                    int c = (length - 1) ^ length;
                    cipherText[cipherText.Count - i] ^= (byte)(c);
                } 
            }
        }

        public void addPlainText(int correctGuess)
        {
            plainText.Add((byte)correctGuess);
        }

        public void reversePlainText()
        {
            plainText.Reverse();
        }

        //public void obtainByte(int guess, int lastNBlock, int length)
        //{
        //    List<byte> currentBlock = plainText[plainText.Count - lastNBlock - 1];
        //    //Console.WriteLine("\n======{0:x2} ", currentBlock[currentBlock.Count - 1]);
        //    int delta = guess ^ length;
        //    currentBlock[currentBlock.Count - 1] ^= (byte)delta;
        //}

        //public static int BLOCK_SIZE = 16;
        private List<byte> cipherText;
        private List<byte> plainText;
    }
}

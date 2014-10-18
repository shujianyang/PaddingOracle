using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddingOracle
{
    class Oracle
    {
        public Oracle(string cipherString)
        {
            cipherText = cipherString;

            cipher = new List<List<byte>>();
            plainText = new List<byte>();
            //plainText = new List<List<byte>>();

            List<byte> blockData = new List<byte>();

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                string val = cipherText.Substring(i, 2);
                blockData.Add(Byte.Parse(val, System.Globalization.NumberStyles.HexNumber));
                if ((i + 2) / 2 % BLOCK_SIZE == 0)
                {
                    cipher.Add(blockData);
                    blockData = new List<byte>();
                }
            }
        }

        public void resetCipher()
        {
            cipher = new List<List<byte>>();
            List<byte> blockData = new List<byte>();

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                string val = cipherText.Substring(i, 2);
                blockData.Add(Byte.Parse(val, System.Globalization.NumberStyles.HexNumber));
                if ((i + 2) / 2 % BLOCK_SIZE == 0)
                {
                    cipher.Add(blockData);
                    blockData = new List<byte>();
                }
            }
        }

        public string getModifiedCipherText(int lastBlock)
        {
            string hex = "";
            for (int i = 0; i <= cipher.Count - lastBlock; ++i)
            {
                foreach (byte b in cipher[i])
                    hex += String.Format("{0:x2}", b);
                //hex += '\n';
            }
            return hex;
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

        public void guessByte(int guess, int lastNBlock, int length)
        {
            List<byte> currentBlock = cipher[cipher.Count - lastNBlock - 1];
            //Console.WriteLine("\n======{0:x2} ", currentBlock[currentBlock.Count - 1]);
            int delta;
            if (guess == 0)
                delta = guess ^ length;
            else
                delta = (guess - 1) ^ guess;
            currentBlock[currentBlock.Count - length] ^= (byte)delta;

            if (guess == 0)
            {
                for (int i = 1; i < length; i++)
                {
                    int c = (length - 1) ^ length;
                    currentBlock[currentBlock.Count - i] ^= (byte)(c);
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

        public static int BLOCK_SIZE = 16;
        private string cipherText;
        private List<List<byte>> cipher;
        private List<byte> plainText;
    }
}

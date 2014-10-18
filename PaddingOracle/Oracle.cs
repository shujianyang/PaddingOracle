//=============================================
//Author: Shujian Yang
//Organization: University of Central Oklahoma
//Note: This class implements padding oracle decryption of AES in CBC mode.
//=============================================

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

            List<byte> blockData = new List<byte>();

            //Parse hex string to bytes and store according to block size.
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
            //Console.WriteLine("Cipher size: {0}", cipher.Count);
        }

        //Reset modified cipher text into its original form.
        //Decrypted block is dumped.
        public void resetCipher(int block)
        {
            cipher = new List<List<byte>>();
            List<byte> blockData = new List<byte>();

            for (int i = 0; i < cipherText.Length - block * BLOCK_SIZE * 2; i += 2)
            {
                string val = cipherText.Substring(i, 2);
                blockData.Add(Byte.Parse(val, System.Globalization.NumberStyles.HexNumber));
                if ((i + 2) / 2 % BLOCK_SIZE == 0)
                {
                    cipher.Add(blockData);
                    blockData = new List<byte>();
                }
            }
            //Console.WriteLine("Cipher size: {0}", cipher.Count);
        }

        public string getModifiedCipherText()
        {
            string hex = "";
            foreach(List<byte> bl in cipher)
            {
                foreach (byte b in bl)
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

        public int getBlockCount()
        {
            return cipher.Count;
        }

        //Modified cipher text one byte each time to guess one byte in plain text.
        public void guessByte(int guess, int length)
        {
            List<byte> currentBlock = cipher[cipher.Count - 2]; //To guess one byte in block N, modified cipher text in block N-1.

            int delta;
            if (guess == 0)
                delta = guess ^ length; //When guessing a byte for the first time.
            else
                delta = (guess - 1) ^ guess; //If previous guess is wrong, increase guess by 1.

            currentBlock[currentBlock.Count - length] ^= (byte)delta; 

            if (guess == 0) //Modified all following bytes to match the padding pattern.
            {
                for (int i = 1; i < length; i++)
                {
                    int c = (length - 1) ^ length;
                    currentBlock[currentBlock.Count - i] ^= (byte)(c);
                } 
            }
        }

        //Append obtained plain text in reverse order
        public void addPlainText(int correctGuess)
        {
            plainText.Add((byte)correctGuess);
        }

        //Reverse to get correct plain text.
        public void reversePlainText()
        {
            plainText.Reverse();
        }

        public static int BLOCK_SIZE = 16;

        private string cipherText;
        private List<List<byte>> cipher;
        private List<byte> plainText;
    }
}

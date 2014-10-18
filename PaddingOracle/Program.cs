//=============================================
//Author: Shujian Yang
//Organization: University of Central Oklahoma
//Note: Main program
//=============================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PaddingOracle
{
    class Program
    {
        static void Main(string[] args)
        {
            string cipherText = "f20bdba6ff29eed7b046d1df9fb7000058b1ffb4210a580f748b4ac714c001bd4a61044426fb515dad3f21f18aa577c0bdf302936266926ff37dbf7035d5eeb4";
            
            Oracle ora = new Oracle(cipherText);
            Console.WriteLine(ora.getModifiedCipherText());

            int initBlockCount = ora.getBlockCount();

            for (int currentBlock = 1; currentBlock < initBlockCount; currentBlock++)
            {
                for (int guessByte = 1; guessByte < 17; guessByte++)
                {
                    Console.WriteLine("Guessing byte {0} in block {1}...", Oracle.BLOCK_SIZE - guessByte, ora.getBlockCount() - 1);

                    for (int oneGuess = 0; oneGuess < 256; oneGuess++)
                    {
                        ora.guessByte(oneGuess, guessByte);

                        Pad tryPad = new Pad(ora.getModifiedCipherText());

                        int r = tryPad.getResponseCode();

                        if (r != 403)
                        {
                            if (r == 200 && currentBlock == 1 && guessByte == 1)
                                continue; //This guess results in original cipher text, which is acceptable by server.
                            Console.WriteLine("Correct guess of byte {0} in block {1}: {2}", 
                                Oracle.BLOCK_SIZE - guessByte, ora.getBlockCount() - 1, oneGuess);
                            ora.addPlainText(oneGuess);
                            Console.WriteLine("Plain text (reversed): " + ora.getPlainText() + '\n');
                            break;
                        }
                    }
                }
                ora.resetCipher(currentBlock);
            }

            ora.reversePlainText();
            Console.WriteLine("Decryption completed.");
            Console.WriteLine("Decrypted plaintext in hex:");
            Console.WriteLine(ora.getPlainText());
            Console.WriteLine("Decrypted plaintext in char:");
            Console.WriteLine(ora.getPlainTextInChar());
        }
    }
}

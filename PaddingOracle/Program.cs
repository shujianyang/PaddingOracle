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

            Pad p = new Pad(cipherText);
            int rr = p.getResponseCode();
            Console.WriteLine("Response code: {0}", rr);

            Oracle ora = new Oracle(cipherText);
            Console.WriteLine(ora.getCipherText());


            for (int guessLength = 1; guessLength <= ora.getCipherTextSize() - Oracle.BLOCK_SIZE; guessLength++)
            {
                Console.WriteLine("Guessing length {0}: ", guessLength);

                for (int oneGuess = 0; oneGuess < 256; oneGuess++)
                {
                    //Console.WriteLine("Guess {0} of length {1}: ", oneGuess, guessLength);
                    ora.guessByte(oneGuess, guessLength);
                    //Console.WriteLine(ora.getCipherText());

                    Pad tryPad = new Pad(ora.getCipherText());

                    int r = tryPad.getResponseCode();

                    //Console.WriteLine("Response code: {0}\n", r);

                    if (r != 403)
                    {
                        //Console.WriteLine("Response code: {0}\n", r);
                        if (r == 200 && guessLength == 1)
                            continue;
                        Console.WriteLine("Correct guess {0} of length {1}: ", oneGuess, guessLength);
                        ora.addPlainText(oneGuess);
                        Console.WriteLine("Plain text: " + ora.getPlainText() + '\n');
                        break;
                    }
                }
            }

            ora.reversePlainText();
            Console.WriteLine(ora.getPlainText().Length);
            Console.WriteLine(ora.getPlainText());
            Console.WriteLine(ora.getPlainTextInChar().Length);
            Console.WriteLine(ora.getPlainTextInChar());
        }
    }
}

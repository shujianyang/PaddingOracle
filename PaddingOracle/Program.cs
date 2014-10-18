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

            //Pad p = new Pad(cipherText);
            //int rr = p.getResponseCode();
            //Console.WriteLine("Response code: {0}", rr);
            
            Oracle ora = new Oracle(cipherText);
            Console.WriteLine(ora.getModifiedCipherText(1));

            for (int last = 1; last < 4; last++)
            {
                ora.resetCipher();
                for (int guessLength = 1; guessLength < 17; guessLength++)
                {
                    Console.WriteLine("Guessing length {0} in last {1} block: ", guessLength, last);

                    for (int oneGuess = 0; oneGuess < 256; oneGuess++)
                    {
                        //Console.WriteLine("Guess {0} of length {1} in last {2} block: ", oneGuess, guessLength, last);
                        ora.guessByte(oneGuess, last, guessLength);
                        if(oneGuess == 0) Console.WriteLine(ora.getModifiedCipherText(last));

                        Pad tryPad = new Pad(ora.getModifiedCipherText(last));

                        int r = tryPad.getResponseCode();

                        //Console.WriteLine("Response code: {0}\n", r);

                        if (r != 403)
                        {
                            if (r == 200 && last == 1 && guessLength == 1)
                                continue;
                            Console.WriteLine("Correct guess {0} of length {1} in last {2} block: ", oneGuess, guessLength, last);
                            ora.addPlainText(oneGuess);
                            Console.WriteLine("Plain text: " + ora.getPlainText() + '\n');
                            break;
                        }
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

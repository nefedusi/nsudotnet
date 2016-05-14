using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Nefedov.Nsudotnet.Enigma
{
    class Program
    {
        public enum Algorithm
        {
            undef, aes, des, rc2, rijndael
        }

        static Mode mode = Mode.undef;
        static Algorithm alg = Algorithm.undef;
        static String inFilename = null;
        static String keyFilename = null;
        static String outFilename = null;

        private static void ParseArguments(string[] args)
        {
            for (int i = 0; i < args.Length - 3; i++)
            {
                switch (args[i])
                {
                    case "encrypt":
                        {
                            mode = Mode.encrypt;
                            break;
                        }
                    case "decrypt":
                        {
                            mode = Mode.decrypt;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("ERROR!!! - wrong set or order of arguments");
                            return;
                        }
                }
                i++;
                inFilename = args[i];    //if I use String class - will args[i] chars copy or there will be a link to them?
                i++;

                switch (args[i])
                {
                    case "aes":
                        {
                            alg = Algorithm.aes;
                            break;
                        }
                    case "des":
                        {
                            alg = Algorithm.des;
                            break;
                        }
                    case "rc2":
                        {
                            alg = Algorithm.rc2;
                            break;
                        }
                    case "rijndael":
                        {
                            alg = Algorithm.rijndael;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("ERROR!!! - wrong set or order of arguments");
                            return;
                        }
                }
                i++;
                if (mode == Mode.decrypt)
                {
                    keyFilename = args[i];
                    i++;
                    if (i == args.Length)
                    {
                        Console.WriteLine("ERROR!!! - too little arguments");
                        return;
                    }
                }
                outFilename = args[i];
            }
            if (mode == Mode.encrypt)
            {
                keyFilename = String.Concat(inFilename.Substring(0, inFilename.LastIndexOf('.')) + ".key.txt");
            }
        }

        private static void SwitchAlgorithm()
        {
            switch (alg)
            {
                case Algorithm.aes:
                    {
                        Coder.Execute<AesManaged>(mode, inFilename, keyFilename, outFilename);
                        break;
                    }
                case Algorithm.des:
                    {
                        Coder.Execute<DESCryptoServiceProvider>(mode, inFilename, keyFilename, outFilename);
                        break;
                    }
                case Algorithm.rc2:
                    {
                        Coder.Execute<RC2CryptoServiceProvider>(mode, inFilename, keyFilename, outFilename);
                        break;
                    }
                case Algorithm.rijndael:
                    {
                        Coder.Execute<RijndaelManaged>(mode, inFilename, keyFilename, outFilename);
                        break;
                    }
                default:
                    {
                        System.Console.WriteLine("ERROR!!! - can't find the necessary coder");
                        return;
                    }
            }
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Enigma starts its work!");
            if (args.Length < 4)
            {
                Console.WriteLine("ERROR!!! - too little arguments");
            }
            /*for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("Arg[{0}] = [{1}]", i, args[i]);
            }*/

            ParseArguments(args);
            //Console.WriteLine("keyfilename={0}", keyFilename);

            SwitchAlgorithm();
        }
    }
}

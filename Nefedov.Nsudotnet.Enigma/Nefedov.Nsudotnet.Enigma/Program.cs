using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Nefedov.Nsudotnet.Enigma
{
    class Program
    {
        private enum Algorithm
        {
            Undef, Aes, Des, Rc2, Rijndael
        }

        private static Mode _mode = Mode.Undef;
        private static Algorithm _alg = Algorithm.Undef;
        private static String _inFileName = null;
        private static String _keyFileName = null;
        private static String _outFileName = null;

        private static bool ParseArguments(string[] args)
        {
            for (int i = 0; i < args.Length - 3; i++)
            {
                switch (args[i])
                {
                    case "encrypt":
                        {
                            _mode = Mode.Encrypt;
                            break;
                        }
                    case "decrypt":
                        {
                            _mode = Mode.Decrypt;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("ERROR!!! - wrong set or order of arguments");
                            return false;
                        }
                }
                i++;
                _inFileName = args[i];    //if I use String class - will args[i] chars copy or there will be a link to them?
                i++;

                switch (args[i])
                {
                    case "aes":
                        {
                            _alg = Algorithm.Aes;
                            break;
                        }
                    case "des":
                        {
                            _alg = Algorithm.Des;
                            break;
                        }
                    case "rc2":
                        {
                            _alg = Algorithm.Rc2;
                            break;
                        }
                    case "rijndael":
                        {
                            _alg = Algorithm.Rijndael;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("ERROR!!! - wrong set or order of arguments");
                            System.Environment.Exit(0);
                            return false;
                        }
                }
                i++;
                if (_mode == Mode.Decrypt)
                {
                    _keyFileName = args[i];
                    i++;
                    if (i == args.Length)
                    {
                        Console.WriteLine("ERROR!!! - too little arguments");
                        return false;
                    }
                }
                _outFileName = args[i];
            }
            if (_mode == Mode.Encrypt)
            {
                _keyFileName = String.Concat(_inFileName.Substring(0, _inFileName.LastIndexOf('.')) + ".key.txt");
            }
            return true;
        }

        private static void SwitchAlgorithm()
        {
            switch (_alg)
            {
                case Algorithm.Aes:
                    {
                        Coder.Execute<AesManaged>(_mode, _inFileName, _keyFileName, _outFileName);
                        break;
                    }
                case Algorithm.Des:
                    {
                        Coder.Execute<DESCryptoServiceProvider>(_mode, _inFileName, _keyFileName, _outFileName);
                        break;
                    }
                case Algorithm.Rc2:
                    {
                        Coder.Execute<RC2CryptoServiceProvider>(_mode, _inFileName, _keyFileName, _outFileName);
                        break;
                    }
                case Algorithm.Rijndael:
                    {
                        Coder.Execute<RijndaelManaged>(_mode, _inFileName, _keyFileName, _outFileName);
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
                return;
            }
            /*for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("Arg[{0}] = [{1}]", i, args[i]);
            }*/

            if (!ParseArguments(args))
            {
                return;
            }
            //Console.WriteLine("keyfilename={0}", keyFilename);

            SwitchAlgorithm();
        }
    }
}

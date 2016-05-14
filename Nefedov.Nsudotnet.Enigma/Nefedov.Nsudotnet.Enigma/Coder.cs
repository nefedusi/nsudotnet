using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Nefedov.Nsudotnet.Enigma
{
    public enum Mode
    {
        undef, encrypt, decrypt
    }

    static class Coder
    {
        public static void Execute<TType>(Mode mode, String inFilename, String keyFilename, String outFilename)
            where TType : SymmetricAlgorithm, new()
        {
            switch (mode)
            {
                case Mode.encrypt:
                    {
                        Encrypt<TType>(inFilename, keyFilename, outFilename);
                        break;
                    }
                case Mode.decrypt:
                    {
                        Decrypt<TType>(inFilename, keyFilename, outFilename);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("ERROR!!! - can't recognize coder mode");
                        return;
                    }
            }
        }

        public static void Encrypt<TType>(String inFilename, String keyFilename, String outFilename)
            where TType : SymmetricAlgorithm, new()
        {
            try
            {
                Console.WriteLine("Try to encrypt data...");
                using (FileStream inFs = new FileStream(inFilename, FileMode.Open, FileAccess.Read))
                {
                    using (TType coder = new TType())
                    {
                        coder.GenerateIV();
                        coder.GenerateKey();
                        using (FileStream keyFs = new FileStream(keyFilename, FileMode.Create, FileAccess.Write))
                        {
                            using (BinaryWriter binWriter = new BinaryWriter(keyFs))
                            {
                                string siv;
                                binWriter.Write(siv = Convert.ToBase64String(coder.IV));
                                string skey;
                                binWriter.Write(skey = Convert.ToBase64String(coder.Key));
                                binWriter.Flush();
                            }
                        }

                        using (ICryptoTransform transform = coder.CreateEncryptor())
                        {
                            using (FileStream outFs = new FileStream(outFilename, FileMode.Create, FileAccess.Write))
                            {
                                using (CryptoStream cs = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                                {
                                    inFs.CopyTo(cs);
                                    inFs.Flush();//necessary?
                                    cs.Flush();//necessary?
                                }
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ERROR!!! - input file is not found");
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR!!! - can't encode data");
            }
            Console.WriteLine("Data successfully encrypted!");
        }

        public static void Decrypt<TType>(String inFilename, String keyFilename, String outFilename)
            where TType : SymmetricAlgorithm, new()
        {
            Console.WriteLine("Try to decrypt data...");
            try
            {
                using (FileStream inFs = new FileStream(inFilename, FileMode.Open, FileAccess.Read))
                {
                    using (TType coder = new TType())
                    {
                        try
                        {
                            using (FileStream keyFs = new FileStream(keyFilename, FileMode.Open, FileAccess.Read))
                            {
                                using (BinaryReader str = new BinaryReader(keyFs))
                                {
                                    string base64IV = str.ReadString();
                                    coder.IV = Convert.FromBase64String(base64IV);
                                    string base64Key = str.ReadString();
                                    coder.Key = Convert.FromBase64String(base64Key);
                                }
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("ERROR!!! - can't find key file");
                            return;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERROR!!! - can't read initialization vector or key, maybe wrong algorythm selected");
                            return;
                        }

                        using (ICryptoTransform transform = coder.CreateDecryptor())
                        {
                            using (CryptoStream cs = new CryptoStream(inFs, transform, CryptoStreamMode.Read))
                            {
                                using (FileStream outFs = new FileStream(outFilename, FileMode.Create, FileAccess.Write))
                                {
                                    cs.CopyTo(outFs);
                                    cs.Flush();//necessary?
                                    outFs.Flush();//necessary?
                                }
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ERROR!!! - encoded file is not found");
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR!!! - can't decode data");
            }
            Console.WriteLine("Data successfully decrypted!");
        }
    }
}

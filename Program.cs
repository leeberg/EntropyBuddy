using System;
using System.Collections.Generic;
using System.IO;
using NETCore.Encrypt;

namespace EntropyBuddy
{
    class Program
    {
        

        static void Main(string[] arguments)
        {
            string mode = "null";
            string filepath = "null";
            bool cmdlinemode = false;

            NETCore.Encrypt.Internal.AESKey aesKey = EncryptProvider.CreateAesKey();

            string currentdatetime = (DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + "-ENCRYPTKEY.log");
            var keylogfile = File.Create(currentdatetime);


            using (StreamWriter outputFile = new StreamWriter(keylogfile))
            {
                outputFile.WriteLine("Welcome to your EntropyBuddy Key Backup Log File!");
                outputFile.WriteLine("AES Key: " + aesKey.Key);
                outputFile.WriteLine("AES IV: " + aesKey.IV);
            }



            if (arguments.Length == 0 || null == arguments)
            {
                string text = "WELCOME to the Entropy Buddy@-----------------------------@You have started the util without params so we will be verbose!@Specify a mode of operation:@ a for Encryption@ b for EntropyCheck output JUST entropy value@ c for EntropyCheck/Encrypt/EntropyCheck@ d for EntropyCheck/Encrypt/EntropyCheck/Decrypt@ e for Cross-EntropyCheck@ f for Check/Encrypt/Check, and output Pre,Post values";
                                                                                        
                text = text.Replace("@",  System.Environment.NewLine);

                Console.WriteLine(text);
                

                string line1 = Console.ReadLine();
                if (line1 != null)
                {
                    mode = line1.ToLower(); 
                }
                Console.WriteLine("Specify a File Path:");
                string line2 = Console.ReadLine();
                if (line2 != null)
                {
                    filepath = line2;
                }
                System.Console.WriteLine("Starting!!");
            }
            else
            {
                mode = arguments[0];
                filepath = arguments[1];
                char[] charsToTrim = { '"', '\''};
                filepath = arguments[1].Trim(charsToTrim);
                cmdlinemode = true;

            }

            if (filepath != "null" && mode != "null")
            {
                char[] charsToTrim = { '"', '\'' };
                filepath = filepath.Trim(charsToTrim);

                if (mode == "a")
                {
                    EncryptionOperation(filepath, false, aesKey);
                }
                else if (mode == "b")
                {
                    double entropyresult = CheckEntropy(filepath);
                    System.Console.WriteLine(entropyresult);
                }
                else if (mode == "c")
                {
                    System.Console.WriteLine("Getting Entropy of: " + filepath);
                    double entropyresult = CheckEntropy(filepath);
                    System.Console.WriteLine(entropyresult);


                    FileInfo fi = new FileInfo(filepath); //uri is the full path and file name
                    var fileInfo = fi.Attributes;


                    System.Console.WriteLine("Encrypting: " + filepath);
                    EncryptionOperation(filepath, false, aesKey);

                    System.Console.WriteLine("Getting Entropy of: " + filepath);
                    double entropyresult2 = CheckEntropy(filepath);
                    System.Console.WriteLine(entropyresult2);

                    System.Console.WriteLine("Entropy'ness changed by: " + (entropyresult2 - entropyresult));

                }

                else if (mode == "d")
                {
                    System.Console.WriteLine("Getting Entropy of: " + filepath);
                    double entropyresult = CheckEntropy(filepath);
                    System.Console.WriteLine(entropyresult);

                    System.Console.WriteLine("Encrypting: " + filepath);
                    EncryptionOperation(filepath, false, aesKey);

                    System.Console.WriteLine("Getting Entropy of: " + filepath);
                    double entropyresult2 = CheckEntropy(filepath);
                    System.Console.WriteLine(entropyresult2);

                    System.Console.WriteLine("Entropy'ness changed by: " + (entropyresult2 - entropyresult));

                    EncryptionOperation(filepath, true, aesKey);
                }

                else if (mode == "e")
                {
                    CrossEntropy Crosser = new CrossEntropy();
                    Crosser.RunCrossEntropy("test");
                    // RunCrossEntropy();
                }
                else if (mode == "f")
                {
                    double entropyresult = CheckEntropy(filepath);
                    EncryptionOperation(filepath, false, aesKey);
                    double entropyresultencrypted = CheckEntropy(filepath);
                    System.Console.WriteLine(entropyresult + "," + entropyresultencrypted);
                }
            }
            if(cmdlinemode == false)
            {
                Console.WriteLine("OK I'm done!");
                Console.ReadLine();
            }
            

        }

        public static double CheckEntropy(string path)
        {
            string contents = File.ReadAllText(path);
            double entropyNess = ShannonEntropy(contents);
            return entropyNess;
        }

        

        //https://stackoverflow.com/questions/990477/how-to-calculate-the-entropy-of-a-file
        public static double ShannonEntropy(string s)
        {
            var map = new Dictionary<char, int>();
            foreach (char c in s)
            {
                if (!map.ContainsKey(c))
                    map.Add(c, 1);
                else
                    map[c] += 1;
            }

            double result = 0.0;
            int len = s.Length;
            foreach (var item in map)
            {
                var frequency = (double)item.Value / len;
                result -= frequency * (Math.Log(frequency) / Math.Log(2));
            }

            return result;
        }


        public static void EncryptionOperation(string filepath,bool Decrypt, NETCore.Encrypt.Internal.AESKey aesKey)
        {
            

            var key = aesKey.Key;
            var iv = aesKey.IV;

            // TODO BACKUP MODE
          
            if(Decrypt == true)
            {
                string encryptedContents = File.ReadAllText(filepath);
                string decrypted = EncryptProvider.AESDecrypt(encryptedContents, key, iv);
                File.WriteAllText(filepath, decrypted);
            }
            else
            {
                string contents = File.ReadAllText(filepath);
                string encrypted = EncryptProvider.AESEncrypt(contents, key, iv);
                File.WriteAllText(filepath, encrypted);
            }

        }


  
    }

   }

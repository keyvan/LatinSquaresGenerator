using System;

namespace LatinSquaresGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Latin Squares Generator";

            Console.WriteLine("Compress Stream?");
            bool compressStream = (Console.ReadLine().ToLower() == ("yes") ? true : false);

            Console.WriteLine("Enter Symbols:");
            string symbolList = Console.ReadLine();
            Console.WriteLine("Enter Path:");
            string Path = Console.ReadLine();
            Console.WriteLine();

            int count = 0;
            foreach (string parm in args)
            {
                if (parm.Equals("-C")) compressStream = true;
                else if (count == 0) symbolList = parm;
                else if (count == 1) Path = parm;
                count++;
            }

            if (symbolList.Length == 0 || Path.Length == 0)
            {
                Console.WriteLine("Usage: LatinSquaresGenerator [-C] symbolList outputFile");
                Console.WriteLine("You cannot provide more than " + byte.MaxValue + " symbols.");
                Console.WriteLine("params: C - Compress the output stream");
                return;
            }

            string[] strItems = symbolList.Split(' ');

            if (strItems.Length >= byte.MaxValue)
            {
                Console.WriteLine("You provided more than " + byte.MaxValue + " values, items.");
                return;
            }

            byte numSymbols = (byte)strItems.Length;

            Console.WriteLine("Please wait. This process may take several minutes or even hours to complete...");

            DateTime startTime = DateTime.Now;
            Console.Write("Process started at: ");
            Console.WriteLine(startTime);

            TreeNodeListener theListener = new TreeNodeListener(Path,
                                                                strItems,
                                                                compressStream);
            Tree theTree = new Tree(numSymbols, theListener);
            theListener.Dispose();

            DateTime stopTime = DateTime.Now;
            Console.WriteLine();
            Console.Write("Process ended at:   ");
            Console.WriteLine(stopTime);

            TimeSpan duration = stopTime - startTime;
            Console.Write("Generation took ");
            Console.Write(duration);
            Console.WriteLine(" to complete.");

            /*
                        ArrayList List = theTree.toArrayList();

                        foreach (byte[,] objItem in List)
                          {
                            for (byte i = 0; (i <= objItem.GetUpperBound(0)); i++)
                              {
                                for (byte j = 0; (j <= objItem.GetUpperBound(1)); j++)
                                  {
                                    strOutput = (strOutput
                                                + (strItems[(objItem[i, j] - 1)] + " "));
                                    Console.Write((strItems[(objItem[i, j] - 1)] + " "));
                                  }

                                strOutput = (strOutput + "\r\n");
                              }

                            Console.WriteLine(strSeparator);
                            strOutput = (strOutput + "\r\n");
                          }

                        using (StreamWriter sw = new StreamWriter(Path))
                          {
                            // Add some text to the file.
                            sw.Write(strOutput);
                          }
            */

            Console.ReadLine();
        }
    }
}

using System;
using System.IO;
using System.IO.Compression;

namespace LatinSquaresGenerator
{
    internal class TreeNodeListener : IDisposable
    {
        private StreamWriter m_out;
        private string[] m_strItems;
        private int m_count = 0;

        /// <summary>
        ///   This constructs the listener and prepares the output stream.
        /// </summary>
        /// <param name="OutputFileName">The file to write to.</param>
        /// <param name="strItems">The mapping characters to use for the output.</param>
        /// <param name="compress">Should it compress the output stream (necessary for larger sets).</param>
        public TreeNodeListener(string OutputFileName,
                                 string[] strItems,
                                 bool compress)
        {

            FileStream theFile = new FileStream(OutputFileName,
                                                FileMode.Create);

            if (compress)
                m_out = new StreamWriter(
                        new GZipStream(theFile, CompressionMode.Compress));
            else
                m_out = new StreamWriter(
                        new BufferedStream(theFile));

            m_strItems = strItems;
        }

        public void putNode(TreeNode node)
        {
            byte[,] objItem = node.Board;
            for (byte i = 0; (i <= objItem.GetUpperBound(0)); i++)
            {
                for (byte j = 0; (j <= objItem.GetUpperBound(1)); j++)
                {
                    m_out.Write(m_strItems[(objItem[i, j] - 1)] + " ");
                    //Console.Write(m_strItems[(objItem[i, j] - 1)] + " ");
                }

                m_out.Write("\r\n");
                //Console.Write("\r\n");
            }

            m_out.Write("- \r\n");
            Console.Write(++m_count + "\r");
        }

        public void Dispose()
        {
            m_out.Close();
        }
    }
}

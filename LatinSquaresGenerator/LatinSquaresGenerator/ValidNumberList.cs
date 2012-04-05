using System.Collections;

namespace LatinSquaresGenerator
{
    internal class ValidNumberList
    {
        private int m_row;
        private int m_column;
        private System.Collections.Stack m_values;

        public int Row
        {
            get { return (m_row); }
            set { m_row = value; }
        }

        public int Col
        {
            get { return (m_column); }
            set { m_column = value; }
        }

        public int Count
        {
            get { return (m_values.Count); }
        }

        public byte Pop()
        {
            return ((byte)m_values.Pop());
        }

        public ValidNumberList(int row, int col, Stack values)
        {
            m_row = row;
            m_column = col;
            m_values = values;
        }
    }
}

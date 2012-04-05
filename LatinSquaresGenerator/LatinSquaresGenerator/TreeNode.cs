using System;
using System.Diagnostics;

namespace LatinSquaresGenerator
{
    internal class TreeNode
    {
        // Save Matrixes
        protected byte[,] m_board = null; // Save Matrixes
        /// <summary>
        ///   The Board property is not copied for speed, but the caller
        ///   should not modify the array.  If you need a board that you can
        ///   modify, you should copy it yourself first.
        /// </summary>
        public byte[,] Board
        {
            get { return m_board; }
        }

        private TreeNode m_son = null; // Point to Child
        public TreeNode Son
        {
            get { return m_son; }
            set { m_son = value; }
        }

        private TreeNode m_parent = null; // Point to Child
        public TreeNode Parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }

        private TreeNode m_nextNode = null; // Point to Next
        public TreeNode NextNode
        {
            get { return m_nextNode; }
            set { m_nextNode = value; }
        }

        private TreeNode m_prevNode = null; // Point to Prev
        public TreeNode PrevNode
        {
            get { return m_prevNode; }
            set { m_prevNode = value; }
        }

        protected int m_order = 0;
        public int Order
        {
            get { return m_order; }
        }

        protected bool m_complete = false;
        public bool Complete
        {
            get { return m_complete; }
            set
            {
                Debug.Assert(value == true,
                    "You cannot set Complete to false!");

                if (value)
                    m_complete = true;
            }
        }

        protected bool m_deadend = false;
        public bool Deadend
        {
            get { return m_deadend; }
            set
            {
                Debug.Assert(value == true,
                    "You cannot set Deadend to false!");

                if (value)
                    m_deadend = true;
            }
        }

        // Instiate a node of order Order
        public TreeNode(int order)
        {
            m_order = order;
            m_board = new byte[m_order, m_order];
        }

        /// <summary>
        ///   Instantiate a TreeNode using a source node for the board
        ///   and order.  Does not copy the "tree" (Son and NextNode).
        /// </summary>
        /// <param name="sourceNode">The node to copy.</param>
        public TreeNode(TreeNode sourceNode)
            : this(sourceNode.m_order)
        {
            CopyBoard(sourceNode);
        }

        /// <summary>
        ///   This method returns the value at the specified
        ///   coordinates.
        /// </summary>
        /// <param name="row">The row coordinate.</param>
        /// <param name="col">The column coordinate.</param>
        /// <returns>
        ///   The value at the specific row and column (or zero if no
        ///   value has yet been stored).
        /// </returns>
        public byte GetAt(int row, int col)
        {
            Debug.Assert(row > 0 && row < m_order);
            Debug.Assert(col > 0 && col < m_order);
            return (m_board[row, col]);
        }

        /// <summary>
        ///   This method sets the value at the specified
        ///   coordinates.
        /// </summary>
        /// <param name="row">The row coordinate.</param>
        /// <param name="col">The column coordinate.</param>
        /// <param name="value">The value to set (cannot be zero).</param>
        /// <returns>
        ///   The value at the specific row and column (or zero if no
        ///   value has yet been stored).
        /// </returns>
        public void SetAt(int row, int col, byte value)
        {
            Debug.Assert(row >= 0 && row < m_order);
            Debug.Assert(col >= 0 && col < m_order);
            Debug.Assert(value > 0);
            m_board[row, col] = value;
        }

        public void CopyBoard(TreeNode sourceNode)
        {
            Debug.Assert(sourceNode.m_order == m_order);
            Array.Copy(sourceNode.m_board, m_board, (m_order * m_order));
        }
    }
}

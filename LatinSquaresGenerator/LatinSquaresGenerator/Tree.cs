using System.Collections;
using System.Diagnostics;

namespace LatinSquaresGenerator
{
    internal class Tree
    {
        private TreeNodeListener m_listener = null;

        private byte m_order;
        public byte Order
        {
            get { return m_order; }
        }

        public Tree(byte Order, TreeNodeListener listener)
        {
            m_order = Order;
            m_listener = listener;

            // Call generate node and prime the tree with a root node
            GenerateTree(new TreeNode(m_order));
        }

        private void killNode(TreeNode deadNode)
        {
            deadNode.Deadend = true;

            // Remove this dead node if possible.
            if (deadNode.PrevNode != null)
            {
                Debug.Assert(deadNode.PrevNode.NextNode == deadNode);

                TreeNode prevNode = deadNode.PrevNode;

                // Take deadNode out of the loop
                prevNode.NextNode.PrevNode = deadNode.PrevNode;
                prevNode.NextNode = deadNode.NextNode;

                // Note that we can't clear deadNode.NextNode
                // yet since the caller may still use it to move
                // to the next node, but we can clear PrevNode.
                deadNode.PrevNode = null;

                // This object can be cleaned up as soon as the
                // caller drops its reference.
            }
            else
            {
                // Since we are first in line, see if we can point
                // our parent to our next node to get out of memory.
                if (deadNode.Parent != null)
                {
                    TreeNode ourParentNode = deadNode.Parent;

                    // If we are an only child deadend then our parent
                    // is a Deadend as well by definition
                    if (null == deadNode.NextNode)
                    {
                        deadNode.Parent = null;
                        ourParentNode.Son = null;

                        // Kill our parent too.
                        killNode(ourParentNode);
                    }
                    else
                    {
                        // Take deadNode out of the loop
                        ourParentNode.Son = deadNode.NextNode;
                        deadNode.NextNode.Parent = ourParentNode;
                        deadNode.NextNode.PrevNode = null;
                    }
                }
            }
        }

        /// <summary>
        ///   This method tries to generate all possible children for a
        ///   ParentNode and generate their children recursively by
        ///   calling itself again on each child
        /// </summary>
        /// <param name="ParentNode">
        ///   Parent node - we will generate its possible children
        /// </param>
        /// <remarks></remarks>
        private void GenerateTree(TreeNode ParentNode)
        {
            ValidNumberList LegalValueList = FindLegalValues(ParentNode);

            if (null == LegalValueList)
            {
                // ParentNode is either a complete node or a dead end,
                // either way it can now be removed from the tree
                killNode(ParentNode);

                return;
            }

            TreeNode CursorNode = new TreeNode(ParentNode);

            CursorNode.SetAt(LegalValueList.Row,
                             LegalValueList.Col,
                             LegalValueList.Pop());

            TreeNode Head = CursorNode;

            // Loop through the children, skipping the first item (item
            // zero) since we took care of it above.
            while (LegalValueList.Count > 0)
            {
                // Create a new TreeNode for each possible value found
                TreeNode TempNode = new TreeNode(ParentNode);
                TempNode.SetAt(LegalValueList.Row,
                               LegalValueList.Col,
                               LegalValueList.Pop());

                // Add it to the end of the chain
                CursorNode.NextNode = TempNode;
                TempNode.PrevNode = CursorNode;
                CursorNode = TempNode;
            }

            ParentNode.Son = Head;
            Head.Parent = ParentNode;
            TreeNode AnotherNode = new TreeNode(m_order);
            AnotherNode = Head;
            while (null != AnotherNode)
            {
                GenerateTree(AnotherNode);
                AnotherNode = AnotherNode.NextNode;
            }

            // Release nodes to free memory
            Head = null;
            AnotherNode = null;
            ParentNode = null;
        }

        /// <summary>
        ///   This method searches the board for an empty slot and then
        ///   enumerates all of the numbers that can possibly fit in
        ///   that hole.
        /// </summary>
        /// <param name="Node">The current node</param>
        /// <returns>
        ///   An array of TempStruct nodes that contain the Row and
        ///   Column of the empty slot along with the values that can go
        ///   in that empty slot.
        /// </returns>
        /// <remarks></remarks>
        private ValidNumberList FindLegalValues(TreeNode Node)
        {
            int Row = -1;
            int Col = -1;
            // Find the next row and column that still contains a
            // zero (has no value).
            bool foundHole = false;
            for (int i = 0; !foundHole && i < Node.Order; i++)
            {
                for (int j = 0; !foundHole && j < Node.Order; j++)
                {
                    if (Node.Board[i, j] == 0)
                    {
                        Row = i;
                        Col = j;
                        foundHole = true;
                    }
                }
            }

            if (!foundHole)
            {
                // The board is complete, no holes, mark it as complete
                // and push it.
                Node.Complete = true;
                m_listener.putNode(Node);
                return (null);
            }

            // Walk through all of the potential numbers from 1 to
            // m_order that might be in this slot and see if it works.
            // Any that do work are stored in validNumberList.
            Stack validNumberStack = new Stack();
            for (byte nextNum = 1; (nextNum <= m_order); nextNum++)
            {
                bool itFits = true;

                // Is this number already in the current row?
                for (int i = 0; itFits && i <= (Row - 1); i++)
                {
                    if (Node.Board[i, Col] == nextNum)
                    {
                        itFits = false;
                    }
                }

                // Is this number already in the current column?
                for (int j = 0; itFits && j <= (Col - 1); j++)
                {
                    if (Node.Board[Row, j] == nextNum)
                    {
                        itFits = false;
                    }
                }


                if (itFits) // This Number can go in this space
                {
                    validNumberStack.Push(nextNum);
                }
            }

            if (validNumberStack.Count == 0)
                return (null);
            else
                return (new ValidNumberList(Row, Col, validNumberStack));
        }
    }
}

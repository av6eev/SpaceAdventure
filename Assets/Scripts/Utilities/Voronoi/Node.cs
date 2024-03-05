using System.Collections.Generic;

namespace Utilities.Voronoi
{
    public class Node
    {
        public static readonly Stack<Node> Pool = new();
		
        public Node Parent;
        public int TreeSize;
    }
}
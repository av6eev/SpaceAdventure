namespace Utilities.Voronoi
{
    public static class SideHelper
    {
        public static Side Other(Side leftRight)
        {
            return leftRight == Side.Left ? Side.Right : Side.Left;
        }
    }
}
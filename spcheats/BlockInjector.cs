using Nuterra.BlockInjector;
using System.Linq;

namespace SPCheats
{
    internal class BlockInjector
    {
        public static int[] CustomBlocks { get => BlockLoader.CustomBlocks.Keys.ToArray(); }
    }
}
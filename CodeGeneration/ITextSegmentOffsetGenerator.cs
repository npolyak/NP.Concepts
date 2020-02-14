using System.Collections.Generic;
using System.Linq;

namespace NP.Concepts.CodeGeneration
{
    public interface ITextSegmentOffsetGenerator
    {
        string GenerateFullCode();

        string CodeBody { get; set; }

        int CodeBodyStart { get; }
        
        int CodeBodyEnd { get; }
    }

    public static class TextSegmentOffsetGeneratorExtensions
    {
        public static IEnumerable<(int Start, int End)> GetOffsetPairs(this IEnumerable<int> offsets, bool oddOrEven = true)
        {
            bool isOddIteration = true;
            int previousOffset = offsets.First();
            foreach (int offset in offsets.Skip(1))
            {
                if (isOddIteration == oddOrEven)
                {
                    yield return (previousOffset, offset);
                }

                isOddIteration = !isOddIteration;
                previousOffset = offset;
            }
        }
    }
}

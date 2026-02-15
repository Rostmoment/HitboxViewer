using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer.Enums
{
    public static class RoundedHitboxAlgorithmExtensions
    {
        public static readonly RoundedHitboxAlgorithm[] all = (RoundedHitboxAlgorithm[])Enum.GetValues(typeof(RoundedHitboxAlgorithm));
        private static readonly Dictionary<RoundedHitboxAlgorithm, string> descriptions = new Dictionary<RoundedHitboxAlgorithm, string>()
        {
            [RoundedHitboxAlgorithm.LatitudeLongitude] = "Most accurate, but the slowest",
            [RoundedHitboxAlgorithm.Fibonacci] = "Faster, but less accurate",
            [RoundedHitboxAlgorithm.ThreeAxis] = "Just three rings, really fast",
            [RoundedHitboxAlgorithm.TwoAxis] = "Just two rings, the fastest"
        };

        public static string GetDescription(this RoundedHitboxAlgorithm algorithm)
        {
            if (descriptions.TryGetValue(algorithm, out string description))
                return description;
            return "";
        }
    }
    public enum RoundedHitboxAlgorithm
    {
        LatitudeLongitude,
        Fibonacci,
        ThreeAxis,
        TwoAxis
    }
}

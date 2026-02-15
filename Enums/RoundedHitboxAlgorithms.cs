using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer.Enums
{
    public static class RoundedHitboxAlgorithmExtensions
    {
        public static readonly RoundedHitboxAlgorithms[] all = (RoundedHitboxAlgorithms[])Enum.GetValues(typeof(RoundedHitboxAlgorithms));
        private static Dictionary<RoundedHitboxAlgorithms, string> descriptions = new Dictionary<RoundedHitboxAlgorithms, string>()
        {
            [RoundedHitboxAlgorithms.LatitudeLongitude] = "Most accurate, but the slowest",
            [RoundedHitboxAlgorithms.Fibonacci] = "Faster, but less accurate",
            [RoundedHitboxAlgorithms.ThreeAxis] = "Just three rings, really fast",
            [RoundedHitboxAlgorithms.TwoAxis] = "Just two rings, the fastest"
        };

        public static string GetDescription(this RoundedHitboxAlgorithms algorithm)
        {
            if (descriptions.TryGetValue(algorithm, out string description))
                return description;
            return "";
        }
    }
    public enum RoundedHitboxAlgorithms
    {
        LatitudeLongitude,
        Fibonacci,
        ThreeAxis,
        TwoAxis
    }
}

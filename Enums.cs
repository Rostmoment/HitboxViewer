using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer
{
    public enum Plane
    {
        XY,
        XZ,
        YZ
    }
    public enum Quadrant
    {
        First,
        Second,
        Third,
        Fourth
    }
    public enum SphereVisualizationMode
    {
        Full,
        ThreeAxis,
        TwoAxis
    }
    public enum CollidersVisualizationMode
    {
        Hide,
        NotTrigger,
        Trigger,
        All
    }
    public enum NavMeshObstacleVisualizationMode
    {
        Hide,
        Box,
        Capsule,
        All
    }
}

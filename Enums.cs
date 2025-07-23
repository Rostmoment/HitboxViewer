using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer
{
    enum Plane
    {
        XY,
        XZ,
        YZ
    }
    enum Quadrant
    {
        First,
        Second,
        Third,
        Fourth
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

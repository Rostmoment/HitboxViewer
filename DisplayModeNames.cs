using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer
{
    public static class DisplayModeNames
    {
        private static Dictionary<CollidersVisualizationMode, string> colliders = new Dictionary<CollidersVisualizationMode, string>()
        {
            {CollidersVisualizationMode.NotTrigger, "Not Trigger" }
        };
        private static Dictionary<NavMeshObstacleVisualizationMode, string> navMeshes = new Dictionary<NavMeshObstacleVisualizationMode, string>();
        private static Dictionary<UIVisualizationMode, string> ui = new Dictionary<UIVisualizationMode, string>();

        public static string ToName(this CollidersVisualizationMode collider)
        {
            if (colliders.TryGetValue(collider, out string name))
                return name;
            return collider.ToString();
        }
        public static string ToName(this NavMeshObstacleVisualizationMode navMesh)
        {
            if (navMeshes.TryGetValue(navMesh, out string name))
                return name;
            return navMesh.ToString();
        }
        public static string ToName(this UIVisualizationMode uiMode
            )
        {
            if (ui.TryGetValue(uiMode, out string name))
                return name;
            return uiMode.ToString();
        }
    }
}

using HitboxViewer.Configs;
using HitboxViewer.Displayers;
using HitboxViewer.Displayers.Colliders;
using HitboxViewer.Displayers.Colliders2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer
{
    public class HitboxUpdater : MonoBehaviour
    {
        public static Action OnUpdate;

        private static Dictionary<Type, Type> colliders = new Dictionary<Type, Type>()
        {
            [typeof(BoxCollider)] = typeof(BoxColliderDisplayer),
            [typeof(SphereCollider)] = typeof(SphereColliderDisplayer),
            [typeof(CapsuleCollider)] = typeof(CapsuleColliderDisplayer),
            [typeof(MeshCollider)] = typeof(MeshColliderDisplayer),
            [typeof(CharacterController)] = typeof(CharacterControllerDisplayer) 
        };

        private static Dictionary<Type, Type> colliders2d = new Dictionary<Type, Type>()
        {
            [typeof(BoxCollider2D)] = typeof(BoxCollider2DDisplayer),
            [typeof(CircleCollider2D)] = typeof(CircleCollider2DDisplayer)
        };

        private int updateCounter;

        private void UpdateColliders()
        {
            foreach (Collider collider in GameObject.FindObjectsOfType<Collider>())
            {
                if (colliders.TryGetValue(collider.GetType(), out Type displayerType))
                {
                    BaseDisplayer displayer = BaseDisplayer.GetOrAdd(collider, displayerType);
                    displayer.Visualize();
                }
            }
        }
        private void UpdateColliders2D()
        {
            foreach (Collider2D collider in GameObject.FindObjectsOfType<Collider2D>())
            {
                if (colliders2d.TryGetValue(collider.GetType(), out Type displayerType))
                {
                    BaseDisplayer displayer = BaseDisplayer.GetOrAdd(collider, displayerType);
                    displayer.Visualize();
                }
            }
        }

        private void UpdateNavMeshObstacles()
        {
            foreach (NavMeshObstacle nav in GameObject.FindObjectsOfType<NavMeshObstacle>())
            {
                NavMeshObstacleDisplayer displayer = BaseDisplayer.GetOrAdd<NavMeshObstacleDisplayer>(nav);
                displayer.Visualize();
            }
        }

        private void Update()
        {
            if (updateCounter < 0)
                return;

            if (updateCounter > 0)
            {
                updateCounter--;
                return;
            }

            updateCounter = HitboxViewerConfig.UpdateRate;

            UpdateColliders();
            UpdateColliders2D();
            UpdateNavMeshObstacles();

            OnUpdate?.Invoke();
        }
    }
}

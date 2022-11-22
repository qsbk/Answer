#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
namespace UniversalFramework {
    /// <summary>
    /// Uses Unity's LineRenderer component to render paths.
    /// <summary>
    [RequireComponent(typeof(WayPath))]
    public class WayPathRenderer : MonoBehaviour {
        /// <summary>
        /// Spacing between LineRenderer positions on the path.
        /// <summary>
        public float spacing = 0.05f;
        [SerializeField] Color SelectedColor = Color.yellow;
        [SerializeField] Color LineColor = new Color(255, 255, 0, 0.5f);
        WayPath path;

        //updates LineRenderer positions
        void OnDrawGizmosSelected() {
            Gizmos.color = SelectedColor;
            DrawPath();

        }
        void OnDrawGizmos() {
            Gizmos.color = LineColor;
            DrawPath();
        }

        void DrawPath() {
            path = GetComponent<WayPath>();
            if (path == null) return;

            //set initial size based on waypoint count
            if (path.PointCount > 0) {
                //draw line
                Vector3 lastpos = path.GetPoint(0);
                for (int i = 1; i < path.PointCount; i++) {
                    Vector3 point = path.GetPoint(i);
                    Gizmos.DrawLine(lastpos, point);
                    lastpos = point;
                }

                //draw anchor
                for (int i = 0; i < path.anchors.Count; i++) {
                    Gizmos.DrawWireSphere(path.anchors[i], 1);
                }
            }
        }
    }
}
#endif
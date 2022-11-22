using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UniversalFramework
{
    public class WaypathTracker : MonoBehaviour {
        // This script can be used with any object that is supposed to follow a
        // route marked out by waypoints.

        public WayPath path; // A reference to the waypoint-based route we should follow

        // If loop track
        public Transform target;
        public float targetStep = 10.0f;
        public float trackDist = 10.0f;
        public bool isLooping = true;

        public int curPathIndex { get; private set; } // The progress round the route, used in smooth mode.
        private int pathLength = 0;//total index count

        // setup script properties
        private void Start() {
            // the point to aim position on curve
            if (target == null) {
                target = new GameObject(name + " Waypoint Target").transform;
            }

            // init relations of path
            SwitchPath(path);
            // interate
            Reset();
        }
        private void OnDestroy() {
            if (path != null) path.PathChanged -= Reset;
            path = null;
        }

        // reset the object to sensible values
        public void Reset() {
            if (path == null) return;
            pathLength = path.PointCount;
            curPathIndex = path.GetNearistIndex(transform.position);
            FixedUpdate();
        }

        /// <summary>
        /// switch to another followed path
        /// </summary>
        /// <param name="newpath"></param>
        public void SwitchPath(WayPath newpath) {
            if (path == newpath) return;
            if (path != null) path.PathChanged -= Reset;

            path = newpath;

            if (path == null) return;
            path.PathChanged += Reset;
            Reset();
        }

        private void FixedUpdate() {
            //if change target
            if (path == null) return;

            //Get target point
            target.position = path.GetPoint(curPathIndex);

            //If reach the radius within the path then move to next point in the path  
            if (Vector3.Distance(transform.position, target.position) < trackDist) {
                //step move next track index
                Vector3 checkpos = target.position;
                while (Vector3.Distance(target.position, checkpos) < targetStep) {
                    curPathIndex += 1;

                    // determine the position we should currently be aiming for
                    if (curPathIndex >= pathLength)//if reached end point
                    {
                        if (isLooping) {
                            curPathIndex = 0;//return index to path head
                        } else {
                            curPathIndex = pathLength - 1;
                            return;//Don't move the vehicle if path is finished 
                        }
                    }

                    checkpos = path.GetPoint(curPathIndex);
                }
            }
        }


        private void OnDrawGizmos() {
            if (Application.isPlaying) {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, target.position);
                Gizmos.DrawWireSphere(target.position, 1);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(target.position, target.position + target.forward);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;


namespace UniversalFramework {
    /// <summary>
    /// Â·¾¶
    /// </summary>
    public class WayPath : MonoBehaviour {
        public List<Vector3> anchors = new List<Vector3>();
        Vector3[] curveAnchors;
        public int PointCount;

        public Action PathChanged;//path curve changed

        /// <summary>
        /// max anchor count
        /// </summary>
        public int MaxAnchor = 10;
        /// <summary>
        /// smoothy count between anchors
        /// </summary>
        public int SmoothyBetweenPoints = 20;

        [ContextMenu("WayPointsFromChildren")]
        public void WayPointsFromChildren() {
            anchors.Clear();
            for (int i = 0; i < transform.childCount; i++)
                anchors.Add(transform.GetChild(i).position);

            FixedCurve();
        }

        [ContextMenu("WayPointsFromMesh")]
        public void WayPointsFromMesh() {
            MeshFilter meshfilter = GetComponent<MeshFilter>();
            if (meshfilter == null) return;
            Mesh mesh = meshfilter.sharedMesh;
            if (mesh == null) return;

            anchors.Clear();
            anchors.AddRange(mesh.vertices);

            FixedCurve();
        }

        /// <summary>
        /// add anchor at tail of curve
        /// </summary>
        /// <param name="points"></param>
        public void AddPoints(params Vector3[] points) {
            anchors.AddRange(points);

            FixedCurve();
        }

        /// <summary>
        /// clear anchors and reset them by new points.
        /// </summary>
        /// <param name="points"></param>
        public void Reset(params Vector3[] points) {
            anchors.Clear();
            anchors.AddRange(points);

            FixedCurve();
        }

        /// <summary>
        /// clear anchors and curve
        /// </summary>
        public void Clear() {
            anchors.Clear();

            FixedCurve();
        }

        /// <summary>
        /// Get point on curve by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetPoint(int index) {
            if (index < 0) index = 0;
            //empty
            if (anchors.Count < 1) {
                return Vector3.zero;
            }
            //line
            else if (anchors.Count < 3) {
                index %= anchors.Count;
                return anchors[index];
            }
            //points >= 3, can smooth to curve
            else {
                int totalCount = anchors.Count * SmoothyBetweenPoints;
                index %= totalCount;

                if (curveAnchors == null) FixedCurve();

                return Lerp(curveAnchors, 1.0f * index / totalCount);
            }
        }

        /// <summary>
        /// Get nearist point on curve.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int GetNearistIndex(Vector3 point) {
            //empty
            if (anchors.Count < 1) {
                return 0;
            }
            //line
            else if (anchors.Count < 3) {
                float dis = Mathf.Infinity;
                int index = 0;
                for (int i = 0; i < anchors.Count; i++) {
                    float dist = Vector3.Distance(point, anchors[i]);
                    if (dist < dis) {
                        dis = dist;
                        index = i;
                    }
                }
                return index;
            }
            //points > 2, can smooth to curve
            else {
                float dis = Mathf.Infinity;
                int index = 0;

                if (curveAnchors == null) FixedCurve();

                for (int i = 0; i < PointCount; i++) {
                    Vector3 tmppoint = Lerp(curveAnchors, 1.0f * i / PointCount);
                    float dist = Vector3.Distance(point, tmppoint);
                    if (dist < dis) {
                        dis = dist;
                        index = i;
                    }
                }
                return index;
            }
        }

        /// <summary>
        /// Build Beizer Curve by new anchors.
        /// </summary>
        private void FixedCurve() {
            //fix anchor range
            if (anchors.Count > MaxAnchor) {
                int range = anchors.Count - MaxAnchor;
                anchors.RemoveRange(0, range);
            }

            PointCount = anchors.Count > 2 ? anchors.Count * SmoothyBetweenPoints : anchors.Count;

            if (anchors.Count > 1) {
                curveAnchors = new Vector3[anchors.Count + 2];
                //Extension points
                curveAnchors[0] = anchors[0] + anchors[0] - anchors[1];
                curveAnchors[anchors.Count + 1] = anchors[anchors.Count - 1] + anchors[anchors.Count - 1] - anchors[anchors.Count - 2];

                for (int i = 0; i < anchors.Count; i++) {
                    curveAnchors[i + 1] = anchors[i];
                }
            }

            if (PathChanged != null) PathChanged.Invoke();
        }

        /// <summary>
        /// Gets the point on the curve at a given percentage (0-1). Taken and modified from HOTween.
        /// <summary>
        //http://code.google.com/p/hotween/source/browse/trunk/Holoville/HOTween/Core/Path.cs
        private static Vector3 Lerp(Vector3[] pts, float t) {
            int numSections = pts.Length - 3;
            int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
            float u = t * (float)numSections - (float)currPt;

            Vector3 p0 = pts[currPt];
            Vector3 p1 = pts[currPt + 1];
            Vector3 p2 = pts[currPt + 2];
            Vector3 p3 = pts[currPt + 3];

            return 0.5f * (
                (-p0 + 3f * p1 - 3f * p2 + p3) * (u * u * u)
                + (2f * p0 - 5f * p1 + 4f * p2 - p3) * (u * u)
                + (-p0 + p2) * u
                + 2f * p1
                );
        }
    }
}
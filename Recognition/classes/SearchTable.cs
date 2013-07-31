using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Recognition
{
    /// <summary>
    /// Table for saving points and to increase their frequencies
    /// </summary>
    public class SearchTable
    {
        private List<PointW> searchTable;

        // Maximal frequency a point can get
        private int highboundFrequency = 10;

        private PointW mostCommon;
        private int mostCommonFrequency = 0;

        public SearchTable(int highboundFrequency)
        {
            this.highboundFrequency = highboundFrequency;
            searchTable = new List<PointW>();
        }

        public void addPoint(PointF p)
        {
            searchTable.Add(new PointW(p));
        }

        /// <summary>
        /// Search for the closest point of a given point p
        ///     - If the closest point is close enough ( smaller then maxDistanceBetweenPoints)
        ///         - Increase it's frequency
        ///     - else add it to the list
        /// </summary>
        /// <param name="p"></param>
        /// <param name="maxDistanceBetweenPoints"></param>
        /// <returns></returns>
        public PointW searchPoint(PointW p, int maxDistanceBetweenPoints)
        {
            // Look for closest point
            PointW nearest = null;
            int distance = 999999999;
            for (int i = 0; i < searchTable.Count; i++)
            {
                int dist = p.euclidianDistance(searchTable[i]);
                if (distance > dist)
                {
                    distance = dist;
                    nearest = searchTable[i];
                }
            }
            // If closest point is located, increase it's frequency
            // else add the point to list 
            if ((object)nearest != null)
            {
                if (p.closeEnough(nearest, maxDistanceBetweenPoints))
                {
                    if (nearest.frequency < highboundFrequency)
                        nearest.frequency++;
                    return nearest;
                }
                else
                    addPoint(p.p);
            }
            // If list is empty 
            else
                addPoint(p.p);
            return p;
        }

        /// <summary>
        /// Find the nearest point 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="maxDistanceBetweenPoints"></param>
        /// <returns></returns>
        public PointW findMostCommonPoint(PointW p, int maxDistanceBetweenPoints)
        {
            PointW nearest = searchPoint(p, maxDistanceBetweenPoints);
            // If nearest point has a larger frequency than
            // the mostcommon point => ( mostCommon = nearest )
            if (mostCommonFrequency <= nearest.frequency)
            {
                mostCommon = nearest;
                mostCommonFrequency = nearest.frequency;
            }
            return mostCommon;
        }
    }
}

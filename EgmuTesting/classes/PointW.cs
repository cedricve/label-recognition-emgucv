using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EgmuTesting
{
    // Wrapper klasse voor PointF
    // Nieuw datatype: frequentie
    // Operators overloading ==, !=
    // Methodes: kwadratische afstand tussen 2 punten
    public class PointW
    {
        public int frequency;
        public PointF p;
        public PointW()
        {
            new PointW(new PointF(0, 0));
        }
        public PointW(PointF p)
        {
            this.p = p;
            this.frequency = 0;
        }
        public void increase()
        {
            frequency++;
        }
        public static bool operator ==(PointW p1, PointW p2)
        {
            if ((object)p1 == null || (object)p2 == null) return false;
            return (p1.p.X == p2.p.X) && (p1.p.Y == p2.p.Y);

        }
        public static bool operator !=(PointW p1, PointW p2)
        {
            return !(p1 == p2);
        }

        public bool closeEnough(PointW p, int distance)
        {
            return euclidianDistance(p) < distance;
        }

        public int euclidianDistance(PointW p1)
        {
            return Convert.ToInt32(Math.Round((this.p.X - p1.p.X) * (this.p.X - p1.p.X) +
                                                  (this.p.Y - p1.p.Y) * (this.p.Y - p1.p.Y)));
        }
    }
}

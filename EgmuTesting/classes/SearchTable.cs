using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EgmuTesting
{
    // Structuur voor punten te zoeken
    //      - structuur houdt punt bij met hoogste waarschijnlijke positie
    //        voor het label
    // Methodes:
    //      - zoeken van dichtstbijzijnde punt: Als punt gevonden is incrementeren frequentie
    //        of zelfs toevoegen
    public class SearchTable
    {
        private List<PointW> searchTable;
        // Maximale frequency dat een punt kan krijgen
        // Nodig opdat een punt niet te veel stemmen kan krijgen
        private int highbound_frequency;

        // Tijdelijk beste punt
        private PointW mostcommon;
        private int mostcommon_frequency = 0;

        public SearchTable(int highbound_frequency = 10)
        {
            this.highbound_frequency = highbound_frequency;
            searchTable = new List<PointW>();
        }

        public void addPoint(PointF p)
        {
            searchTable.Add(new PointW(p));
        }

        // Zoek operatie: zoek het punt met kleinste kwadratische afstand
        // in de lijst met punten
        public PointW searchPoint(PointW p, int max_distance_between_points)
        {
            // Opzoek gaan naar dichtse punt
            // in de lijst
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

            // Dichtste gevonden of lijst leeg (nearest=null)
            // als punt dicht genoeg ligt (< rect_distance), dan beschouwen we als gelijkaardig
            // we verhogen frequentie met 1
            // anders voegen we het nieuwe punt toe
            if ((object)nearest != null)
            {
                //Console.WriteLine("size list: " + searchTable.Count + " " + nearest.p.X + "," + nearest.p.Y + " dist: " + nearest.euclidianDistance(p));

                // Als dicht genoeg bij elkaar liggen, incrementeren we
                // frequentie van het dichtste punt
                if (p.closeEnough(nearest, max_distance_between_points))
                {
                    if (nearest.frequency < highbound_frequency)
                        nearest.frequency++;
                }
                // anders is dit een nieuwe positie voor het label
                else
                    addPoint(p.p);
            }

            // list leeg 
            // out of bounds
            // punten zijn gelijk
            return nearest;
        }

        // 
        public PointW findNearest(PointW p, int max_distance_between_points)
        {
            // Zoek het dichtste punt op
            PointW nearest = searchPoint(p, max_distance_between_points);

            // Als er een dichtste punt is
            if ((object)nearest != null)
            {
                // Als frequentie van meest voorkomende kleiner is
                // dan dichtsbijzijnde punten wordt dit ons nieuwe
                // meest voorkomende punt
                if (mostcommon_frequency <= nearest.frequency)
                {
                    mostcommon = nearest;
                    mostcommon_frequency = nearest.frequency;
                }
                return mostcommon;
            }
            // Geen dichtste punt => de lijst is leeg dus sowieso toevoegen
            else
            {
                addPoint(p.p);
                return p;
            }
        }
    }
}

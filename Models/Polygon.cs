using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trading.Models
{

    public class PolygonList : BaseDataObject
    {
        private List<Polygon> _polygonList;

        public PolygonList(int ShopID)
        {
            _polygonList =
                DB.MapSectors.Where(x => x.ShopID == ShopID && x.MapPoints.Count > 2)
                    .Select(x => new Polygon(x))
                    .ToList();
        }

        public List<Car> GetDefaultCars(Order order)
        {
            if (order.OrderDelivery.MapPoint != null)
            {
                var car =
                    _polygonList.Where(x => x.Sector.Type == (int)SectorTypes.CarSector)
                        .FirstOrDefault(x => x.IsIn(order.OrderDelivery.MapPoint));
                if (car != null)
                {
                    return DB.MapSectorCars.Where(x=> x.SectorID == car.SectorID).Select(x=> x.Car).ToList();
                }
            }
            return new List<Car>();
        }
        public List<Worker> GetDefaultCouriers(Order order)
        {
            if (order.OrderDelivery.MapPoint != null)
            {
                var car =
                    _polygonList.Where(x => x.Sector.Type == (int)SectorTypes.CourierSector)
                        .FirstOrDefault(x => x.IsIn(order.OrderDelivery.MapPoint));
                if (car != null)
                {
                    return DB.MapSectorCouriers.Where(x => x.SectorID == car.SectorID).Select(x => x.Worker).ToList();
                }
            }
            return new List<Worker>();
        }
    }

    public class Polygon
    {
        public int SectorID { get; set; }
        public MapSector Sector { get; set; }

        public Polygon(MapSector sector)
        {
            SectorID = sector.ID;
            Sector = sector;
            polyCorners = sector.MapPoints.Count - 1;
            polyX = sector.MapPoints.Where(x => x.Num > 0).OrderBy(x => x.Num).Select(x => x.Lat).ToArray();
            polyY = sector.MapPoints.Where(x => x.Num > 0).OrderBy(x => x.Num).Select(x => x.Lng).ToArray();
            constant = new decimal[polyCorners];
            multiple = new decimal[polyCorners];
            precalc_values();
        }

        public bool IsIn(MapPoint address)
        {
            x = address.Lat;
            y = address.Lng;

            return pointInPolygon();
        }

        private int polyCorners;
        private decimal[] polyX;
        private decimal[] polyY;
        private decimal x, y;
        private decimal[] constant;
        private decimal[] multiple;
        //  Globals which should be set before calling these functions:
        //
        //  int    polyCorners  =  how many corners the polygon has (no repeats)
        //  float  polyX[]      =  horizontal coordinates of corners
        //  float  polyY[]      =  vertical coordinates of corners
        //  float  x, y         =  point to be tested
        //
        //  The following global arrays should be allocated before calling these functions:
        //
        //  float  constant[] = storage for precalculated constants (same size as polyX)
        //  float  multiple[] = storage for precalculated multipliers (same size as polyX)
        //
        //  (Globals are used in this example for purposes of speed.  Change as
        //  desired.)
        //
        //  USAGE:
        //  Call precalc_values() to initialize the constant[] and multiple[] arrays,
        //  then call pointInPolygon(x, y) to determine if the point is in the polygon.
        //
        //  The function will return YES if the point x,y is inside the polygon, or
        //  NO if it is not.  If the point is exactly on the edge of the polygon,
        //  then the function may return YES or NO.
        //
        //  Note that division by zero is avoided because the division is protected
        //  by the "if" clause which surrounds it.

        private void precalc_values()
        {

            int i, j = polyCorners - 1;

            for (i = 0; i < polyCorners; i++)
            {
                if (polyY[j] == polyY[i])
                {
                    constant[i] = polyX[i];
                    multiple[i] = 0;
                }
                else
                {
                    constant[i] = polyX[i] - (polyY[i] * polyX[j]) / (polyY[j] - polyY[i]) + (polyY[i] * polyX[i]) / (polyY[j] - polyY[i]);
                    multiple[i] = (polyX[j] - polyX[i]) / (polyY[j] - polyY[i]);
                }
                j = i;
            }
        }

        private bool pointInPolygon()
        {

            int i, j = polyCorners - 1;
            bool oddNodes = false;

            for (i = 0; i < polyCorners; i++)
            {
                if ((polyY[i] < y && polyY[j] >= y
                || polyY[j] < y && polyY[i] >= y))
                {
                    oddNodes ^= (y * multiple[i] + constant[i] < x);
                }
                j = i;
            }

            return oddNodes;
        }
    }
}
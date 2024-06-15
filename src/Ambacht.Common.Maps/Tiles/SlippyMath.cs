using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Maps.Projections;
using Ambacht.Common.Mathmatics;

namespace Ambacht.Common.Maps.Tiles
{
    public static class SlippyMath
    {

        private static double LongitudeToTileX(double lon, double zoom)
        {
            return (lon + 180.0f) / 360.0f * Math.Pow(2, zoom);
        }

        private static double LatitudeToTileY(double lat, double zoom)
        {
            return (1 - Math.Log(Math.Tan(ToRadians(lat)) + 1 / Math.Cos(ToRadians(lat))) / Math.PI) / 2 * Math.Pow(2, zoom);
        }

        private static double TileXToLongitude(double x, double zoom)
        {
            return x / Math.Pow(2, zoom) * 360.0f - 180f;
        }

        private static double TileYToLatitude(double y, double zoom)
        {
            double n = Math.PI - 2.0 * Math.PI * y / Math.Pow(2, zoom);
            return 180.0 / Math.PI * Math.Atan(0.5 * (Math.Exp(n) - Math.Exp(-n)));
        }

        private static double ToRadians(double degrees)
        {
            return MathF.PI * degrees / 180.0;
        }


        /// <summary>
        /// Converts the lat/lng coordinates to tile coordinates.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static Vector2<double> LatLngToTile(LatLng coords, double zoom)
        {
            return new Vector2<double>(
              LongitudeToTileX(coords.Longitude, zoom),
              LatitudeToTileY(coords.Latitude, zoom));
        }

        /// <summary>
        /// Converts 
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static LatLng TileToLatLng(Vector2<double> xy, double zoom)
        {
            return new LatLng(TileYToLatitude(xy.Y, zoom), TileXToLongitude(xy.X, zoom));
        }

        /// <summary>
        /// Converts the lat/lng coordinates to tile coordinates.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static Vector2<double> LatLngToPixel(LatLng coords, double zoom, int tileSize)
        {
            return LatLngToTile(coords, zoom) * tileSize;
        }

        /// <summary>
        /// Converts 
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static LatLng PixelToLatLng(Vector2<double> xy, double zoom, int tileSize)
        {
            return TileToLatLng(xy / tileSize, zoom);
        }


        public static double MetersPerTile(LatLng center, double zoom)
        {
          var tiles = Math.Pow(2, zoom);
          return Earth.Circumference * Math.Cos(MathUtil.DegreesToRadians(center.Latitude)) / tiles;
        }

        public static double MetersPerPixel(LatLng center, double zoom, int tileSize) => MetersPerTile(center, zoom) / tileSize;

    }
}

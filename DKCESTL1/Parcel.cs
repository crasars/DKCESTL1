using System;

namespace DKCESTL1
{
    public class Parcel
    {
        public ParcelType parcelType { get; set; }

        public City startCity { get; set; }

        public City endCity { get; set; }

        public Boolean signed { get; set; }

        public int weight { get; set; }
    }
}
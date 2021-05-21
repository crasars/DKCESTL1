using System;

namespace DKCESTL1
{
    public class Parcel
    {
        public City startCity { get; set; }

        public City endCity { get; set; }

        public ParcelType parcelType { get; set; }

        public int weight { get; set; }

        public Boolean route { get; set; }

        public Boolean signed { get; set; }

    }
}
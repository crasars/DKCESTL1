using System;

namespace DKCESTL1
{
    public class Edge
    {
        public City fromCity { get; set; }
        public City toCity { get; set; }
        public int price { get; set; }
        public int time { get; set; }
        public int node { get; set; }
        public Vehicle vehicle { get; set; }

        public Boolean available { get; set; }

        public Edge(City fromCity, City toCity, int price, int time, int node, Vehicle vehicle, Boolean available)
        {
            this.fromCity = fromCity;
            this.toCity = toCity;
            this.price = price;
            this.time = time;
            this.node = node;
            this.vehicle = vehicle;
            this.available = available;
        }
    }
}
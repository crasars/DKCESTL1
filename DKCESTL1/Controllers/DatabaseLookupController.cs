using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using String = System.String;

namespace DKCESTL1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseLookupController : ControllerBase
    {
        public DatabaseLookupController()
        {

        }

        [HttpGet("getPricingData")]
        public ActionResult<List<String[]>> queryForPricingData()
        {
            string connectionString = @"Data Source=dbs-tl-dk1.database.windows.net;Initial Catalog=db-tl-dk1;User ID=admin-tl-dk1;Password=telStarRox16";

            SqlConnection cnn = new SqlConnection(connectionString);

            string query = "SELECT * FROM dbo.price_data";

            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataReader reader;

            command.Connection.Open();
            reader = command.ExecuteReader();

            List<String[]> output = new List<string[]>();

            while (reader.Read())
            {
                String[] currentRow = new string[]
                    {reader.GetValue(2).ToString(), reader.GetValue(0).ToString(), reader.GetValue(1).ToString()};

                output.Add(currentRow);
            }

            cnn.Close();
            

            return output;
        }
        

        public ActionResult<List<Edge>> queryForMap()
        {
            string connectionString = @"Data Source=dbs-tl-dk1.database.windows.net;Initial Catalog=db-tl-dk1;User ID=admin-tl-dk1;Password=telStarRox16";

            SqlConnection cnn = new SqlConnection(connectionString);

            string query = "SELECT * FROM dbo.map";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataReader reader;

            command.Connection.Open();
            reader = command.ExecuteReader();

            List<Edge> output = new List<Edge>();

            int i = 0;
            while (reader.Read())
            {
                i++;
                City fromCity = new City(reader.GetValue(0).ToString());
                City toCity = new City(reader.GetValue(1).ToString());

                Vehicle vehicle;

                if (reader.GetValue(3).Equals("T"))
                {
                    vehicle = Vehicle.Truck;
                }
                else if (reader.GetValue(3).Equals("S"))
                {
                    vehicle = Vehicle.Ship;
                }
                else if (reader.GetValue(3).Equals("A"))
                {
                    vehicle = Vehicle.Airplane;
                }
                else
                {
                    throw new Exception("Vehicle not specified in database for row number: " + i);
                }

                int node;

                // Workaround for the segmentinformationcontroller to be able to keep using its method for signifying unavailable edge
                if (!(Boolean) reader.GetValue(4))
                {
                    node = 0;
                }
                else
                {
                    node = (int) reader.GetValue(2);
                }

                Boolean available = (Boolean)reader.GetValue(4);

                Edge currentEdge = new Edge(fromCity, toCity, 0, node * 4, node, vehicle, available);

                output.Add(currentEdge);
            }

            cnn.Close();

            return output;
        }

        [HttpGet("getCompleteMap")]
        public ActionResult<List<Edge>> queryForCompleteMap()
        {
            string connectionString = @"Data Source=dbs-tl-dk1.database.windows.net;Initial Catalog=db-tl-dk1;User ID=admin-tl-dk1;Password=telStarRox16";

            SqlConnection cnn = new SqlConnection(connectionString);

            string query = "SELECT * FROM dbo.map";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataReader reader;

            command.Connection.Open();
            reader = command.ExecuteReader();

            List<Edge> output = new List<Edge>();

            int i = 0;
            while (reader.Read())
            {
                i++;
                City fromCity = new City(reader.GetValue(0).ToString());
                City toCity = new City(reader.GetValue(1).ToString());

                Vehicle vehicle;

                if (reader.GetValue(3).Equals("T"))
                {
                    vehicle = Vehicle.Truck;
                }
                else if (reader.GetValue(3).Equals("S"))
                {
                    vehicle = Vehicle.Ship;
                }
                else if (reader.GetValue(3).Equals("A"))
                {
                    vehicle = Vehicle.Airplane;
                }
                else
                {
                    throw new Exception("Vehicle not specified in database for row number: " + i);
                }

                int node = (int) reader.GetValue(2);



                Boolean available = (Boolean) reader.GetValue(4);


                Edge currentEdge = new Edge(fromCity, toCity, 0, node * 4, node, vehicle,available);

                output.Add(currentEdge);
            }

            cnn.Close();

            return output;
        }

        [HttpGet("getCityIdMapping")]
        public ActionResult<List<String[]>> queryForCityIdMapping()
        {

            string connectionString = @"Data Source=dbs-tl-dk1.database.windows.net;Initial Catalog=db-tl-dk1;User ID=admin-tl-dk1;Password=telStarRox16";

            SqlConnection cnn = new SqlConnection(connectionString);

            string query = "SELECT * FROM dbo.cityIdMapping";
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataReader reader;

            command.Connection.Open();
            reader = command.ExecuteReader();

            List<string[]> output = new List<string[]>();

            int i = 0;
            while (reader.Read())
            {
                string[] cityIdCorrelation = new string[] {reader.GetValue(0).ToString(), reader.GetValue(1).ToString()};

                output.Add(cityIdCorrelation);
            }

            cnn.Close();

            return output;
        }


        public void logParcelData(Parcel parcel)
        {
            string connectionString = @"Data Source=dbs-tl-dk1.database.windows.net;Initial Catalog=db-tl-dk1;User ID=admin-tl-dk1;Password=telStarRox16";

            SqlConnection cnn = new SqlConnection(connectionString);

            int signed;

            if (parcel.signed)
            {
                signed = 1;
            }
            else
            {
                signed = 0;
            }

            string query = "INSERT INTO dbo.recordedParcelData " +
                           "(fromCity, " +
                           "toCity, " +
                           "parcelType, " +
                           "signed, " +
                           "weight) " +
                           "VALUES " +
                           "('" + parcel.startCity.name + "','" + parcel.endCity.name + "','" + parcel.parcelType + "'," + signed + "," + parcel.weight + ");";


            SqlCommand command = new SqlCommand(query, cnn);

            command.Connection.Open();
            int rowsAdded = command.ExecuteNonQuery();



            cnn.Close();

        }

        [HttpGet("disableCity/{city}")]
        public string disableCity(string city)
        {
            string connectionString = @"Data Source=dbs-tl-dk1.database.windows.net;Initial Catalog=db-tl-dk1;User ID=admin-tl-dk1;Password=telStarRox16";
            
            SqlConnection cnn = new SqlConnection(connectionString);


            string query = "UPDATE dbo.completeMap SET available = 0 WHERE city1 LIKE '" + city + "' OR city2 LIKE '" + city + "';";



            SqlCommand command = new SqlCommand(query, cnn);

            command.Connection.Open();

            int rowsAdded = command.ExecuteNonQuery();

            cnn.Close();


            string queryLocalTable = "UPDATE dbo.map SET available = 0 WHERE city1 LIKE '" + city + "' OR city2 LIKE '" + city + "';";

            SqlCommand commandLocalTable = new SqlCommand(queryLocalTable, cnn);

            commandLocalTable.Connection.Open();

            int rowsAddedSecondTime = commandLocalTable.ExecuteNonQuery();

            cnn.Close();

            return "Disabled " + city;
        }


        [HttpGet("enableCity/{city}")]
        public string enableCity(string city)
        {
            string connectionString = @"Data Source=dbs-tl-dk1.database.windows.net;Initial Catalog=db-tl-dk1;User ID=admin-tl-dk1;Password=telStarRox16";

            SqlConnection cnn = new SqlConnection(connectionString);


            string query = "UPDATE dbo.completeMap SET available = 1 WHERE city1 LIKE '" + city + "' OR city2 LIKE '" + city + "';";



            SqlCommand command = new SqlCommand(query, cnn);

            command.Connection.Open();

            int rowsAdded = command.ExecuteNonQuery();

            cnn.Close();


            string queryLocalTable = "UPDATE dbo.map SET available = 1 WHERE city1 LIKE '" + city + "' OR city2 LIKE '" + city + "';";

            SqlCommand commandLocalTable = new SqlCommand(queryLocalTable, cnn);

            commandLocalTable.Connection.Open();

            int rowsAddedSecondTime = commandLocalTable.ExecuteNonQuery();


            cnn.Close();

            return "Enabled " + city;
        }


        [HttpGet("updatePrice/{parcelType}/{price}")]
        public string updatePrice(string parcelType, string price)
        {
            string connectionString = @"Data Source=dbs-tl-dk1.database.windows.net;Initial Catalog=db-tl-dk1;User ID=admin-tl-dk1;Password=telStarRox16";

            SqlConnection cnn = new SqlConnection(connectionString);

            string query = "UPDATE dbo.price_data SET rate = " + price + " WHERE parcelType LIKE '" + parcelType + "';";


            SqlCommand command = new SqlCommand(query, cnn);

            command.Connection.Open();

            int rowsAdded = command.ExecuteNonQuery();

            cnn.Close();

            return "Updated price of " + parcelType;
        }

    }
}
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;

namespace BiancasBikeShop.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection("server=localhost\\SQLExpress;database=BiancasBikeShop;integrated security=true;TrustServerCertificate=true");
            }
        }

        public List<Bike> GetAllBikes()
        {
            var bikes = new List<Bike>();
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                            b.Id as BikeId, b.Brand, b.Color, b.OwnerId, b.BikeTypeId,
                                            o. Id as OwnerId, o.Name as OwnerName, o.Address, o.Email, o.Telephone,
                                            bt.Id as BikeTypeId, bt.Name as BikeTypeName
                                        FROM Bike b
                                        LEFT JOIN Owner o ON b.OwnerId = o.Id
                                        LEFT JOIN BikeType bt ON b.BikeTypeId = bt.Id";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bike bike = new Bike()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BikeId")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                Owner = new Owner()
                                { 
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Telephone = reader.GetString(reader.GetOrdinal("Telephone")),
                                },
                                    BikeType = new BikeType()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                        Name = reader.GetString(reader.GetOrdinal("BikeTypeName")),
                                    }
                            };
                            bikes.Add(bike);
                        }
                        
                    }
                }
            }
            return bikes;
        }

        public Bike GetBikeById(int id)
        {
            Bike bike = null;
            //implement code here...
            return bike;
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            // implement code here... 
            return count;
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;
using BiancasBikeShop.Utils;

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

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                            b.Id as BikeId, b.Brand, b.Color, b.OwnerId, b.BikeTypeId,
                                            o. Id as OwnerId, o.Name as OwnerName, o.Address, o.Email, o.Telephone,
                                            bt.Id as BikeTypeId, bt.Name as BikeTypeName,
                                            wo.Id as WorkOrderId, wo.DateInitiated, wo.Description, wo.DateCompleted, wo.BikeId
                                        FROM Bike b
                                        LEFT JOIN Owner o ON b.OwnerId = o.Id
                                        LEFT JOIN BikeType bt ON b.BikeTypeId = bt.Id
                                        LEFT JOIN WorkOrder wo ON b.Id = wo.BikeId
                                        WHERE b.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            bike = new Bike()
                            {
                                Id = id,
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
                                },
                                WorkOrders = new List<WorkOrder>()
                            };
                            
                            while(reader.Read())
                            {
                                if (DbUtils.IsNotDbNull(reader, "WorkOrderId"))
                                {
                                    WorkOrder workOrder = new WorkOrder()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("WorkOrderId")),
                                        DateInitiated = reader.GetDateTime(reader.GetOrdinal("DateInitiated")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        DateCompleted = reader.GetDateTime(reader.GetOrdinal("DateCompleted"))
                                    };
                                    bike.WorkOrders.Add(workOrder);
                                }
                            }
                          

                            return bike;
                        }
                        else
                        {
                            return null;
                        }
                        
                    }

                }
            }
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            // implement code here... 
            return count;
        }
    }
}

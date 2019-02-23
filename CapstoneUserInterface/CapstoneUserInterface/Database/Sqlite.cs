using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

// To install System.Data.SQLite: Install-Package System.Data.SQLite -Version 1.0.109.2

namespace SQLiteDatabase
{
    class Sqlite
    {
        private readonly SQLiteConnection dbConnection;

        public Sqlite(string connectionString)
        {
            dbConnection = new SQLiteConnection(connectionString);
        }

        public List<Image> GetAllImages()
        {
            try
            {
                dbConnection.Open();
                var imageList = new List<Image>();

                var selectQuery = new SQLiteCommand
                {
                    CommandText = "SELECT * FROM Images;",
                    Connection = dbConnection
                };

                var reader = selectQuery.ExecuteReader();

                // Create a list of Image objects from data returned from Database
                while (reader.Read())
                {
                    imageList.Add(new Image
                    {
                        ImageID = int.Parse(reader["ImageID"].ToString()),
                        PatientID = reader["PatientID"].ToString(),
                        FolderPath = reader["FolderPath"].ToString(),
                        DateUpdated = reader["DateUpdated"].ToString()
                    });
                }

                dbConnection.Close();

                return imageList;
            }
            catch (SQLiteException ex)
            {
                if (dbConnection.State != ConnectionState.Closed)
                {
                    dbConnection.Close();
                }

                Console.WriteLine(ex);

                return new List<Image>()
                {
                    new Image
                    {
                        ImageID = 0,
                        PatientID = "N/A",
                        FolderPath = "Error loading Image entries from database",
                        DateUpdated = "N/A"
                    }
                };
            }
        }

        public void InsertImage(Image image)
        {
            try
            {
                dbConnection.Open();
                var insertStatement = new SQLiteCommand("INSERT INTO Images (PatientID, FolderPath, DateUpdated) VALUES (@PatientID, @FolderPath, @DateUpdated);", dbConnection);
                insertStatement.Parameters.AddWithValue("PatientID", image.PatientID);
                insertStatement.Parameters.AddWithValue("FolderPath", image.FolderPath);

                if (image.DateUpdated != null)
                {
                    insertStatement.Parameters.AddWithValue("DateUpdated", image.DateUpdated);
                }
                else
                {
                    insertStatement.Parameters.AddWithValue("DateUpdated", DateTime.Now.ToShortDateString());
                }

                insertStatement.ExecuteNonQuery();
                dbConnection.Close();
            }
            catch (SQLiteException ex)
            {
                if (dbConnection.State != ConnectionState.Closed)
                {
                    dbConnection.Close();
                }

                Console.WriteLine(ex);
            }
        }
    }
}

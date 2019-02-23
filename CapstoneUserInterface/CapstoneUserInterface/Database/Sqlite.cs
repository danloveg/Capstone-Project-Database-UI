using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

// To install System.Data.SQLite: Install-Package System.Data.SQLite -Version 1.0.109.2

namespace SQLiteDatabase
{
    class Sqlite
    {
        private readonly string dbConnectionString;
        private readonly string dbPath;

        public Sqlite(string databasePath)
        {
            dbConnectionString = "Data Source=" + databasePath;
            dbPath = databasePath;
        }

        public void CreateDatabaseIfDoesNotExist()
        {
            if (System.IO.File.Exists(dbPath) == false)
            {
                SQLiteConnection.CreateFile(dbPath);

                using (var dbConnection = new SQLiteConnection(dbConnectionString))
                {
                    dbConnection.Open();

                    var createQuery = new SQLiteCommand
                    {
                        CommandText = "CREATE TABLE Images (ImageID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, PatientID TEXT, FolderPath TEXT, DateUpdated TEXT)",
                        Connection = dbConnection
                    };

                    createQuery.ExecuteNonQuery();
                }
            }
        }

        public List<Image> GetAllImages()
        {
            using (var dbConnection = new SQLiteConnection(dbConnectionString))
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

                    return imageList;
                }
                catch (SQLiteException ex)
                {
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
        }

        public void InsertImage(Image image)
        {
            using (var dbConnection = new SQLiteConnection(dbConnectionString))
            {
                try
                {
                    dbConnection.Open();
                    var insertStatement = new SQLiteCommand
                    {
                        CommandText = "INSERT INTO Images (FolderPath, DateUpdated) VALUES (@FolderPath, @DateUpdated);",
                        Connection = dbConnection
                    };

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
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}

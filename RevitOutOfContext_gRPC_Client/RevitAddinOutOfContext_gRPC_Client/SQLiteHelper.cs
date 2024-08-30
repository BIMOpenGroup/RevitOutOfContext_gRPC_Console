namespace RevitAddinOutOfContext_gRPC_Client
{
    using Microsoft.Data.Sqlite;
    using System.IO;

    public class SQLiteHelper
    {
        private readonly string _dbPath;

        public SQLiteHelper(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void InitializeDatabase()
        {
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }

            //File.Create(_dbPath);

            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();

                string tableCommand = @"
                CREATE TABLE IF NOT EXISTS RevitElements (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CategoryName TEXT,
                    ElemName TEXT,
                    ElemGuid TEXT,
                    GeomParameters TEXT,
                    DataParameters TEXT
                )";

                using (var createTable = new SqliteCommand(tableCommand, connection))
                {
                    createTable.ExecuteNonQuery();
                }
            }
        }

        public void InsertElement(string categoryName, string elemName, string elemGuid, string geomParameters, string dataParameters)
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();

                string insertCommand = @"
                INSERT INTO RevitElements (CategoryName, ElemName, ElemGuid, GeomParameters, DataParameters) 
                VALUES (@CategoryName, @ElemName, @ElemGuid, @GeomParameters, @DataParameters)";

                using (var insert = new SqliteCommand(insertCommand, connection))
                {
                    insert.Parameters.AddWithValue("@CategoryName", categoryName);
                    insert.Parameters.AddWithValue("@ElemName", elemName);
                    insert.Parameters.AddWithValue("@ElemGuid", elemGuid);
                    insert.Parameters.AddWithValue("@GeomParameters", geomParameters);
                    insert.Parameters.AddWithValue("@DataParameters", dataParameters);
                    insert.ExecuteNonQuery();
                }
            }
        }
    }

}

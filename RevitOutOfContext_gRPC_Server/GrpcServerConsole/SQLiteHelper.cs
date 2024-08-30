namespace GrpcServerConsole
{
    using Microsoft.Data.Sqlite;
    using System.IO;

    public class SQLiteHelper
    {
        private string _dbPath;
        private string _tableName;

        public SQLiteHelper(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void InitializeTable(string tableName)
        {
            _tableName = tableName;
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();

                string tableCommand = @$"
                CREATE TABLE IF NOT EXISTS {_tableName} (
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

        public void InsertElement(string fileName, string categoryName, string elemName, string elemGuid, string geomParameters, string dataParameters)
        {
            if (fileName != _tableName)
            {
                _tableName = fileName;
                InitializeTable(_tableName);
            }
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();

                string insertCommand = @$"
                INSERT INTO {_tableName} (CategoryName, ElemName, ElemGuid, GeomParameters, DataParameters) 
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

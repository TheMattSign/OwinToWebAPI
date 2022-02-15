using System;
using System.Data.SQLite;
using System.IO;

namespace OwinAPI.Database
{
    class DbFactory
    {

        private static readonly string PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "utility.db");
        private static SQLiteConnection Connection;

        public static SQLiteConnection GetConnection()
        {
            string connectionString = string.Format("Data Source={0};Version=3;Pooling=True;Max Pool Size=100;", PATH);

            if (Connection == null)
            {
                Connection = new SQLiteConnection(connectionString);
                Connection.Open(); // Not sure if we should have this here or deeper in
                CheckVersion();
            }

            return Connection;
        }

        private static void CheckVersion()
        {
            string selectFromVersionTable = "SELECT * FROM version";

            using (var command = new SQLiteCommand(selectFromVersionTable, Connection)) { 

                try
                {
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {

                        string version = "0";

                        while (dataReader.Read())
                        {
                            version = dataReader.GetString(0);
                        }

                        // Switch statement on the version to run scripts
                        switch (version)
                        {
                            case "0":
                                break;
                            case "1":
                                break;
                        }
                    }
                }
                catch (SQLiteException e)
                {
                    CreateDatabase();
                }
            }
        }

        private static void CreateDatabase()
        {
            using (var command = new SQLiteCommand(Connection))
            {
                string createVersionTable = "CREATE TABLE version (value TEXT);";
                string insertIntoVersionTable = "INSERT INTO version (value) values (1)";

                string createPrinterTable = "CREATE TABLE printers (type TEXT PRIMARY KEY, name TEXT)";

                command.CommandText = createVersionTable;
                command.ExecuteNonQuery();

                command.CommandText = insertIntoVersionTable;
                command.ExecuteNonQuery();

                command.CommandText = createPrinterTable;
                command.ExecuteNonQuery();
            }
        }
    }
}

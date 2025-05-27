using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Npgsql;

namespace ReestrForm
{
    public static class Data
    {
        private const string ConnectionString =
    "User Id=postgres.yeinrfprzrcdwckdldqz;" +
    "Password=xoCXk7AauhCwseAS;" +
    "Server=aws-0-eu-north-1.pooler.supabase.com;" +
    "Port=6543;" +
    "Database=postgres;" +
    "Timeout=15;" +              // Час очікування на підключення
    "CommandTimeout=15;" + "Pooling=true;Maximum Pool Size=50;";

        // Статичний менеджер — лише одне з’єднання і один потік
        private static readonly DataReaderManager _readerManager = new DataReaderManager(ConnectionString);

        public static ObservableCollection<T> LoadData<T>(string tableName) where T : new()
        {
            var list = new ObservableCollection<T>();
            string query = $"SELECT * FROM {tableName};";


            var rows = _readerManager.ReadRows(query); // читаємо всі рядки в RAM, без відкритого reader

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertyMap = properties.ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                T obj = new T();

                foreach (var kv in row)
                {
                    if (propertyMap.TryGetValue(kv.Key, out PropertyInfo prop))
                    {
                        if (kv.Value != null)
                        {
                            prop.SetValue(obj, Convert.ChangeType(kv.Value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
                        }
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        public static void SaveData<T>(ObservableCollection<T> items, string tableName, string keyPropertyName)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            foreach (var item in items)
            {
                var props = typeof(T).GetProperties().Where(p => p.CanRead).ToList();
                var columns = props.Select(p => p.Name).ToList();
                var parameters = props.Select(p => $"@{p.Name}").ToList();
                var updateSet = props
                    .Where(p => p.Name != keyPropertyName)
                    .Select(p => $"{p.Name} = EXCLUDED.{p.Name}")
                    .ToList();

                string query = $@"
            INSERT INTO {tableName} ({string.Join(", ", columns)})
            VALUES ({string.Join(", ", parameters)})
            ON CONFLICT ({keyPropertyName}) DO UPDATE
            SET {string.Join(", ", updateSet)};";

                using var cmd = new NpgsqlCommand(query, conn);

                foreach (var prop in props)
                {
                    var value = prop.GetValue(item) ?? DBNull.Value;
                    cmd.Parameters.AddWithValue($"@{prop.Name}", value);
                }

                // ВАЖЛИВО: не використовуй ExecuteReader() тут
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error executing SQL:\n{query}\nException: {ex.Message}");
                    Console.WriteLine($"Error executing SQL:\n{query}\nException: {ex.Message}");
                    throw;
                }
            }
        }


        private class DataReaderManager
        {
            private readonly NpgsqlConnection _conn;

            public DataReaderManager(string connectionString)
            {
                _conn = new NpgsqlConnection(connectionString);
                _conn.Open();
            }

            // Повертає всі рядки як список словників (стовпець -> значення)
            public List<Dictionary<string, object?>> ReadRows(string query)
            {
                using var cmd = new NpgsqlCommand(query, _conn);
                using var reader = cmd.ExecuteReader();

                var result = new List<Dictionary<string, object?>>();

                while (reader.Read())
                {
                    var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        row[reader.GetName(i)] = value;
                    }
                    result.Add(row);
                }

                return result;
            }

        }
        public static List<Dictionary<string, object?>> ReadQuery(string query)
        {
            return _readerManager.ReadRows(query);
        }
    }
}
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using Npgsql;

//namespace ReestrForm
//{
//    public static class Data
//    {
//        private const string ConnectionString =
//            "User Id=postgres.yeinrfprzrcdwckdldqz;" +
//            "Password=xoCXk7AauhCwseAS;" +
//            "Server=aws-0-eu-north-1.pooler.supabase.com;" +
//            "Port=6543;" +
//            "Database=postgres;" +
//            "Timeout=30;" +
//            "CommandTimeout=30;" +
//            "Pooling=true;Maximum Pool Size=50;";

//        public static async Task<ObservableCollection<T>> LoadDataAsync<T>(string tableName) where T : new()
//        {
//            string query = $"SELECT * FROM {tableName};";
//            var rows = await ReadRowsAsync(query);

//            var list = new ObservableCollection<T>();
//            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
//            var propertyMap = properties.ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

//            foreach (var row in rows)
//            {
//                T obj = new T();

//                foreach (var kv in row)
//                {
//                    if (propertyMap.TryGetValue(kv.Key, out PropertyInfo prop) && kv.Value != null)
//                    {
//                        prop.SetValue(obj, Convert.ChangeType(kv.Value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
//                    }
//                }

//                list.Add(obj);
//            }

//            return list;
//        }

//        public static async Task SaveDataAsync<T>(ObservableCollection<T> items, string tableName, string keyPropertyName)
//        {
//            using var conn = new NpgsqlConnection(ConnectionString);
//            await conn.OpenAsync();

//            using var transaction = await conn.BeginTransactionAsync();

//            foreach (var item in items)
//            {
//                var props = typeof(T).GetProperties().Where(p => p.CanRead).ToList();
//                var columns = props.Select(p => p.Name).ToList();
//                var parameters = props.Select(p => $"@{p.Name}").ToList();
//                var updateSet = props
//                    .Where(p => p.Name != keyPropertyName)
//                    .Select(p => $"{p.Name} = EXCLUDED.{p.Name}")
//                    .ToList();

//                string query = $@"
//                    INSERT INTO {tableName} ({string.Join(", ", columns)})
//                    VALUES ({string.Join(", ", parameters)})
//                    ON CONFLICT ({keyPropertyName}) DO UPDATE
//                    SET {string.Join(", ", updateSet)};";

//                using var cmd = new NpgsqlCommand(query, conn, transaction);

//                foreach (var prop in props)
//                {
//                    var value = prop.GetValue(item) ?? DBNull.Value;
//                    cmd.Parameters.AddWithValue($"@{prop.Name}", value);
//                }

//                try
//                {
//                    await cmd.ExecuteNonQueryAsync();
//                }
//                catch (Exception ex)
//                {
//                    System.Diagnostics.Debug.WriteLine($"Error executing SQL:\n{query}\nException: {ex.Message}");
//                    Console.WriteLine($"Error executing SQL:\n{query}\nException: {ex.Message}");
//                    throw;
//                }
//            }

//            await transaction.CommitAsync();
//        }

//        public static async Task<List<Dictionary<string, object?>>> ReadQueryAsync(string query)
//        {
//            return await ReadRowsAsync(query);
//        }

//        private static async Task<List<Dictionary<string, object?>>> ReadRowsAsync(string query)
//        {
//            using var conn = new NpgsqlConnection(ConnectionString);
//            await conn.OpenAsync();

//            using var cmd = new NpgsqlCommand(query, conn);
//            using var reader = await cmd.ExecuteReaderAsync();

//            var result = new List<Dictionary<string, object?>>();

//            while (await reader.ReadAsync())
//            {
//                var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
//                for (int i = 0; i < reader.FieldCount; i++)
//                {
//                    var value = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
//                    row[reader.GetName(i)] = value;
//                }
//                result.Add(row);
//            }

//            return result;
//        }
//    }
//}

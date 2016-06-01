using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace VSTS.WIT.Reporting.Providers.SQLite
{
    public class SQLiteTargetProvider : ITargetProvider
    {
        private string connectionString;

        public SQLiteTargetProvider(SQLiteTargetProviderOptions providerOptions)
        {
            this.connectionString = $"Data Source={providerOptions.DatabaseFile};Version=3;";

            if (!File.Exists(providerOptions.DatabaseFile))
            {
                SQLiteConnection.CreateFile(providerOptions.DatabaseFile);

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        var createTable = @"
                            CREATE TABLE workItems(
                                id INTEGER NOT NULL,
                                rev INTEGER NOT NULL,
                                watermark INTEGER NOT NULL,
                                field TEXT NOT NULL,
                                value TEXT
                            )";
                        command.CommandText = createTable;
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "CREATE INDEX id_rev_idx ON workItems (id, rev)";
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public async Task SaveWorkItems(IEnumerable<WorkItem> workItems)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                foreach (var workItem in workItems)
                {
                    using (var command = connection.CreateCommand())
                    {
                        var parameterIndex = 0;
                        var query = new StringBuilder();
                        foreach (var field in workItem.Fields)
                        {
                            var idParam = command.Parameters.AddWithValue($"@p{parameterIndex++}", workItem.Id);
                            var revParam = command.Parameters.AddWithValue($"@p{parameterIndex++}", workItem.Rev);
                            var watermarkParam = command.Parameters.AddWithValue($"@p{parameterIndex++}", workItem.Fields["System.Watermark"]);
                            var fieldParam = command.Parameters.AddWithValue($"@p{parameterIndex++}", field.Key);
                            var valueParam = command.Parameters.AddWithValue($"@p{parameterIndex++}", field.Value);

                            query.AppendLine(
                                $@"
INSERT OR REPLACE INTO WorkItems
(Id, Rev, Watermark, Field, Value)
VALUES 
({idParam.ParameterName}, {revParam.ParameterName}, {watermarkParam.ParameterName}, {fieldParam.ParameterName}, {valueParam.ParameterName})
;");
                        }

                        command.CommandText = query.ToString();
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }
}

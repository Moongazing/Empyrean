using Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.Messages;
using Serilog;
using Serilog.Sinks.PostgreSQL;

namespace Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.Logger;

public class PostgreSqlLogger : LoggerServiceBase
{
    public PostgreSqlLogger(PostgreSqlConfiguration logConfiguration) : base(logger: null!)
    {
        if (logConfiguration == null)
        {
            throw new Exception(SerilogMessages.NullOptionsMessage);
        }

        IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "Timestamp", new TimestampColumnWriter() },
            { "Level", new LevelColumnWriter() },
            { "Message", new RenderedMessageColumnWriter() },
            { "Exception", new ExceptionColumnWriter() },
            { "Properties", new LogEventSerializedColumnWriter() }
        };

        Logger = new LoggerConfiguration()
            .WriteTo.PostgreSQL(
                connectionString: logConfiguration.ConnectionString,
                tableName: logConfiguration.TableName,
                columnOptions: columnWriters,
                needAutoCreateTable: logConfiguration.AutoCreateSqlTable
            )
            .CreateLogger();
    }
}

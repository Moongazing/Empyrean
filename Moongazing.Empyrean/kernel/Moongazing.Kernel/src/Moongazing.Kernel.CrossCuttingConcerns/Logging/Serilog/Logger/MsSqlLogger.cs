using Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.Messages;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.Logger;

public class MsSqlLogger : LoggerServiceBase
{
    public MsSqlLogger(MsSqlConfiguration logConfiguration) : base(logger: null!)
    {
        if (logConfiguration == null)
        {
            throw new Exception(SerilogMessages.NullOptionsMessage);
        }

        MSSqlServerSinkOptions sinkOptions = new()
        {
            TableName = logConfiguration.TableName,
            AutoCreateSqlTable = logConfiguration.AutoCreateSqlTable
        };

        ColumnOptions columnOptions = new();

        Logger = new LoggerConfiguration()
            .WriteTo.MSSqlServer(
                connectionString: logConfiguration.ConnectionString,
                sinkOptions: sinkOptions,
                columnOptions: columnOptions
            )
            .CreateLogger();
    }
}

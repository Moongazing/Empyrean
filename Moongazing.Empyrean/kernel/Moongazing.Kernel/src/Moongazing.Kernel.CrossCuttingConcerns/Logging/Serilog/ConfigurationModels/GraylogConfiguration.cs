﻿namespace Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;

public class GraylogConfiguration
{
    public string? HostnameOrAddress { get; set; }
    public int Port { get; set; }
}

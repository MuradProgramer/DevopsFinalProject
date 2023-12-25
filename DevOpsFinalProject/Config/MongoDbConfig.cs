﻿namespace DevOpsFinalProject.Config;

public class MongoDbConfig
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string Collection { get; set; } = string.Empty;
}

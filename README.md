LocalForwarder
==============

[![.NET](https://github.com/koturn/LocalForwarder/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/koturn/LocalForwarder/actions/workflows/dotnet.yml)

A SSH local port forwarding tool configurable via JSON files.

## Dependent Libraries

- [Newtonsoft.Json 13.0.4](https://www.nuget.org/packages/Newtonsoft.Json/13.0.4 "NuGet Gallery | Newtonsoft.Json 13.0.4")
- [SSH.NET 2023.0.1](https://www.nuget.org/packages/SSH.NET/2023.0.1 "NuGet Gallery | SSH.NET 2023.0.1")

## Sample Json file

```json
{
  "proxy_config": [
    {
      "description": "Sample proxy",
      "host": "192.168.101.11",
      "port": 22,
      "username": "sampleuser",
      "password": "xyzw1234",
      "server_alive_interval": 60,
      "local_forward": [
        {
          "description": "SSH to A",
          "local_host": "127.0.0.1",
          "local_port": 8022,
          "remote_host": "192.168.101.12",
          "remote_port": 22,
          "enabled": true
        },
        {
          "description": "HTTP to B",
          "local_host": "127.0.0.1",
          "local_port": 8080,
          "remote_host": "192.168.101.13",
          "remote_port": 80,
          "enabled": true
        },
      ],
      "enabled": true
    }
  ]
}
```

## LICENSE

This software is released under the MIT License, see [LICENSE](LICENSE "LICENSE").

NetworkMapper
=== 

NetworkMapper, some ugly code wrapping `nmap -sP` to JSON. 

```
scriptcs NetworkMapper.csx -- [ipRange]
```

Example: 

```
$ scriptcs NetworkMapper.csx -- 192.168.1.* | jq
[
  {
    "HostName": "192.168.1.1",
    "IpAddress": "192.168.1.1",
    "Alive": true
  },
  {
    "HostName": "192.168.1.100",
    "IpAddress": "192.168.1.100",
    "Alive": true
  },
  {
    "HostName": "192.168.1.108",
    "IpAddress": "192.168.1.108",
    "Alive": true
  }
]
```
NetworkMapper
=== 

NetworkMapper, some ugly code wrapping `nmap -sP` to JSON. 
Requires `nmap`: 

```
brew install nmap
apt-get intall nmap
choco install nmap
```

```
scriptcs NetworkMapper.csx -- [ipRange]
```

Example: 

```
$ scriptcs NetworkMapper.csx -- 192.168.1.* | jq
[
  {
    "HostName": "Pegasus.testdomain.local",
    "IpAddress": "192.168.1.1",
    "Alive": true
  },
  {
    "HostName": "Atlantis.testdomain.local",
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
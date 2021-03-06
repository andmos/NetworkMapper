#r "System.Diagnostics.Process"
#r "Newtonsoft.Json"

using System.Diagnostics;
using Newtonsoft.Json;

const string NmapLineDeliminator = "nmap scan report for";

var networkHostsJsonResponse = JsonConvert.SerializeObject(GetNetworkHosts(Env.ScriptArgs[0]));

if (Env.ScriptArgs.Count() > 1 && Env.ScriptArgs.Contains("count"))
{
  Console.WriteLine(JsonConvert.SerializeObject(CountNumberOfLiveHosts(GetNetworkHosts(Env.ScriptArgs[0]))));
}else
{
    Console.WriteLine(networkHostsJsonResponse);
}

private IEnumerable<NetworkHost> GetNetworkHosts(string ipRange)
{
    var networkHosts = new List<NetworkHost>();
    var rawString = RunNmap(ipRange).ToLower();
    var nmapLines = rawString.Split('\n');

    foreach(var line in nmapLines)
    {
        if(line.StartsWith(NmapLineDeliminator))
        {
            var ipLine = line.Split(new string[] {NmapLineDeliminator}, StringSplitOptions.None).Select(l => l.Trim()).ToArray();
            if(ipLine[1].Contains('('))
            {
                var ipAndHostname = ipLine[1].Split('(', ')').Select(l => l.Trim()).ToArray();
                networkHosts.Add(new NetworkHost { HostName = ipAndHostname[0], IpAddress = ipAndHostname[1], Alive = true });

            }else
            {
                networkHosts.Add(new NetworkHost { HostName = ipLine[1], IpAddress = ipLine[1], Alive = true });
            }
        }
    }
    return networkHosts;
}

private Dictionary<string, int> CountNumberOfLiveHosts(IEnumerable<NetworkHost> networkHosts)
{
    return new Dictionary<string, int>() { { "hosts:", networkHosts.Count() } };
}

private string RunNmap(string ipRange)
{
    var nmapProcess = SetUpProcess("nmap", $"-sP {ipRange}");
    nmapProcess.Start();
    string nmapOutput = nmapProcess.StandardOutput.ReadToEnd();
    nmapProcess.WaitForExit();

    return nmapOutput;
}

private Process SetUpProcess(string command, string argument)
{
    var startinfo = new ProcessStartInfo();
    var proc = new Process();
    proc.StartInfo.FileName = command;
    proc.StartInfo.Arguments = argument;
    proc.StartInfo.UseShellExecute = false;
    proc.StartInfo.RedirectStandardOutput = true;
    proc.StartInfo.RedirectStandardError = true;
    return proc;
}

private class NetworkHost
{
    public string HostName {get; set;}
    public string IpAddress {get; set;}
    public bool Alive {get; set;}
}

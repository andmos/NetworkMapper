#r "System.Diagnostics"
#r "Newtonsoft.Json"

using System.Diagnostics;
using Newtonsoft.Json;

const string NmapLineDeliminator = "nmap scan report for";

var json = JsonConvert.SerializeObject(GetNetworkHosts(Env.ScriptArgs[0]));

Console.WriteLine(json);

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
            
            networkHosts.Add(new NetworkHost { HostName = ipLine[1], IpAddress = ipLine[1], Alive = true });
        }
    }
    return networkHosts; 
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

using System.Diagnostics;

namespace Ip2regionApi.Utils;

public class ShellCommandHelper
{
    /// <summary>
    /// 执行Shell命令
    /// </summary>
    /// <param name="command"></param>
    public static void ExecuteShellCommand(string command)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = "-c \"" + command + "\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = new Process();
        process.StartInfo = processInfo;
        process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
        process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
    }
}
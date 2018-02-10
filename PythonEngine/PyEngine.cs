using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

namespace PythonEngine
{
    public class PyEngine : IDisposable
    {
        private readonly Process python;

        public PyEngine(string pythonPath)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = "-i",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                ErrorDialog = false
            };

            python = new Process
            {
                StartInfo = processStartInfo
            };
            python.Start();

            python.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);
            python.BeginErrorReadLine();
        }

        public void FromImport(string from, string import, string asName = null)
        {
            var asText = (asName ?? "") != "" ? $" as {asName}" : "";
            WriteLine($"from {from} import {import}{asText}");
        }

        public void Import(string import, string asName = null)
        {
            var asText = (asName ?? "") != "" ? $" as {asName}" : "";
            WriteLine($"import {import}{asText}");
        }

        public void UseJsonFunc()
        {
            Import("json");
        }

        public void WriteLineObjectToJson(string format, params object[] args)
        {
            var jsons = args.Select(n => JsonConvert.SerializeObject(n)).ToArray();
            var input = string.Format(format, jsons);
            WriteLine(input);
        }

        public T DeserializeObjectFromJson<T>(string objectName)
        {
            WriteLine($"print(json.dumps({objectName}))");
            var json = ReadLine();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void WriteLine(string input)
        {
            python.StandardInput.WriteLine(input);
        }

        public void WriteLine(string format, params object[] args)
        {
            python.StandardInput.WriteLine(format, args);
        }

        public string ReadLine()
        {
            return python.StandardOutput.ReadLine();
        }

        public void Dispose()
        {
            python?.Dispose();
        }
    }
}

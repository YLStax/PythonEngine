using System;
using System.Collections.Generic;
using PythonEngine;

namespace PythonEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var python = new PyEngine(@"C:\ProgramData\Anaconda3\python.exe"))
            {
                python.UseJsonFunc();
                python.Import("numpy", "np");
                python.Import("test", "ts");

                python.WriteLine("print('Hello')");
                Console.WriteLine(python.ReadLine());
                python.WriteLine("t=ts.test('World')");
                python.WriteLine("print(t)");
                Console.WriteLine(python.ReadLine());

                var dic = new Dictionary<string, string> { { "りんご", "果物" }, { "にんじん", "野菜" }, { "バナナ", "果物" } };
                python.WriteLineObjectToJson("a={0}", dic);
                python.WriteLine("a['キャベツ']='野菜'");
                var a = python.DeserializeObjectFromJson<Dictionary<string, string>>("a");
                foreach (var n in a)
                {
                    Console.WriteLine(n);
                }

                var list = new int[,] { { 1, 2 }, { 3, 4 } };
                python.WriteLineObjectToJson("b=np.array({0})", list);
                python.WriteLine("b=2*b");
                var b = python.DeserializeObjectFromJson<int[,]>("b.tolist()");
                foreach (var n in b)
                {
                    Console.WriteLine(n);
                }
            }
            Console.ReadKey();
        }
    }
}

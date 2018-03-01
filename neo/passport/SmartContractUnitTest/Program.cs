using System;
using System.IO;
using System.Linq;
using System.Numerics;

using Neo.Cryptography;
using Neo.VM;

namespace SmartContractUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new ExecutionEngine(null, Crypto.Default);
            engine.LoadScript(File.ReadAllBytes(@"..\..\..\PassportPoints\bin\PassportPoints.avm"));

            CallFunction(engine, "IssuePoints", "Roland", 25);
            CallFunction(engine, "GetPoints", "Roland");
            
            Console.WriteLine($"Execution result: {GetBigIntegerReturn(engine)}");
            Console.ReadLine();
        }

        static void CallFunction(ExecutionEngine engine, params object[] args)
        {
            using (var sb = new ScriptBuilder())
            {
                foreach (var arg in args.Reverse())
                {
                    sb.EmitPush(arg);
                }

                engine.LoadScript(sb.ToArray());
            }

            engine.Execute();
        }

        static BigInteger GetBigIntegerReturn(ExecutionEngine engine)
        {
            return engine.EvaluationStack.Pop().GetBigInteger();
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Numerics;

using Neo.Cryptography;
using Neo.SmartContract;
using Neo.VM;

namespace SmartContractUnitTest
{
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            var engine = new ExecutionEngine(null, Crypto.Default);
            engine.LoadScript(File.ReadAllBytes(@"..\..\..\PassportPoints\bin\PassportPoints.avm"));
            
            CallVarFunction(engine, "IssuePoints", "Roland", 25);
            CallVarFunction(engine, "GetPoints", "Roland");

            Console.WriteLine($"Execution result: {GetBytesReturn(engine)}");
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

        static void CallVarFunction(ExecutionEngine engine, string operation, params object[] args)
        {
            using (var sb = new ScriptBuilder())
            {
                var paramArray = new List<ContractParameter>();

                foreach (var arg in args)
                {
                    ContractParameterType type;

                    if (arg is string)
                    {
                        type = ContractParameterType.String;
                    }
                    else if (arg is int || arg is BigInteger)
                    {
                        type = ContractParameterType.Integer;
                    }
                    else
                    {
                        throw new ArgumentException("Unsupported type.");
                    }

                    paramArray.Add(new ContractParameter(type) { Value = arg });
                }

                var arrayOfArgs = new ContractParameter(ContractParameterType.Array) { Value = paramArray.ToArray() };

                sb.EmitPush(arrayOfArgs);
                sb.EmitPush(operation);

                engine.LoadScript(sb.ToArray());
            }

            engine.Execute();
        }

        static string GetStringReturn(ExecutionEngine engine)
        {
            return engine.EvaluationStack.Pop().GetString();
        }

        static BigInteger GetBigIntegerReturn(ExecutionEngine engine)
        {
            return engine.EvaluationStack.Pop().GetBigInteger();
        }

        static string GetBytesReturn(ExecutionEngine engine)
        {
            return BitConverter.ToString(engine.EvaluationStack.Pop().GetByteArray());
        }
    }
}

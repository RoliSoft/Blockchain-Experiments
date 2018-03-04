using System;
using System.IO;
using System.Numerics;

using LunarParser;

using Neo.Emulator;
using Neo.Emulator.API;

namespace SmartContractUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var bytecodes = File.ReadAllBytes(@"..\..\..\PassportPoints\bin\PassportPoints.avm");
            var blockchain = new Blockchain();
            var emulator = new NeoEmulator(blockchain)
                {
                    checkWitnessMode = CheckWitnessMode.AlwaysTrue
                };
            
            var address = blockchain.DeployContract(nameof(bytecodes), bytecodes);
            emulator.SetExecutingAddress(address);

            CallVarFunction(emulator, "IssuePoints", "Roland", 25);
            CallVarFunction(emulator, "IssuePoints", "Roland", 20);
            CallVarFunction(emulator, "GetPoints", "Roland");
            
            Console.WriteLine($"Execution result: {emulator.GetOutput().GetBigInteger()}");
            Console.ReadLine();
        }

        static void CallFunction(NeoEmulator emulator, params object[] args)
        {
            var inputs = DataNode.CreateArray();

            foreach (var arg in args)
            {
                if (arg is int || arg is uint || arg is long || arg is ulong || arg is BigInteger)
                {
                    inputs.AddValue(double.Parse(arg.ToString()));
                }
                else
                {
                    inputs.AddValue(arg);
                }
            }

            emulator.Reset(inputs);
            emulator.Run();
        }

        static void CallVarFunction(NeoEmulator emulator, string operation, params object[] args)
        {
            var argumentInputs = DataNode.CreateArray();

            foreach (var arg in args)
            {
                if (arg is int || arg is uint || arg is long || arg is ulong || arg is BigInteger)
                {
                    argumentInputs.AddValue(double.Parse(arg.ToString()));
                }
                else
                {
                    argumentInputs.AddValue(arg);
                }
            }

            var inputs = DataNode.CreateArray();

            inputs.AddValue(operation);
            inputs.AddNode(argumentInputs);

            emulator.Reset(inputs);
            emulator.Run();
        }
    }
}

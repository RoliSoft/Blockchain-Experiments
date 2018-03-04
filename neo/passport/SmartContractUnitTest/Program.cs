using System;
using System.IO;

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

            dynamic contract = new SmartContractEmulator(emulator);

            contract.IssuePoints("Roland", 25);
            contract.IssuePoints("Roland", 20);
            var result = contract.GetPoints("Roland");
            
            Console.WriteLine($"Execution result: {result.GetBigInteger()}");
            Console.ReadLine();
        }
    }
}

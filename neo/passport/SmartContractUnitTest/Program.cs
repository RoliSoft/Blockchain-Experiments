using System;

namespace SmartContractUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic contract = new SmartContractEmulator(@"..\..\..\PassportPoints\bin\PassportPoints.avm");

            contract.IssuePoints("Roland", 25);
            contract.IssuePoints("Roland", 20);
            var result = contract.GetPoints("Roland");
            
            Console.WriteLine($"Execution result: {result.GetBigInteger()}");
            Console.ReadLine();
        }
    }
}

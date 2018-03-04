using System.Dynamic;
using System.IO;
using System.Numerics;

using LunarParser;

using Neo.Emulator;
using Neo.Emulator.API;

namespace SmartContractUnitTest
{
    public class SmartContractEmulator : DynamicObject
    {
        public Blockchain Blockchain { get; }
        public NeoEmulator Emulator { get; }
        public Address ContractAddress { get; }

        public SmartContractEmulator(string filename) : this(File.ReadAllBytes(filename))
        {

        }

        public SmartContractEmulator(byte[] bytecodes)
        {
            Blockchain = new Blockchain();
            Emulator = new NeoEmulator(Blockchain)
                {
                    checkWitnessMode = CheckWitnessMode.AlwaysTrue
                };

            ContractAddress = Blockchain.DeployContract(string.Empty, bytecodes);
            Emulator.SetExecutingAddress(ContractAddress);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            CallFunction(binder.Name, args);

            try
            {
                result = Emulator.GetOutput();
            }
            catch
            {
                result = null;
            }

            return true;
        }

        private void CallFunction(string operation, params object[] args)
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

            Emulator.Reset(inputs);
            Emulator.Run();
        }
    }
}

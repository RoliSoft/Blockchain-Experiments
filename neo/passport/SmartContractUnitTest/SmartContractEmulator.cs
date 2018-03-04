using System.Dynamic;
using System.Numerics;

using LunarParser;

using Neo.Emulator;

namespace SmartContractUnitTest
{
    class SmartContractEmulator : DynamicObject
    {
        private readonly NeoEmulator _emulator;

        public SmartContractEmulator(NeoEmulator emulator)
        {
            _emulator = emulator;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            CallFunction(binder.Name, args);

            try
            {
                result = _emulator.GetOutput();
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

            _emulator.Reset(inputs);
            _emulator.Run();
        }
    }
}

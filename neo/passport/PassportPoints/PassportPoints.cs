using System.Numerics;

using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework;

namespace PassportPoints
{
    public class PassportPoints : SmartContract
    {
        private const string PointsPrefix = "points_";

        private static readonly byte[] Owner = "AbTu9zV73T25nQeHg4BdhWrBq6m3ASQerc".ToScriptHash();

        public static object Main(string operation, params object[] args)
        {
            switch (operation)
            {
                case nameof(IssuePoints):
                    return IssuePoints((string)args[0], (BigInteger)args[1]);

                case nameof(GetPoints):
                    return GetPoints((string)args[0]);

                default:
                    return false;
            }
        }

        public static bool IssuePoints(string name, BigInteger quantity)
        {
            if (!Runtime.CheckWitness(Owner))
            {
                Runtime.Log("Only owner can issue points.");
                return false;
            }

            if (quantity < 0)
            {
                Runtime.Log("Cannot issue negative points.");
                return false;
            }
            
            Storage.Put(Storage.CurrentContext, PointsPrefix + name, quantity + GetPoints(name));

            return true;
        }

        public static BigInteger GetPoints(string name)
        {
            return Storage.Get(Storage.CurrentContext, PointsPrefix + name).AsBigInteger();
        }
    }
}

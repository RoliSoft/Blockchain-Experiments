using NUnit.Framework;

namespace SmartContractUnitTest
{
    [TestFixture]
    public class TestPassportPoints
    {
        private const string FirstUser = "Lorem";
        private const string SecondUser = "Ipsum";

        private dynamic _contract;

        [SetUp]
        public void Initialize()
        {
            _contract = new SmartContractEmulator(@"C:\Users\RoliSoft\Desktop\blockchain\neo\passport\PassportPoints\bin\PassportPoints.avm");
        }

        [Test]
        public void InitialBalance()
        {
            Assert.AreEqual(0, (int)_contract.GetPoints(FirstUser).GetBigInteger(), "There should be no points initially.");
            Assert.AreEqual(0, (int)_contract.GetPoints(SecondUser).GetBigInteger(), "There should be no points initially.");
        }

        [Test]
        public void Issuance()
        {
            _contract.IssuePoints(FirstUser, 100);

            Assert.AreEqual(100, (int)_contract.GetPoints(FirstUser).GetBigInteger(), "First user should have 100 points.");
            Assert.AreEqual(0, (int)_contract.GetPoints(SecondUser).GetBigInteger(), "Second user should have 0 points.");
            
            _contract.IssuePoints(SecondUser, 200);

            Assert.AreEqual(100, (int)_contract.GetPoints(FirstUser).GetBigInteger(), "First user should have 100 points.");
            Assert.AreEqual(200, (int)_contract.GetPoints(SecondUser).GetBigInteger(), "Second user should have 200 points.");
        }
    }
}

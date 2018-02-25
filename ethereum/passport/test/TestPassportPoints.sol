pragma solidity ^0.4.18;

import "truffle/Assert.sol";
import "contracts/PassportPoints.sol";

contract TestPassportPoints {

	string firstUser = "Lorem";
	string secondUser = "Ipsum";

	PassportPoints lpp = new PassportPoints();

	function testInitialBalance() public {
		Assert.equal(lpp.getPoints(firstUser), 0, "There should be no points initially.");
		Assert.equal(lpp.getPoints(secondUser), 0, "There should be no points initially.");
	}

	function testIssuance() public {
		lpp.issuePoints(firstUser, 100);

		Assert.equal(lpp.getPoints(firstUser), 100, "First user should have 100 points.");
		Assert.equal(lpp.getPoints(secondUser), 0, "Second user should have 0 points.");

		lpp.issuePoints(secondUser, 200);

		Assert.equal(lpp.getPoints(firstUser), 100, "First user should have 100 points.");
		Assert.equal(lpp.getPoints(secondUser), 200, "Second user should have 200 points.");
	}

}
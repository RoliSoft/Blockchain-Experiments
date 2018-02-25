pragma solidity ^0.4.17;

contract Migrations {

	address public owner;
	uint public lastMigration;

	modifier onlyOwner {
		require(msg.sender == owner);
		_;
	}

	function Migrations() public {
		owner = msg.sender;
	}

	function setCompleted(uint _completed) onlyOwner public {
		lastMigration = _completed;
	}

	function upgrade(address _address) onlyOwner public {
		Migrations upgraded = Migrations(_address);
		upgraded.setCompleted(lastMigration);
	}

}
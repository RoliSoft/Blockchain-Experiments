pragma solidity ^0.4.18;

contract PassportPoints {

	address owner;
	mapping (string => uint) points;

	function PassportPoints() public {
		owner = msg.sender;
	}

	modifier onlyOwner {
		require(msg.sender == owner);
		_;
	}

	function issuePoints(string _name, uint _quantity) onlyOwner public {
		require(points[_name] + _quantity >= points[_name]);
		points[_name] += _quantity;
	}

	function getPoints(string _name) view public returns (uint) {
		return points[_name];
	}

}
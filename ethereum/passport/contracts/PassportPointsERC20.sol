pragma solidity ^0.4.18;

contract ERC20Basic {

	string public name;
	string public symbol;
	uint public decimals;
	uint public totalSupply;

	event Transfer(address indexed from, address indexed to, uint value);

	function balanceOf(address who) public view returns (uint);
	function transfer(address to, uint value) public;

}

contract PassportPointsERC20 is ERC20Basic {

	string public name = "Passport Points";
	string public symbol = "LPP";
	uint public decimals = 0;
	uint public totalSupply = 1000000000;

	mapping (address => uint) points;

	event Transfer(address indexed from, address indexed to, uint value);

	function PassportPointsERC20() public {
		points[msg.sender] = totalSupply;
	}

	function balanceOf(address _who) public view returns (uint) {
		return points[_who];
	}

	function transfer(address _to, uint _value) public {
		require(points[msg.sender] >= _value);
		require(points[_to] + _value >= points[_to]);

		points[msg.sender] -= _value;
		points[_to] += _value;

		Transfer(msg.sender, _to, _value);
	}

}
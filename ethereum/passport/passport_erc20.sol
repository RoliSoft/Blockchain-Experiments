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

contract LateralPassportERC20 is ERC20Basic {

    string public name = "Lateral Passport Points";
    string public symbol = "LPP";
    uint public decimals = 0;
    uint public totalSupply = 1000000000;

    mapping (address => uint) points;

    event Transfer(address indexed from, address indexed to, uint value);

    function LateralPassportERC20() public {
        points[msg.sender] = totalSupply;
    }

    function balanceOf(address who) public view returns (uint) {
        return points[who];
    }

    function transfer(address to, uint value) public {
        points[msg.sender] -= value;
        points[to] += value;
        Transfer(msg.sender, to, value);
    }

}
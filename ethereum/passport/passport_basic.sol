pragma solidity ^0.4.18;

contract LateralPassportPoints {

    address owner;
    mapping (string => uint) points;
    
    function LateralPassportPoints() public {
        owner = msg.sender;
    }
    
    modifier onlyOwner {
        require(msg.sender == owner);
        _;
    }
    
    function issuePoints(string _name, uint _quantity) onlyOwner public {
        points[_name] += _quantity;
    }
    
    function getPoints(string _name) view public returns (uint) {
        return points[_name];
    }

}
pragma solidity ^0.4.18;

contract LateralPassportPoints {

    address owner;
    string[] participants;
    mapping (string => uint) points;

    event Issued(string name, uint quantity, uint total);

    function LateralPassportPoints() public {
        owner = msg.sender;
    }

    modifier onlyOwner {
        require(msg.sender == owner);
        _;
    }

    function getParticipantCount() view public returns (uint) {
        return participants.length;
    }

    function getParticipantAt(uint _index) view public returns (string, uint) {
        string storage participant = participants[_index];
        return (participant, points[participant]);
    }

    function issuePoints(string _name, uint _quantity) onlyOwner public {
        _ensureParticipant(_name);
        points[_name] += _quantity;
        Issued(_name, _quantity, points[_name]);
    }

    function getOwner() view public returns (address) {
        return owner;
    }

    function _ensureParticipant(string _name) private {
        bool found = false;
        bytes32 nameHash = keccak256(_name);
        for (uint i = 0; i < participants.length; i++) {
            if (keccak256(participants[i]) == nameHash) {
                found = true;
                break;
            }
        }
        if (!found) {
            participants.push(_name);
        }
    }

}
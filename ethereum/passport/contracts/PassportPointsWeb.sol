pragma solidity ^0.4.18;

contract PassportPointsWeb {

	address owner;
	string[] participants;
	mapping (string => uint) points;

	event Issue(string name, uint quantity, uint total);

	function PassportPointsWeb() public {
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
		require(points[_name] + _quantity >= points[_name]);

		_ensureParticipant(_name);
		points[_name] += _quantity;

		Issue(_name, _quantity, points[_name]);
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
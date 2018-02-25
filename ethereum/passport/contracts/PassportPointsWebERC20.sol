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

contract PassportPointsWebERC20 is ERC20Basic {

    string public name = "Passport Points";
    string public symbol = "LPP";
    uint public decimals = 0;
    uint public totalSupply = 1000000000;

    address owner;
    string[] participants;
    mapping (string => uint) points;
    mapping (address => string) users;
    mapping (string => address) addresses;

    address constant EMPTY_ADDRESS = address(0);

    event Issue(string name, int quantity, uint total, address target);
    event Purchase(string name, uint total);
    event Transfer(address indexed from, address indexed to, uint value);

    function PassportPointsWebERC20() public {
        owner = msg.sender;
    }

    modifier onlyOwner {
        require(msg.sender == owner);
        _;
    }

    function balanceOf(address who) public view returns (uint) {
        return points[users[who]];
    }

    function transfer(address to, uint value) public {
        require(to == address(this) || addresses[users[to]] != EMPTY_ADDRESS);
		require(points[users[msg.sender]] >= value);

        if (to == address(this)) {
            points[users[msg.sender]] -= value;

            Transfer(msg.sender, to, value);
            Issue(users[msg.sender], int(value) * -1, points[users[msg.sender]], msg.sender);
            Purchase(users[msg.sender], value);

            return;
        }

		require(points[users[to]] + value >= points[users[to]]);

        points[users[msg.sender]] -= value;
        points[users[to]] += value;

        Transfer(msg.sender, to, value);

        Issue(users[msg.sender], int(value) * -1, points[users[msg.sender]], msg.sender);
        Issue(users[to], int(value), points[users[to]], to);
    }

    function getParticipantCount() view public returns (uint) {
        return participants.length;
    }

    function getParticipantAt(uint _index) view public returns (string, uint, address) {
        string storage participant = participants[_index];
        return (participant, points[participant], addresses[participant]);
    }

    function lookupAddress(address who) public view returns (string, uint) {
        return (users[who], points[users[who]]);
    }

    function issuePoints(string _name, uint _quantity, address _address) onlyOwner public {
		require(points[_name] + _quantity >= points[_name]);

        _ensureParticipant(_name, _address);
        points[_name] += _quantity;

        Transfer(owner, addresses[_name], _quantity);
        Issue(_name, int(_quantity), points[_name], addresses[_name]);
    }

    function getOwner() view public returns (address) {
        return owner;
    }

    function _ensureParticipant(string _name, address _address) private {
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

        if (_address != EMPTY_ADDRESS) {
            if (addresses[_name] != EMPTY_ADDRESS) {
                delete users[addresses[_name]];
            }

            users[_address] = _name;
            addresses[_name] = _address;
        }
    }

}
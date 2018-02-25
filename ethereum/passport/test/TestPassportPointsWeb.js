const PassportPointsWeb = artifacts.require('PassportPointsWeb');

var firstUser = 'Lorem';
var secondUser = 'Ipsum';

contract('PassportPointsWeb', async (accounts) => {

	var instance;

	before(async () => {
		instance = await PassportPointsWeb.new({ from: accounts[0] })
	});

	it('correct owner', async () => {
		let owner = await instance.getOwner.call();
		assert.equal(owner, accounts[0], 'owner is not the correct account');
	});

	it('0 participants at the start', async () => {
		let count = await instance.getParticipantCount.call();
		assert.equal(count.valueOf(), 0, 'number of participants is not 0 initially');
	});

	it('issue 100 tokens to first user', async () => {
		await instance.issuePoints.sendTransaction(firstUser, 100);

		let count = await instance.getParticipantCount.call();
		assert.equal(count.valueOf(), 1, 'number of participants should be 1');

		let participant = await instance.getParticipantAt.call(0);
		assert.equal(participant[0], firstUser, 'first user\'s name is incorrect');
		assert.equal(participant[1].valueOf(), 100, 'first user should have 100 tokens');
	});

	it('issue 200 tokens to second user', async () => {
		await instance.issuePoints.sendTransaction(secondUser, 200);

		let count = await instance.getParticipantCount.call();
		assert.equal(count.valueOf(), 2, 'number of participants should be 2');

		let participant = await instance.getParticipantAt.call(1);
		assert.equal(participant[0], secondUser, 'second user\'s name is incorrect');
		assert.equal(participant[1].valueOf(), 200, 'second user should have 200 tokens');
	});

	it('issue 50 more tokens to first user', async () => {
		await instance.issuePoints.sendTransaction(firstUser, 50);

		let count = await instance.getParticipantCount.call();
		assert.equal(count.valueOf(), 2, 'number of participants should be 2');

		let participant = await instance.getParticipantAt.call(0);
		assert.equal(participant[0], firstUser, 'first user\'s name is incorrect');
		assert.equal(participant[1].valueOf(), 150, 'first user should have 150 tokens');
	});

	it('issue 2^256-1 tokens to second user', async () => {
		let failed = false;
		try {
			await instance.issuePoints.sendTransaction(secondUser, '0xffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff');
		}
		catch (e) {
			failed = true;
		}
		finally {
			if (!failed) {
				assert.fail('tokens should not have been transferred');
			}
		}

		let count = await instance.getParticipantCount.call();
		assert.equal(count.valueOf(), 2, 'number of participants should be 2');

		let participant = await instance.getParticipantAt.call(1);
		assert.equal(participant[0], secondUser, 'second user\'s name is incorrect');
		assert.equal(participant[1].valueOf(), 200, 'second user should have 200 tokens');
	});

});
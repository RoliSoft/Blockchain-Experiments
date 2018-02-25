const PassportPointsWeb = artifacts.require('PassportPointsWebERC20');

var firstUser = 'Lorem';
var secondUser = 'Ipsum';

contract('PassportPointsWebERC20', async (accounts) => {

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
		await instance.issuePoints.sendTransaction(firstUser, 100, accounts[1]);

		let count = await instance.getParticipantCount.call();
		assert.equal(count.valueOf(), 1, 'number of participants should be 1');

		let participant = await instance.getParticipantAt.call(0);
		assert.equal(participant[0], firstUser, 'first user\'s name is incorrect');
		assert.equal(participant[1].valueOf(), 100, 'first user should have 100 tokens');
		assert.equal(participant[2], accounts[1], 'first user\'s address is incorrect');
	});

	it('issue 200 tokens to second user', async () => {
		await instance.issuePoints.sendTransaction(secondUser, 200, accounts[2]);

		let count = await instance.getParticipantCount.call();
		assert.equal(count.valueOf(), 2, 'number of participants should be 2');

		let participant = await instance.getParticipantAt.call(1);
		assert.equal(participant[0], secondUser, 'second user\'s name is incorrect');
		assert.equal(participant[1].valueOf(), 200, 'second user should have 200 tokens');
		assert.equal(participant[2], accounts[2], 'second user\'s address is incorrect');
	});

	it('issue 50 more tokens to first user', async () => {
		await instance.issuePoints.sendTransaction(firstUser, 50, '');

		let count = await instance.getParticipantCount.call();
		assert.equal(count.valueOf(), 2, 'number of participants should be 2');

		let participant = await instance.getParticipantAt.call(0);
		assert.equal(participant[0], firstUser, 'first user\'s name is incorrect');
		assert.equal(participant[1].valueOf(), 150, 'first user should have 150 tokens');
		assert.equal(participant[2], accounts[1], 'first user\'s address is incorrect');
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
		assert.equal(participant[2], accounts[2], 'second user\'s address is incorrect');
	});

	it('transfer 25 tokens to second user', async () => {
		let balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 150, 'first account should have 150 tokens');

		await instance.transfer.sendTransaction(accounts[2], 25, { from: accounts[1] });

		balance = await instance.balanceOf.call(accounts[2]);
		assert.equal(balance.valueOf(), 225, 'second account does not have 225 tokens');

		balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 125, 'first account does not have 125 tokens');
	});

	it('transfer more tokens than available', async () => {
		let balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 125, 'first account should have 125 tokens');

		let failed = false;
		try {
			await instance.transfer.sendTransaction(accounts[2], 155, { from: accounts[1] });
		}
		catch (e) {
			failed = true;
		}
		finally {
			if (!failed) {
				assert.fail('tokens should not have been transferred');
			}
		}

		balance = await instance.balanceOf.call(accounts[2]);
		assert.equal(balance.valueOf(), 225, 'second account does not have 225 tokens');

		balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 125, 'first account does not have 125 tokens');
	});

	it('transfer 2^256-1 tokens to the third user', async () => {
		let balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 125, 'first account should have 125 tokens');

		let failed = false;
		try {
			await instance.transfer.sendTransaction(accounts[2], '0xffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff', { from: accounts[1] });
		}
		catch (e) {
			failed = true;
		}
		finally {
			if (!failed) {
				assert.fail('tokens should not have been transferred');
			}
		}

		balance = await instance.balanceOf.call(accounts[2]);
		assert.equal(balance.valueOf(), 225, 'second account does not have 225 tokens');

		balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 125, 'first account does not have 125 tokens');
	});

});
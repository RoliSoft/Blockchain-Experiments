const PassportERC20 = artifacts.require('PassportPointsERC20');

contract('PassportPointsERC20', async (accounts) => {

	var instance;

	before(async () => {
		instance = await PassportERC20.new({ from: accounts[0] })
	});

	it('owner should have total supply initially', async () => {
		let balance = await instance.balanceOf.call(accounts[0]);
		assert.equal(balance.valueOf(), 1000000000, 'owner does not have total supply');
	});

	it('issue 100 tokens to first user', async () => {
		let balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 0, 'second account is not empty');

		await instance.transfer.sendTransaction(accounts[1], 100, { from: accounts[0] });

		balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 100, 'second account does not have 100 tokens');

		balance = await instance.balanceOf.call(accounts[0]);
		assert.equal(balance.valueOf(), 999999900, 'owner should have exactly 1 billion - 100 tokens');
	});

	it('transfer 25 tokens to second user', async () => {
		let balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 100, 'second account should have 100 tokens');

		await instance.transfer.sendTransaction(accounts[2], 25, { from: accounts[1] });

		balance = await instance.balanceOf.call(accounts[2]);
		assert.equal(balance.valueOf(), 25, 'third account does not have 25 tokens');

		balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 75, 'second account does not have 75 tokens');
	});

	it('transfer more tokens than available', async () => {
		let balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 75, 'second account should have 75 tokens');

		let failed = false;
		try {
			await instance.transfer.sendTransaction(accounts[2], 80, { from: accounts[1] });
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
		assert.equal(balance.valueOf(), 25, 'third account does not have 25 tokens');

		balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 75, 'second account does not have 75 tokens');
	});

	it('transfer 2^256-1 tokens to the third user', async () => {
		let balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 75, 'second account should have 75 tokens');

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
		assert.equal(balance.valueOf(), 25, 'third account does not have 25 tokens');

		balance = await instance.balanceOf.call(accounts[1]);
		assert.equal(balance.valueOf(), 75, 'second account does not have 75 tokens');
	});

});
var PassportPointsWeb = artifacts.require('PassportPointsWeb');
var PassportPointsERC20 = artifacts.require('PassportPointsERC20');
var PassportPointsWebERC20 = artifacts.require('PassportPointsWebERC20');

module.exports = function(deployer) {
	deployer.deploy(PassportPointsWeb);
	deployer.deploy(PassportPointsERC20);
	deployer.deploy(PassportPointsWebERC20);
};
using NUnit.Framework;
using System;

[TestFixture]
public class TestRomanNumeral {
	[Test]
	public void TestValidRomanNumerals(
		[Range(1,3999)] int input
	)
	{
		string check = RomanNumerals.RomanNumeralize(input);
		int value = RomanNumerals.RomanDenumeralize(check);
		Assert.AreEqual(input, value);
	}

	[Test]
	public void TestHandledInvalidRomanNumerals(
		[Values(0,4000,100000000)] int input
	)
	{
		string check = RomanNumerals.RomanNumeralize(input);
		int value = RomanNumerals.RomanDenumeralize(check);
		Assert.AreEqual(input, value);
	}
		
	[Test]
	public void TestNegativeValue(
		[Values(-1,-10000)] int input
	)
	{
		string check = RomanNumerals.RomanNumeralize(input);
		int value = RomanNumerals.RomanDenumeralize(check);
		Assert.AreEqual(0, value);
	}

	[Test]
	public void TestInvalidStrings(
		[Values("-1", "CMCM", "CCM", "IIIV", "GG", "CMC", "IIII")] string input
	)
	{
		TestDelegate failingCall = () => { RomanNumerals.RomanDenumeralize(input); };
		Assert.Throws<ArgumentException>(failingCall);
	}
}

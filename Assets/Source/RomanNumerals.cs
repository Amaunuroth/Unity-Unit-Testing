using UnityEngine;
using System;
using System.Collections;
using System.Text;

public static class RomanNumerals
{
	public static string RomanNumeralize(this int number)
	{
		if (number >= 4000)
			return number.ToString();
		StringBuilder sb = new StringBuilder();
		while (number > 0)
		{
			if (number >= 1000)
			{
				number -= 1000;
				sb.Append("M");
			}
			else if (number >= 900)
			{
				number -= 900;
				sb.Append("CM");
			}
			else if (number >= 500)
			{
				number -= 500;
				sb.Append("D");
			}
			else if (number >= 400)
			{
				number -= 400;
				sb.Append("CD");
			}
			else if (number >= 100)
			{
				number -= 100;
				sb.Append("C");
			}
			else if (number >= 90)
			{
				number -= 90;
				sb.Append("XC");
			}
			else if (number >= 50)
			{
				number -= 50;
				sb.Append("L");
			}
			else if (number >= 40)
			{
				number -= 40;
				sb.Append("XL");
			}
			else if (number >= 10)
			{
				number -= 10;
				sb.Append("X");
			}
			else if (number == 9)
			{
				number -= 9;
				sb.Append("IX");
			}
			else if (number >= 5)
			{
				number -= 5;
				sb.Append("V");
			}
			else if (number == 4)
			{
				number -= 4;
				sb.Append("IV");
			}
			else if (number >= 1)
			{
				number -= 1;
				sb.Append("I");
			}
		}
		return sb.ToString();
	}

	public static int RomanDenumeralize(this string numerals)
	{
		if (string.IsNullOrEmpty(numerals))
			return 0;
		if (char.IsNumber(numerals[0]))
			return (int.Parse(numerals));

		int number = 0;
		int previous = 10000;
		int count = 0;
		int i = 0;
		int value = 0;

		while (i < numerals.Length)
		{
			if (numerals[i] == 'M')
			{
				value = 1000;
			}
			else if (numerals[i] == 'D')
			{
				value = 500;
			}
			else if (numerals[i] == 'C')
			{
				if (i < numerals.Length - 1)
				{
					if (numerals[i + 1] == 'M')
					{
						i++;
						value = 900;
					}
					else
					if (numerals[i + 1] == 'D')
					{
						i++;
						value = 400;
					}
					else
					{
						value = 100;
					}
				}
				else
				{
					value = 100;
				}
			}
			else if (numerals[i] == 'L')
			{
				value = 50;
			}
			else if (numerals[i] == 'X')
			{
				if (i < numerals.Length - 1)
				{
					if (numerals[i + 1] == 'C')
					{
						i++;
						value = 90;
					}
					else
					if (numerals[i + 1] == 'L')
					{
						i++;
						value = 40;
					}
					else
					{
						value = 10;
					}
				}
				else
				{
					value = 10;
				}
			}
			else if (numerals[i] == 'V')
			{
				value = 5;
			}
			else if (numerals[i] == 'I')
			{
				if (i < numerals.Length - 1)
				{
					if (numerals[i + 1] == 'X')
					{
						i++;
						value = 9;
					}
					else
					if (numerals[i + 1] == 'V')
					{
						i++;
						value = 4;
					}
					else
					{
						value = 1;
					}
				}
				else
				{
					value = 1;
				}
			}
			else
			{
				throw new ArgumentException("Input string contained an unknown character: " + numerals[i]);
			}

			if (previous < value)
				throw new ArgumentException("Input string contains higher value following shorter value!");
			if (value > 1 && previous == value && NonZeroDigit(value) % 5 != 1)
				throw new ArgumentException("Input string contains multiple special pairs of the same type!");
			int prevNonZero = NonZeroDigit(previous);
			if (prevNonZero > 1 && prevNonZero % 5 != 0)
			{
				if (Math.Floor(Math.Log10(previous)) <= Math.Floor(Math.Log10(value)))
					throw new ArgumentException("Input string contains a non-power separated special set and following characters!");
			}
			if (previous != value)
				count = 0;
			previous = value;
			count++;
			if (count > 3)
				throw new ArgumentException("Input string is not a valid Roman Numeral - no more than 3 values of a category should exist");

			number += value;

			i++;
		}
		return number;
	}

	static int NonZeroDigit(int value)
	{
		while (value >= 10)
			value /= 10;
		return value;
	}
}

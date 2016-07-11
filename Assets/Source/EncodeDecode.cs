using System.Collections;
using System.Collections.Generic;
using System.Text;

public class EncodeDecode
{
    // 3x3ddddddab
    // dddddddddx3hab
    // 111111111x2ab
    // aabbcc
    public static string Encode(string input)
    {
        StringBuilder sb = new StringBuilder();

        int count = 0;
        char? current = null;
        bool trailing = false;

        for (int i = 0; i < input.Length; i++)
        {
            if (i < input.Length - 2 && input[i] == input[i + 1] && input[i + 1] == input[i + 2])
            {
                if (count < 3)
                {
                    current = input[i];
                    count = 3;
                }
                else
                {
                    count++;
                }
            }
            else if (count > 0)
            {
                sb.Append(string.Format("{0}x{1}", current.Value, count));
                count = -1;
            }
            else if (count < 0)
            {
                count++;
                trailing = true;
            }
            else if (i < input.Length - 2 && input[i + 1] == 'x' && char.GetNumericValue(input[i+2]) >= 0)
            {
                sb.Append(string.Format("{0}x1", input[i]));
                trailing = true;
            }
            else if (trailing && char.GetNumericValue(input[i]) >= 0)
            {
                sb.Append(string.Format("{0}x1", input[i]));
            }
            else
            {
                trailing = false;
                sb.Append(input[i]);
            }
        }

        return sb.ToString();
    }

    public static string Decode(string input)
    {
        StringBuilder sb = new StringBuilder();
        int i = 0;
        int multiplier = 0;
        char? current = null;
        while (i < input.Length)
        {
            if (i < input.Length - 2 && input[i + 1] == 'x' && char.GetNumericValue(input[i + 2]) >= 0)
            {
                if (multiplier > 0)
                {
                    for (int j = 0; j < multiplier; j++)
                    {
                        sb.Append(current.Value);
                    }
                }
                current = input[i];
                multiplier = (int)char.GetNumericValue(input[i + 2]);
                i += 3;
            }
            else if (multiplier > 0 && char.GetNumericValue(input[i]) >= 0)
            {
                multiplier *= 10;
                multiplier += (int)char.GetNumericValue(input[i]);
                i++;
            }
            else
            {
                if (multiplier > 0)
                {
                    for (int j = 0; j < multiplier; j++)
                    {
                        sb.Append(current.Value);
                    }
                    multiplier = 0;
                }
                sb.Append(input[i]);
                i++;
            }
        }

        return sb.ToString();
    }
}

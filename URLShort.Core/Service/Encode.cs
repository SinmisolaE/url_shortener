using System;
using Microsoft.VisualBasic;
using URLShort.API.Interfaces;

namespace URLShort.API.Service;

public class Encode : IEncode
{
    private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMONPQRSTUVWXYZ";
        
    private static readonly int Base = Alphabet.Length;


    public string EncodeValue(int value)
    {
        if (value == 0)
        {
            return Alphabet[0].ToString();
        }

        var s = new Stack<char>();

        while (value > 0)
        {
            s.Push(Alphabet[value % Base]);

            value /= Base;
        }

        return string.Join("", s);

    }

}

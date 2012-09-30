using System;
using System.IO;

namespace twistedsharp.Utils
{
	public class EncodingHelper
	{
		public static byte[] GetBytes(string s)
		{
			byte[] bytes = new byte[s.Length];
			for(int i = 0; i < s.Length; i++)
			{
				bytes[i] = (byte)s[i];
			}
			return bytes;
		}

		public static string GetString(byte[] bytes)
		{
			return GetString(bytes, 0, bytes.Length);
		}
		public static string GetString(byte[] bytes, int index, int count)
		{
			string s = "";
			for(int i = index; i < index + count; i++)
			{
				s += (char)bytes[i];
			}
			return s;
		}
	}
}


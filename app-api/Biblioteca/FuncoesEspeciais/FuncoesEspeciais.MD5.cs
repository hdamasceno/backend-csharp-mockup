using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Biblioteca
{
	public static partial class FuncoesEspeciais
	{
		public static string MD5_String(string chave)
		{
			System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(chave));
			StringBuilder sBuilder = new StringBuilder();

			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}

		public static string MD5_Arquivo(string pathSrc)
		{
			String md5Result;
			var sb = new StringBuilder();
			var md5Hasher = MD5.Create();

			using (var fs = File.OpenRead(pathSrc))
			{
				foreach (Byte b in md5Hasher.ComputeHash(fs))
					sb.Append(b.ToString("x2").ToLower());
			}

			md5Result = sb.ToString();

			return md5Result;
		}
	}
}

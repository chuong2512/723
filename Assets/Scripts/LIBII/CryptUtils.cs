using SevenZip.Compression.LZMA;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

namespace LIBII
{
	public sealed class CryptUtils
	{
		[ExecuteInEditMode]
		public class MD5Thread : MonoBehaviour
		{
			private Thread thread;

			private ED_1Param<string> onFinished;

			private byte[] bytes;

			public static void Create(Thread thread, byte[] bytes, ED_1Param<string> onfinished)
			{
				CryptUtils.MD5Thread mD5Thread = new GameObject("__MD5Thread__").AddComponent<CryptUtils.MD5Thread>();
				mD5Thread.thread = thread;
				mD5Thread.onFinished = onfinished;
				mD5Thread.bytes = bytes;
			}

			private void Start()
			{
				this.thread.Start(this.bytes);
			}

			private void Update()
			{
				if (!this.thread.IsAlive)
				{
					if (this.onFinished != null)
					{
						this.onFinished(CryptUtils.lastMd5Value);
					}
					UnityEngine.Object.DestroyImmediate(base.gameObject);
				}
			}
		}

		[ExecuteInEditMode]
		public class LZMAThread : MonoBehaviour
		{
			private string mName;

			private Thread mThread;

			private ED_1Param<string> onFinished;

			private string[] mFilePaths = new string[]
			{
				string.Empty,
				string.Empty
			};

			public static void Create(string name, Thread thread, ED_1Param<string> onfinished, string inFile, string outFile)
			{
				CryptUtils.LZMAThread lZMAThread = new GameObject("__LZMAThread__").AddComponent<CryptUtils.LZMAThread>();
				lZMAThread.mName = name;
				lZMAThread.mThread = thread;
				lZMAThread.onFinished = onfinished;
				lZMAThread.mFilePaths[0] = inFile;
				lZMAThread.mFilePaths[1] = outFile;
			}

			private void Start()
			{
				this.mThread.Start(this.mFilePaths);
			}

			private void Update()
			{
				if (!this.mThread.IsAlive)
				{
					if (this.onFinished != null)
					{
						this.onFinished(this.mName);
					}
					UnityEngine.Object.DestroyImmediate(base.gameObject);
				}
			}
		}

		public enum EncryptType
		{
			C = 1,
			T,
			N
		}

		private static string lastMd5Value = string.Empty;

		private static uint[] crcTable = new uint[]
		{
			0u,
			1996959894u,
			3993919788u,
			2567524794u,
			124634137u,
			1886057615u,
			3915621685u,
			2657392035u,
			249268274u,
			2044508324u,
			3772115230u,
			2547177864u,
			162941995u,
			2125561021u,
			3887607047u,
			2428444049u,
			498536548u,
			1789927666u,
			4089016648u,
			2227061214u,
			450548861u,
			1843258603u,
			4107580753u,
			2211677639u,
			325883990u,
			1684777152u,
			4251122042u,
			2321926636u,
			335633487u,
			1661365465u,
			4195302755u,
			2366115317u,
			997073096u,
			1281953886u,
			3579855332u,
			2724688242u,
			1006888145u,
			1258607687u,
			3524101629u,
			2768942443u,
			901097722u,
			1119000684u,
			3686517206u,
			2898065728u,
			853044451u,
			1172266101u,
			3705015759u,
			2882616665u,
			651767980u,
			1373503546u,
			3369554304u,
			3218104598u,
			565507253u,
			1454621731u,
			3485111705u,
			3099436303u,
			671266974u,
			1594198024u,
			3322730930u,
			2970347812u,
			795835527u,
			1483230225u,
			3244367275u,
			3060149565u,
			1994146192u,
			31158534u,
			2563907772u,
			4023717930u,
			1907459465u,
			112637215u,
			2680153253u,
			3904427059u,
			2013776290u,
			251722036u,
			2517215374u,
			3775830040u,
			2137656763u,
			141376813u,
			2439277719u,
			3865271297u,
			1802195444u,
			476864866u,
			2238001368u,
			4066508878u,
			1812370925u,
			453092731u,
			2181625025u,
			4111451223u,
			1706088902u,
			314042704u,
			2344532202u,
			4240017532u,
			1658658271u,
			366619977u,
			2362670323u,
			4224994405u,
			1303535960u,
			984961486u,
			2747007092u,
			3569037538u,
			1256170817u,
			1037604311u,
			2765210733u,
			3554079995u,
			1131014506u,
			879679996u,
			2909243462u,
			3663771856u,
			1141124467u,
			855842277u,
			2852801631u,
			3708648649u,
			1342533948u,
			654459306u,
			3188396048u,
			3373015174u,
			1466479909u,
			544179635u,
			3110523913u,
			3462522015u,
			1591671054u,
			702138776u,
			2966460450u,
			3352799412u,
			1504918807u,
			783551873u,
			3082640443u,
			3233442989u,
			3988292384u,
			2596254646u,
			62317068u,
			1957810842u,
			3939845945u,
			2647816111u,
			81470997u,
			1943803523u,
			3814918930u,
			2489596804u,
			225274430u,
			2053790376u,
			3826175755u,
			2466906013u,
			167816743u,
			2097651377u,
			4027552580u,
			2265490386u,
			503444072u,
			1762050814u,
			4150417245u,
			2154129355u,
			426522225u,
			1852507879u,
			4275313526u,
			2312317920u,
			282753626u,
			1742555852u,
			4189708143u,
			2394877945u,
			397917763u,
			1622183637u,
			3604390888u,
			2714866558u,
			953729732u,
			1340076626u,
			3518719985u,
			2797360999u,
			1068828381u,
			1219638859u,
			3624741850u,
			2936675148u,
			906185462u,
			1090812512u,
			3747672003u,
			2825379669u,
			829329135u,
			1181335161u,
			3412177804u,
			3160834842u,
			628085408u,
			1382605366u,
			3423369109u,
			3138078467u,
			570562233u,
			1426400815u,
			3317316542u,
			2998733608u,
			733239954u,
			1555261956u,
			3268935591u,
			3050360625u,
			752459403u,
			1541320221u,
			2607071920u,
			3965973030u,
			1969922972u,
			40735498u,
			2617837225u,
			3943577151u,
			1913087877u,
			83908371u,
			2512341634u,
			3803740692u,
			2075208622u,
			213261112u,
			2463272603u,
			3855990285u,
			2094854071u,
			198958881u,
			2262029012u,
			4057260610u,
			1759359992u,
			534414190u,
			2176718541u,
			4139329115u,
			1873836001u,
			414664567u,
			2282248934u,
			4279200368u,
			1711684554u,
			285281116u,
			2405801727u,
			4167216745u,
			1634467795u,
			376229701u,
			2685067896u,
			3608007406u,
			1308918612u,
			956543938u,
			2808555105u,
			3495958263u,
			1231636301u,
			1047427035u,
			2932959818u,
			3654703836u,
			1088359270u,
			936918000u,
			2847714899u,
			3736837829u,
			1202900863u,
			817233897u,
			3183342108u,
			3401237130u,
			1404277552u,
			615818150u,
			3134207493u,
			3453421203u,
			1423857449u,
			601450431u,
			3009837614u,
			3294710456u,
			1567103746u,
			711928724u,
			3020668471u,
			3272380065u,
			1510334235u,
			755167117u
		};

		private static ParameterizedThreadStart __f__mg_cache0;

		private static ParameterizedThreadStart __f__mg_cache1;

		private static ParameterizedThreadStart __f__mg_cache2;

		public static string Md5FromFile(string filePath)
		{
			return CryptUtils.Md5(File.ReadAllBytes(filePath));
		}

		public static string Md5(string text)
		{
			return CryptUtils.Md5(Encoding.Default.GetBytes(text));
		}

		public static string Md5(string text, Encoding encoding)
		{
			return CryptUtils.Md5(encoding.GetBytes(text));
		}

		public static string Md5(byte[] bytes)
		{
			byte[] array = new MD5CryptoServiceProvider().ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		public static void Md5FromFileAsyn(string filePath, ED_1Param<string> onfinished)
		{
			CryptUtils.Md5Asyn(File.ReadAllBytes(filePath), onfinished);
		}

		public static void Md5Asyn(byte[] bytes, ED_1Param<string> onfinished)
		{
			if (CryptUtils.__f__mg_cache0 == null)
			{
				CryptUtils.__f__mg_cache0 = new ParameterizedThreadStart(CryptUtils.OnMd5Process);
			}
			CryptUtils.MD5Thread.Create(new Thread(CryptUtils.__f__mg_cache0), bytes, onfinished);
		}

		private static void OnMd5Process(object obj)
		{
			byte[] bytes = (byte[])obj;
			CryptUtils.lastMd5Value = CryptUtils.Md5(bytes);
		}

		public static string CRC32Base64FromFile(string filePath)
		{
			return Convert.ToBase64String(BitConverter.GetBytes(CryptUtils.CRC32FromFile(filePath)));
		}

		public static string CRC32Base64(string text)
		{
			return Convert.ToBase64String(BitConverter.GetBytes(CryptUtils.CRC32(text)));
		}

		public static string CRC32Base64(string text, Encoding encoding)
		{
			return Convert.ToBase64String(BitConverter.GetBytes(CryptUtils.CRC32(text, encoding)));
		}

		public static string CRC32Base64(byte[] bytes)
		{
			return Convert.ToBase64String(BitConverter.GetBytes(CryptUtils.CRC32(bytes)));
		}

		public static int CRC32FromFile(string filePath)
		{
			return CryptUtils.CRC32(File.ReadAllBytes(filePath));
		}

		public static int CRC32(string text)
		{
			return CryptUtils.CRC32(Encoding.Default.GetBytes(text));
		}

		public static int CRC32(string text, Encoding encoding)
		{
			return CryptUtils.CRC32(encoding.GetBytes(text));
		}

		public static int CRC32(byte[] bytes)
		{
			int num = bytes.Length;
			uint num2 = 4294967295u;
			for (int i = 0; i < num; i++)
			{
				num2 = ((num2 >> 8 & 16777215u) ^ CryptUtils.crcTable[(int)((UIntPtr)((num2 ^ (uint)bytes[i]) & 255u))]);
			}
			return (int)(num2 ^ 4294967295u);
		}

		private static void OnLZMACompress(object obj)
		{
			string[] array = (string[])obj;
			CryptUtils.CompressFileLZMA(array[0], array[1]);
		}

		private static void OnLZMADecompress(object obj)
		{
			string[] array = (string[])obj;
			CryptUtils.DecompressFileLZMA(array[0], array[1]);
		}

		public static void CompressFileLZMAAsyn(string inFile, string outFile, string key, ED_1Param<string> onFinished)
		{
			if (CryptUtils.__f__mg_cache1 == null)
			{
				CryptUtils.__f__mg_cache1 = new ParameterizedThreadStart(CryptUtils.OnLZMACompress);
			}
			CryptUtils.LZMAThread.Create(key, new Thread(CryptUtils.__f__mg_cache1), onFinished, inFile, outFile);
		}

		public static void DecompressFileLZMAAsyn(string inFile, string outFile, string key, ED_1Param<string> onFinished)
		{
			if (CryptUtils.__f__mg_cache2 == null)
			{
				CryptUtils.__f__mg_cache2 = new ParameterizedThreadStart(CryptUtils.OnLZMADecompress);
			}
			CryptUtils.LZMAThread.Create(key, new Thread(CryptUtils.__f__mg_cache2), onFinished, inFile, outFile);
		}

		public static void CompressFileLZMA(string inFile, string outFile)
		{
			SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
			FileStream fileStream = new FileStream(inFile, FileMode.Open);
			FileStream fileStream2 = new FileStream(outFile, FileMode.Create);
			encoder.WriteCoderProperties(fileStream2);
			fileStream2.Write(BitConverter.GetBytes(fileStream.Length), 0, 8);
			encoder.Code(fileStream, fileStream2, fileStream.Length, -1L, null);
			fileStream2.Flush();
			fileStream2.Close();
			fileStream.Close();
		}

		public static void DecompressFileLZMA(string inFile, string outFile)
		{
			SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
			FileStream fileStream = new FileStream(inFile, FileMode.Open);
			FileStream fileStream2 = new FileStream(outFile, FileMode.Create);
			byte[] array = new byte[5];
			byte[] array2 = new byte[8];
			fileStream.Read(array, 0, 5);
			fileStream.Read(array2, 0, 8);
			long outSize = BitConverter.ToInt64(array2, 0);
			decoder.SetDecoderProperties(array);
			decoder.Code(fileStream, fileStream2, fileStream.Length, outSize, null);
			fileStream2.Flush();
			fileStream2.Close();
			fileStream.Close();
		}

		public static bool IsLZMACompressFile(string filePath)
		{
			FileStream fileStream = File.Open(filePath, FileMode.Open);
			byte[] array = new byte[4];
			fileStream.Read(array, 0, 4);
			fileStream.Close();
			return BitConverter.ToUInt32(array, 0) == 1073741917u;
		}

		private static string EncryptC(string content, string pw, string iv)
		{
			byte[] array = Encoding.UTF8.GetBytes(content);
			for (int i = 0; i < array.Length; i += 2)
			{
				byte[] expr_1A_cp_0 = array;
				int expr_1A_cp_1 = i;
				expr_1A_cp_0[expr_1A_cp_1] ^= 10;
			}
			for (int j = 1; j < array.Length; j += 2)
			{
				byte[] expr_3C_cp_0 = array;
				int expr_3C_cp_1 = j;
				expr_3C_cp_0[expr_3C_cp_1] ^= 12;
			}
			array = CryptUtils.TripleDESEncrypt(array, pw, iv);
			return Convert.ToBase64String(array);
		}

		private static string DecryptC(string content, string pw, string iv)
		{
			byte[] array = Convert.FromBase64String(content);
			array = CryptUtils.TripleDESDecrypt(array, pw, iv);
			for (int i = 0; i < array.Length; i += 2)
			{
				byte[] expr_1E_cp_0 = array;
				int expr_1E_cp_1 = i;
				expr_1E_cp_0[expr_1E_cp_1] ^= 10;
			}
			for (int j = 1; j < array.Length; j += 2)
			{
				byte[] expr_40_cp_0 = array;
				int expr_40_cp_1 = j;
				expr_40_cp_0[expr_40_cp_1] ^= 12;
			}
			return Encoding.UTF8.GetString(array);
		}

		private static string EncryptT(string content, string pw, string iv)
		{
			byte[] array = Encoding.UTF8.GetBytes(content);
			for (int i = 0; i < array.Length; i += 2)
			{
				byte[] expr_1A_cp_0 = array;
				int expr_1A_cp_1 = i;
				expr_1A_cp_0[expr_1A_cp_1] ^= 9;
			}
			for (int j = 1; j < array.Length; j += 2)
			{
				byte[] expr_3C_cp_0 = array;
				int expr_3C_cp_1 = j;
				expr_3C_cp_0[expr_3C_cp_1] ^= 7;
			}
			array = CryptUtils.AESEncrypt(array, pw, iv);
			return Convert.ToBase64String(array);
		}

		private static string DecryptT(string content, string pw, string iv)
		{
			byte[] array = Convert.FromBase64String(content);
			array = CryptUtils.AESDecrypt(array, pw, iv);
			for (int i = 0; i < array.Length; i += 2)
			{
				byte[] expr_1E_cp_0 = array;
				int expr_1E_cp_1 = i;
				expr_1E_cp_0[expr_1E_cp_1] ^= 9;
			}
			for (int j = 1; j < array.Length; j += 2)
			{
				byte[] expr_40_cp_0 = array;
				int expr_40_cp_1 = j;
				expr_40_cp_0[expr_40_cp_1] ^= 7;
			}
			return Encoding.UTF8.GetString(array);
		}

		public static string Encrypt(string content, string pw, string iv, string encryptType)
		{
			CryptUtils.EncryptType type = (CryptUtils.EncryptType)Enum.Parse(typeof(CryptUtils.EncryptType), encryptType);
			return CryptUtils.Encrypt(content, pw, iv, type);
		}

		public static string Decrypt(string content, string pw, string iv, string decryptType)
		{
			CryptUtils.EncryptType type = (CryptUtils.EncryptType)Enum.Parse(typeof(CryptUtils.EncryptType), decryptType);
			return CryptUtils.Decrypt(content, pw, iv, type);
		}

		public static string Encrypt(string content, string pw = "tripledesencryptionkey12", string iv = "12345678", CryptUtils.EncryptType type = CryptUtils.EncryptType.C)
		{
			string result = string.Empty;
			if (type != CryptUtils.EncryptType.C)
			{
				if (type != CryptUtils.EncryptType.T)
				{
					if (type == CryptUtils.EncryptType.N)
					{
						result = content;
					}
				}
				else
				{
					result = CryptUtils.EncryptT(content, pw, iv);
				}
			}
			else
			{
				result = CryptUtils.EncryptC(content, pw, iv);
			}
			return result;
		}

		public static string Decrypt(string content, string pw = "tripledesencryptionkey12", string iv = "12345678", CryptUtils.EncryptType type = CryptUtils.EncryptType.C)
		{
			string result = string.Empty;
			if (type != CryptUtils.EncryptType.C)
			{
				if (type != CryptUtils.EncryptType.T)
				{
					if (type == CryptUtils.EncryptType.N)
					{
						result = content;
					}
				}
				else
				{
					result = CryptUtils.DecryptT(content, pw, iv);
				}
			}
			else
			{
				result = CryptUtils.DecryptC(content, pw, iv);
			}
			return result;
		}

		private static byte[] TripleDESEncrypt(byte[] data, string key, string iv)
		{
			return new TripleDESCryptoServiceProvider
			{
				Key = Encoding.ASCII.GetBytes(key),
				IV = Encoding.ASCII.GetBytes(iv),
				Mode = CipherMode.CBC,
				Padding = PaddingMode.PKCS7
			}.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
		}

		private static byte[] TripleDESDecrypt(byte[] data, string key, string iv)
		{
			return new TripleDESCryptoServiceProvider
			{
				Key = Encoding.ASCII.GetBytes(key),
				IV = Encoding.ASCII.GetBytes(iv),
				Mode = CipherMode.CBC,
				Padding = PaddingMode.PKCS7
			}.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
		}

		private static byte[] AESEncrypt(byte[] data, string key, string iv)
		{
			Rijndael rijndael = Rijndael.Create();
			rijndael.Key = Encoding.ASCII.GetBytes(key);
			rijndael.IV = Encoding.ASCII.GetBytes(iv);
			rijndael.Mode = CipherMode.CBC;
			rijndael.Padding = PaddingMode.PKCS7;
			return rijndael.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
		}

		private static byte[] AESDecrypt(byte[] data, string key, string iv)
		{
			Rijndael rijndael = Rijndael.Create();
			rijndael.Key = Encoding.ASCII.GetBytes(key);
			rijndael.IV = Encoding.ASCII.GetBytes(iv);
			rijndael.Mode = CipherMode.CBC;
			rijndael.Padding = PaddingMode.PKCS7;
			return rijndael.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
		}
	}
}

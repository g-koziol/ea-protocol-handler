/*
   Copyright 2009 Canonic Corp

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace EA.ProtocolHandler.Utility
{
	/// <summary>
	/// Handles binary operations such as compression and encryption.
	/// </summary>
	/// <remarks>This assembly directly includes unmodified portions of the ICSharpCode.SharpZipLib library
	/// rather than linking to it, because an external DLL link would be vulnerable to having Canonic's
	/// proprietary keys intercepted.</remarks>
	public static class Binary
	{
		private static byte[] DICT_1 =
            {
                #region Bytes
                (byte)'1', (byte)'\0', (byte)'2', (byte)'\0', (byte)'3', (byte)'\0', (byte)'4', (byte)'\0', 
                (byte)'5', (byte)'\0', (byte)'6', (byte)'\0', (byte)'7', (byte)'\0', (byte)'8', (byte)'\0', 
                (byte)'9', (byte)'\0', (byte)'0', (byte)'\0', (byte)'a', (byte)'\0', (byte)'b', (byte)'\0', 
                (byte)'c', (byte)'\0', (byte)'d', (byte)'\0', (byte)'e', (byte)'\0', (byte)'f', (byte)'\0', 
                (byte)'g', (byte)'\0', (byte)'h', (byte)'\0', (byte)'i', (byte)'\0', (byte)'j', (byte)'\0', 
                (byte)'k', (byte)'\0', (byte)'l', (byte)'\0', (byte)'m', (byte)'\0', (byte)'n', (byte)'\0', 
                (byte)'o', (byte)'\0', (byte)'p', (byte)'\0', (byte)'q', (byte)'\0', (byte)'r', (byte)'\0', 
                (byte)'s', (byte)'\0', (byte)'t', (byte)'\0', (byte)'u', (byte)'\0', (byte)'v', (byte)'\0', 
                (byte)'w', (byte)'\0', (byte)'x', (byte)'\0', (byte)'y', (byte)'\0', (byte)'z', (byte)'\0', 
                (byte)'A', (byte)'\0', (byte)'B', (byte)'\0', (byte)'C', (byte)'\0', (byte)'D', (byte)'\0', 
                (byte)'E', (byte)'\0', (byte)'F', (byte)'\0', (byte)'G', (byte)'\0', (byte)'H', (byte)'\0', 
                (byte)'I', (byte)'\0', (byte)'J', (byte)'\0', (byte)'K', (byte)'\0', (byte)'L', (byte)'\0', 
                (byte)'M', (byte)'\0', (byte)'N', (byte)'\0', (byte)'O', (byte)'\0', (byte)'P', (byte)'\0', 
                (byte)'Q', (byte)'\0', (byte)'R', (byte)'\0', (byte)'S', (byte)'\0', (byte)'T', (byte)'\0', 
                (byte)'U', (byte)'\0', (byte)'V', (byte)'\0', (byte)'W', (byte)'\0', (byte)'X', (byte)'\0', 
                (byte)'Y', (byte)'\0', (byte)'X', (byte)'\0', (byte)'Z', (byte)'\0', (byte)'{', (byte)'\0', 
                (byte)'}', (byte)'\0', (byte)'.', (byte)'\0', (byte)';', (byte)'\0', (byte)'\x3d', (byte)'\0', 
                (byte)'-', (byte)'\0', (byte)'y', (byte)'e', (byte)'s', (byte)'\0', (byte)'n', (byte)'o', (byte)'\0', 
                (byte)'T', (byte)'r', (byte)'u', (byte)'e', (byte)'\0', (byte)'F', (byte)'a', (byte)'l', (byte)'s', (byte)'e', (byte)'\0', 
                (byte)'S', (byte)'S', (byte)'P', (byte)'I', (byte)'\0', (byte)'D', (byte)'B', (byte)'T', (byte)'y', (byte)'p', (byte)'e', (byte)'\x3d', (byte)'1', (byte)'\0', 
                (byte)'D', (byte)'B', (byte)'T', (byte)'y', (byte)'p', (byte)'e', (byte)'\x3d', (byte)'4', (byte)'\0', 
                (byte)'D', (byte)'a', (byte)'t', (byte)'a', (byte)'\x20', (byte)'S', (byte)'o', (byte)'u', (byte)'r', (byte)'c', (byte)'e', (byte)'\x3d', (byte)'\0', 
                (byte)'D', (byte)'a', (byte)'t', (byte)'a', (byte)'\x20', (byte)'S', (byte)'o', (byte)'u', (byte)'r', (byte)'c', (byte)'e', (byte)'\x3d', (byte)'T', (byte)'O', (byte)'R', (byte)'C', (byte)'L', (byte)'\0', 
                (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'\0', 
                (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'m', (byte)'s', (byte)'d', (byte)'a', (byte)'o', (byte)'r', (byte)'a', (byte)'\0', 
                (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'S', (byte)'Q', (byte)'L', (byte)'O', (byte)'L', (byte)'E', (byte)'D', (byte)'B', (byte)'.', (byte)'1', (byte)'\0', 
                (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'S', (byte)'Q', (byte)'L', (byte)'N', (byte)'C', (byte)'L', (byte)'I', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'{', (byte)'M', (byte)'i', (byte)'c', (byte)'r', (byte)'o', (byte)'s', (byte)'o', (byte)'f', (byte)'t', (byte)'\x20', (byte)'O', (byte)'D', (byte)'B', (byte)'C', (byte)'\x20', (byte)'f', (byte)'o', (byte)'r', (byte)'\x20', (byte)'O', (byte)'r', (byte)'a', (byte)'c', (byte)'l', (byte)'e', (byte)'}', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'(', (byte)'O', (byte)'r', (byte)'a', (byte)'c', (byte)'l', (byte)'e', (byte)'\x20', (byte)'i', (byte)'n', (byte)'\x20', (byte)'X', (byte)'E', (byte)'C', (byte)'l', (byte)'i', (byte)'e', (byte)'n', (byte)'t', (byte)')', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'{', (byte)'P', (byte)'o', (byte)'s', (byte)'t', (byte)'g', (byte)'r', (byte)'e', (byte)'S', (byte)'Q', (byte)'L', (byte)'}', (byte)';', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'{', (byte)'S', (byte)'Q', (byte)'L', (byte)'\x20', (byte)'N', (byte)'a', (byte)'t', (byte)'i', (byte)'v', (byte)'e', (byte)'\x20', (byte)'C', (byte)'l', (byte)'i', (byte)'e', (byte)'n', (byte)'t', (byte)'}', (byte)'\0', 
                (byte)';', (byte)'A', (byte)'s', (byte)'y', (byte)'n', (byte)'c', (byte)'h', (byte)'r', (byte)'o', (byte)'n', (byte)'o', (byte)'u', (byte)'s', (byte)'\x20', (byte)'P', (byte)'r', (byte)'o', (byte)'c', (byte)'e', (byte)'s', (byte)'s', (byte)'i', (byte)'n', (byte)'g', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'A', (byte)'t', (byte)'t', (byte)'a', (byte)'c', (byte)'h', (byte)'D', (byte)'b', (byte)'F', (byte)'i', (byte)'l', (byte)'e', (byte)'n', (byte)'a', (byte)'m', (byte)'e', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'C', (byte)'o', (byte)'n', (byte)'n', (byte)'e', (byte)'c', (byte)'t', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'C', (byte)'o', (byte)'n', (byte)'n', (byte)'e', (byte)'c', (byte)'t', (byte)'S', (byte)'t', (byte)'r', (byte)'i', (byte)'n', (byte)'g', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'D', (byte)'a', (byte)'t', (byte)'a', (byte)'\x20', (byte)'S', (byte)'o', (byte)'u', (byte)'r', (byte)'c', (byte)'e', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'D', (byte)'a', (byte)'t', (byte)'a', (byte)'b', (byte)'a', (byte)'s', (byte)'e', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'d', (byte)'b', (byte)'q', (byte)'\x3d', (byte)'\0', (byte)';', (byte)'E', (byte)'n', (byte)'c', (byte)'r', (byte)'y', (byte)'p', (byte)'t', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'I', (byte)'n', (byte)'i', (byte)'t', (byte)'i', (byte)'a', (byte)'l', (byte)'\x20', (byte)'C', (byte)'a', (byte)'t', (byte)'a', (byte)'l', (byte)'o', (byte)'g', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'I', (byte)'n', (byte)'t', (byte)'e', (byte)'g', (byte)'r', (byte)'a', (byte)'t', (byte)'e', (byte)'d', (byte)'\x20', (byte)'S', (byte)'e', (byte)'c', (byte)'u', (byte)'r', (byte)'i', (byte)'t', (byte)'y', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'O', (byte)'S', (byte)'A', (byte)'u', (byte)'t', (byte)'h', (byte)'e', (byte)'n', (byte)'t', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'P', (byte)'a', (byte)'s', (byte)'s', (byte)'w', (byte)'o', (byte)'r', (byte)'d', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'P', (byte)'e', (byte)'r', (byte)'s', (byte)'i', (byte)'s', (byte)'t', (byte)'\x20', (byte)'S', (byte)'e', (byte)'c', (byte)'u', (byte)'r', (byte)'i', (byte)'t', (byte)'y', (byte)'\x20', (byte)'I', (byte)'n', (byte)'f', (byte)'o', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'P', (byte)'w', (byte)'d', (byte)'\x3d', (byte)'\0', (byte)';', (byte)'S', (byte)'e', (byte)'r', (byte)'v', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'T', (byte)'r', (byte)'u', (byte)'s', (byte)'t', (byte)'e', (byte)'d', (byte)'_', (byte)'C', (byte)'o', (byte)'n', (byte)'n', (byte)'e', (byte)'c', (byte)'t', (byte)'i', (byte)'o', (byte)'n', (byte)'\x3d', (byte)'\0', 
                (byte)';', (byte)'U', (byte)'i', (byte)'d', (byte)'\x3d', (byte)'\0', (byte)';', (byte)'U', (byte)'s', (byte)'e', (byte)'r', (byte)'\x20', (byte)'I', (byte)'D', (byte)'\x3d', (byte)'\0', 
                (byte)'D', (byte)'B', (byte)'T', (byte)'y', (byte)'p', (byte)'e', (byte)'\x3d', (byte)'1', (byte)';', (byte)'C', (byte)'o', (byte)'n', (byte)'n', (byte)'e', (byte)'c', (byte)'t', (byte)'\x3d', (byte)';', (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'\x3d', (byte)'S', (byte)'Q', (byte)'L', (byte)'O', (byte)'L', (byte)'E', (byte)'D', (byte)'B', (byte)'.', (byte)'1', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'1', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'2', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'3', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'4', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'5', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'6', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'7', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'8', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'9', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'0', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'a', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'b', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'c', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'d', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'e', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'f', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'g', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'h', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'i', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'j', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'k', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'l', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'m', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'n', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'o', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'p', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'q', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'r', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'s', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'t', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'u', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'v', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'w', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'x', (byte)';', (byte)'\0', 
                (byte)'\x24', (byte)'\x3d', (byte)'y', (byte)';', (byte)'\0', (byte)'\x24', (byte)'\x3d', (byte)'z', (byte)';', (byte)'\0'
                #endregion
            };

		/// <summary>
		/// 32 byte (256 bit) key.
		/// </summary>
		private static byte[] KEY =
            {
                #region Bytes
                (byte)'\x1d', (byte)'í', (byte)'e', (byte)'\x08', (byte)'\x07', (byte)';', (byte)'[', 
                (byte)'n', (byte)'Â', (byte)'\x05', (byte)'Â', (byte)'f', (byte)'\x93', (byte)'S', 
                (byte)'?', (byte)'@', (byte)'k', (byte)'\x05', (byte)'·', (byte)'\x0c', (byte)'Û', 
                (byte)'¡', (byte)'\x2b', (byte)'\x18', (byte)'_', (byte)'Ä', (byte)'\x19', (byte)'v', 
                (byte)'B', (byte)'\x89', (byte)'\x03', (byte)'\xbc'
                #endregion
            };

		/// <summary>
		/// 16 byte (128 bit) initial vector.
		/// </summary>
		private static byte[] IV =
            {
                #region Bytes
                (byte)'\x05', (byte)'\x86', (byte)'"', (byte)'Ý', (byte)'\xae', (byte)'æ', (byte)'Û', (byte)'\x01', 
                (byte)'Ò', (byte)'*', (byte)'\x8c', (byte)'M', (byte)'\x24', (byte)'\x9d', (byte)'\xbe', (byte)'\x88', 
                #endregion
            };

		/// <summary>
		/// Encrypts the specified string.
		/// </summary>
		/// <param name="orig">The string.</param>
		/// <returns></returns>
		public static string Encrypt( string orig )
		{
			string abbr = Abbreviate( orig );
			byte[] comp = CompressString( abbr );
			byte[] cryp = EncryptBytes( comp );
			string b64 = Convert.ToBase64String( cryp, Base64FormattingOptions.None );

			return b64;
		}

		/// <summary>
		/// Decrypts the specified  base64 string.
		/// </summary>
		/// <param name="b64">The base64 string.</param>
		/// <returns></returns>
		public static string Decrypt( string b64 )
		{
			byte[] cryp = Convert.FromBase64String( b64 );
			byte[] comp = DecryptBytes( cryp );
			string dcom = DecompressBytes( comp );
			string deab = Deabbreviate( dcom );
			return deab;
		}

		private static string Abbreviate( string orig )
		{
			// DB Specific
			orig = orig.Replace( "DBType=4;Connect=Provider=MSDASQL.1", "$=1;" );
			orig = orig.Replace( "DBType=4;Connect=;Provider=MSDASQL.1", "$=1;" );
			orig = orig.Replace( "DBType=1;Connect=Provider=SQLOLEDB.1", "$=2;" );
			orig = orig.Replace( "DBType=1;Connect=;Provider=SQLOLEDB.1", "$=2;" );
			orig = orig.Replace( "DBType=3;Connect=Provider=OraOLEDB.Oracle.1", "$=3;" );
			orig = orig.Replace( "DBType=3;Connect=;Provider=OraOLEDB.Oracle.1", "$=3;" );


			// Generic
			orig = orig.Replace( ";Password=", "$=a;" );
			orig = orig.Replace( ";Persist Security Info=", "$=b;" );
			orig = orig.Replace( ";User ID=", "$=c;" );
			orig = orig.Replace( ";Data Source=", "$=d;" );
			orig = orig.Replace( ";Initial Catalog=", "$=e;" );

			return orig;
		}

		private static string Deabbreviate( string abbr )
		{
			// DB Specific (orig sparx with missing semicolon)
			abbr = abbr.Replace( "$=1;", "DBType=4;Connect=Provider=MSDASQL.1" );
			abbr = abbr.Replace( "$=2;", "DBType=1;Connect=Provider=SQLOLEDB.1" );
			abbr = abbr.Replace( "$=3;", "DBType=3;Connect=Provider=OraOLEDB.Oracle.1" );


			// Generic
			abbr = abbr.Replace( "$=a;", ";Password=" );
			abbr = abbr.Replace( "$=b;", ";Persist Security Info=" );
			abbr = abbr.Replace( "$=c;", ";User ID=" );
			abbr = abbr.Replace( "$=d;", ";Data Source=" );
			abbr = abbr.Replace( "$=e;", ";Initial Catalog=" );

			return abbr;
		}

		private static byte[] EncryptBytes( byte[] clearData )
		{
			// Create a MemoryStream to accept the encrypted bytes 
			using( MemoryStream ms = new MemoryStream() )
			{
				// Create a symmetric algorithm. 
				// We are going to use Rijndael because it is strong and
				// available on all platforms. 
				// You can use other algorithms, to do so substitute the
				// next line with something like 
				//      TripleDES alg = TripleDES.Create(); 
				using( Rijndael alg = Rijndael.Create() )
				{
					// Now set the key and the IV. 
					// We need the IV (Initialization Vector) because
					// the algorithm is operating in its default 
					// mode called CBC (Cipher Block Chaining).
					// The IV is XORed with the first block (8 byte) 
					// of the data before it is encrypted, and then each
					// encrypted block is XORed with the 
					// following block of plaintext.
					// This is done to make encryption more secure. 

					// There is also a mode called ECB which does not need an IV,
					// but it is much less secure. 
					alg.Key = KEY;
					alg.IV = IV;

					// Create a CryptoStream through which we are going to be
					// pumping our data. 
					// CryptoStreamMode.Write means that we are going to be
					// writing data to the stream and the output will be written
					// in the MemoryStream we have provided. 
					using( CryptoStream cs = new CryptoStream( ms, alg.CreateEncryptor(), CryptoStreamMode.Write ) )
					{

						// Write the data and make it do the encryption 
						cs.Write( clearData, 0, clearData.Length );

						// Close the crypto stream (or do FlushFinalBlock). 
						// This will tell it that we have done our encryption and
						// there is no more data coming in, 
						// and it is now a good time to apply the padding and
						// finalize the encryption process. 
						cs.Close();

						// Now get the encrypted data from the MemoryStream.
						// Some people make a mistake of using GetBuffer() here,
						// which is not the right way. 
						return ms.ToArray();
					}
				}
			}
		}

		private static byte[] DecryptBytes( byte[] cipherData )
		{
			// Create a MemoryStream that is going to accept the
			// decrypted bytes 
			using( MemoryStream ms = new MemoryStream() )
			{
				// Create a symmetric algorithm. 
				// We are going to use Rijndael because it is strong and
				// available on all platforms. 
				// You can use other algorithms, to do so substitute the next
				// line with something like 
				//     TripleDES alg = TripleDES.Create(); 
				using( Rijndael alg = Rijndael.Create() )
				{
					// Now set the key and the IV. 
					// We need the IV (Initialization Vector) because the algorithm
					// is operating in its default 
					// mode called CBC (Cipher Block Chaining). The IV is XORed with
					// the first block (8 byte) 
					// of the data after it is decrypted, and then each decrypted
					// block is XORed with the previous 
					// cipher block. This is done to make encryption more secure. 
					// There is also a mode called ECB which does not need an IV,
					// but it is much less secure. 
					alg.Key = KEY;
					alg.IV = IV;

					// Create a CryptoStream through which we are going to be
					// pumping our data. 
					// CryptoStreamMode.Write means that we are going to be
					// writing data to the stream 
					// and the output will be written in the MemoryStream
					// we have provided. 
					using( CryptoStream cs = new CryptoStream( ms, alg.CreateDecryptor(), CryptoStreamMode.Write ) )
					{
						// Write the data and make it do the decryption 
						cs.Write( cipherData, 0, cipherData.Length );

						// Close the crypto stream (or do FlushFinalBlock). 
						// This will tell it that we have done our decryption
						// and there is no more data coming in, 
						// and it is now a good time to remove the padding
						// and finalize the decryption process. 
						cs.Close();

						// Now get the decrypted data from the MemoryStream. 
						// Some people make a mistake of using GetBuffer() here,
						// which is not the right way. 
						return ms.ToArray();
					}
				}
			}
		}

		private static byte[] Dict( int? key )
		{
			// Eventually key will be the Adler hash of the dictionary to be used to encrypt/decrypt to allow
			// for better tuned dictionaries in the future.
			return DICT_1;
		}

		private static byte[] StringToBytes( string stringValue )
		{
			int length = stringValue.Length;
			byte[] resultBytes = new byte[length];
			for( int i = 0; i < length; i++ )
			{
				resultBytes[i] = (byte) stringValue[i];
			}
			return resultBytes;
		}

		private static string BytesToString( byte[] byteValues, int numberBytes )
		{
			char[] buffer = new char[numberBytes];
			for( int i = 0; i < numberBytes; i++ )
			{
				buffer[i] = (char) byteValues[i];
			}
			return new string( buffer );
		}

		/// <summary>
		/// Compresses the string.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static byte[] CompressString( string text )
		{
			byte[] outBytes = new byte[2048];

			Deflater def = new Deflater();
			def.SetDictionary( Dict( null ) );

			byte[] inBytes = StringToBytes( text );
			def.SetInput( inBytes );
			def.Finish();

			int size;
			byte[] data = new byte[2048];
			int outIdx = 0;
			do
			{
				def.Flush();
				size = def.Deflate( data, 0, data.Length );
				System.Array.Copy( data, 0, outBytes, outIdx, size );
				outIdx += size;
			}
			while( size > 0 );

			Array.Resize<byte>( ref outBytes, outIdx );
			return outBytes;// BytesToString(outBytes, outIdx);
		}

		/// <summary>
		/// Decompresses the bytes.
		/// </summary>
		/// <param name="inBytes">The input bytes.</param>
		/// <returns></returns>
		public static string DecompressBytes( byte[] inBytes )
		{
			byte[] outBytes = new byte[2048];

			Inflater inf = new Inflater();

			inf.SetInput( inBytes );

			int size;
			byte[] data = new byte[2048];
			int outIdx = 0;
			bool addedDict = false;
			do
			{
				size = inf.Inflate( data, 0, data.Length );
				if( inf.IsNeedingDictionary )
				{
					addedDict = true;
					inf.SetDictionary( Dict( null ) );
				}
				else
				{
					System.Array.Copy( data, 0, outBytes, outIdx, size );
					outIdx += size;
					addedDict = false;
				}
			}
			while( addedDict || ( size > 0 ) );

			return BytesToString( outBytes, outIdx );
		}

		/// <summary>
		/// Compacts the GUID from a representation like {45AA44F5-FDB9-40e8-8626-0A907D2CCC6B} to
		/// 45AA44F5FDB940e886260A907D2CCC6B
		/// </summary>
		/// <param name="guid">The GUID.</param>
		/// <returns></returns>
		public static string CompactGuid( string guid )
		{
			string ret = guid.Replace( "{", "" ).Replace( "-", "" ).Replace( "}", "" );
			return ret;
		}

		/// <summary>
		/// Expands the GUID from a representation like 45AA44F5FDB940e886260A907D2CCC6B
		/// to {45AA44F5-FDB9-40e8-8626-0A907D2CCC6B}
		/// </summary>
		/// <param name="guid">The GUID.</param>
		/// <returns></returns>
		public static string ExpandGuid( string guid )
		{
			StringBuilder sb = new StringBuilder();

			if( guid.Contains( "{" ) || guid.Contains( "-" ) || guid.Contains( "}" ) || guid.Length != 32 )
			{
				throw new Exception( "Guid should be short format: " + guid );
			}

			sb.Append( "{" );
			sb.Append( guid.Substring( 0, 8 ) );
			sb.Append( "-" );
			sb.Append( guid.Substring( 8, 4 ) );
			sb.Append( "-" );
			sb.Append( guid.Substring( 12, 4 ) );
			sb.Append( "-" );
			sb.Append( guid.Substring( 16, 4 ) );
			sb.Append( "-" );
			sb.Append( guid.Substring( 20 ) );
			sb.Append( "}" );

			return sb.ToString();
		}
	}
}

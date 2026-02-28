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
using System.IO.Compression;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Canonic.EAPlugin.ProtocolHandler
{
    /// <summary>
    /// This is the original class to test encryption.  Not used, just left it in to give hackers more
    /// stuff to look at ;-)
    /// </summary>
    /// <remarks>Now that this is open source the "hackers" remark is obsolete, but still may
    /// be interesting for someone to look at.</remarks>
    public class CUTest
    {
        static public void Test1()
        {
            FrmMakeDict frmDict = new FrmMakeDict();
            frmDict.ShowDialog();

            // 153
            //string orig = "DBType=1;Connect=;Provider=SQLOLEDB.1;Password=secret;Persist Security Info=True;User ID=ea;Initial Catalog=EaMercuryIntegration;Data Source=eric-laptop";
            //string comp = CompressionUtil.CompressString(orig);

            string orig = "DBType=1;Connect=;Provider=SQLOLEDB.1;Password=SomePassword;Persist Security Info=True;User ID=ea;Initial Catalog=EaMercuryIntegration;Data Source=eric-laptop";
            //string orig = "T=1;P=1;PW=cire42;PS=1;U=ea;C=EaMercuryIntegration;D=eric-laptop";
            byte[] comp = CompressionUtil.CompressString(orig);
            string dcom = CompressionUtil.DecompressString(comp);
            string b64 = Convert.ToBase64String(comp, Base64FormattingOptions.None);

            Console.WriteLine(
                "Orig: " + orig.Length + 
                "; Comp: " + comp.Length + 
                "; b64: " + b64.Length);
        }
    }

    /// <summary>
    /// Implement some compression helper methods that can compress and decompress
    /// C# strings quickly.
    /// </summary>
    public class CompressionUtil
    {
        private static byte[] dictA =
            {
                (byte)'A', (byte)'\0', (byte)'B', (byte)'\0', (byte)'C', (byte)'\0', 
                (byte)'D', (byte)'\0', (byte)'E', (byte)'\0', (byte)'A', (byte)'C', (byte)'\0'
            };

        private static byte[] dictB =
            {
                (byte)'A', (byte)'\0', (byte)'B', (byte)'\0', (byte)'C', (byte)'\0', 
                (byte)'D', (byte)'\0', (byte)'E', (byte)'\0', (byte)'A', (byte)'C', (byte)'\0',
                (byte)'D', (byte)'B', (byte)'T', (byte)'y', (byte)'p', (byte)'e', (byte)'\0'
            };

        private static byte[] dictC =
            {
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
                (byte)'Y', (byte)'\0', (byte)'X', (byte)'\0', (byte)';', (byte)'\0', (byte)'=', (byte)'\0', 
                (byte)'T', (byte)'=', (byte)'1', (byte)'\0', (byte)'T', (byte)'=', (byte)'4', (byte)'\0', 
                (byte)'P', (byte)'=', (byte)'1', (byte)'\0', (byte)'P', (byte)'=', (byte)'2', (byte)'\0', 
                (byte)';', (byte)'P', (byte)'W', (byte)'=', (byte)'\0', (byte)';', (byte)'P', (byte)'S', (byte)'=', (byte)'1', (byte)'\0', 
                (byte)';', (byte)'P', (byte)'S', (byte)'=', (byte)'0', (byte)'\0', (byte)';', (byte)'U', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'C', (byte)'=', (byte)'\0', (byte)';', (byte)'D', (byte)'=', (byte)'\0', 
                (byte)'E', (byte)'a', (byte)'M', (byte)'e', (byte)'r', (byte)'c', (byte)'u', (byte)'r', (byte)'y', (byte)'I', (byte)'n', (byte)'t', (byte)'e', (byte)'g', (byte)'r', (byte)'a', (byte)'t', (byte)'i', (byte)'o', (byte)'n', (byte)'\0', 
                (byte)'e', (byte)'r', (byte)'i', (byte)'c', (byte)'-', (byte)'l', (byte)'a', (byte)'p', (byte)'t', (byte)'o', (byte)'p', (byte)'\0'
            };

        private static byte[] dictD =
            {
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
                (byte)'}', (byte)'\0', (byte)'.', (byte)'\0', (byte)';', (byte)'\0', (byte)'=', (byte)'\0', 
                (byte)'-', (byte)'\0', (byte)'y', (byte)'e', (byte)'s', (byte)'\0', (byte)'n', (byte)'o', (byte)'\0', 
                (byte)'T', (byte)'r', (byte)'u', (byte)'e', (byte)'\0', (byte)'F', (byte)'a', (byte)'l', (byte)'s', (byte)'e', (byte)'\0', 
                (byte)'S', (byte)'S', (byte)'P', (byte)'I', (byte)'\0', (byte)'D', (byte)'B', (byte)'T', (byte)'y', (byte)'p', (byte)'e', (byte)'=', (byte)'1', (byte)'\0', 
                (byte)'D', (byte)'B', (byte)'T', (byte)'y', (byte)'p', (byte)'e', (byte)'=', (byte)'4', (byte)'\0', 
                (byte)'D', (byte)'a', (byte)'t', (byte)'a', (byte)' ', (byte)'S', (byte)'o', (byte)'u', (byte)'r', (byte)'c', (byte)'e', (byte)'=', (byte)'\0', 
                (byte)'D', (byte)'a', (byte)'t', (byte)'a', (byte)' ', (byte)'S', (byte)'o', (byte)'u', (byte)'r', (byte)'c', (byte)'e', (byte)'=', (byte)'T', (byte)'O', (byte)'R', (byte)'C', (byte)'L', (byte)'\0', 
                (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'=', (byte)'\0', 
                (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'=', (byte)'m', (byte)'s', (byte)'d', (byte)'a', (byte)'o', (byte)'r', (byte)'a', (byte)'\0', 
                (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'=', (byte)'S', (byte)'Q', (byte)'L', (byte)'O', (byte)'L', (byte)'E', (byte)'D', (byte)'B', (byte)'.', (byte)'1', (byte)'\0', 
                (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'=', (byte)'S', (byte)'Q', (byte)'L', (byte)'N', (byte)'C', (byte)'L', (byte)'I', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'=', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'=', (byte)'{', (byte)'M', (byte)'i', (byte)'c', (byte)'r', (byte)'o', (byte)'s', (byte)'o', (byte)'f', (byte)'t', (byte)' ', (byte)'O', (byte)'D', (byte)'B', (byte)'C', (byte)' ', (byte)'f', (byte)'o', (byte)'r', (byte)' ', (byte)'O', (byte)'r', (byte)'a', (byte)'c', (byte)'l', (byte)'e', (byte)'}', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'=', (byte)'(', (byte)'O', (byte)'r', (byte)'a', (byte)'c', (byte)'l', (byte)'e', (byte)' ', (byte)'i', (byte)'n', (byte)' ', (byte)'X', (byte)'E', (byte)'C', (byte)'l', (byte)'i', (byte)'e', (byte)'n', (byte)'t', (byte)')', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'=', (byte)'{', (byte)'P', (byte)'o', (byte)'s', (byte)'t', (byte)'g', (byte)'r', (byte)'e', (byte)'S', (byte)'Q', (byte)'L', (byte)'}', (byte)';', (byte)'\0', 
                (byte)'D', (byte)'r', (byte)'i', (byte)'v', (byte)'e', (byte)'r', (byte)'=', (byte)'{', (byte)'S', (byte)'Q', (byte)'L', (byte)' ', (byte)'N', (byte)'a', (byte)'t', (byte)'i', (byte)'v', (byte)'e', (byte)' ', (byte)'C', (byte)'l', (byte)'i', (byte)'e', (byte)'n', (byte)'t', (byte)'}', (byte)'\0', 
                (byte)';', (byte)'A', (byte)'s', (byte)'y', (byte)'n', (byte)'c', (byte)'h', (byte)'r', (byte)'o', (byte)'n', (byte)'o', (byte)'u', (byte)'s', (byte)' ', (byte)'P', (byte)'r', (byte)'o', (byte)'c', (byte)'e', (byte)'s', (byte)'s', (byte)'i', (byte)'n', (byte)'g', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'A', (byte)'t', (byte)'t', (byte)'a', (byte)'c', (byte)'h', (byte)'D', (byte)'b', (byte)'F', (byte)'i', (byte)'l', (byte)'e', (byte)'n', (byte)'a', (byte)'m', (byte)'e', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'C', (byte)'o', (byte)'n', (byte)'n', (byte)'e', (byte)'c', (byte)'t', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'C', (byte)'o', (byte)'n', (byte)'n', (byte)'e', (byte)'c', (byte)'t', (byte)'S', (byte)'t', (byte)'r', (byte)'i', (byte)'n', (byte)'g', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'D', (byte)'a', (byte)'t', (byte)'a', (byte)' ', (byte)'S', (byte)'o', (byte)'u', (byte)'r', (byte)'c', (byte)'e', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'D', (byte)'a', (byte)'t', (byte)'a', (byte)'b', (byte)'a', (byte)'s', (byte)'e', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'d', (byte)'b', (byte)'q', (byte)'=', (byte)'\0', (byte)';', (byte)'E', (byte)'n', (byte)'c', (byte)'r', (byte)'y', (byte)'p', (byte)'t', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'I', (byte)'n', (byte)'i', (byte)'t', (byte)'i', (byte)'a', (byte)'l', (byte)' ', (byte)'C', (byte)'a', (byte)'t', (byte)'a', (byte)'l', (byte)'o', (byte)'g', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'I', (byte)'n', (byte)'t', (byte)'e', (byte)'g', (byte)'r', (byte)'a', (byte)'t', (byte)'e', (byte)'d', (byte)' ', (byte)'S', (byte)'e', (byte)'c', (byte)'u', (byte)'r', (byte)'i', (byte)'t', (byte)'y', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'O', (byte)'S', (byte)'A', (byte)'u', (byte)'t', (byte)'h', (byte)'e', (byte)'n', (byte)'t', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'P', (byte)'a', (byte)'s', (byte)'s', (byte)'w', (byte)'o', (byte)'r', (byte)'d', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'P', (byte)'e', (byte)'r', (byte)'s', (byte)'i', (byte)'s', (byte)'t', (byte)' ', (byte)'S', (byte)'e', (byte)'c', (byte)'u', (byte)'r', (byte)'i', (byte)'t', (byte)'y', (byte)' ', (byte)'I', (byte)'n', (byte)'f', (byte)'o', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'P', (byte)'w', (byte)'d', (byte)'=', (byte)'\0', (byte)';', (byte)'S', (byte)'e', (byte)'r', (byte)'v', (byte)'e', (byte)'r', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'T', (byte)'r', (byte)'u', (byte)'s', (byte)'t', (byte)'e', (byte)'d', (byte)'_', (byte)'C', (byte)'o', (byte)'n', (byte)'n', (byte)'e', (byte)'c', (byte)'t', (byte)'i', (byte)'o', (byte)'n', (byte)'=', (byte)'\0', 
                (byte)';', (byte)'U', (byte)'i', (byte)'d', (byte)'=', (byte)'\0', (byte)';', (byte)'U', (byte)'s', (byte)'e', (byte)'r', (byte)' ', (byte)'I', (byte)'D', (byte)'=', (byte)'\0', 
                (byte)'D', (byte)'B', (byte)'T', (byte)'y', (byte)'p', (byte)'e', (byte)'=', (byte)'1', (byte)';', (byte)'C', (byte)'o', (byte)'n', (byte)'n', (byte)'e', (byte)'c', (byte)'t', (byte)'=', (byte)';', (byte)'P', (byte)'r', (byte)'o', (byte)'v', (byte)'i', (byte)'d', (byte)'e', (byte)'r', (byte)'=', (byte)'S', (byte)'Q', (byte)'L', (byte)'O', (byte)'L', (byte)'E', (byte)'D', (byte)'B', (byte)'.', (byte)'1', (byte)'\0'
            };

        /// <summary>
        /// The custom dictionary is used for compression based on a knowledge of characters and 
        /// strings more likely to be present in the input stream.
        /// </summary>
        private static byte[] Dict
        {
            get { return dictD; }
        }

        private static byte[] StringToBytes(string stringValue)
        {
            int length = stringValue.Length;
            byte[] resultBytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                resultBytes[i] = (byte)stringValue[i];
            }
            return resultBytes;
        }

        private static string BytesToString(byte[] byteValues, int numberBytes)
        {
            char[] buffer = new char[numberBytes];
            for (int i = 0; i < numberBytes; i++)
            {
                buffer[i] = (char)byteValues[i];
            }
            return new string(buffer);
        }

        public static byte[] CompressString(string text)
        {
            byte[] outBytes = new byte[2048];

            Deflater def = new Deflater();
            def.SetDictionary(Dict);
            
            byte[] inBytes = StringToBytes(text);
            def.SetInput(inBytes);
            def.Finish();

            int size;
            byte[] data = new byte[2048];
            int outIdx = 0;
            do
            {
                def.Flush();
                size = def.Deflate(data, 0, data.Length);
                System.Array.Copy(data, 0, outBytes, outIdx, size);
                outIdx += size;
            } 
            while (size > 0);

            Array.Resize<byte>(ref outBytes, outIdx);
            return outBytes;// BytesToString(outBytes, outIdx);
        }

        public static string DecompressString(byte[] inBytes /*string comp*/)
        {
            byte[] outBytes = new byte[2048];

            Inflater inf = new Inflater();

            //byte[] inBytes = StringToBytes(comp);
            inf.SetInput(inBytes);
            
            
            int size;
            byte[] data = new byte[2048];
            int outIdx = 0;
            bool addedDict = false;
            do
            {
                size = inf.Inflate(data, 0, data.Length);
                if (inf.IsNeedingDictionary)
                {
                    addedDict = true;
                    inf.SetDictionary(Dict);
                }
                else
                {
                    System.Array.Copy(data, 0, outBytes, outIdx, size);
                    outIdx += size;
                    addedDict = false;
                }
            }
            while (addedDict || (size > 0));

            return BytesToString(outBytes, outIdx);
        }

        /// <summary>
        /// Compress the input string and return the value in string format.
        /// </summary>
        /// <param name="stringValue">The string you wish to compress.</param>
        /// <returns>The compressed data.</returns>
        public static string CompressStringGZip(string stringValue)
        {
            // Convert the string to a byte array.
            byte[] byteArray = StringToBytes(stringValue);

            // Use a memory stream as storage for the GZipStream.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Write the data to the memory stream through the GZipStream.
                using (GZipStream gZipStream = new GZipStream(memoryStream,
                    CompressionMode.Compress))
                {                
                    gZipStream.Write(byteArray, 0, byteArray.Length);
                }
                // Convert memory stream back to byte array.
                byteArray = memoryStream.ToArray();
            }
            // Convert the bytes back to a string and return.
            return BytesToString(byteArray, byteArray.Length);
        }

        /// <summary>
        /// Decompress (expand) a GZip-compressed string and return the real string.
        /// </summary>
        /// <param name="stringValue">The compressed string of data.</param>
        /// <returns>The decompressed data (usable for display).</returns>
        public static string DecompressStringGZip(string stringValue)
        {
            // Convert the string to a byte array.
            byte[] byteArray = StringToBytes(stringValue);
            int byteCount = 0;

            // Use a memory stream as storage for the GZipStream.
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                // Decompress the data by writing through the GZipStream.
                using (GZipStream gZipStream = new GZipStream(memoryStream,
                    CompressionMode.Decompress))
                {
                    byteArray = new byte[byteArray.Length];
                    byteCount = gZipStream.Read(byteArray, 0, byteArray.Length);
                }
            }
            // Return the byte array in string form.
            return BytesToString(byteArray, byteCount);
        }
    }
}

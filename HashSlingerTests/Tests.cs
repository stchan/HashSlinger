using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using NUnit.Framework;


namespace HashSlingerTests
{
    [TestFixture()]
    public class Tests
    {
        public Tests()
        { }

        [Test]
        public void Test1()
        {
            HashSlingerCore.MD5Hasher coreTest = new HashSlingerCore.MD5Hasher();
            String[] testFiles = { "C:\\TEMP\\PCDTOJPEG.EXE" };
            byte[] hash = coreTest.ComputeFileHash(testFiles, null);
            byte[] controlHash = ComputeMD5StreamHash(testFiles[0]);
            Debug.WriteLine(BitConverter.ToString(hash));
            Debug.WriteLine(BitConverter.ToString(controlHash));
        }

        [Test]
        public void CompareOutputVsStreamControl()
        {
            HashSlingerCore.MD5Hasher coreTest = new HashSlingerCore.MD5Hasher();
            String[] testFiles = { "C:\\TEMP\\PCDTOJPEG.EXE" };
            byte[] hash = coreTest.ComputeFileHash(testFiles, null);
            byte[] controlHash = ComputeMD5StreamHash(testFiles[0]);
            Assert.AreEqual(BitConverter.ToString(hash), BitConverter.ToString(controlHash));
        }

        private byte[] ComputeMD5StreamHash(String filename)
        {
            MD5CryptoServiceProvider cryptoProvider = new MD5CryptoServiceProvider();
            byte[] hashValue;
            using (FileStream inputFile = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                hashValue = cryptoProvider.ComputeHash(inputFile);
            }
            return hashValue;
        }
    }
}

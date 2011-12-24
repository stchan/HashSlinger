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
        public void CompareOutputVsStreamControl()
        {
            HashSlingerCore.MD5Hasher coreTest = new HashSlingerCore.MD5Hasher();
            String[] testFiles = { @"C:\Code\HashSlinger\HashSlingerTests\White_noise.ogg" };
            byte[] hash = coreTest.ComputeFileHash(testFiles, null);
            byte[] controlHash = ComputeMD5StreamHash(testFiles[0]);
            Assert.AreEqual(BitConverter.ToString(hash), BitConverter.ToString(controlHash));
        }

        private int progressEventCount;
        [Test]
        public void AreProgressEventsBeingRaised()
        {
            this.progressEventCount = 0;
            int blockSize = 16384;
            HashSlingerCore.MD5Hasher coreTest = new HashSlingerCore.MD5Hasher();

            String[] testFiles = { @"C:\Code\HashSlinger\HashSlingerTests\NTB-Buchs-Campus_with_national_and_international_flags.jpg" };
            coreTest.HashBlockProcessed += new EventHandler<HashSlingerCore.HasherEventArgs>(coreTest_HashBlockProcessed);
            FileInfo inputInfo = new FileInfo(testFiles[0]);
            int expectedBlockCount = (int)((inputInfo.Length / blockSize));
            if (expectedBlockCount == 0) expectedBlockCount++;
            if ((inputInfo.Length % blockSize) > 0) expectedBlockCount++;
            byte[] hash = coreTest.ComputeFileHash(testFiles, blockSize);
            Assert.AreEqual(expectedBlockCount, this.progressEventCount);
        }

        private void coreTest_HashBlockProcessed(object sender, HashSlingerCore.HasherEventArgs e)
        {
            this.progressEventCount++;
        }

        bool hashComputedRaised = false;
        [Test]
        public void IsHashComputedEventRaised()
        {
            HashSlingerCore.MD5Hasher coreTest = new HashSlingerCore.MD5Hasher();
            String[] testFiles = { @"C:\Code\HashSlinger\HashSlingerTests\Electrochromic_glass.ogv" };
            coreTest.HashComputed += new EventHandler<HashSlingerCore.HasherEventArgs>(coreTest_HashComputed);
            byte[] hash = coreTest.ComputeFileHash(testFiles, null);
            Assert.IsTrue(this.hashComputedRaised);
        }

        private void coreTest_HashComputed(object sender, HashSlingerCore.HasherEventArgs e)
        {
            this.hashComputedRaised = true;
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

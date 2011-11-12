using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HashSlingerCore
{
    abstract public class HashCore
    {
        public HashCore()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filenames"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public virtual byte[] ComputeFileHash(String[] filenames, int? bufferSize)
        {
            ValidateParameters(filenames, ref bufferSize);
            using (MD5CryptoServiceProvider cryptoProvider = new MD5CryptoServiceProvider())
            {
                ICryptoTransform cryptoInterface = cryptoProvider;
                ComputeHashes(filenames, cryptoInterface, bufferSize.Value);
                return cryptoProvider.Hash;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filenames"></param>
        /// <param name="bufferSize"></param>
        protected virtual void ValidateParameters(String[] filenames, ref int? bufferSize)
        {
            if (filenames != null && filenames.Length > 0)
            {
                if (bufferSize == null || bufferSize <= 0) bufferSize = 16384; // 16K default size
            }
            else
            {
                if (filenames == null)
                {
                    throw new ArgumentNullException("filenames");
                }
                else
                {
                    throw new ArgumentOutOfRangeException("filenames", "filenames parameter is a zero-length array.");
                }
            }

        }


        /// <summary>
        /// Does the actual work of pulling the filestreams in
        /// and passing the blocks into the crytoprovider
        /// </summary>
        /// <param name="filenames">An array of String objects, each containing a filename</param>
        /// <param name="cryptoInterface">an <see cref="ICryptoTransform"/> interface point to a cryptoprovider</param>
        /// <param name="bufferSize">Size in bytes of the read buffer</param>
        protected void ComputeHashes(String[] filenames, ICryptoTransform cryptoInterface, int bufferSize)
        {
            for (int loop = 0; loop <= filenames.GetUpperBound(0); loop++)
            {
                using (FileStream inputFile = new FileStream(filenames[loop], FileMode.Open, FileAccess.Read))
                {
                    byte[] readBuffer = new byte[(int)bufferSize];
                    byte[] copyBuffer;
                    long fileLength = inputFile.Length;
                    int bytesRead = 0;
                    long totalBytesRead = 0;
                    while (totalBytesRead < fileLength)
                    {
                        bytesRead = inputFile.Read(readBuffer, 0, (int)bufferSize);
                        if (bytesRead == bufferSize) { copyBuffer = readBuffer; }
                        else
                        {
                            copyBuffer = new byte[bytesRead];
                            Array.Copy(readBuffer, copyBuffer, bytesRead);
                        }
                        totalBytesRead += bytesRead;
                        if (totalBytesRead == fileLength && loop == filenames.GetUpperBound(0))
                        {
                            // Last block of the last file
                            cryptoInterface.TransformFinalBlock(copyBuffer, 0, copyBuffer.Length);
                        }
                        else
                        {
                            cryptoInterface.TransformBlock(copyBuffer, 0, copyBuffer.Length, copyBuffer, 0);
                        }
                    }
                }
            }
        }

    }
}

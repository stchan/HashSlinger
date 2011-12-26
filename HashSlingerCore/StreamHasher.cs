using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HashSlingerCore
{
    public class StreamHasher
    {
        public StreamHasher()
        { }

        #region Public Events

        /// <summary>
        /// This event is raised when a
        /// hash transform block is processed
        /// </summary>
        public event EventHandler<HasherEventArgs> HashBlockProcessed;
        protected virtual void OnHashBlockProcessed(HasherEventArgs e)
        {
            if (HashBlockProcessed != null)
            {
                HashBlockProcessed(this, e);
            }
        }

        /// <summary>
        /// This event is raised when
        /// all input streams have been read
        /// and the hash has been computed
        /// </summary>
        public event EventHandler<HasherEventArgs> HashComputed;
        protected void OnHashComputed(HasherEventArgs e)
        {
            if (HashComputed != null)
            {
                HashComputed(this, e);
            }
        }

        #endregion

        /// <summary>
        /// Computes hash value for one more files. Note that
        /// all files will be treated as one continuous stream.
        /// </summary>
        /// <param name="filenames">File(s) to compute a hash for</param>
        /// <param name="blockSize">Crypto transform block size (in bytes) - pass null to use default</param>
        /// <returns>The hash value as a byte array</returns>
        public virtual byte[] ComputeFileHash(String[] filenames, int? blockSize)
        {
            ValidateParameters(filenames, ref blockSize);
            using (MD5CryptoServiceProvider cryptoProvider = new MD5CryptoServiceProvider())
            {
                ICryptoTransform cryptoInterface = cryptoProvider;
                ComputeHashes(filenames, cryptoInterface, blockSize.Value);
                return cryptoProvider.Hash;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filenames"></param>
        /// <param name="blockSize"></param>
        protected virtual void ValidateParameters(String[] filenames, ref int? blockSize)
        {
            if (filenames != null && filenames.Length > 0)
            {
                if (blockSize == null || blockSize <= 0) blockSize = 16384; // 16K default size
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
        /// <param name="cryptoInterface">An <see cref="ICryptoTransform"/> interface pointing to a cryptoprovider</param>
        /// <param name="blockSize">Size in bytes of the transform block / read buffer</param>
        protected void ComputeHashes(String[] filenames, ICryptoTransform cryptoInterface, int blockSize)
        {
            for (int loop = 0; loop <= filenames.GetUpperBound(0); loop++)
            {
                using (FileStream inputFile = new FileStream(filenames[loop], FileMode.Open, FileAccess.Read))
                {
                    byte[] readBuffer = new byte[(int)blockSize];
                    byte[] copyBuffer;
                    long fileLength = inputFile.Length;
                    int bytesRead = 0;
                    long totalBytesRead = 0;
                    while (totalBytesRead < fileLength)
                    {
                        bytesRead = inputFile.Read(readBuffer, 0, (int)blockSize);
                        if (bytesRead == blockSize) { copyBuffer = readBuffer; }
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
                        // Report progress and
                        // check for cancellation request
                        OnHashBlockProcessed(new HasherEventArgs(HasherEventReportType.ProgressReport,
                                         filenames.Length,
                                         loop + 1,
                                         totalBytesRead,
                                         fileLength));
                        if (this.cancelRequested == true)
                        {
                            throw new OperationCanceledException();
                        }

                    }
                }
            }
            // Report hash computed
            OnHashComputed(new HasherEventArgs(HasherEventReportType.Completed, null, null, null, null));
            
        }

        protected bool cancelRequested = false;
        /// <summary>
        /// Cancels the hash computation
        /// </summary>
        protected void Cancel()
        {
            this.cancelRequested = true;
        }

    }
}

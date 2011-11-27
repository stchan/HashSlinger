using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HashSlingerCore
{
    public class MD160Hasher : StreamHasher
    {
        public MD160Hasher()
        { }

        public override byte[] ComputeFileHash(string[] filenames, int? bufferSize)
        {
            ValidateParameters(filenames, ref bufferSize);
            using (RIPEMD160Managed cryptoProvider = new RIPEMD160Managed())
            {
                ICryptoTransform cryptoInterface = cryptoProvider;
                ComputeHashes(filenames, cryptoInterface, bufferSize.Value);
                return cryptoProvider.Hash;
            }
        }
    }
}

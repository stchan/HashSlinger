using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HashSlingerCore
{
    public class SHA256Hasher : HashCore
    {
        public SHA256Hasher()
        { }

        public override byte[] ComputeFileHash(string[] filenames, int? bufferSize)
        {
            ValidateParameters(filenames, ref bufferSize);
            using (SHA256CryptoServiceProvider cryptoProvider = new SHA256CryptoServiceProvider())
            {
                ICryptoTransform cryptoInterface = cryptoProvider;
                ComputeHashes(filenames, cryptoInterface, bufferSize.Value);
                return cryptoProvider.Hash;
            }
        }
    }
}

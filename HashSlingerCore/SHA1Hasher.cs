using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HashSlingerCore
{
    public class SHA1Hasher : HashCore
    {
        public SHA1Hasher()
        { }

        public override byte[] ComputeFileHash(string[] filenames, int? bufferSize)
        {
            ValidateParameters(filenames, ref bufferSize);
            using (SHA1CryptoServiceProvider cryptoProvider = new SHA1CryptoServiceProvider())
            {
                ICryptoTransform cryptoInterface = cryptoProvider;
                ComputeHashes(filenames, cryptoInterface, bufferSize.Value);
                return cryptoProvider.Hash;
            }
        }
    }
}

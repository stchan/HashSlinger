using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HashSlingerCore
{
    public class SHA384Hasher : HashCore
    {
        public SHA384Hasher()
        { }

        public override byte[] ComputeFileHash(string[] filenames, int? bufferSize)
        {
            ValidateParameters(filenames, ref bufferSize);
            using (SHA384CryptoServiceProvider cryptoProvider = new SHA384CryptoServiceProvider())
            {
                ICryptoTransform cryptoInterface = cryptoProvider;
                ComputeHashes(filenames, cryptoInterface, bufferSize.Value);
                return cryptoProvider.Hash;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommandLine;

namespace HashSlinger
{
    public class Options
    {
        [Option("a", "algorithm", Required=false, HelpText="Hash algorithm. Permitted vaues are MD5, MD160, SHA1,SHA256, SHA384, and SHA512. Defaults to MD5 if not specified.")]
        public String HashAgorithm = null;

        [Option("b", "blocksize", Required = false, HelpText = "Block size in bytes. Default is 16384.")]
        public long BlockSize = 16384;

    }
}

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
        public String HashAgorithm = "MD5";

        [Option("b", "blocksize", Required = false, HelpText = "Block size in bytes. Default is 16384.")]
        public long BlockSize = 16384;
         
        [Option("c", "concat", Required = false, HelpText = "Treat all input files as a single stream. Default is false.")]
        public bool Concatenate = false;

        [HelpOption(HelpText = "Display this help text.")]
        public String ShowUsage()
        {
            StringBuilder helpMessage = new StringBuilder();
            helpMessage.AppendLine("Usage:");
            helpMessage.AppendLine("\n   hashslinger [-a {MD5|MD160|SHA1|SHA256|SHA384|SHA512}] [-b blocksize] [-c] file [file2 ...]");
            helpMessage.AppendLine("\nOptions:");
            helpMessage.AppendLine("\t-a, --algorithm\t\tSets hash algorithm. Default is MD5.");
            helpMessage.AppendLine("\t-b, --blocksize\t\tBlock size in bytes. Default is 16384.");
            helpMessage.AppendLine("\t-c, --concat\t\tTreat all files as a single stream. Default is false.");
            helpMessage.AppendLine("\nExample:");
            helpMessage.AppendLine("\n   hashslinger -a MD5 -b 65536 foo.txt");
            helpMessage.AppendLine("\nComputes MD5 hash for foo.txt using a 65536 byte transform block.");
            helpMessage.AppendLine("\nExample 2:");
            helpMessage.AppendLine("\n   hashslinger -c foo.txt foo2.txt");
            helpMessage.AppendLine("\nComputes MD5 hash for foo.txt and foo2.txt as if they were one continous file.");

            return helpMessage.ToString();
        }

        [ValueList(typeof(List<string>))]
        public IList<string> Items = null;
    }
}

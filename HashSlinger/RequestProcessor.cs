using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommandLine;
using HashSlingerCore;

namespace HashSlinger
{
    public class RequestProcessor
    {

        #region Ctor

        public RequestProcessor(Options commandLineOptions)
        {
            ProcessRequest(commandLineOptions);
        }

        #endregion


        private void ProcessRequest(Options commandLineOptions)
        {
            StreamHasher fileHashMaker;
            switch (commandLineOptions.HashAgorithm.ToUpper())
            {
                case "MD160":
                    fileHashMaker = new MD160Hasher();
                    break;
                case "SHA1":
                    fileHashMaker = new SHA1Hasher();
                    break;
                case "SHA256":
                    fileHashMaker = new SHA256Hasher();
                    break;
                case "SHA384":
                    fileHashMaker = new SHA384Hasher();
                    break;
                case "SHA512":
                    fileHashMaker = new SHA512Hasher();
                    break;
                case "MD5":
                default:
                    fileHashMaker = new MD5Hasher();
                    break;
            }
            List<String[]> inputFiles = new List<String[]>();
            if (commandLineOptions.Concatenate)
            {
                // Files will be treated as a single stream -
                // copy all filenames into a string array,
                // then add the array to the List
                String[] files = new String[commandLineOptions.Items.Count];
                for (int loop = 0; loop < commandLineOptions.Items.Count; loop++)
                {
                    files[loop] = commandLineOptions.Items[loop];
                }
                inputFiles.Add(files);
            }
            else
            {
                // Each file treated as a separate entity -
                // copy each filename into a separate string array,
                // then add each array to the List
                foreach (String fileToProcess in commandLineOptions.Items)
                {
                    String[] file = new String[] { fileToProcess };
                    inputFiles.Add(file);
                }
            }
            foreach (String[] fileEntry in inputFiles)
            {
                byte[] fileHash = fileHashMaker.ComputeFileHash(fileEntry, (int)commandLineOptions.BlockSize);
                Console.WriteLine(commandLineOptions.HashAgorithm.ToUpper() + ": " + BitConverter.ToString(fileHash));
            }
        }
    }
}

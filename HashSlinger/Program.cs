using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CommandLine;

namespace HashSlinger
{
    public class Program
    {
        const string messageFileNotFound = "{0} was not found or inaccessible.";
        const string messageNoInputFileSpecifed = "No input file(s) specified.";

        const string messageMinimumBlockSize = "Minimum block size is 512 bytes.";
        const string messageUnknownAlgorithm = "Unknown hash algorithm. Valid values are MD5, MD160, SHA1, SHA256, SHA384, SHA512.";

        static void Main(string[] args)
        {
            Options commandLineOptions = new Options();
            ICommandLineParser commandParser = new CommandLineParser();
            if (commandParser.ParseArguments(args, commandLineOptions, Console.Error))
            {
                if (ValidateOptions(commandLineOptions))
                {
                    RequestProcessor procesor = new RequestProcessor(commandLineOptions);
                }
            }
            else
            {
                // Command line params could not be parsed,
                // or help was requested
                Environment.ExitCode = -1;
            }
        }

        private static bool ValidateOptions(Options commandLineOptions)
        {
            bool validatedOK = false;
            String errorMessage = null;

            if (commandLineOptions.BlockSize >= 512)
            {
                switch (commandLineOptions.HashAgorithm.ToUpper())
                {
                    case "MD5":
                    case "MD160":
                    case "SHA1":
                    case "SHA256":
                    case "SHA384":
                    case "SHA512":
                        if (commandLineOptions.Items.Count > 0)
                        {
                            // Make sure all the files exist and
                            // are accessible
                            String fileNotFoundMessage = null;
                            foreach (String inputItem in commandLineOptions.Items)
                            {
                                try
                                {
                                    using (FileStream inputFile = new FileStream(inputItem, FileMode.Open, FileAccess.Read))
                                    {
                                        inputFile.Close();
                                    }
                                }
                                catch
                                {
                                    fileNotFoundMessage = String.Format(messageFileNotFound, inputItem);
                                    break;
                                }
                            }
                            if (String.IsNullOrEmpty(fileNotFoundMessage))
                            { validatedOK = true; }
                            else
                            { errorMessage = fileNotFoundMessage; }
                        }
                        else
                        {
                            errorMessage = messageNoInputFileSpecifed;
                        }
                        break;
                    default:
                        errorMessage = messageUnknownAlgorithm;
                        break;
                }

            }
            else
            {
                errorMessage = messageMinimumBlockSize;
            }
            if (!String.IsNullOrEmpty(errorMessage))
            {
                Console.Error.WriteLine(errorMessage);
                Environment.ExitCode = 1;
            }
            return validatedOK;
        }

    }
}

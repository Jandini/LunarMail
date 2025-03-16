﻿using CommandLine;

internal sealed class Options
{
    [Verb("run", isDefault: true, HelpText = "Run program.")]
    internal class Run
    {

        [Option('p', "pst", Required = true, HelpText = "Path to the PST file.")]
        public string PstFilePath { get; set; }

    }
}

using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7ScriptGenerator
{
    class CmdOptions
    {
        [Option('c', "Count", Required = true, HelpText = "Count")]
        public int Count { get; set; }
        [Option('f', "FileNames", Required = true, HelpText = "File Name(s), seperated by ','")]
        public string FileName { get; set; }
        [Option('m', "Mode", Required = true, HelpText = "Event Start/Stop Mode, 1 for 112233 and 2 for 123123")]
        public int Mode { get; set; }
        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("<<app title>>", "<<app version>>"),
                Copyright = new CopyrightInfo("<<app author>>", 2014),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("<<license details here.>>");
            help.AddPreOptionsLine("Usage: app -p Someone");
            help.AddOptions(this);
            return help;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;


/**
 * VoiceroidTalker.
 */
namespace VoiceroidTalker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("few arguments.");
                Console.WriteLine("");
                PrintUsage();
                return;
            }

            Dictionary<string, string> argsMap = ParseArgs(args);

            string command = argsMap["command"];

            if(argsMap["message"] == string.Empty)
            {
                Console.WriteLine("message is empty.");
                Console.WriteLine("");
                PrintUsage();
                return;
            }

            Voiceroid voiceroid = find(argsMap["-v"]);
            if (voiceroid == null)
            {
                Console.WriteLine("Can't find voiceroid process {0}.", argsMap["-v"]);
                Console.WriteLine("");
                return;
            }

            switch(command)
            {
                case "talk":
                    Talk(voiceroid, argsMap);
                    break;
                case "save":
                    Record(voiceroid, argsMap);
                    break;
                case "record":
                    Record(voiceroid, argsMap, true);
                    break;
                default:
                    Console.WriteLine("unsupported command: {0}.", command);
                    Console.WriteLine("");
                    PrintUsage();
                    break;
            }
            return;
        }

        private static Voiceroid find(string name)
        {
            switch (name)
            {
                case "yukari":
                    return Yukaroid.getInstance();
                case "maki":
                    return Makiroid.getInstance();
                default:
                    Console.WriteLine("{0} is not a supported voiceroid. try yukari or maki.", name);
                    Console.WriteLine("");
                    return null;
            }
        }

        private static void Talk(Voiceroid voiceroid, Dictionary<string, string> argsMap)
        {
            string message = argsMap["message"];
            voiceroid.CopyAndPaste(message);
            voiceroid.Play();
        }

        private static void Record(Voiceroid voiceroid, Dictionary<string, string> argsMap, bool isPlay=false)
        {
            String file = argsMap["-f"];
            if(file.Length < 1)
            {
                Console.WriteLine("output file is not spexified. save default path, voice.wav.");
                file = "voice.wav";
            }
            string message = argsMap["message"];
            voiceroid.CopyAndPaste(message);
            if (!isPlay)
            {
                voiceroid.Save(file);
            }
            else
            {
                voiceroid.Save(file).PlaySync();
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    VoiceroidTalker <command> <message> -v <voiceroid> -f <filepath>");
            Console.WriteLine("");
            Console.WriteLine("    Parameters");
            Console.WriteLine("        (required) command:");
            Console.WriteLine("            talk   : play message on GUI.");
            Console.WriteLine("            save   : save wav file into specified file path.");
            Console.WriteLine("            record : save wav file into specified file path, then play that wav.");
            Console.WriteLine("        (required) message: message should not be empty.");
            Console.WriteLine("        (required) -v: specify voideroid. set yukari or maki");
            Console.WriteLine("          (option) -f: specify file path to save wav. default is voice.wav.");
        }

        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> argsMap = new Dictionary<string, string>();

            // Initialize disctionary
            argsMap["command"] = "";
            argsMap["message"] = "";
            argsMap["-v"] = "";
            argsMap["-f"] = "";

            // 4 arguments are required at least.
            if(args.Length < 4)
            {
                return argsMap;
            }

            // store main command.
            argsMap["command"] = args[0];
            // store message.
            argsMap["message"] = args[1];

            // store options
            for(int i = 2; i < args.Length; i+=2)
            {
                //argsMap.Add(args[i], args[i + 1]);
                argsMap[args[i]] = args[i + 1];
            }

            return argsMap;
        }
    }
}

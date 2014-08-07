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
 * VoiceConsoroid
 */
namespace VoiceConsoroid
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
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

            Voiceroid voiceroid = find(argsMap["voice"]);
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
            Console.WriteLine("    VoiceConsoroid <voiceroid> <command> <message> -f <filepath>");
            Console.WriteLine("");
            Console.WriteLine("    Parameters");
            Console.WriteLine("        (required) voiceroid: specify voideroid. set yukari or maki");
            Console.WriteLine("        (required) command:");
            Console.WriteLine("            talk   : play message on GUI.");
            Console.WriteLine("            save   : save wav file into specified file path.");
            Console.WriteLine("            record : save wav file into specified file path, then play that wav.");
            Console.WriteLine("        (required) message: message should not be empty.");
            Console.WriteLine("          (option) -f: specify file path to save wav. default is voice.wav.");
        }

        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> argsMap = new Dictionary<string, string>();

            // Initialize disctionary
            argsMap["voice"] = "";
            argsMap["command"] = "";
            argsMap["message"] = "";
            argsMap["-f"] = "";

            // store voiceroid
            argsMap["voice"] = args[0];
            // store main command.
            argsMap["command"] = args[1];
            // store message.
            argsMap["message"] = args[2];

            // store options
            for(int i = 3; i < args.Length; i+=2)
            {
                argsMap[args[i]] = args[i + 1];
            }

            return argsMap;
        }
    }
}

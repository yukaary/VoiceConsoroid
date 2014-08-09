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
                Console.WriteLine("Can't find voiceroid process {0}.", argsMap["voice"]);
                Console.WriteLine("");
                return;
            }

            // wait async execution.
            // reffered from http://stackoverflow.com/questions/9208921/async-on-main-method-of-console-app
            Task.Run(async () =>
            {
                await executeCommand(voiceroid, command, argsMap);
            }).Wait();

            return;
        }

        private static async Task executeCommand(Voiceroid voiceroid, string command, Dictionary<string, string> argsMap)
        {
            switch (command)
            {
                case "talk":
                    Talk(voiceroid, argsMap);
                    break;
                case "save":
                    await Record(voiceroid, argsMap);
                    break;
                case "record":
                    await Record(voiceroid, argsMap, true);
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
                case "zunko":
                    return VoiceroidTypeB.getInstance();
                default:
                    Console.WriteLine("{0} is not a supported voiceroid. try yukari or maki or zunko.", name);
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

        private static async Task Record(Voiceroid voiceroid, Dictionary<string, string> argsMap, bool isPlay=false)
        {
            String file = argsMap["file"];
            if(file.Length < 1)
            {
                Console.WriteLine("output file is not spexified. save default path, voice.wav.");
                file = "voice.wav";
            }
            string message = argsMap["message"];
            voiceroid.CopyAndPaste(message);
            if (!isPlay)
            {
                await voiceroid.Save(file);
            }
            else
            {
                await voiceroid.Record(file);
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    VoiceConsoroid <voiceroid> <command> <message> <filepath>");
            Console.WriteLine("");
            Console.WriteLine("    Parameters");
            Console.WriteLine("        (required) voiceroid: specify voideroid. You can specify 3 girls, yukari, maki, zunko");
            Console.WriteLine("        (required) command:");
            Console.WriteLine("            talk   : play message on GUI.");
            Console.WriteLine("            save   : save wav file into specified file path.");
            Console.WriteLine("            record : save wav file into specified file path, then play that wav.");
            Console.WriteLine("        (required)  message: message should not be empty.");
            Console.WriteLine("          (option) filepath: specify file path to save wav. default is voice.wav.");
        }

        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> argsMap = new Dictionary<string, string>();

            // Initialize disctionary
            argsMap["voice"] = "";
            argsMap["command"] = "";
            argsMap["message"] = "";
            argsMap["file"] = "";

            // store voiceroid
            argsMap["voice"] = args[0];
            // store main command.
            argsMap["command"] = args[1];
            // store message.
            argsMap["message"] = args[2];

            if(args.Length == 4)
            {
                argsMap["file"] = args[3];
            }

            return argsMap;
        }
    }
}

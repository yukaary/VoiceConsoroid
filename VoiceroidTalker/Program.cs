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
            Dictionary<string, string> argsMap = ParseArgs(args);

            string command = argsMap["command"];

            if(argsMap["message"] == string.Empty)
            {
                Console.WriteLine("メッセージが空っぽだよ。");
                return;
            }

            Voiceroid voiceroid = find(argsMap["-v"]);
            if (voiceroid == null)
            {
                Console.WriteLine("ボイスロイドプロセス「{0}」を発見できません。", argsMap["-v"]);
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
                    Console.WriteLine("{0}はサポート外。yukariかmakiで試して欲しいのん。", name);
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
                Console.WriteLine("保存先が指定されていないためデフォルト値「voice.wav」を使います。");
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
        }

        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> argsMap = new Dictionary<string, string>();

            // Initialize disctionary
            argsMap["command"] = "";
            argsMap["message"] = "";
            argsMap["-v"] = "";
            argsMap["-f"] = "";

            //argsMap.Add("command", "");
            //argsMap.Add("message", "");
            //argsMap.Add("-v", "");
            //argsMap.Add("-f", "");

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

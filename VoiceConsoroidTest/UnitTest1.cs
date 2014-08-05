using System;
using VoiceConsoroid;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VoiceroidTalkerTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void SaveAndPlay()
        {
            Voiceroid yukarin = Yukaroid.getInstance();

            yukarin.CopyAndPaste("ここをこうして、こっちはこんなふうにして...。がさごそ。がさごそ。");
            yukarin.Save("voice01.wav").PlaySync();

            yukarin.CopyAndPaste("うごけうごけ。");
            yukarin.Save("voice02.wav").PlaySync();

            yukarin.CopyAndPaste("動いた！！。やりましたよまきさん。");
            yukarin.Save("voice03.wav").PlaySync();

            // assert voice01-03.wav and voice01-03.txt should be exist.
            // also it should play a voice, but there are no way to test it =(.

            return;
        }

        [TestMethod]
        public void Save()
        {
            Voiceroid yukarin = Yukaroid.getInstance();

            yukarin.CopyAndPaste("てすとてすと。");
            yukarin.Save("voice01.wav");

            // assert voice01.wav and voice01.txt should be exist.
        }

        [TestMethod]
        public void PlayMaki()
        {
            Voiceroid maki = Makiroid.getInstance();
            maki.CopyAndPaste("あれれー？");
            maki.Play();
        }
    }
}

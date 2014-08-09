using System;
using System.Threading.Tasks;
using VoiceConsoroid;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VoiceroidTalkerTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task SaveAndPlay()
        {
            Voiceroid yukarin = Yukaroid.getInstance();

            yukarin.CopyAndPaste("ここをこうして、こっちはこんなふうにして...。");
            await yukarin.Record("voice01.wav");

            yukarin.CopyAndPaste("うごけうごけ。");
            await yukarin.Record("voice02.wav");

            yukarin.CopyAndPaste("動いた！！。やりましたよまきさん。");
            await yukarin.Record("voice03.wav");

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

        [TestMethod]
        public void ZunCopyAndPaste()
        {
            Voiceroid zunko = VoiceroidTypeB.getInstance();
            zunko.CopyAndPaste("ずんだもち食え");
            zunko.CopyAndPaste("枝豆うまうま");
        }

        [TestMethod]
        public void ZunPlay()
        {
            Voiceroid zunko = VoiceroidTypeB.getInstance();
            zunko.CopyAndPaste("ずんだもち食え");
            zunko.Play();
        }

        [TestMethod]
        public void ZunSave()
        {
            Voiceroid zunko = VoiceroidTypeB.getInstance();
            zunko.CopyAndPaste("保存しますね。");         
            zunko.Save("zunko.wav");
        }

        [TestMethod]
        public async Task ZunRecording()
        {
            Voiceroid zunko = VoiceroidTypeB.getInstance();
            zunko.CopyAndPaste("そろそろ私もデビューですね。");
            await zunko.Record("zunko1.wav");

            zunko.CopyAndPaste("ふふふ。ゆかりちゃんとまきちゃんと一緒。");
            await zunko.Record("zunko2.wav");
        }
    }
}

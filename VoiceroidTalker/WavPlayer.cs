using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace VoiceroidTalker
{
    /// <summary>
    /// Voivceroidが生成したwavの再生用。
    /// </summary>
    public class WavPlayer
    {
        private readonly string _path;
        private SoundPlayer player = null;

        public WavPlayer(string path)
        {
            _path = path;
        }

        /// <summary>
        /// wavを再生する。
        /// 再生終了まで実行を待機します。
        /// </summary>
        public void PlaySync()
        {
            player = new SoundPlayer(_path);
            try
            {
                player.PlaySync();
            }
            catch (TimeoutException e)
            {
                Console.WriteLine("TimeoutException source: {0}", e.Source);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("FileNotFoundException source: {0}", e.Source);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException source: {0}", e.Source);
            }
            StopWav();
        }

        /// <summary>
        /// wavを非同期再生する。
        /// </summary>
        public void PlayAsync()
        {
            player = new SoundPlayer(_path);

            try
            {
                player.Play();
            } 
            catch(TimeoutException e)
            {
                Console.WriteLine("TimeoutException source: {0}", e.Source);
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("FileNotFoundException source: {0}", e.Source);
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException source: {0}", e.Source);
            }
        }

        /// <summary>
        /// SoundPlayerオブジェクトを破棄する。
        /// </summary>
        private void StopWav()
        {
            player.Stop();
            player.Dispose();
            player = null;
        }
    }
}

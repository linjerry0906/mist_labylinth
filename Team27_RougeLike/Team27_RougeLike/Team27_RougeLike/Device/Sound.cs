using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;  // コンテンツ用
using Microsoft.Xna.Framework.Audio;    // WAVデータ
using Microsoft.Xna.Framework.Media;    // MP3データ

using System.Diagnostics;               // Assert
using Team27_RougeLike.Utility;

namespace Team27_RougeLike.Device
{
    /// <summary>
    /// 音データ管理クラス
    /// </summary>
    class Sound
    {
        // フィールド
        private ContentManager contentManager;  // コンテンツ管理

        private Dictionary<string, Song> bgms;                  // MP3管理用
        private Dictionary<string, SoundEffect> soundEffects;   // WAV管理用
        // WAVインスタンス管理用（WAVの高度な利用）
        private Dictionary<string, SoundEffectInstance> seInstances;
        // WAVインスタンスの再生リスト
        private List<SoundEffectInstance> sePlayList;

        // 現在再生中のアセット名
        private string currentBGM;
        private string nextBGM;
        //Fade用Timer
        private Timer fadeTimer;
        private bool isFade;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">コンテンツ管理</param>
        public Sound(ContentManager content)
        {
            // 引数の受け取り
            contentManager = content;
            // BGMの繰り返し再生設定
            MediaPlayer.IsRepeating = true;

            // 各Dictionaryの実体生成
            bgms = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            seInstances = new Dictionary<string, SoundEffectInstance>();

            // 再生Listの実体生成
            sePlayList = new List<SoundEffectInstance>();

            // 何も再生していないのでnull初期化
            currentBGM = null;
            nextBGM = null;

            fadeTimer = new Timer(2.0f);
            fadeTimer.Initialize();

            isFade = false;
        }

        /// <summary>
        /// Assert用メッセージ
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名（" + name + "）がありません\n" +
                    "アセット名の確認、Dictionaryに登録されているか確認してください\n";
        }


        #region BGM関連処理

        /// <summary>
        /// BGM（MP3）の読み込み
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">ファイルへのパス</param>
        public void LoadBGM(string name, string filepath = "./")
        {
            // 既に登録されているか？
            if (bgms.ContainsKey(name))
            {
                return;
            }
            // MP3の読み込みと、Dictionaryへの登録
            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }

        /// <summary>
        /// 特定のBGMを解放
        /// </summary>
        /// <param name="name">アセット名</param>
        public void UnLoadBGM(string name)
        {
            bgms.Remove(name);
        }

        /// <summary>
        /// BGMが停止中か？
        /// </summary>
        /// <returns>停止していたらtrue</returns>
        public bool IsStoppedBGM()
        {
            return (MediaPlayer.State == MediaState.Stopped);
        }

        /// <summary>
        /// BGMが再生中か？
        /// </summary>
        /// <returns>再生中だったらtrue</returns>
        public bool IsPlayingBGM()
        {
            return (MediaPlayer.State == MediaState.Playing);
        }

        /// <summary>
        /// BGM一時停止中か？
        /// </summary>
        /// <returns>一時停止中だったらtrue</returns>
        public bool IsPausedBGM()
        {
            return (MediaPlayer.State == MediaState.Paused);
        }

        /// <summary>
        /// BGMを停止
        /// </summary>
        public void StopBGM()
        {
            fadeTimer.Update();
            isFade = true;
            if (fadeTimer.IsTime())
            {
                MediaPlayer.Stop();
                currentBGM = null;
                fadeTimer.Initialize();
                isFade = false;
            }
            else
            {
                MediaPlayer.Volume = fadeTimer.Rate() * 0.7f;
            }
        }

        /// <summary>
        /// ロード中などのシーンもFade処理
        /// </summary>
        public void UpdateVolume()
        {
            if(nextBGM != null)
                PlayBGM(nextBGM);

            if (isFade)
                return;

            if (MediaPlayer.Volume < 0.7f)
            {
                MediaPlayer.Volume += 0.003f;
            }
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="name">アセット名</param>
        public void PlayBGM(string name)
        {
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));

            
            if (MediaPlayer.Volume < 0.7f && !isFade)
            {
                MediaPlayer.Volume += 0.003f;
            }

            // 同じ曲か？
            if (currentBGM == name)
            {
                // 同じ曲だったら何もしない
                return;
            }

            nextBGM = name;

            // BGMは再生中か？
            if (IsPlayingBGM())
            {
                // 再生中の場合、停止処理をする
                StopBGM();
                return;
            }

            MediaPlayer.Volume = 0.0f;

            // 現在のBGM名を設定
            currentBGM = name;

            // 再生開始
            MediaPlayer.Play(bgms[currentBGM]);
        }

        /// <summary>
        /// BGMループフラグの変更
        /// </summary>
        /// <param name="loopFlag">変更後のフラグ</param>
        public void ChangeBGMLoopFlag(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }

        #endregion


        #region WAV関連処理

        /// <summary>
        /// SEのロード
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">ファイルへのパス</param>
        public void LoadSE(string name, string filepath = "./")
        {
            // すでに登録されていれば何もしない
            if (soundEffects.ContainsKey(name))
            {
                return;
            }

            // 読み込みと追加
            soundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }

        /// <summary>
        /// SoundEffectInstanceの生成
        /// </summary>
        /// <param name="name">アセット名</param>
        public void CreateSEInstance(string name)
        {
            // 既に登録されていれば何もしない
            if (seInstances.ContainsKey(name))
            {
                return;
            }

            // WAV用ディクショナリに登録されていないと無理
            Debug.Assert(
                soundEffects.ContainsKey(name),
                "先に" + name + "の読み込み処理をしてください");

            // WAVデータのインスタンス生成し、登録
            seInstances.Add(name, soundEffects[name].CreateInstance());
        }

        /// <summary>
        /// 単純SE再生（連続で呼ばれた場合、音は重なる。途中停止不可）
        /// </summary>
        /// <param name="name">アセット名</param>
        public void PlaySE(string name)
        {
            // WAV用ディクショナリをチェック
            Debug.Assert(soundEffects.ContainsKey(name), ErrorMessage(name));

            soundEffects[name].Play();
        }

        /// <summary>
        /// InstanceSE再生
        /// （音は重ならない。ループの指定可能。再生リストを使って停止可能にする。）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="loopFlag">ループするかどうか</param>
        public void PlaySEInstance(string name, bool loopFlag = false)
        {
            // WAVインスタンス用ディクショナリをチェック
            Debug.Assert(seInstances.ContainsKey(name), ErrorMessage(name));

            // SEInstancesから再生SEを取り出す
            var data = seInstances[name];
            // ループするかどうか設定
            data.IsLooped = loopFlag;
            // 再生
            data.Play();
            // 再生リストに追加して管理
            sePlayList.Add(data);
        }

        /// <summary>
        /// sePlayListにある再生中の音を停止
        /// </summary>
        public void StopSE()
        {
            // 再生中のSEを1つずつ取り出して処理
            foreach (var se in sePlayList)
            {
                // 取り出したSEが再生中であれば
                if (se.State == SoundState.Playing)
                {
                    // SEを停止
                    se.Stop();
                }
            }
        }

        /// <summary>
        /// sePlayListにある再生中の音を一時停止
        /// </summary>
        /// <param name="name"></param>
        public void PauseSE()
        {
            // 再生中のSEを1つずつ取り出して処理
            foreach (var se in sePlayList)
            {
                // 取り出したSEが再生中であれば
                if (se.State == SoundState.Playing)
                {
                    // SEを一時停止
                    se.Pause();
                }
            }
        }

        /// <summary>
        /// 停止しているSEの削除
        /// </summary>
        public void RemoveSE()
        {
            // 停止中のものはListから削除
            sePlayList.RemoveAll(se => (se.State == SoundState.Stopped));
        }

        #endregion

        /// <summary>
        /// 解放
        /// </summary>
        public void Unload()
        {
            bgms.Clear();
            soundEffects.Clear();
            sePlayList.Clear();
        }
    }
}

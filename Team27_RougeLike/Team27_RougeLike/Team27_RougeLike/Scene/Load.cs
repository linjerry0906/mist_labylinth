using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Scene
{
    class Load : IScene
    {
        private Renderer renderer;
        private Sound sound;
        private bool endFlag;
        private TextureLoader textureLoader;
        private BGMLoader bgmLoader;
        private SELoader seLoader;
        private int totalResouceNum;

        private string[,] TextureList()
        {
            string path = "./Texture/";
            string[,] list = new string[,]
            {
                {"cursor" ,path},
                {"attack" ,path},
                {"slash" ,path},
                {"cubeTest" ,path},
                {"test" ,path},
                {"test_run" ,path},
                {"bat" ,path},
                {"Snow" ,path},
                {"enemy_light" ,path},
                {"enemy_mud" ,path},
                {"enemy_snowman" ,path},
                {"enemy_armor" ,path},
                {"enemy_snake" ,path},
                {"enemy_weak" ,path},
                {"enemy_griffon" ,path},
                {"enemy_wasp" ,path},
                {"enemy_shadow" ,path},
                {"enemy_skeleton" ,path},
                {"enemy_rabbit" ,path},
                {"enemy_worm" ,path},
                {"enemy_eye" ,path},
                {"enemy_flower" ,path},
                {"enemy_sword" ,path},
                {"enemy_cristal" ,path},
                {"enemy_metal" ,path},
                {"enemy_wolf" ,path},
                {"enemy_wolf_boss" ,path},
                {"fog" ,path},
                {"town" ,path},
                {"guild_rank" ,path},
                {"guild_background" ,path},
                {"guild_gage_back" ,path},
                {"guild_gage_middle" ,path},
                {"guild_gage_front" ,path},
                {"exp" ,path},
                {"hp" ,path},
                {"hp_back" ,path},
                {"hp_gage" ,path},
                {"hp_deco" ,path},
                {"dungeon_image_tutorial" ,path},
                {"dungeon_image2" ,path},
                {"dungeon_image_thomb" ,path},
                {"dungeon_image_ice" ,path},
                {"dungeon_image_tree" ,path},
                {"ground_tutorial" ,path},
                {"ground_ice" ,path},
                {"ground_tree" ,path},
                {"ground_infinite" ,path},
                {"wall_default" ,path},
                {"wall_tutorial" ,path},
                {"wall_ice" ,path},
                {"wall_tree" ,path},
                {"wall_infinite" ,path},
                {"magic_center" ,path},
                {"magic_in" ,path},
                {"magic_middle" ,path},
                {"magic_out" ,path},
                {"particle" ,path},
                {"Depotbutton" ,path},
                {"Dungeonbutton" ,path},
                {"Guildtbutton" ,path},
                {"Shopbutton" ,path},
                {"Upgradebutton" ,path},
                {"NEEC1280black" ,path},
                {"warning" ,path},
                {"player" ,path},
                {"player_run" ,path},
                {"title" ,path},
                {"titlelogo" ,path},
                {"pressspace" ,path},
            };
            return list;
        }

        private string[,] BGMList()
        {
            string path = "./Sound/BGM/";
            string[,] list = new string[,]
            {
                {"Remotest-Liblary_SE", path },         //Title
                {"Voyage_SE", path },                   //村
                {"n36", path },                         //演習場
                {"m-art_Candle", path },                //鉱山
                {"n51", path },                         //宮殿
                {"m-art_Komorebi", path },              //樹海
                {"n14", path },                         //回廊
                {"n17", path },                         //Boss後
            };
            return list;
        }
        private string[,] SEMList()
        {
            string path = "./Sound/SE/";
            string[,] list = new string[,]
            {
                {"slash",path },
                {"sowrd",path },
                {"press",path },
                {"press2",path },
                {"select",path },
                {"attack1",path },
                {"damage1",path },
                {"damage7",path },
                {"damage5",path },
                {"powerup10",path }
            };
            return list;
        }

        public Load(GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            sound = gameDevice.Sound;

            //TextureLoaderの実態生成
            textureLoader = new TextureLoader(renderer, TextureList());

            //BGMLoaderの実態生成
            bgmLoader = new BGMLoader(sound, BGMList());

            //SELoaderの実態生成
            seLoader = new SELoader(sound, SEMList());
        }

        public void Initialize(SceneType sceneType)
        {
            endFlag = false;

            textureLoader.Initialize();
            bgmLoader.Initialize();
            seLoader.Initialize();

            //全リソース数を計算
            totalResouceNum = textureLoader.Count() +
                bgmLoader.Count() +
                seLoader.Count();
        }

        public void Update(GameTime gameTime)
        {
            if (!textureLoader.IsEnd())
            {
                textureLoader.Update();
                return;
            }
            if (!bgmLoader.IsEnd())
            {
                bgmLoader.Update();
                return;
            }
            if (!seLoader.IsEnd())
            {
                seLoader.Update();
                return;
            }


            //読み込み処理が終わっていたらシーンを終了
            if (textureLoader.IsEnd() &&
                bgmLoader.IsEnd() &&
                seLoader.IsEnd())
            {
                endFlag = true;
            }

        }

        public void Draw()
        {
            renderer.Begin();
            renderer.DrawTexture("fade", Vector2.Zero, new Vector2(Def.WindowDef.WINDOW_WIDTH, Def.WindowDef.WINDOW_HEIGHT));
            renderer.DrawString("ロード中", Vector2.Zero, Color.Black, new Vector2(1, 1));

            //読み込んでいる数を取得
            int currentCount = textureLoader.CurrentCount() +
                bgmLoader.CurrentCount() +
                seLoader.CurrentCount();

            if (totalResouceNum != 0)
            {
                renderer.DrawString(((int)(currentCount / (float)totalResouceNum * 100)).ToString(),
                    new Vector2(0, 100), Color.Black, new Vector2(1, 1));
            }

            renderer.End();

        }

        public void ShutDown()
        {

        }

        public SceneType Next()
        {
            return SceneType.Logo;
        }

        public bool IsEnd()
        {
            return endFlag;
        }


    }
}

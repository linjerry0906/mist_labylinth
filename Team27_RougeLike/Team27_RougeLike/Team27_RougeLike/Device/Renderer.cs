﻿//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.16 ～ 2017.11.26
// 作成部分：3D描画
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Diagnostics;

namespace Team27_RougeLike.Device
{
    class Renderer
    {
        private ContentManager contentManager;  // コンテンツ管理者
        private GraphicsDevice graphicsDevice;  // グラフィック機器
        private SpriteBatch spriteBatch;        // スプライト一括
        private BasicEffect basicEffect;        // 3D描画用
        private FogManager fogManager;          // 霧の管理者

        private Projector mainProjector;        // メインプロジェクター

        // Dictionaryで複数の画像を管理
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">コンテンツ管理</param>
        /// <param name="graphics">グラフィック機器</param>
        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);
            basicEffect = new BasicEffect(graphicsDevice);
            fogManager = new FogManager();

            mainProjector = new Projector();
            DefaultRenderSetting();
        }

        /// <summary>
        /// メインプロジェクター
        /// </summary>
        public Projector MainProjector
        {
            get { return mainProjector; }
        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">ファイル名までのパス</param>
        public void LoadTexture(string name, string filepath = "./")
        {
            // ガード節 Dictionaryへの2重登録を回避
            if (textures.ContainsKey(name))
            {
#if DEBUG // DEBUGモードの時のみ有効
                System.Console.WriteLine("この" + name + "はKeyで、すでに登録してます");
#endif
                // 処理終了
                return;
            }
            // 画像の読み込みとDictionaryにアセット名と画像を追加
            textures.Add(name, contentManager.Load<Texture2D>(filepath + name));
        }

        /// <summary>
        /// 画像の登録
        /// </summary>
        /// <param name="name">登録名</param>
        /// <param name="texture">登録したいテクスチャ</param>
        public void LoadTexture(string name, Texture2D texture)
        {
            if (textures.ContainsKey(name))
            {
#if DEBUG   // DEBUGモード時のみ有効
                System.Console.WriteLine("この" + name +
                    "はKeyで、すでに登録されています");
#endif
                // 処理終了
                return;
            }
            textures.Add(name, texture);
        }

        /// <summary>
        /// アンロード
        /// </summary>
        public void Unload()
        {
            // Dictionary登録情報をクリア
            textures.Clear();
        }

        #region 3D用

        /// <summary>
        /// DepthStencil, Cull, AlphaBlend, Color
        /// </summary>
        public void DefaultRenderSetting()
        {
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;  //カーリング
            graphicsDevice.BlendState = BlendState.AlphaBlend;                      //アルファブレンド
            basicEffect.VertexColorEnabled = true;                                  //頂点色を有効
        }

        /// <summary>
        /// メインプロジェクターにレンダリング
        /// </summary>
        public void RendererMainProjector()
        {
            graphicsDevice.Viewport = mainProjector.ViewPort;       //Viewport指定
            basicEffect.World = mainProjector.World;                //ワールド
            basicEffect.View = mainProjector.LookAt;                //View
            basicEffect.Projection = mainProjector.Projection;      //プロジェクション
        }

        /// <summary>
        /// ポリゴンを描画する
        /// </summary>
        /// <param name="name">テクスチャ</param>
        /// <param name="vertices">頂点</param>
        /// <param name="alpha">透明度</param>
        public void DrawPolygon(string name, VertexPositionColorTexture[] vertices, float alpha = 1)
        {
            basicEffect.TextureEnabled = true;            //テクスチャを有効
            basicEffect.Alpha = alpha;                    //Alpha指定
            basicEffect.Texture = textures[name];         //登録していないためにコメントアウト
            foreach (var effect in basicEffect.CurrentTechnique.Passes)
            {
                effect.Apply();
            }
            graphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
        }

        /// <summary>
        /// スクリーンに向いてポリゴンを描画
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="size">ポリゴンのサイズ</param>
        /// <param name="rect">アセットのUV</param>
        /// <param name="color">ポリゴンの色</param>
        /// <param name="alpha">アルファ値</param>
        public void DrawPolygon(string name, Vector3 position, Vector2 size, Rectangle rect, Color color,float alpha = 1)
        {
            graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Vector3 axis = Vector3.Cross(mainProjector.Front, mainProjector.Right);     //回転軸
            axis.Normalize();
            basicEffect.TextureEnabled = true;                                          //テクスチャ有効
            int textureHeight = textures[name].Height;                                  //テクスチャのサイズを取得
            int textureWidth = textures[name].Width;
            //四つの頂点を設定
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
            vertices[0] = new VertexPositionColorTexture(
                Vector3.Left * size.X / 2 + Vector3.Up * size.Y / 2,
                color,
                new Vector2(
                    rect.X * 1.0f / textureWidth,
                    (rect.Y + rect.Height) * 1.0f / textureHeight));
            vertices[1] = new VertexPositionColorTexture(
                Vector3.Left * size.X / 2 + Vector3.Down * size.Y / 2,
                color,
                new Vector2(
                    rect.X * 1.0f / textureWidth,
                    rect.Y * 1.0f / textureHeight));
            vertices[2] = new VertexPositionColorTexture(
                Vector3.Right * size.X / 2 + Vector3.Up * size.Y / 2,
                color,
                new Vector2(
                    (rect.X + rect.Width) * 1.0f / textureWidth,
                    (rect.Y + rect.Height) * 1.0f / textureHeight));
            vertices[3] = new VertexPositionColorTexture(
                Vector3.Right * size.X / 2 + Vector3.Down * size.Y / 2,
                color,
                new Vector2(
                    (rect.X + rect.Width) * 1.0f / textureWidth,
                    rect.Y * 1.0f / textureHeight));

            basicEffect.Alpha = alpha;                  //アルファ値を指定
            basicEffect.Texture = textures[name];       //テクスチャを指定
            basicEffect.World = 
                Matrix.CreateBillboard(position, mainProjector.Position, axis, mainProjector.Front); //ビルボードマトリクス 
            foreach (var effect in basicEffect.CurrentTechnique.Passes)
            {
                effect.Apply();
            }
            graphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
            basicEffect.TextureEnabled = false;
        }

        /// <summary>
        /// Debug用      線を引く
        /// </summary>
        public void DrawLine(VertexPositionColor[] vertices, float alpha = 1)
        {
            basicEffect.Alpha = alpha;
            foreach (var effect in basicEffect.CurrentTechnique.Passes)
            {
                effect.Apply();
            }
            graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);
        }

        #endregion

        #region Fog関連

        /// <summary>
        /// Fogを有効
        /// </summary>
        public void StartFog()
        {
            fogManager.FogOn();
            fogManager.SetFog(ref basicEffect);
        }

        /// <summary>
        /// Fog無効
        /// </summary>
        public void EndFog()
        {
            fogManager.FogOff();
            fogManager.SetFog(ref basicEffect);
        }

        /// <summary>
        /// Fogの詳細設定が必要な場合Getできる
        /// </summary>
        public FogManager FogManager
        {
            get { return fogManager; }
        }

        #endregion

        #region 2D用

        /// <summary>
        /// 描画開始
        /// </summary>
        public void Begin()
        {
            spriteBatch.Begin();
        }

        /// <summary>
        /// 描画終了
        /// </summary>
        public void End()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// 指定されたアセット名の絵を描く
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">描く位置</param>
        /// <param name="alpha">透明度（指定しなければそのまま1.0f）</param>
        public void DrawTexture(string name, Vector2 position, float alpha = 1.0f)
        {
            // 登録されているキーがなければエラー表示
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名を間違えていませんか？\n" +
                "大文字小文字が間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");

            spriteBatch.Draw(textures[name], position, Color.White * alpha);
        }

        public void DrawTexture(RenderTarget2D texture, Vector2 position, float alpha = 1.0f)
        {
            spriteBatch.Draw(texture, position, Color.White * alpha);
        }

        /// <summary>
        /// 画像の描画（指定範囲）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">画像の切り出し範囲</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(string name, Vector2 position, 
                                        Rectangle rect, float alpha = 1.0f)
        {
            // 登録されているキーがなければエラー表示
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名を間違えていませんか？\n" +
                "大文字小文字が間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");

            spriteBatch.Draw(
                textures[name], // 画像
                position,       // 位置
                rect,           // 矩形の指定範囲（左上の座標x,y,幅,高さ）
                Color.White * alpha);
        }

        /// <summary>
        /// （拡大縮小対応版）画像の描画
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="scale">拡大縮小値</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(string name, Vector2 position,
                                Vector2 scale, float alpha = 1.0f)
        {
            Debug.Assert(textures.ContainsKey(name),
                "アセット名を間違えていませんか？\n" +
                "大文字小文字が間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください\n");
            spriteBatch.Draw(
                textures[name],         // 画像
                position,               // 位置
                null,                   // 切り取り範囲
                Color.White * alpha,    // 透過
                0.0f,                   // 回転
                Vector2.Zero,           // 回転軸の位置
                scale,                  // 拡大縮小
                SpriteEffects.None,     // 表示反転効果
                0.0f                    // スプライト表示深度
                );
        }

        /// <summary>
        /// 数字の描画（整数のみ版）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="number"></param>
        /// <param name="alpha"></param>
        public void DrawNumber(string name, Vector2 position, int number, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名を間違えていませんか？\n" +
                "大文字小文字間違っていませんか？\n" +
                "LoadTextureメソッドで読み込んでいますか？\n" +
                "プログラムを確認してください\n");

            // マイナスの数は0
            if (number < 0)
            {
                number = 0;
            }

            // 一番大きい桁から数字を書いていく
            foreach (var n in number.ToString())
            {
                spriteBatch.Draw(
                    textures[name],
                    position,
                    new Rectangle((n - '0') * 32, 0, 32, 64),
                    Color.White * alpha
                    );
                position.X += 32;   // 1桁分右へずらす
            }
        }

        /// <summary>
        /// 数字の描画（詳細版）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="number">描画したい数（文字列でもらう）</param>
        /// <param name="digit">桁数</param>
        /// <param name="alpha">透明値</param>
        public void DrawNumber(string name, Vector2 position,
            string number, int digit, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名を間違えていませんか？\n" +
                "大文字小文字間違っていませんか？\n" +
                "LoadTextureメソッドで読み込んでいますか？\n" +
                "プログラムを確認してください\n");

            // 桁数ループして、１の位を表示
            for (int i = 0; i < digit; i++)
            {
                if (number[i] == '.')
                {
                    // 幅をかけて座標を求め、1文字を絵から切り出す
                    spriteBatch.Draw(
                        textures[name],
                        position,
                        new Rectangle(10 * 32, 0, 32, 64),
                        Color.White * alpha
                        );
                }
                else
                {
                    // 1も自分の数値を数値文字で取得
                    char n = number[i];

                    // 幅をかけて座標を求め、1文字を絵から切り出す
                    spriteBatch.Draw(
                        textures[name],
                        position,
                        new Rectangle((n - '0') * 32, 0, 32, 64),
                        Color.White * alpha
                        );
                }

                // 表示座標のX座標を右へ移動
                position.X += 32;
            }
        }

        #endregion
    }
}

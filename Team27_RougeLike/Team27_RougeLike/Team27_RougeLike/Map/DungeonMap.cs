//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.19 ~ 2017.12.01
// 内容  ：マップの実体
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Object;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Box;

namespace Team27_RougeLike.Map
{
    class DungeonMap
    {
        private GameDevice gameDevice;      //Debug可視化用

        private List<Cube> mapBlocks;       //モデルに変更可
        private List<Point> space;          //スペースのあるマス
        private int[,] mapChip;             //マップチップ

        private List<Cube> mapBlocksToDraw; //描画するマップ
        private Point previousPosition;     //前フレームの描画中心座標
        private Point currentPosition;      //今のフレームの描画中心座標
        private Point entryPoint;           //入口
        private Point exitPoint;            //出口
        private List<Point> bossPoint;
        private int radius = 15;            //描画半径

        private bool drawExit;              //出口の印を描画するか
        private EndPoint exitEffect;        //出口の印

        /// <summary>
        /// マップ実体のコンストラクタ
        /// </summary>
        /// <param name="mapChip">マップチップ</param>
        /// <param name="gameDevice">ゲームデバイス</param>
        public DungeonMap(int[,] mapChip, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;

            this.mapChip = mapChip;
            space = new List<Point>();
            mapBlocks = new List<Cube>();
            mapBlocksToDraw = new List<Cube>();
            bossPoint = new List<Point>();

            currentPosition = new Point(0, 0);      //描画中心
            previousPosition = currentPosition;     //描画中心（前フレーム）
            entryPoint = new Point(0, 0);           //スタート位置
            exitPoint = new Point(0, 0);            //エンド位置
        }

        /// <summary>
        /// マップのオブジェクトを生成
        /// </summary>
        public void Initialize(BlockStyle blockStyle)
        {
            Dictionary<MapDef.BlockDef, string> blockDef = blockStyle.GetDefination();

            for (int y = 0; y < mapChip.GetLength(0); y++)          //マップのY軸
            {
                for (int x = 0; x < mapChip.GetLength(1); x++)      //マップのX軸
                {
                    Cube c;
                    //ブロック定義で生成
                    switch (mapChip[y, x])
                    {
                        case (int)MapDef.BlockDef.Wall:
                            c = new Cube(
                                new Vector3(x * MapDef.TILE_SIZE, 0, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE * 2.5f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            c.SetTexture(blockDef[MapDef.BlockDef.Wall]);
                            c.SetMiniMapWallColor(new Color(0.25f, 0.25f, 0.25f));
                            mapBlocks.Add(c);
                            break;
                        case (int)MapDef.BlockDef.Space:
                            c = new Cube(
                                new Vector3(x * MapDef.TILE_SIZE, 0, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            c.SetTexture(blockDef[MapDef.BlockDef.Space]);
                            mapBlocks.Add(c);
                            space.Add(new Point(x, y));
                            break;
                        case (int)MapDef.BlockDef.Fall:
                            c = new FallBlock(
                                new Vector3(x * MapDef.TILE_SIZE, 0, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE / 6.0f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            mapBlocks.Add(c);
                            break;
                        case (int)MapDef.BlockDef.Entry:
                            entryPoint = new Point(x, y);
                            c = new Cube(
                                new Vector3(x * MapDef.TILE_SIZE, 0, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            c.SetTexture(blockDef[MapDef.BlockDef.Space]);
                            mapBlocks.Add(c);
                            break;
                        case (int)MapDef.BlockDef.Exit:
                            exitPoint = new Point(x, y);
                            c = new Cube(
                                new Vector3(x * MapDef.TILE_SIZE, 0, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            c.SetTexture(blockDef[MapDef.BlockDef.Space]);
                            mapBlocks.Add(c);
                            break;
                        case (int)MapDef.BlockDef.Boss:
                            Point bossP = new Point(x, y);
                            bossPoint.Add(bossP);
                            c = new Cube(
                                new Vector3(x * MapDef.TILE_SIZE, 0, y * MapDef.TILE_SIZE),
                                new Vector3(MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE / 2.0f, MapDef.TILE_SIZE / 2.0f),
                                gameDevice);
                            c.SetTexture(blockDef[MapDef.BlockDef.Space]);
                            mapBlocks.Add(c);
                            break;
                    }
                }
            }

            Vector3 endPos = new Vector3(
                    exitPoint.X * MapDef.TILE_SIZE,
                    MapDef.TILE_SIZE / 2 + 0.01f,
                    exitPoint.Y * MapDef.TILE_SIZE);
            exitEffect = new EndPoint(endPos, gameDevice);
            drawExit = true;
        }

        /// <summary>
        /// 入口の配列座標
        /// </summary>
        public Point EntryPoint
        {
            get { return entryPoint; }
        }

        /// <summary>
        /// 出口の配列座標
        /// </summary>
        public Point EndPoint
        {
            get { return exitPoint; }
        }

        /// <summary>
        /// ワールド座標をマップチップ座標に変換
        /// </summary>
        /// <param name="worldPosition">ワールド座標</param>
        /// <returns></returns>
        public Point WorldToMap(Vector3 worldPosition)
        {
            int x = (int)((MapDef.TILE_SIZE / 2.0f + worldPosition.X) / MapDef.TILE_SIZE);        //マス：X
            int z = (int)((MapDef.TILE_SIZE / 2.0f + worldPosition.Z) / MapDef.TILE_SIZE);        //マス：Y
            ClampPoint(ref x, ref z);   //エラー対策

            return new Point(x, z);
        }

        /// <summary>
        /// 描画の中心位置を設定
        /// </summary>
        /// <param name="worldPosition"></param>
        public void FocusCenter(Vector3 worldPosition)
        {
            int x = (int)((MapDef.TILE_SIZE / 2.0f + worldPosition.X) / MapDef.TILE_SIZE);      //マップチップに変換
            int z = (int)((MapDef.TILE_SIZE / 2.0f + worldPosition.Z) / MapDef.TILE_SIZE);

            currentPosition.X = x;      //描画中心を更新
            currentPosition.Y = z;
            ClampPoint(ref currentPosition.X, ref currentPosition.Y);          //エラーがないように配列の中に納める
        }

        /// <summary>
        /// 描画領域の更新
        /// </summary>
        public void Update()
        {
            exitEffect.Update();
            if (previousPosition == currentPosition)    //描画領域変わってなかったら更新する必要がない
                return;

            previousPosition = currentPosition;         //前フレームのPositionを設定
            mapBlocksToDraw.Clear();

            ClampPoint(ref currentPosition.X, ref currentPosition.Y);                       //エラーがないように配列の中に納める
            for (int y = currentPosition.Y - radius; y <= currentPosition.Y + radius; y++)        //Y軸
            {
                if (y < 0 || y > mapChip.GetLength(0) - 1)                          //マップのサイズ外はスキップ
                    continue;
                for (int x = currentPosition.X - radius; x <= currentPosition.X + radius; x++)    //X軸
                {
                    if (x < 0 || x > mapChip.GetLength(1) - 1)                      //マップのサイズ外はスキップ
                        continue;
                    int index = y * mapChip.GetLength(1) + x;                       //Add順でIndexの計算
                    mapBlocksToDraw.Add(mapBlocks[index]);                          //描画部分に追加
                }
            }
        }

        /// <summary>
        /// 配列の長さを超えないように設定
        /// </summary>
        /// <param name="x">X ユニット</param>
        /// <param name="y">Y ユニット</param>
        private void ClampPoint(ref int x, ref int y)
        {
            y = (y < 0) ? 0 : y;                                                //Yの最小値
            y = (y >= mapChip.GetLength(0)) ? mapChip.GetLength(0) - 1 : y;     //Yの最大値
            x = (x < 0) ? 0 : x;                                                //Xの最小値
            x = (x >= mapChip.GetLength(1)) ? mapChip.GetLength(1) - 1 : x;     //Xの最大値
        }

        /// <summary>
        /// マップチップ
        /// </summary>
        public int[,] MapChip
        {
            get { return mapChip; }
        }

        /// <summary>
        /// マップオブジェクトを消す
        /// </summary>
        public void Clear()
        {
            mapChip = null;
            exitEffect = null;
            mapBlocks.Clear();
            mapBlocksToDraw.Clear();
            bossPoint.Clear();
            space.Clear();
        }

        /// <summary>
        /// 多数のキャラクターとあたり判定
        /// </summary>
        /// <param name="characters"></param>
        public void MapCollision(List<CharacterBase> characters)
        {
            characters.ForEach(c => MapCollision(c));
        }

        /// <summary>
        /// 飯泉　多数の移動する箱と当たり判定
        /// </summary>
        /// <param name="characters"></param>
        public void MapCollision(List<HitBoxBase> boxs)
        {
            boxs.ForEach(c => { if (c is MoveDamageBox) { MapCollision(c); } });
        }
        /// <summary>
        /// 箱とマップのあたり判定
        /// </summary>
        /// <param name="chara">チェックするキャラクター</param>
        public void MapCollision(HitBoxBase hitbox)
        {
            float floatX = ((MapDef.TILE_SIZE / 2.0f + hitbox.collision.Center.X) / MapDef.TILE_SIZE);      //そのマスの左右半分
            float floatZ = ((MapDef.TILE_SIZE / 2.0f + hitbox.collision.Center.Z) / MapDef.TILE_SIZE);      //そのマスの上下半分
            int x = (int)((MapDef.TILE_SIZE / 2.0f + hitbox.collision.Center.X) / MapDef.TILE_SIZE);        //マス：X
            int z = (int)((MapDef.TILE_SIZE / 2.0f + hitbox.collision.Center.Z) / MapDef.TILE_SIZE);        //マス：Y
            //判定半径（キャラクターのサイズにより違う）
            int collisionRange = (int)(hitbox.collision.Radius * 2 / MapDef.TILE_SIZE) + 1;
            Point checkDir = new Point(0, 0);           //判定基準点
            if (floatX - x > 0.5f)      //int型を引くと値は0～1まで　判定方向もわかる
                checkDir.X = 0;         //右半分と判定する場合は基準変動なし
            else
                checkDir.X = -1 * collisionRange;       //左半分と判定する場合は基準をずらす
            if (floatZ - z > 0.5f)      //X軸と同じ
                checkDir.Y = 0;
            else
                checkDir.Y = -1 * collisionRange;

            x += checkDir.X;            //基準をずらす
            z += checkDir.Y;

            ClampPoint(ref x, ref z);   //エラー対策

            for (int mapchipZ = z; mapchipZ <= z + collisionRange; mapchipZ++)   //基準から半径の長さのマスと判定
            {
                if (mapchipZ < 0 || mapchipZ > mapChip.GetLength(0) - 1)         //エラー対策
                    continue;
                for (int mapchipX = x; mapchipX <= x + collisionRange; mapchipX++)
                {
                    if (mapchipX < 0 || mapchipX > mapChip.GetLength(1) - 1)     //エラー対策
                        continue;
                    int index = mapchipZ * mapChip.GetLength(1) + mapchipX;      //添え字を計算
                    if (hitbox.collision.Intersects(mapBlocks[index].Collision))       //当たっていれば
                    {
                        ((MoveDamageBox)hitbox).End();     　//キャラクターの位置修正
                    }
                }
            }
        }

        /// <summary>
        /// キャラクターとマップのあたり判定
        /// </summary>
        /// <param name="chara">チェックするキャラクター</param>
        public void MapCollision(CharacterBase chara)
        {
            float floatX = ((MapDef.TILE_SIZE / 2.0f + chara.Collision.Position.X) / MapDef.TILE_SIZE);      //そのマスの左右半分
            float floatZ = ((MapDef.TILE_SIZE / 2.0f + chara.Collision.Position.Z) / MapDef.TILE_SIZE);      //そのマスの上下半分
            int x = (int)((MapDef.TILE_SIZE / 2.0f + chara.Collision.Position.X) / MapDef.TILE_SIZE);        //マス：X
            int z = (int)((MapDef.TILE_SIZE / 2.0f + chara.Collision.Position.Z) / MapDef.TILE_SIZE);        //マス：Y
            //判定半径（キャラクターのサイズにより違う）
            int collisionRange = (int)(chara.Collision.Radius * 2 / MapDef.TILE_SIZE) + 1;
            Point checkDir = new Point(0, 0);           //判定基準点
            if (floatX - x > 0.5f)      //int型を引くと値は0～1まで　判定方向もわかる
                checkDir.X = 0;         //右半分と判定する場合は基準変動なし
            else
                checkDir.X = -1 * collisionRange;       //左半分と判定する場合は基準をずらす
            if (floatZ - z > 0.5f)      //X軸と同じ
                checkDir.Y = 0;
            else
                checkDir.Y = -1 * collisionRange;

            x += checkDir.X;            //基準をずらす
            z += checkDir.Y;

            ClampPoint(ref x, ref z);   //エラー対策

            for (int mapchipZ = z; mapchipZ <= z + collisionRange; mapchipZ++)   //基準から半径の長さのマスと判定
            {
                if (mapchipZ < 0 || mapchipZ > mapChip.GetLength(0) - 1)         //エラー対策
                    continue;
                for (int mapchipX = x; mapchipX <= x + collisionRange; mapchipX++)
                {
                    if (mapchipX < 0 || mapchipX > mapChip.GetLength(1) - 1)     //エラー対策
                        continue;
                    int index = mapchipZ * mapChip.GetLength(1) + mapchipX;      //添え字を計算
                    if (chara.Collision.IsCollision(mapBlocks[index].Collision))       //当たっていれば
                    {
                        chara.Collision.Hit(mapBlocks[index].Collision);       　//キャラクターの位置修正
                    }
                }
            }
        }

        /// <summary>
        /// プロジェクターとマップの当たり判定
        /// </summary>
        /// <param name="projecter">判定するプロジェクター</param>
        public void MapCollision(Projector projecter)
        {
            float floatX = ((MapDef.TILE_SIZE / 2.0f + projecter.Collision.Position.X) / MapDef.TILE_SIZE);      //そのマスの左右半分
            float floatZ = ((MapDef.TILE_SIZE / 2.0f + projecter.Collision.Position.Z) / MapDef.TILE_SIZE);      //そのマスの上下半分
            int x = (int)((MapDef.TILE_SIZE / 2.0f + projecter.Collision.Position.X) / MapDef.TILE_SIZE);        //マス：X
            int z = (int)((MapDef.TILE_SIZE / 2.0f + projecter.Collision.Position.Z) / MapDef.TILE_SIZE);        //マス：Y
            //判定半径（キャラクターのサイズにより違う）
            int collisionRange = (int)(projecter.Collision.Collision.Radius * 2 / MapDef.TILE_SIZE) + 1;
            Point checkDir = new Point(0, 0);           //判定基準点
            if (floatX - x > 0.5f)                      //int型を引くと値は0～1まで　判定方向もわかる
                checkDir.X = 0;                         //右半分と判定する場合は基準変動なし
            else
                checkDir.X = -1 * collisionRange;       //左半分と判定する場合は基準をずらす
            if (floatZ - z > 0.5f)                      //X軸と同じ
                checkDir.Y = 0;
            else
                checkDir.Y = -1 * collisionRange;

            x += checkDir.X;            //基準をずらす
            z += checkDir.Y;

            ClampPoint(ref x, ref z);   //エラー対策

            for (int mapchipZ = z; mapchipZ <= z + collisionRange; mapchipZ++)   //基準から半径の長さのマスと判定
            {
                if (mapchipZ < 0 || mapchipZ > mapChip.GetLength(0) - 1)         //エラー対策
                    continue;
                for (int mapchipX = x; mapchipX <= x + collisionRange; mapchipX++)
                {
                    if (mapchipX < 0 || mapchipX > mapChip.GetLength(1) - 1)     //エラー対策
                        continue;
                    int index = mapchipZ * mapChip.GetLength(1) + mapchipX;      //添え字を計算
                    if (mapBlocks[index] is FallBlock)
                        continue;
                    if (projecter.Collision.IsCollision(mapBlocks[index].Collision))       //当たっていれば
                    {
                        projecter.Collision.Hit(mapBlocks[index].Collision);       　      //プロジェクターの位置修正
                    }
                }
            }
            projecter.UpdateLook();        //プロジェクターのViewマトリクス更新
        }

        /// <summary>
        /// マップを描画
        /// </summary>
        public void Draw()
        {
            foreach (Cube c in mapBlocksToDraw)     //描画領域しか描画しない
            {
                c.Draw();
            }

            if(drawExit)
                exitEffect.Draw();
        }

        /// <summary>
        /// 画面上にマップ表示
        /// </summary>
        public void DrawMiniMap()
        {
            gameDevice.Renderer.RenderMiniMap();    //ミニマップにレンダリング
            foreach (Cube c in mapBlocksToDraw)     //描画領域しか描画しない
            {
                c.DrawMiniMap();
            }
            if(drawExit)
                exitEffect.Draw();
            gameDevice.Renderer.RenderMainProjector();      //メインプロジェクターに戻す
        }

        /// <summary>
        /// 終点描画するかを設定
        /// </summary>
        /// <param name="toDraw">trueは描画</param>
        public void SwitchDrawExit(bool toDraw)
        {
            if (drawExit != toDraw)
                exitEffect.Reset();

            drawExit = toDraw;
        }

        public bool IsOver()
        {
            return drawExit;
        }

        public void SetExitColor(Color color)
        {
            exitEffect.SetColor(color);
        }

        /// <summary>
        /// 空地の座標を返す
        /// </summary>
        /// <returns></returns>
        public Vector3 RandomSpace()
        {
            if (space.Count <= 0)
                return Vector3.Zero;    //Error対策

            int index = gameDevice.Random.Next(0, space.Count);
            Point spaceCell = space[index];
            space.RemoveAt(index);

            Vector3 position = new Vector3(
                spaceCell.X * MapDef.TILE_SIZE,
                MapDef.TILE_SIZE / 2.0f,
                spaceCell.Y * MapDef.TILE_SIZE);
            return position;
        }

        /// <summary>
        /// Bossの設置位置
        /// </summary>
        /// <param name="bossIndex"></param>
        /// <returns></returns>
        public Vector3 BossPoint(int bossIndex)
        {
            Point p = bossPoint[bossIndex];
            Vector3 position = new Vector3(
                p.X * MapDef.TILE_SIZE,
                MapDef.TILE_SIZE / 2.0f,
                p.Y * MapDef.TILE_SIZE);
            return position;
        }
    }
}

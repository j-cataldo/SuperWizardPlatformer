﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Maps.Tiled;
using FarseerPhysics.Dynamics;

namespace SuperWizardPlatformer
{
    class GameplayScene : IScene
    {
        private static Color defaultBgColor = Color.Black;

        private ContentManager content;
        private TiledMap map;
        private SpriteBatch spriteBatch;
        private World physicsWorld;
        private GameObjectFactory factory;
        private Color bgColor;

        private List<IEntity> entities;
        private List<IDrawable> drawables;

        public GameplayScene(Game game, string mapName)
        {
            content = new ContentManager(game.Services, game.Content.RootDirectory);
            map = content.Load<TiledMap>(mapName);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            physicsWorld = new World(new Vector2(0, 9.8f));

            bgColor = map.BackgroundColor != null ? (Color)map.BackgroundColor : defaultBgColor;

            factory = new GameObjectFactory(spriteBatch);
            var results = factory.CreateScene(map, physicsWorld);
            entities = results.Item1;
            drawables = results.Item2;

            //foreach (var objGroup in map.ObjectGroups)
            //{
            //    foreach (var obj in objGroup.Objects)
            //    {
            //        if (obj.Gid == null)
            //        {
            //            throw new ArgumentNullException(
            //                string.Format("obj.Gid for object '{0}'", obj.Name));
            //        }
            //        else
            //        {
            //            var fixPos = new Vector2(obj.X, obj.Y);
            //            var fixSize = new Vector2(obj.Width, obj.Height);
            //            DrawableFixture fixture = new DrawableFixture(fixPos, fixSize, obj.IsVisible);

            //            DrawableTexture drawable = new DrawableTexture(
            //                fixture, map.GetTileRegion((int)obj.Gid), spriteBatch);

            //            fixture.Drawable = drawable;

            //            entities.Add(fixture);
            //            drawables.Add(drawable);
            //        }
            //    }
            //}
        }

        public bool IsReadyToQuit { get; private set; } = false;

        public bool IsDisposed { get; private set; } = false;

        public void Dispose()
        {
            if (!IsDisposed)
            {
                content.Dispose();
                spriteBatch.Dispose();

                IsDisposed = true;
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            graphicsDevice.Clear(bgColor);

            spriteBatch.Begin();

            map.Draw(spriteBatch, new Rectangle(0, 0, 640, 480));

            foreach (var drawable in drawables)
            {
                Console.WriteLine("about to draw!");
                drawable.Draw();
            }

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                entity.Update(gameTime);
            }

            physicsWorld.Step(1.0f / 60.0f);
        }
    }
}

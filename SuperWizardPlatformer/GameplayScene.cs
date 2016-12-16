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
        private static readonly Color defaultBgColor = Color.Black;
        private const int CAPACITY_DEFAULT = 32;
        private const float GRAVITY_Y_DEFAULT = 9.8f;

        private ContentManager content;
        private TiledMap map;
        private SpriteBatch spriteBatch;
        private GameObjectFactory factory;
        private Color bgColor;

        public GameplayScene(Game game, string mapName)
        {
            content = new ContentManager(game.Services, game.Content.RootDirectory);
            map = content.Load<TiledMap>(mapName);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            bgColor = map.BackgroundColor != null ? (Color)map.BackgroundColor : defaultBgColor;

            factory = new GameObjectFactory(PhysicsWorld, spriteBatch);
            var results = factory.CreateScene(map);
            Entities = results.Item1;
            Drawables = results.Item2;
        }

        public List<IEntity> Entities { get; private set; } = new List<IEntity>(CAPACITY_DEFAULT);

        public List<IDrawable> Drawables { get; private set; } = new List<IDrawable>(CAPACITY_DEFAULT);

        public World PhysicsWorld { get; private set; } = new World(new Vector2(0, GRAVITY_Y_DEFAULT));

        public bool IsReadyToQuit { get; private set; } = false;

        public bool IsDisposed { get; private set; } = false;

        public void Dispose()
        {
            if (!IsDisposed)
            {
                PhysicsWorld.ClearForces();
                PhysicsWorld.Clear();
                content.Dispose();
                spriteBatch.Dispose();

                IsDisposed = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in Entities)
            {
                entity.Update(gameTime);
            }

            PhysicsWorld.Step(1.0f / 60.0f);
        }

        public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            graphicsDevice.Clear(bgColor);

            spriteBatch.Begin();

            map.Draw(spriteBatch, new Rectangle(0, 0, 640, 480));

            foreach (var drawable in Drawables)
            {
                drawable.Draw();
            }

            spriteBatch.End();
        }
    }
}

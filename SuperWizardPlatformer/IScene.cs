﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SuperWizardPlatformer
{
    interface IScene : IDisposable
    {
        bool IsReadyToQuit { get; }

        void Update(GameTime gameTime);

        void Draw(GraphicsDevice graphicsDevice, GameTime gameTime);
    }
}
﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Krypton
{
    public static class LightTextureBuilder
    {
        public static Texture2D CreatePointLight(
            GraphicsDevice device,
            int size)
        {
            return CreateConicLight(
                device: device,
                size: size,
                fov: MathHelper.TwoPi,
                nearPlaneDistance: 0);
        }

        public static Texture2D CreateConicLight(
            GraphicsDevice device,
            int size,
            float fov)
        {
            return CreateConicLight(
                device: device,
                size: size,
                fov: fov,
                nearPlaneDistance: 0);
        }

        public static Texture2D CreateConicLight(
            GraphicsDevice device,
            int size,
            float fov,
            float nearPlaneDistance)
        {
            //if (!IsPowerOfTwo(size))
            //{
            //    throw new ArgumentOutOfRangeException(nameof(size), "The size must be a power of 2");
            //}

            var data = new float[size, size];

            float center = size >> 1;

            fov = fov/2;

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    var distance = Vector2.Distance(
                        new Vector2(x, y),
                        new Vector2(center));

                    var difference = new Vector2(x, y) - new Vector2(center);

                    var angle = (float) Math.Atan2(difference.Y, difference.X);

                    if (distance <= center && distance >= nearPlaneDistance && Math.Abs(angle) <= fov)
                    {
                        data[x, y] = (center - distance)/center;
                    }
                    else
                    {
                        data[x, y] = 0;
                    }
                }
            }

            var tex = new Texture2D(device, size, size);

            var data1D = new Color[size*size];

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    data1D[x + y*size] = new Color(new Vector3(data[x, y]));
                }
            }

            tex.SetData(data1D);

            return tex;
        }

        private static bool IsPowerOfTwo(int x)
        {
            return 0 == (x & (x - 1));
        }
    }
}

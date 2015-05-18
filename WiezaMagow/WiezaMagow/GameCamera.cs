using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDef
{
    class GameCamera
    {
        public float cameraHeight = 20000.0f;

        public Vector3 cameraPosition;
        KeyboardState newState, oldState;

        float targetX, targetY;

        float prevWheelValue;
        float currWheelValue;

        public GameCamera()
        {
            targetX = GlobalGameConstants.mapWidth * GlobalGameConstants.mapGroundPlatesDistance / 2;
            targetY = GlobalGameConstants.mapHeight * GlobalGameConstants.mapGroundPlatesDistance / 2;

            cameraPosition = new Vector3(targetX, targetY-20000.0f, cameraHeight + 10000.0f);
        }

            public void update(float aspectRatio)
            {
                newState = Keyboard.GetState();

                if (newState.IsKeyDown(Keys.W))
                {
                    cameraPosition.Y += 500.0f;
                    targetY +=500.0f;
                }
                else if (newState.IsKeyDown(Keys.S))
                {
                    cameraPosition.Y -= 500.0f;
                    targetY -=500.0f;
                }
                else if (newState.IsKeyDown(Keys.A))
                {
                    cameraPosition.X -= 500.0f;
                    targetX -= 500.0f;
                }
                else if (newState.IsKeyDown(Keys.D))
                {
                    cameraPosition.X += 500.0f;
                    targetX += 500.0f;
                }
                else if (newState.IsKeyDown(Keys.Subtract))
                {
                    if (cameraPosition.Z - 1000.0f >= 6000.0f)
                    {
                        cameraPosition.Z -= 1000.0f;
                        cameraPosition.Y += 600.0f;
                    }
                }
                else if (newState.IsKeyDown(Keys.Add))
                {
                    if (cameraPosition.Z + 1000.0f <= 30000.0f)
                    {
                        cameraPosition.Z += 1000.0f;
                        cameraPosition.Y -= 600.0f;
                    }
                }

                prevWheelValue = currWheelValue;
                currWheelValue = Mouse.GetState().ScrollWheelValue;

                if (prevWheelValue < currWheelValue)
                {
                    if (cameraPosition.Z - 1000.0f >= 6000.0f)
                    {
                        cameraPosition.Z -= 1000.0f;
                        cameraPosition.Y += 600.0f;
                    }
                }
                else if (prevWheelValue > currWheelValue)
                {
                    if (cameraPosition.Z + 1000.0f <= 30000.0f)
                    {
                        cameraPosition.Z += 1000.0f;
                        cameraPosition.Y -= 600.0f;
                    }
                }

                oldState = newState;             

                Game1.viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(targetX,targetY,0.0f), Vector3.Up);
                Game1.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45.0f),
                    aspectRatio,
                    GlobalGameConstants.nearClip,
                    GlobalGameConstants.farClip);
            }
    }
}

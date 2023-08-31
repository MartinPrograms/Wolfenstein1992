using System.Drawing;
using Wolfenstein1992.Gamer;

namespace Wolfenstein1992.Render;

public class Raycaster
{
    public static Color[] Render(Game game, Map map, Player player)
    {
        var colData = new Color[game.XWidth * game.YHeight];
        var ZBuffer = new double[game.XWidth];
        var spriteOrder = new int[game.map.Entities.Count];
        var spriteDistance = new double[game.map.Entities.Count];
        for (int PixelX = 0; PixelX < game.XWidth; PixelX++)
        {
            // calculate the ray position and direction
            double cameraX = 2 * PixelX / (double)game.XWidth - 1;
            double rayDirX = player.DirX + player.PlaneX * cameraX;
            double rayDirY = player.DirY + player.PlaneY * cameraX;

            int mapX = (int)player.PosX;
            int mapY = (int)player.PosY;

            double sideDistX;
            double sideDistY;

            double deltaDistX = Math.Abs(1 / rayDirX);
            double deltaDistY = Math.Abs(1 / rayDirY);
            double perpWallDist;

            int stepX;
            int stepY;

            int hit = 0; //was there a wall hit?
            int side = 0; //was a NS or a EW wall hit?

            if (rayDirX < 0)
            {
                stepX = -1;
                sideDistX = (player.PosX - mapX) * deltaDistX;
            }
            else
            {
                stepX = 1;
                sideDistX = (mapX + 1.0 - player.PosX) * deltaDistX;
            }

            if (rayDirY < 0)
            {
                stepY = -1;
                sideDistY = (player.PosY - mapY) * deltaDistY;
            }
            else
            {
                stepY = 1;
                sideDistY = (mapY + 1.0 - player.PosY) * deltaDistY;
            }

            
            while (hit == 0)
            {
                if (sideDistX < sideDistY)
                {
                    sideDistX += deltaDistX;
                    mapX += stepX;
                    side = 0;
                }
                else
                {
                    sideDistY += deltaDistY;
                    mapY += stepY;
                    side = 1;
                }
                
                if (map.MapData[mapX][mapY] > 0)
                {
                    hit = 1;
                }
            }

            if (side == 0)
            {
                perpWallDist = (sideDistX - deltaDistX);
            }
            else
            {
                perpWallDist = (sideDistY - deltaDistY);
            }
            
            int lineHeight = (int)(game.YHeight / perpWallDist);
            
            
            
            int drawStart = -lineHeight / 2 + game.YHeight / 2;
            if(drawStart < 0)
            {
                drawStart = 0;
            }
            
            int drawEnd = lineHeight / 2 + game.YHeight / 2;
            if(drawEnd >= game.YHeight)
            {
                drawEnd = game.YHeight - 1;
            }

            if (mapX < 0 || mapY < 0 || mapX >= map.Width || mapY >= map.Height)
            {
                throw new Exception("Map out of bounds!");
            }
            int texNum = map.MapData[mapX][mapY] - 1;
            
            double wallX;
            if(side == 0)
            {
                wallX = player.PosY + perpWallDist * rayDirY;
            }
            else
            {
                wallX = player.PosX + perpWallDist * rayDirX;
            }
            wallX -= Math.Floor(wallX);
            
            int texX = (int)(wallX * 64);
            if(side == 0 && rayDirX > 0)
            {
                texX = 64 - texX - 1;
            }
            if(side == 1 && rayDirY < 0)
            {
                texX = 64 - texX - 1;
            }
            
            double step = 1.0 * 64 / lineHeight;
            double texPos = (drawStart - game.YHeight / 2 + lineHeight / 2) * step;
            var fog = 1 - (perpWallDist / 35);
            if (fog < 0)
            {
                fog = 0;
            }
            var fogColor = Color.FromArgb(255,0,0,0);

            for (int y = drawStart; y < drawEnd; y++)
            {
                int texY = (int)texPos & (64 - 1);
                texPos += step;
                Color col = game.Textures[texNum].GetPixel(texX, texY);
                // Fog
                var r = (byte)(col.R * fog);
                var g = (byte)(col.G * fog);
                var b = (byte)(col.B * fog);
                if (side == 1)
                {
                    col = Color.FromArgb(col.A, col.R / 2, col.G / 2, col.B / 2);
                }
                
                if (r < fogColor.R)
                {
                    r = fogColor.R;
                }
                if (g < fogColor.G)
                {
                    g = fogColor.G;
                }
                if (b < fogColor.B)
                {
                    b = fogColor.B;
                }

                col = Color.FromArgb(col.A, r, g, b);


                
                colData[PixelX + y * game.XWidth] = col;
            }
            ZBuffer[PixelX] = perpWallDist; //perpendicular distance is used
            
            // Floor casting
            double floorXWall;
            double floorYWall;
            
            if(side == 0 && rayDirX > 0)
            {
                floorXWall = mapX;
                floorYWall = mapY + wallX;
            }
            else if(side == 0 && rayDirX < 0)
            {
                floorXWall = mapX + 1.0;
                floorYWall = mapY + wallX;
            }
            else if(side == 1 && rayDirY > 0)
            {
                floorXWall = mapX + wallX;
                floorYWall = mapY;
            }
            else
            {
                floorXWall = mapX + wallX;
                floorYWall = mapY + 1.0;
            }
            
            double distWall;
            double distPlayer;
            double currentDist;
            
            distWall = perpWallDist;
            distPlayer = 0.0;
            
            if(drawEnd < 0)
            {
                drawEnd = game.YHeight;
            }

            for (int y = drawEnd + 1; y < game.YHeight; y++)
            {
                currentDist = game.YHeight / (2.0 * y - game.YHeight);
                
                double weight = (currentDist - distPlayer) / (distWall - distPlayer);
                
                double currentFloorX = weight * floorXWall + (1.0 - weight) * player.PosX;
                double currentFloorY = weight * floorYWall + (1.0 - weight) * player.PosY;
                
                int floorTexX;
                int floorTexY;
                floorTexX = (int)(currentFloorX * 64) % 64;
                floorTexY = (int)(currentFloorY * 64) % 64;
                
                Color floorCol = game.Textures[3].GetPixel(floorTexX, floorTexY);
                Color ceilingCol = game.Textures[4].GetPixel(floorTexX, floorTexY);

                colData[PixelX + y * game.XWidth] = Color.FromArgb(255, floorCol.R, floorCol.G, floorCol.B);
                colData[PixelX + (game.YHeight - y) * game.XWidth] = Color.FromArgb(255, ceilingCol.R, ceilingCol.G, ceilingCol.B);
            }
            
            
        }
        
        // Sprite casting
        for (int i = 0; i < game.map.Entities.Count; i++)
        {
            spriteOrder[i] = i;
            spriteDistance[i] = ((player.PosX - game.map.Entities[i].PosX) * (player.PosX - game.map.Entities[i].PosX) + (player.PosY - game.map.Entities[i].PosY) * (player.PosY - game.map.Entities[i].PosY));
        }
        
        Array.Sort(spriteOrder, spriteDistance);

        for (int i = 0; i < game.map.Entities.Count; i++)
        {
            double spriteX = game.map.Entities[spriteOrder[i]].PosX - player.PosX;
            double spriteY = game.map.Entities[spriteOrder[i]].PosY - player.PosY;
            
            double invDet = 1.0 / (player.PlaneX * player.DirY - player.DirX * player.PlaneY);
            
            double transformX = invDet * (player.DirY * spriteX - player.DirX * spriteY);
            
            double transformY = invDet * (-player.PlaneY * spriteX + player.PlaneX * spriteY);
            
            int spriteScreenX = (int)((game.XWidth / 2) * (1+ transformX / transformY));
            
            float uDiv = game.map.Entities[i].Size.X;
            float vDiv = game.map.Entities[i].Size.Y;
            float vMove = game.map.Entities[i].VMove;
            
            int vMoveScreen = (int)(vMove / transformY);
            
            int spriteHeight = (int)(Math.Abs(game.YHeight / (transformY)) / vDiv);
            
            int drawStartY = -spriteHeight / 2 + game.YHeight / 2 + vMoveScreen;
            if(drawStartY < 0)
            {
                drawStartY = 0;
            }
            
            int drawEndY = spriteHeight / 2 + game.YHeight / 2 + vMoveScreen;
            if(drawEndY >= game.YHeight)
            {
                drawEndY = game.YHeight - 1;
            }
            
            int spriteWidth = (int)(Math.Abs(game.YHeight / (transformY)) / uDiv);
            
            int drawStartX = -spriteWidth / 2 + spriteScreenX;
            
            if(drawStartX < 0)
            {
                drawStartX = 0;
            }
            
            int drawEndX = spriteWidth / 2 + spriteScreenX;
            if(drawEndX >= game.XWidth)
            {
                drawEndX = game.XWidth - 1;
            }

            for (int stripe = drawStartX; stripe < drawEndX; stripe++)
            {
                int texX = (int)(256 * (stripe - (-spriteWidth / 2 + spriteScreenX)) * 64 / spriteWidth) / 256;
                
                if(transformY > 0 && transformY < ZBuffer[stripe])
                {
                    for (int y = 0; y < drawStartY; y++)
                    {
                        colData[stripe + y * game.XWidth] = Color.FromArgb(255, 0, 0, 0);
                    }
                    for (int y = drawStartY; y < drawEndY; y++)
                    {
                        int d = (y - vMoveScreen) * 256 - game.YHeight * 128 + spriteHeight * 128;
                        int texY = ((d * 64) / spriteHeight) / 256;
                        Color col = game.map.Entities[i].GetTexture(player, game.map.Entities[i]).GetPixel(texX, texY);
                        if (col != Color.FromArgb(255, 0, 0, 0) && col.A == 255)
                        {
                            colData[stripe + y * game.XWidth] = col;
                        }else if (col.A != 255)
                        {
                            var col2 = colData[stripe + y * game.XWidth];
                            var r = (byte)((col.R * col.A / 255) + (col2.R * (255 - col.A) / 255));
                            var g = (byte)((col.G * col.A / 255) + (col2.G * (255 - col.A) / 255));
                            var b = (byte)((col.B * col.A / 255) + (col2.B * (255 - col.A) / 255));
                            colData[stripe + y * game.XWidth] = Color.FromArgb(255, r, g, b);
                        }
                    }
                }
            }
            
        }
        
        // Gun rendering
        var gun = game.gunRenderer;
        var centerX = game.XWidth / 2;
        var gunWidth = gun.Gun.Width;
        // Guns are 1:1 aspect ratio
        var gunHeight = gunWidth;
        var gunStartX = centerX - gunWidth / 2;
        var gunStartY = game.YHeight - gunHeight;
        for (int x = 0; x < gunWidth; x++)
        {
            // Guns are 64x64
            var texX = (int)(x * 64 / gunWidth);
            
            for (int y = 0; y < gunHeight; y++)
            {
                var texY = (int)(y * 64 / gunHeight);
                
                var col = gun.GetPixel(texX, texY);
                if (col != Color.FromArgb(255, 0, 0, 0) && col.A == 255)
                {
                    colData[x + gunStartX + (y + gunStartY) * game.XWidth] = col;
                }else if (col.A != 255)
                {
                    var col2 = colData[x + gunStartX + (y + gunStartY) * game.XWidth];
                    var r = (byte)((col.R * col.A / 255) + (col2.R * (255 - col.A) / 255));
                    var g = (byte)((col.G * col.A / 255) + (col2.G * (255 - col.A) / 255));
                    var b = (byte)((col.B * col.A / 255) + (col2.B * (255 - col.A) / 255));
                    colData[x + gunStartX + (y + gunStartY) * game.XWidth]  = Color.FromArgb(255, r, g, b);
                }
            }
        }
        
        return colData;
    }
} 
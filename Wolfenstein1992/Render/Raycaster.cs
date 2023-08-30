using System.Drawing;
using Wolfenstein1992.Gamer;

namespace Wolfenstein1992.Render;

public class Raycaster
{
    public static Color[] Render(Game game, Map map, Player player)
    {
        var colData = new Color[game.XWidth * game.YHeight];

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
            
            for (int y = drawStart; y < drawEnd; y++)
            {
                int texY = (int)texPos & (64 - 1);
                texPos += step;
                Color col = game.Textures[texNum].GetPixel(texX, texY);
                
                if (side == 1)
                {
                    col = Color.FromArgb(col.A, col.R / 2, col.G / 2, col.B / 2);
                }
                
                colData[PixelX + y * game.XWidth] = col;
            }
            
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
        
        return colData;
    }
}
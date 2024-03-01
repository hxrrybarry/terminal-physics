using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bouncymcball;

public class Raycast2(Scene scene, int x, int y, Vector2 dir)
{
    public Scene Scn { get; } = scene;
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    private Vector2 Direction { get; } = dir;
    
    public bool WillCollideX()
    {
        for (int i = 0; i < Direction.X; i++)
        {
            X++;
            if (X < Scn.XSize)
                if (Scn.ScreenSpace[X, Y] is not null)
                    return true;
                return true;
        }

        return false;
    }

    public bool WillCollideY()
    {
        for (int i = 0; i < Direction.Y; i++)
        {
            Y++;
            if (Y < Scn.YSize)
                if (Scn.ScreenSpace[X, Y] is not null)
                    return true;
                return true;
        }

        return false;
    }

    public bool WillCollide()
    {
        return WillCollideX() && WillCollideY();
    }
}

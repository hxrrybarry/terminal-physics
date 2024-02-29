using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace bouncymcball;

// this class negates many potential array accesses
// used instead of doing vectors as float[]
public class Vector2(float x, float y)
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;

    public void AddVector2(Vector2 v)
    {
        X += v.X;
        Y += v.Y;
    }
}

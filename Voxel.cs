using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bouncymcball;

public class Voxel(float x, float y, char texture)
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;
    public char Texture { get; } = texture;
}

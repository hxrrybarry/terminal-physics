using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bouncymcball;

public class Voxel(int x, int y, char texture)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public char Texture { get; } = texture;
}

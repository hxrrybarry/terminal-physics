using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bouncymcball;

public class Scene(int xSize, int ySize)
{
    public int XSize { get; } = xSize;
    public int YSize { get; } = ySize;

    private List<Voxel> Voxels { get; } = [];
    public Voxel?[,] ScreenSpace { get; set; }
    // special list of physics objects to save processing when doing physics tick
    public List<RigidBody> PhysicsObjects { get; } = [];

    // we add physics objects into a separate list to save processing-
    //- on calculation
    public void Populate(dynamic v)
    {
        if (v.GetType() == typeof(RigidBody))
            PhysicsObjects.Add(v);
            
        Voxels.Add(v);
    }

    public void Update()
    {
        // forgot why i did this will investigate later
        ScreenSpace = new Voxel?[XSize, YSize];
        foreach (var obj in Voxels)
            ScreenSpace[(int)MathF.Round(obj.X), (int)MathF.Round(obj.Y)] = obj;

        string result = "";
        for (int x = 0; x < XSize; x++)
        {
            result += '\n';
            for (int y = 0; y < YSize; y++)
                if (ScreenSpace[x, y] is not null)
                    result += ScreenSpace[x, y].Texture;
                else
                    result += ' ';
        }

        Console.Clear();
        Console.Write(result);
    }
}

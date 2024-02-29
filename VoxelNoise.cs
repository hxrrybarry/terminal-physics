using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bouncymcball;

public class VoxelNoise(int xSize, int ySize, int numberOfPoints, float threshold, char[] textures, int seed)
{
    private int XSize { get; } = xSize;
    private int YSize { get; } = ySize;
    private int NumberOfPoints { get; } = numberOfPoints;
    private float Threshold { get; } = threshold;

    private char[] Textures { get; } = textures;
    public List<Voxel> PlacedVoxels { get; } = new();
    private int[,] PointsOnGrid { get; } = new int[numberOfPoints, 2];

    public int Seed { get; } = seed;

    private float GetDistanceToNearestPoint(int x, int y)
    {
        // get distance to nearest point by looping and overriding-
        //- the previously recorded nearestDistanceSquared if necessary
        int nearestDistanceSquared = int.MaxValue;
        for (int i = 0; i < NumberOfPoints; i++)
        {
            int dx = x - PointsOnGrid[i, 0];
            int dy = y - PointsOnGrid[i, 1];

            // distance formula: s = √((x1 - x2)^2 + (y1 - y2)^2)
            int distanceSquared = dx * dx + dy * dy;
            if (distanceSquared < nearestDistanceSquared)
                nearestDistanceSquared = distanceSquared;
        }

        // we square root here to avoid constantly doing a calculation during the loop
        return MathF.Sqrt(nearestDistanceSquared);
    }

    public void GenerateGrid()
    {
        Random rnd = new(Seed);

        // choose NumberOfPoints amount of random coordinates to-
        //- sit on the grid for later point-distance calculations
        for (int i = 0; i < NumberOfPoints; i++)
        {
            PointsOnGrid[i, 0] = rnd.Next(XSize);
            PointsOnGrid[i, 1] = rnd.Next(YSize);
        }

        // *this is yikes but no other real solution
        // loop through each point and perform a distance calculation which is then evaluated at a threshold-
        // - to determine whether to fill that point with a value
        for (int x = 0; x < XSize; x++)
        {
            for (int y = 0; y < YSize; y++)
            {
                float distance = GetDistanceToNearestPoint(x, y);
                if (distance > Threshold)
                    PlacedVoxels.Add(new(x, y, Textures[rnd.Next(Textures.Length)]));
            }
        }
    }
}

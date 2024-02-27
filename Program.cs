using System;

namespace bouncymcball;

internal class Program
{
    public static void Main()
    {
        Scene scene = new(27, 120);
        RigidBody ball = new(scene, scene.XSize / 2, scene.YSize / 2, 'O', 4f, 0.3f);

        VoxelNoise map = new(scene.XSize, scene.YSize, 130, 3.5f, '#', new Random().Next());
        map.GenerateGrid();

        foreach (Voxel v in map.PlacedVoxels)
            scene.Populate(v);

        for (int i = 0; i < scene.YSize; i++)
            scene.Populate(new Voxel(scene.XSize - 1, i, '#'));

        scene.Populate(ball);
        scene.Update();

        ball.AddForce([new Random().Next(-200, 200) / 100f, new Random().Next(-200, 200) / 100f]);

        Console.ReadKey();
        // physics loop
        while (true)
        {
            // Console.ReadKey();
            Thread.Sleep(35);

            foreach(RigidBody obj in scene.PhysicsObjects)
                 obj.DoPhysicsTick();
                
            scene.Update();
        }
    }
}
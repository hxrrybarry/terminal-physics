using System;

namespace bouncymcball;

internal class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Scene scene = new(27, 120);
        // RigidBody ball = new(scene, scene.XSize / 2, scene.YSize / 2, 'γ', 4f, 0f, 0.9f);

        VoxelNoise map = new(scene.XSize, scene.YSize, 130, 3.5f, '#', new Random().Next());
        map.GenerateGrid();

        foreach (Voxel v in map.PlacedVoxels)
            scene.Populate(v);

        for (int i = 0; i < 20; i++)
        {
            RigidBody ball = new RigidBody(scene, new Random().Next(0, scene.XSize), new Random().Next(0, scene.YSize), 'O', 4f, 0.1f, 0.3f);
            ball.AddForce([new Random().Next(-300, 300) / 100f, new Random().Next(-300, 300) / 100f]);
            scene.Populate(ball);
        }

        for (int i = 0; i < scene.YSize; i++)
        {
            scene.Populate(new Voxel(scene.XSize - 1, i, '#'));
            scene.Populate(new Voxel(0, i, '#'));
        }
         
        for (int i = 0; i < scene.XSize; i++)
        {
            scene.Populate(new Voxel(i, scene.YSize - 1, '#'));
            scene.Populate(new Voxel(i, 0, '#'));
        }

        scene.Update();

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

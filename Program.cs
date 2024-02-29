using System;

namespace bouncymcball;

internal class Program
{
    public static void Main()
    {
        while (true)
        {
            Random rnd = new();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Scene scene = new(35, 140);

            char[] textures = ['0', '1', '#'];
            VoxelNoise map = new(scene.XSize, scene.YSize, 160, 3.5f, textures, rnd.Next());
            map.GenerateGrid();
            foreach (Voxel v in map.PlacedVoxels)
                scene.Populate(v);
       
            for (int i = 0; i < scene.YSize; i++)
            {
                scene.Populate(new Voxel(scene.XSize - 1, i, textures[rnd.Next(textures.Length)]));
                scene.Populate(new Voxel(0, i, textures[rnd.Next(textures.Length)]));
            }

            for (int i = 0; i < scene.XSize; i++)
            {
                scene.Populate(new Voxel(i, scene.YSize - 1, textures[rnd.Next(textures.Length)]));
                scene.Populate(new Voxel(i, 0, textures[rnd.Next(textures.Length)]));
            }

            scene.Update();

            for (int i = 0; i < 30; i++)
            {
                int xCoord = rnd.Next(0, scene.XSize);
                int yCoord = rnd.Next(0, scene.YSize);

                while (scene.ScreenSpace[xCoord, yCoord] is not null)
                {
                    xCoord = rnd.Next(0, scene.XSize);
                    yCoord = rnd.Next(0, scene.YSize);
                }

                RigidBody ball = new(scene, xCoord, yCoord, 'X', 20f, 0.1f, 0.3f);
                ball.AddForce(new Vector2(rnd.Next(-500, 500) / 100f, rnd.Next(-500, 500) / 100f));
                scene.Populate(ball);
            }

            scene.Update();

            // Console.ReadKey();
            // physics loop
            do
            {
                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(35);

                    foreach (RigidBody obj in scene.PhysicsObjects)
                        obj.DoPhysicsTick();

                    Console.WriteLine($"\nMap seed: {map.Seed}");
                    scene.Update();              
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Enter);
        } 
    }
}

using System;

namespace bouncymcball;

internal class Program
{
    const int SCENE_X = 34;
    const int SCENE_Y = 148;

    //1116511700 - cool seed, funnel things lol haha xD MUAHAHAHAHA
    //1931164057 - COCK SEED!! COCK SEED!! COCK SEED ALERT!!!!

    public static void Main()
    {
        int sleepTime = 15;
        while (true)
        {
            Random rnd = new();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Scene scene = new(SCENE_X, SCENE_Y);

            char[] textures = ['▒', '▓', '█'];

            VoxelNoise map = new(scene.XSize, scene.YSize, 165, 3.5f, textures, rnd.Next());
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

            for (int i = 0; i < 10; i++)
            {
                int xCoord = rnd.Next(0, scene.XSize);
                int yCoord = rnd.Next(0, scene.YSize);

                while (scene.ScreenSpace[xCoord, yCoord] is not null)
                {
                    xCoord = rnd.Next(0, scene.XSize);
                    yCoord = rnd.Next(0, scene.YSize);
                }

                RigidBody ball = new(scene, xCoord, yCoord, '●', 15f, 0f, 1f);
                ball.AddForce(new Vector2(rnd.Next(-1000, 1000) / 100f, rnd.Next(-1000, 1000) / 100f));
                scene.Populate(ball);
            }

            scene.Update();

            // Console.ReadKey();
            // physics loop       
            bool running = true;
            while (running)
            {
                Thread.Sleep(sleepTime);

                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.RightArrow:
                            if (sleepTime > 0)
                                sleepTime--;
                            break;
                        case ConsoleKey.LeftArrow:
                            ++sleepTime;
                            break;
                        case ConsoleKey.Enter:
                            running = false;
                            break;
                    }
                }

                foreach (RigidBody obj in scene.PhysicsObjects)
                    obj.DoPhysicsTick();

                Console.WriteLine($"\nMap seed: {map.Seed} [En]\nTimescale: 1f/{sleepTime}ms [<-/->]");
                scene.Update();          
            }
        } 
    }
}

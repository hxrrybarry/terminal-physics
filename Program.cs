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
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int sleepTime = 22;
        char[] textures = ['▒', '▓', '█'];
        while (true)
        {
            Random rnd = new();
            
            Scene scene = new(SCENE_X, SCENE_Y);

            VoxelNoise map = new(scene.XSize, scene.YSize, 145, 3.5f, textures, rnd.Next());
            map.GenerateGrid();

            foreach (Voxel v in map.PlacedVoxels)
                scene.Populate(v);
       
            for (int i = 0; i < scene.YSize; i++)
            {
                scene.Populate(new Voxel(scene.XSize - 1, i, textures[0]));
                scene.Populate(new Voxel(0, i, textures[0]));
            }

            for (int i = 0; i < scene.XSize; i++)
            {
                scene.Populate(new Voxel(i, scene.YSize - 1, textures[0]));
                scene.Populate(new Voxel(i, 0, textures[0]));
            }

            scene.Update();

            for (int i = 0; i < 10; i++)
            {
                int xCoord = rnd.Next(0, scene.XSize);
                int yCoord = rnd.Next(0, scene.YSize);

                // checks to see if proposed RigidBody position is already taken
                while (scene.ScreenSpace[xCoord, yCoord] is not null)
                {
                    xCoord = rnd.Next(0, scene.XSize);
                    yCoord = rnd.Next(0, scene.YSize);
                }

                RigidBody ball = new(scene, xCoord, yCoord, '●', 15f, 0f, 1f);
                ball.AddForce(new Vector2(rnd.Next(-500, 500) / 100f, rnd.Next(-500, 500) / 100f));
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
                            sleepTime++;
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

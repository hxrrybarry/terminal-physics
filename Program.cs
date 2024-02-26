using System;

namespace bouncymcball;

internal class Program
{
    public static void Main()
    {


        /* for (int i = 0; i < 10; i++)
        {
            scene.Populate(new Voxel(i + 10, 90, '#'));
            scene.Populate(new Voxel(i + 10, 91, '#'));
            scene.Populate(new Voxel(i + 10, 92, '#'));
        } */

        Scene scene = new(27, 120);
        RigidBody v = new(scene, 15, 15, 'O', 5f, 0.2f, 1f);

        for (int i = 0; i < 50; i++)
        {
            scene.Populate(new Voxel(20, i + 43, '#'));
            scene.Populate(new Voxel(21, i + 43, '#'));
            scene.Populate(new Voxel(22, i + 43, '#'));
        }

        scene.Populate(v);
        scene.Update();

        v.AddForce([2, -2]);


        Console.ReadKey();
        // physics loop
        while (true)
        {
            // Console.ReadKey();
            Thread.Sleep(50);

            foreach(RigidBody obj in scene.PhysicsObjects)
                 obj.DoPhysicsTick();
                
            scene.Update();
        }
    }
}
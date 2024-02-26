using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bouncymcball;

public class RigidBody(Scene sc, int x, int y, char texture, float mass, float elasticity, float dampening) : Voxel(x, y, texture)
{
    public float Mass { get; } = mass;
    public float Elasticity { get; } = elasticity;
    private float[] Velocity { get; } = [0, 0];
    private float[] Acceleration { get; } = [0, 0];
    // private float Dampening { get; } = dampening;
    private float GRAVITATIONAL_FORCE { get; } = mass * 9.81f / 100f;

    private Scene Scn { get; } = sc;

    private float[] ResultantForce { get; } = [0, 0];

    // [0] -> x
    // [1] -> y
    public void DoPhysicsTick()
    {
        ResultantForce[1] += GRAVITATIONAL_FORCE;
        Acceleration[1] = ResultantForce[1] / Mass;
        Acceleration[0] = ResultantForce[0] / Mass;

        float newVelX = Velocity[0] + Acceleration[1];
        float newVelY = Velocity[1] + Acceleration[0];
        int newX = (int)MathF.Round(Math.Clamp(X + newVelX, 0, Scn.XSize - 1));
        int newY = (int)MathF.Round(Math.Clamp(Y + newVelY, 0, Scn.YSize - 1));

        // detect for X side collision
        if (Scn.ScreenSpace[newX, Y] is not null && Scn.ScreenSpace[newX, Y] != this)
        {
            // Console.ReadKey();
            ResultantForce[0] *= -1;
            Acceleration[1] = ResultantForce[0] / Mass;
            Velocity[0] *= -1 * Elasticity;
            newVelX = Velocity[0] + Acceleration[1];

            newX = (int)MathF.Round(Math.Clamp(X + newVelX, 0, Scn.XSize - 1));
        }
        // detect for Y side collision
        if (Scn.ScreenSpace[X, newY] is not null && Scn.ScreenSpace[X, newY] != this)
        {
            ResultantForce[1] *= -1;
            Acceleration[0] = ResultantForce[1] / Mass;
            Velocity[1] *= -1 * Elasticity;
            newVelY = Velocity[1] + Acceleration[0];

            newY = (int)MathF.Round(Math.Clamp(Y + newVelY, 0, Scn.YSize - 1));
        }

        Velocity[0] = newVelX;
        Velocity[1] = newVelY;
        X = newX;
        Y = newY;
    }

    public void AddForce(float[] force)
    {
        ResultantForce[0] += force[0];
        ResultantForce[1] += force[1];
    }
}

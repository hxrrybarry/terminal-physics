using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace bouncymcball;

public class RigidBody(Scene sc, int x, int y, char texture, float mass, float elasticity) : Voxel(x, y, texture)
{
    public float Mass { get; } = mass;
    public float Elasticity { get; } = elasticity;
    private float[] Velocity { get; } = [0, 0];
    private float[] Acceleration { get; } = [0, 0];
    // private float Dampening { get; } = dampening;
    private float GRAVITATIONAL_FORCE { get; } = mass * 6 / 100f;

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

        // evaluates a force
        // checks to see if we update the X position, the object collides
        // if it does, then we apply a force only on the X vector
        var obstacleX = Scn.ScreenSpace[newX, Y];
        if (obstacleX is not null && obstacleX != this)
        {
            ResultantForce[1] -= (1 + Elasticity) * ResultantForce[1];
            Acceleration[1] = ResultantForce[1] / Mass;
            newVelX = -Velocity[0] * Elasticity;
            newX = (int)MathF.Round(Math.Clamp(X + newVelX, 0, Scn.XSize - 1));
        }
        // same concept
        var obstacleY = Scn.ScreenSpace[X, newY];
        if (obstacleY is not null && obstacleY != this)
        {
            ResultantForce[0] -= (1 + Elasticity) * ResultantForce[0];
            Acceleration[0] = ResultantForce[0] / Mass;
            newVelY = -Velocity[0] * Elasticity;
            newY = (int)MathF.Round(Math.Clamp(Y + newVelY, 0, Scn.YSize - 1));
        }

        // eventually, update all velocities and coordinates
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

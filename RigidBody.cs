namespace bouncymcball;

public class RigidBody(Scene scene, float x, float y, char texture, float mass, float dampening, float elasticity) : Voxel(x, y, texture)
{
    public float Mass { get; } = mass;
    public float Elasticity { get; } = elasticity;
    private Vector2 Velocity { get; } = new(0, 0);
    private Vector2 Acceleration { get; } = new(0, 0);
    private float Dampening { get; } = dampening;
    private float GRAVITATIONAL_FORCE { get; } = 0 * mass * 9.81f / 100f;

    private Scene ActiveScene { get; } = scene;

    private Vector2 ResultantForce { get; } = new(0, 0);

    // model forces as the physics idea of a resultant force for easy calculation
    public void DoPhysicsTick()
    {
        ResultantForce.Y += GRAVITATIONAL_FORCE;
        ResultantForce.X -= MathF.Sign(ResultantForce.X) * Dampening;

        Acceleration.Y = ResultantForce.Y / Mass;
        Acceleration.X = ResultantForce.X / Mass;

        Velocity.X += Acceleration.Y;
        Velocity.Y += Acceleration.X;

        // we calculate a potential new set of coordinates to then be checked if they are colliding with something
        float newX = Math.Clamp(X + Velocity.X, 0, ActiveScene.XSize - 1);
        float newY = Math.Clamp(Y + Velocity.Y, 0, ActiveScene.YSize - 1);

        int roundedX = (int)MathF.Round(X);
        int roundedY = (int)MathF.Round(Y);
        
        #region Collision Detection
        // evaluates a force
        // checks to see if we update the X position, the object collides
        // if it does, then we apply a force only on the X vector
        var obstacleQuarter = ActiveScene.ScreenSpace[(int)MathF.Round(newX * 0.25f), roundedY];
        var obstacleFourthQuarter = ActiveScene.ScreenSpace[(int)MathF.Round(newX), roundedY];
        bool obstacleIsNotSelf = obstacleQuarter != this && obstacleFourthQuarter != this;
        if ((obstacleFourthQuarter is not null || obstacleFourthQuarter is not null) && obstacleIsNotSelf)
        {
            ResultantForce.Y = 0;
            Acceleration.Y = 0;
            Velocity.X *= -Elasticity;
            newX = Math.Clamp(X + Velocity.X, 0, ActiveScene.XSize - 1);
        }
        // same concept
        obstacleQuarter = ActiveScene.ScreenSpace[roundedX, (int)MathF.Round(newY * 0.25f)];
        obstacleFourthQuarter = ActiveScene.ScreenSpace[roundedX, (int)MathF.Round(newY)];
        obstacleIsNotSelf = obstacleQuarter != this && obstacleFourthQuarter != this;
        if ((obstacleFourthQuarter is not null || obstacleFourthQuarter is not null) && obstacleIsNotSelf)
        {
            ResultantForce.X = 0;
            Acceleration.X = 0;
            Velocity.Y *= -Elasticity;
            newY = Math.Clamp(Y + Velocity.Y, 0, ActiveScene.YSize - 1);
        }
        #endregion

        // eventually, update coordinates
        X = newX;
        Y = newY;
    }

    public void AddForce(Vector2 force)
    {
        ResultantForce.AddVector2(force);
    }
}
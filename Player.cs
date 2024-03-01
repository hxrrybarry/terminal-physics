using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bouncymcball;

public class Player(Scene scene, float x, float y, char texture, float mass, float dampening, float elasticity, int health, float walkSpeed, float jumpForce) : RigidBody(scene, x, y, texture, mass, dampening, elasticity)
{
    public int Health { get; set; } = health;
    public float walkSpeed { get; } = walkSpeed;
    public float jumpForce { get; set; } = jumpForce;


}

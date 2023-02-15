using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
using GXPEngine.Entities;
public class MyGame : Game
{
    private string levelname = "Levels/Placeholder.tmx";

    Player player;


    public MyGame() : base(500, 500, false)
    {
        //LoadLevel(levelname);
        player = new Player();
        player.x = width / 2;
        player.y = height / 2;
        AddChild(player);
    }


    void Update()
    {
        debug();
    }

    void debug()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AsteroidLarge new_asteroid = new AsteroidLarge();
            new_asteroid.x = width / 2;
            new_asteroid.y = height / 2;
            AddChild(new_asteroid);
        }
    }

    static void Main()
    {
        new MyGame().Start();
    }

    public void LoadLevel(string name) // This functions destroys the previous level and creates the new level
    {
        List<GameObject> children = GetChildren();
        for (int i = children.Count - 1; i >= 0; i--)
        {
            children[i].Destroy();
        }
        AddChild(new Level(name));
        scale = 3.0f;


    }

}
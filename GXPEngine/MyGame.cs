using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
using GXPEngine.Entities;
using GXPEngine.UI;
using GXPEngine.Core;

public class MyGame : Game
{
    private string levelname = "Levels/Placeholder.tmx";

    public HUD hud;
    Player player;


    public MyGame() : base(800, 800, false)
    {
        //LoadLevel(levelname);
        player = new Player();
        player.x = width / 2;
        player.y = height / 2;
        hud = new HUD();
        AddChild(hud);
        AddChild(player);
    }


    void Update()
    {
        debug();
    }

    int asteroids_amount = 4;

    Vector2 GetRandomPosition()
    {

        Random rnd = new Random();
        int y_pos = rnd.Next(0, height);


        rnd = new Random();

        int x_pos = rnd.Next(0, width);


        rnd = new Random();
        int side = rnd.Next(1, 4);
        if (side == 1)
        {
            x_pos = 0;
        }
        if (side == 2)
        {
            y_pos = 0;
        }
        if (side == 3)
        {
            x_pos = width;
        }
        if (side == 4)
        {
            y_pos = height;
        }
        return new Vector2(x_pos, y_pos);
    }
    

    void SpawnAsteroids()
    {
        for(int i = 0; i < asteroids_amount; i++)
        {
            AsteroidLarge new_asteroid = new AsteroidLarge(this);

            Vector2 position = GetRandomPosition();

            new_asteroid.x = position.x;
            new_asteroid.y = position.y;

            AddChild(new_asteroid);
        }
    }

    void debug()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnAsteroids();
        }
    }

    static void Main()
    {
        new MyGame().Start();
    }

    public void connectAsteroid(Asteroid new_asteroid)
    {
        new_asteroid.UpdateScore += hud.UpdateScore;
    }

    public void disconnectAsteroid(Asteroid old_asteroid)
    {
        old_asteroid.UpdateScore -= hud.UpdateScore;
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
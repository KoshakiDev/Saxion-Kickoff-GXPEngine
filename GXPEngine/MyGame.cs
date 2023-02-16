using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
using GXPEngine.Entities;
using GXPEngine.UI;
using GXPEngine.Core;

public class MyGame : Game
{
    public HUD hud;
    LostScreen lost_screen;
    Player player;
    Pivot AsteroidContainer;
    State state;


    public MyGame() : base(800, 800, false)
    {
        //LoadLevel(levelname);
        EnterMenuState();
    }


    void Update()
    {
        StateMachine();
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

    void EnterMenuState()
    {
        state = State.Menu;
    }
    bool MenuState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ExitMenuState();
            EnterPlayState();
        }
        return true;
    }
    void ExitMenuState()
    {
        
    }
    void EnterPlayState()
    {
        player = new Player();
        player.x = width / 2;
        player.y = height / 2;
        hud = new HUD();
        AsteroidContainer = new Pivot();

        player.UpdateHealth += hud.UpdateHealth;
        hud.UpdateHealth(player.health);

        AddChild(player);
        AddChild(hud);
        AddChild(AsteroidContainer);

        state = State.Play;
    }
    int last_recorded_extra_life_score = 0;
    int last_recorded_gun_upgrade_score = 0;
    bool PlayState()
    {
        // Extra life
        if (hud.score % 10000 == 0 && hud.score != last_recorded_extra_life_score)
        {
            player.Heal(1);
            last_recorded_extra_life_score = hud.score;
        }
        // Gun Upgrade
        if (hud.score % 50000 == 0 && hud.score != last_recorded_gun_upgrade_score)
        {
            player.IsGunUpgraded = true;
            last_recorded_gun_upgrade_score = hud.score;
        }


        if (AsteroidContainer.GetChildCount() == 0)
        {
            SpawnAsteroids();
            asteroids_amount += 2;
        }
        if (player.health <= 0)
        {
            ExitPlayState();
            EnterLostState();
        }
        return true;
    }


    
    Vector2 GetRandomPosition()
    {
        int y_pos = Utils.Random(0, height);

        int x_pos = Utils.Random(0, width);

        int side = Utils.Random(1, 4);
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

    int asteroids_amount = 1;
    void SpawnAsteroids()
    {
        for (int i = 0; i < asteroids_amount; i++)
        {
            AsteroidLarge new_asteroid = new AsteroidLarge(this);

            Vector2 position = GetRandomPosition();
            new_asteroid.x = position.x;
            new_asteroid.y = position.y;

            AsteroidContainer.AddChild(new_asteroid);
        }
    }

    void debug()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnAsteroids();
        }
    }


    void ExitPlayState()
    {
        RemoveChild(player);
        RemoveChild(hud);
        RemoveChild(AsteroidContainer);

        asteroids_amount = 1;
       
    }
    void EnterLostState()
    {
        lost_screen = new LostScreen(hud.score);
        AddChild(lost_screen);
        state = State.Lost;
    }
    bool LostState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ExitLostState();
            EnterMenuState();
        }
        return true;
    }
    void ExitLostState()
    {
        RemoveChild(lost_screen);
        hud.score = 0;
    }

    void StateMachine()
    {
        switch(state)
        {
            case State.Menu:
                if(MenuState())
                    break;
                break;
            case State.Play:
                if (PlayState())
                    break;
                break;
            case State.Lost:
                if (LostState())
                    break;
                break;
        }
    }

    private enum State
    {
        Menu,
        Play,
        Lost,
    }
    static void Main()
    {
        new MyGame().Start();
    }
}
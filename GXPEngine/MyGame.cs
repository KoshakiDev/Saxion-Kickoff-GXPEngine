using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
using GXPEngine.Entities;
using GXPEngine.UI;
using GXPEngine.Core;

//using System.Threading;
using System.IO.Ports;

public class MyGame : Game
{
    public HUD hud;
    LostScreen lost_screen;
    Menu menu_screen;
    Player player;
    Pivot AsteroidContainer;
    State state;
    PlayStates play_state;


    Background current_background; 

    Timer await_wave_timer;
    int await_wave_duration = 2500;

    Sound game_over = new Sound("sounds/game_over.wav");

    int current_highscore = 0;

    public MyGame() : base(1600, 1200, false, false, 1600 / 2, 1200 / 2, false)
    {
        //LoadLevel(levelname);
        EnterMenuState();
        //EnterLostState();
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
        current_background = new Background("sprites/backgrounds/startscreenf.png");
        AddChild(current_background);

        menu_screen = new Menu(current_highscore);
        AddChild(menu_screen);
        state = State.Menu;
    }
    bool MenuState()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(Key.SPACE))
        {
            Sound button_click = new Sound("sounds/button_click/click" + Utils.Random(1, 2) + ".wav");
            button_click.Play();
            ExitMenuState();
            EnterPlayState();
        }
        return true;
    }
    void ExitMenuState()
    {
        RemoveChild(current_background);
        RemoveChild(menu_screen);   
    }
    void EnterPlayState()
    {
        player = new Player();
        player.x = width / 2;
        player.y = height / 2;
        hud = new HUD();
        AsteroidContainer = new Pivot();

        current_background = new Background("sprites/backgrounds/bg1.png");



        player.UpdateHealth += hud.UpdateHealth;
        hud.UpdateHealth(player.health);

        AddChild(current_background);
        AddChild(player);
        AddChild(AsteroidContainer);
        AddChild(hud);



        state = State.Play;
        play_state = PlayStates.AwaitingWave;
        EnterAwaitingWaveState();

    }
    bool PlayState()
    {
        PlayStateMachine();
        return true;
    }

    void PlayStateMachine()
    {
        switch (play_state)
        {
            case PlayStates.currentPlay:
                if (AsteroidContainer.GetChildCount() == 0)
                {
                    EnterAwaitingWaveState();
                }
                if (player.health <= 0)
                {
                    ExitPlayState();
                    EnterLostState();
                }
                break;
            case PlayStates.AwaitingWave:
                if (await_wave_timer == null || await_wave_timer.finished)
                {
                    SpawnAsteroids();
                    asteroids_amount += 1;
                    ExitAwaitingWaveState();
                    EnterCurrentPlayState();
                }
                break;
        }
    }
    void EnterAwaitingWaveState()
    {
        await_wave_timer = new Timer(await_wave_duration);
        play_state = PlayStates.AwaitingWave;
        hud.CreateAwaitingWaveImage();
    }

    void ExitAwaitingWaveState()
    {
        hud.DeleteAwaitingWaveImage();
    }
    void EnterCurrentPlayState()
    {
        play_state = PlayStates.currentPlay;
    }

    Vector2 GetRandomPosition()
    {
        int y_pos = Utils.Random(0, height);

        int x_pos = Utils.Random(0, width);

        int side = Utils.Random(1, 4);
        if (side == 1)
        {
            x_pos = 1;
        }
        if (side == 2)
        {
            y_pos = 1;
        }
        if (side == 3)
        {
            x_pos = width - 1;
        }
        if (side == 4)
        {
            y_pos = height - 1;
        }
        return new Vector2(x_pos, y_pos);
    }

    int asteroids_amount = 2;
    void SpawnAsteroids()
    {
        for (int i = 0; i < asteroids_amount; i++)
        {
            AsteroidLarge new_asteroid = new AsteroidLarge(this, player);

            Vector2 position = GetRandomPosition();
            new_asteroid.x = position.x;
            new_asteroid.y = position.y;
            new_asteroid.angle_of_movement = Utils.Random(0, 360);
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

        RemoveChild(current_background);

        asteroids_amount = 2;

    }
    void EnterLostState()
    {
        lost_screen = new LostScreen(hud.score);
        //lost_screen = new LostScreen(0);


        current_background = new Background("sprites/backgrounds/gameoverf.png");
        AddChild(current_background);
        AddChild(lost_screen);

        game_over.Play();
        state = State.Lost;
    }
    bool LostState()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(Key.SPACE))
        {
            Sound button_click = new Sound("sounds/button_click/click" + Utils.Random(1, 2) + ".wav");
            button_click.Play();
            ExitLostState();
            EnterMenuState();
        }
        return true;
    }
    void ExitLostState()
    {
        RemoveChild(current_background);
        RemoveChild(lost_screen);
        if(current_highscore < hud.score)
        {
            current_highscore = hud.score;
        }
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
    private enum PlayStates
    {
        currentPlay,
        AwaitingWave,
    }
    private enum State
    {
        Menu,
        Play,
        Lost,
    }
    static void Main()
    {
        /*
        SerialPort port = new SerialPort();
        port.PortName = "/dev/cu.usbmodem1202";

        //port.PortName = "COM4";
        port.BaudRate = 9600;
        port.RtsEnable = true;
        port.DtrEnable = true;
        port.Open();
        while (true)
        {
            string line = port.ReadLine(); // read separated values
                                           //string line = port.ReadExisting(); // when using characters
            if (line != "")
            {
                Console.WriteLine("Read from port: " + line);

            }

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                port.Write(key.KeyChar.ToString());  // writing a string to Arduino
            }
        }
        */
        new MyGame().Start();
    }
}

/*

//Use keyboardmessage template in Arduino

#include "Keyboard.h"
const int buttonW = 3; //Input pin for pushbutton
int previousButtonWState = HIGH;

void setup()
{
    pinMode(buttonW, INPUT);
    Keyboard.begin();
}


void loop(){
    int buttonWState = digitalRead(buttonW);
    if(buttonWState != previousButtonWState && buttonWState == HIGH)
    {
        Keyboard.press('w');    
    }
    previousButtonWState = buttonWState;
    
    Keyboard.releaseAll();
}

*/
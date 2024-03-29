﻿using System.Timers;
using Tetris;

public class Program
{
    const int TIMER_INTERVAL = 500;
    static System.Timers.Timer? aTimer;
    static private Object _lockObject = new object();

    static Figure? figure;
    static FigureGenerator generator;

    static private bool gameOver = false;

    static void Main(string[] args)
    {
        DrawerProvider.Drawer.InitField();
        
        generator = new FigureGenerator(Fields.Width / 2, 0);
        figure = generator.GetNewFigure();

        SetTimer();
        while (!gameOver)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                Monitor.Enter(_lockObject);
                var result = HandleKey(figure, key);
                ProcessResult(result, ref figure);
                Monitor.Exit(_lockObject);
            }
        }

        Console.ReadKey();
    }

    private static void SetTimer()
    {
        if (!gameOver)
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(TIMER_INTERVAL);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
    }

    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        Monitor.Enter(_lockObject);
        var result = figure.TryMove(Direction.DOWN);
        ProcessResult(result, ref figure);
        Monitor.Exit(_lockObject);
    }

    private static bool ProcessResult(Result result, ref Figure figure)
    {
        if (result == Result.HEAP_STRIKE || result == Result.DOWN_BORDER_STRIKE)
        {
            Fields.AddFigure(figure);
            Fields.TryDeleteLines();

            if (figure.IsOnTop())
            {
                DrawerProvider.Drawer.WriteGameOver();
                aTimer.Elapsed -= OnTimedEvent;
                gameOver = true;
                return true;
            }
            else
            {
                figure = generator.GetNewFigure();
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private static Result HandleKey(Figure figure, ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.RightArrow:
                return figure.TryMove(Direction.RIGHT);
            case ConsoleKey.LeftArrow:
                return figure.TryMove(Direction.LEFT);
            case ConsoleKey.DownArrow:
                return figure.TryMove(Direction.DOWN);
            case ConsoleKey.Spacebar:
                return figure.TryRotate();
        }
        return Result.SUCCESS;
    }
}
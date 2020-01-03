using System;
using TicTacToe.Domain;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            GameBase game;
            do
            {
                Console.WriteLine("Choose your destiny: 1 - random AI moves; 2 - recursive AI");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        game = new RandomGame(3);
                        break;
                    case "2":
                        game = new RecursiveGame(3);
                        break;
                    default:
                        continue;
                }

                GameResult gameResult;
                while (!game.IsComplete(out gameResult))
                {
                    game.NextPlayerMove();
                    game.NextAIMove();
                }
                game.Redraw();
                switch (gameResult)
                {
                    case GameResult.Win:
                        Console.WriteLine("You won!");
                        break;
                    case GameResult.Lose:
                        Console.WriteLine("You lost!");
                        break;
                    case GameResult.Draw:
                        Console.WriteLine("Draw!");
                        break;
                    default:
                        Console.WriteLine("This wasn't supposed to happen. Game is not complete");
                        break;
                }
                Console.WriteLine("Wanna play again? (Press escape to quit)");
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Escape);
        }
    }
}

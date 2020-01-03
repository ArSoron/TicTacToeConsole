using System;
using System.Linq;
using TicTacToe.Domain;

namespace TicTacToe
{
    public abstract class GameBase
    {
        protected readonly int _size;

        protected readonly CellState[][] _board;

        protected GameBase(int size)
        {
            _size = size;
            _board = Enumerable.Range(0, _size).Select(_ => new CellState[_size]).ToArray();
        }

        public bool IsComplete(out GameResult gameResult)
        {
            return IsComplete(_board, out gameResult);
        }

        protected bool IsComplete(CellState[][] board, out GameResult gameResult)
        {

            gameResult = GameResult.Incomplete;

            if (WinCondition(CellState.X))
            {
                gameResult = GameResult.Win;
                return true;
            }

            if (WinCondition(CellState.O))
            {
                gameResult = GameResult.Lose;
                return true;
            }

            if (board.All(row => row.All(cell => cell == CellState.O || cell == CellState.X)))
            {
                gameResult = GameResult.Draw;
                return true;
            }

            return false;

            bool WinCondition(CellState cellState)
            {
                if (board.Any(row => row.All(cell => cell == cellState)))
                    return true;
                if (Enumerable.Range(0, 3).Any(i => board.Select(row => row[i]).All(cell => cell == cellState)))
                    return true;
                if (Enumerable.Range(0, 3).Select(i => board[i][i]).All(cell => cell == cellState))
                    return true;
                if (Enumerable.Range(0, 3).Select(i => board[_size - 1 - i][i]).All(cell => cell == cellState))
                    return true;
                return false;
            }
        }

        public void Redraw()
        {
            Console.Clear();
            for (int i = 0; i < _board.Length; i++)
            {
                Console.WriteLine(string.Join('|', _board[i].Select(DisplayValue)));
            }

            string DisplayValue(CellState cell)
            {
                if (cell.HasFlag(CellState.Selected))
                    return "S";
                if (cell.HasFlag(CellState.O))
                    return "O";
                if (cell.HasFlag(CellState.X))
                    return "X";
                return " ";
            }
        }

        public void NextPlayerMove()
        {
            var cursorPosition = new CursorPosition { x = 0, y = 0 };
            _board[cursorPosition.y][cursorPosition.x] = _board[cursorPosition.y][cursorPosition.x] | CellState.Selected;
            while (true)
            {
                Redraw();
                var key = Console.ReadKey();
                _board[cursorPosition.y][cursorPosition.x] =
                    _board[cursorPosition.y][cursorPosition.x] ^ CellState.Selected;
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        cursorPosition.x = Mod(--cursorPosition.x, _size);
                        break;
                    case ConsoleKey.RightArrow:
                        cursorPosition.x = Mod(++cursorPosition.x, _size);
                        break;
                    case ConsoleKey.UpArrow:
                        cursorPosition.y = Mod(--cursorPosition.y, _size);
                        break;
                    case ConsoleKey.DownArrow:
                        cursorPosition.y = Mod(++cursorPosition.y, _size);
                        break;
                    case ConsoleKey.Enter:
                        if (_board[cursorPosition.y][cursorPosition.x].HasFlag(CellState.X) ||
                            _board[cursorPosition.y][cursorPosition.x].HasFlag(CellState.O))
                        {
                            Console.WriteLine("Illegal move");
                            Console.ReadKey();
                            break;
                        }

                        _board[cursorPosition.y][cursorPosition.x] = CellState.X;
                        return;
                }

                _board[cursorPosition.y][cursorPosition.x] =
                    _board[cursorPosition.y][cursorPosition.x] | CellState.Selected;
            }
            int Mod(int x, int m)
            {
                return (x % m + m) % m;
            }
        }

        public abstract void NextAIMove();

        struct CursorPosition
        {
            public int x;
            public int y;
        }
    }
}
using System;
using System.Linq;
using TicTacToe.Domain;

namespace TicTacToe
{
    public class RandomGame : GameBase
    {
        public RandomGame(int size) : base(size)
        {
        }

        public override void NextAIMove()
        {
            var emptyCellCount = _board.Sum(row => row.Count(cell => cell == CellState._));
            int selectedCell = new Random().Next(emptyCellCount);
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_board[i][j] != CellState._)
                        continue;
                    selectedCell--;
                    if (selectedCell <= 0)
                    {
                        _board[i][j] = CellState.O;
                        return;
                    }
                }
            }
        }
    }
}
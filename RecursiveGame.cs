using System;
using System.Linq;
using TicTacToe.Domain;

namespace TicTacToe
{
    public class RecursiveGame : GameBase
    {
        public RecursiveGame(int size): base(size)
        {
        }

        public override void NextAIMove()
        {
            CursorPosition bestMove = new CursorPosition();
            decimal bestChance = -1;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_board[i][j] != CellState._)
                        continue;

                    var newBoard = _board.Select(row => row.ToArray()).ToArray();
                    newBoard[i][j] = CellState.O;
                    var chance = AIWinChance(newBoard, CellState.O);
                    if (chance > bestChance)
                    {
                        bestChance = chance;
                        bestMove = new CursorPosition {x = i,y = j};
                    }
                }
            }

            _board[bestMove.x][bestMove.y] = CellState.O;
        }

        private decimal AIWinChance(CellState[][] board, CellState move)
        {
            if (IsComplete(board, out var result))
            {
                switch (result)
                {
                    case GameResult.Win:
                        return 0;
                    case GameResult.Lose:
                        return 1;
                    case GameResult.Draw:
                        return 0.1M; //May need to tune
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            int moveCount = 0;
            decimal cumulativeWinChance = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (board[i][j] != CellState._)
                        continue;
                    moveCount++;

                    var newBoard = board.Select(row => row.ToArray()).ToArray();
                    newBoard[i][j] = move;
                    cumulativeWinChance += AIWinChance(newBoard, move == CellState.O ? CellState.X : CellState.O);
                }
            }
            return cumulativeWinChance / moveCount;
        }

        struct CursorPosition
        {
            public int x;
            public int y;
        }
    }
}
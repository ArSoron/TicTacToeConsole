using System;

namespace TicTacToe.Domain
{
    [Flags]
    public enum CellState
    {
        //Empty
        _ = 0,
        O = 1,
        X = 2,
        //Selected
        Selected = 4
    }
}
using System;
namespace JungleChess
{
    class UnableToMoveError : Exception
    {
        private Piece p;
        public Piece P { get; }
        private Coordinate c;
        public Coordinate C { get; }

        public UnableToMoveError (Piece _p, Coordinate _c)
        {
            p = _p;
            c = _c;
        }
    }

    class UnableToEatError : Exception
    {
        private Piece eater;
        public Piece Eater { get; }
        private Piece eated;
        public Piece Eated { get; }

        public UnableToEatError (Piece _eater, Piece _eated)
        {
            eater = _eater;
            eated = _eated;
        }
    }
}

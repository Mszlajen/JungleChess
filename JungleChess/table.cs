using System;
using System.Collections.Generic;
namespace JungleChess
{
    public class Coordinate
    {
        public int row = 0, column = 0;

        public Coordinate() { }

        public Coordinate(int _row, int _column)
        {
            row = _row;
            column = _column;
        }

        public void Copy(Coordinate c)
        {
            row = c.row;
            column = c.column;
        }

        public Coordinate Sign ()
        {
            return new Coordinate(Math.Sign(this.row), Math.Sign(this.column));
        }

        public bool IsSide(Coordinate c)
        {
            Coordinate temp = this - c;
            List<int> around = new List<int>(3);
            around.Add(-1);
            around.Add(0);
            around.Add(1);
            return temp.column != temp.row && around.Contains(temp.row) && around.Contains(temp.column);
        }

        public static bool operator ==(Coordinate c1, Coordinate c2)
        { return c1.column == c2.column && c1.row == c2.row; }

        public static bool operator !=(Coordinate c1, Coordinate c2)
        { return c1.column != c2.column || c1.row != c2.row; }

        public static Coordinate operator -(Coordinate c1, Coordinate c2)
        { return new Coordinate(c1.row - c2.row, c1.column - c2.column); }
        public static Coordinate operator +(Coordinate c1, Coordinate c2)
        { return new Coordinate(c1.row + c2.row, c1.column + c2.column); }
		public static Coordinate operator *(Coordinate c1, Coordinate c2)
		{ return new Coordinate(c1.row * c2.row, c1.column * c2.column); }
    }

    public enum playerName { white, black }

    class Square
    {
        public virtual bool IsWater() { return false; }
        public virtual bool IsTrapOf(playerName _owner) { return false; }
        public virtual bool IsDenOf(playerName _owner) { return false; }
    }

    class Water : Square
    {
        public override bool IsWater() { return true; }
    }

    class Trap : Square
    {
        playerName owner;
        public Trap(playerName _owner)
        {
            owner = _owner;
        }
        public override bool IsTrapOf(playerName _owner) { return _owner == owner; }
    }

    class Den : Square
    {
        playerName owner;
        public Den(playerName _owner)
        {
            owner = _owner;
        }

        public override bool IsDenOf(playerName _owner) { return _owner == owner; }
    }

    abstract class Player
    {
        protected List<Piece> pieces;
        public playerName name;

        public bool HasPieces() 
        {
            return pieces.TrueForAll(piece => !piece.InPlay);
        }

        public bool HasPieceIn(Coordinate co) 
        {
            return pieces.Exists(piece => piece.IsInPosition(co));
        }

		/*
         * Throws ArgumentNullException if there is no Piece
         */
        public Piece PieceIn(Coordinate co) 
        {
            return pieces.Find(piece => piece.IsInPosition(co));
        }

        public void Move (animal p, Coordinate c)
        {
            pieces[(int)p].Move(c);
        }
    }

    class WhitePlayer : Player {
        public WhitePlayer () 
        {
            pieces = Table.StartingWhitePieces();
            name = playerName.white;
        }
    }

    class BlackPlayer : Player {
        public BlackPlayer()
        {
            pieces = Table.StartingBlackPieces();
            name = playerName.black;
        }
    }

    public static class Table
    {
        static Square Sqr = new Square();
        static Water Wtr = new Water();
        static Trap WhiteTrp = new Trap(playerName.white);
        static Den WhiteDen = new Den(playerName.white);
        static Trap BlackTrp = new Trap(playerName.black);
        static Den BlackDen = new Den(playerName.black);
        static Square[,] table =
        {{Sqr, Sqr, BlackTrp, BlackDen, BlackTrp, Sqr, Sqr},
        {Sqr, Sqr, Sqr,      BlackTrp, Sqr,      Sqr, Sqr},
        {Sqr, Sqr, Sqr,      Sqr,      Sqr,      Sqr, Sqr},
        {Sqr, Wtr, Wtr,      Sqr,      Wtr,      Wtr, Sqr},
        {Sqr, Wtr, Wtr,      Sqr,      Wtr,      Wtr, Sqr},
        {Sqr, Wtr, Wtr,      Sqr,      Wtr,      Wtr, Sqr},
        {Sqr, Sqr, Sqr,      Sqr,      Sqr,      Sqr, Sqr},
        {Sqr, Sqr, Sqr,      WhiteTrp, Sqr,      Sqr, Sqr},
        {Sqr, Sqr, WhiteTrp, WhiteDen, WhiteTrp, Sqr, Sqr}};
        static Player[] players = { new WhitePlayer(), new BlackPlayer() };

        static public bool IsDenOf(playerName _player, Coordinate co)
        {
            return table[co.row, co.column].IsDenOf(_player);
        }

        static public bool IsTrapOf(playerName _player, Coordinate co)
        {
            return table[co.row, co.column].IsTrapOf(_player);
        }
        static public bool IsWater(Coordinate co)
        {
            return table[co.row, co.column].IsWater();
        }

        /*
         * Throws ArgumentNullException if there is no piece
         */
        static public Piece PieceIn(Coordinate co)
        {
            try
            { return players[(int) playerName.white].PieceIn(co); }
            catch(ArgumentNullException)
            {
                return players[(int)playerName.black].PieceIn(co);
            }
        }

        static public List<Piece> StartingBlackPieces () 
        {
            List<Piece> pieces = new List<Piece>(8);
            pieces.Add(new Rat(new Coordinate(2, 0), playerName.black));
            pieces.Add(new Cat(new Coordinate(1, 5), playerName.black));
            pieces.Add(new Dog(new Coordinate(1, 1), playerName.black));
            pieces.Add(new Wolf(new Coordinate(2, 4), playerName.black));
            pieces.Add(new Leopard(new Coordinate(2, 2), playerName.black));
            pieces.Add(new Tiger(new Coordinate(0, 6), playerName.black));
            pieces.Add(new Lion(new Coordinate(0, 0), playerName.black));
            pieces.Add(new Elephant(new Coordinate(2, 6), playerName.black));
            return pieces;
        }

        static public List<Piece> StartingWhitePieces ()
        {
			List<Piece> pieces = new List<Piece>(8);
            pieces.Add(new Rat(new Coordinate(6, 6), playerName.white));
            pieces.Add(new Cat(new Coordinate(7, 1), playerName.white));
			pieces.Add(new Dog(new Coordinate(7, 5), playerName.white));
			pieces.Add(new Wolf(new Coordinate(6, 2), playerName.white));
			pieces.Add(new Leopard(new Coordinate(6, 4), playerName.white));
            pieces.Add(new Tiger(new Coordinate(8, 0), playerName.white));
            pieces.Add(new Lion(new Coordinate(8, 6), playerName.white));
            pieces.Add(new Elephant(new Coordinate(6, 0), playerName.white));
			return pieces;
        }

        static public void MakeMove (playerName player, animal piece, Coordinate co)
        {
            players[(int)player].Move(piece, co);
        }
    }
}

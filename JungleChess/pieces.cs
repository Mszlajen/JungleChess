using System;
using System.Collections.Generic;

namespace JungleChess
{
	public enum animal { rat, cat, dog, wolf, leopard, tiger, lion, elephant }

    public abstract class Piece
    {
        protected playerName owner;
        public playerName Owner { get; }
        protected Coordinate position = new Coordinate();
        public Coordinate Position { get; protected set; }
        protected bool inPlay = true;
        public bool InPlay { get; protected set; }

		public Piece(Coordinate initialPos, playerName _owner)
		{
            this.Position.Copy(initialPos);
            this.Owner = _owner;
		}

        public abstract animal Value();

        protected virtual bool CanEat(Piece other)
        {
            return other.Owner != this.Owner && (other.IsInTrap() || Value() >= other.Value());
        }

        protected virtual bool CanMoveTo(Coordinate newPos)
        {
            return this.Position.IsSide(newPos) && !Table.IsWater(newPos);
        }

        public void Move (Coordinate newPos)
        {
            if (!CanMoveTo(newPos))
                throw new UnableToMoveError(this, newPos);
            try
            {
                Piece other = Table.PieceIn(newPos);
                if (!this.CanEat(other))
                    throw new UnableToEatError(this, other);
                other.BeEaten();
                this.Position = newPos;
            }
            catch (ArgumentNullException)
            {
                this.Position = newPos;
            }
        }

        protected void BeEaten() { this.InPlay = false; }
        public bool IsInPosition(Coordinate co) { return co == position; }
        public bool IsInTrap() { return Table.IsTrapOf(this.Owner, this.Position); }
	}

    abstract class BigFeline : Piece {
		public BigFeline(Coordinate initialPos, playerName _owner) : base(initialPos, _owner) { }
		protected override bool CanMoveTo(Coordinate newPos)
		{
			return base.CanMoveTo(newPos) || this.isJumping(newPos);
		}
		protected bool isJumping(Coordinate newPos)
        {
			List<Coordinate> distances = new List<Coordinate>(4);
			distances.Add(new Coordinate(3, 0));
			distances.Add(new Coordinate(-3, 0));
			distances.Add(new Coordinate(0, 2));
			distances.Add(new Coordinate(0, -2));
			Coordinate rest = this.Position - newPos;
			return distances.Contains(rest) && Table.IsWater(this.Position - rest.Sign());
		}
	}

    class Rat : Piece {

        public Rat(Coordinate initialPos, playerName _owner) : base(initialPos, _owner) {}

        public override animal Value() { return animal.rat; }

        protected override bool CanEat(Piece other)
        {
            return !this.WaterToEarth(other.Position) && (base.CanEat(other) || other.Value() == animal.elephant);
        }

        private bool WaterToEarth(Coordinate co) 
        {
            return Table.IsWater(co) ^ Table.IsWater(this.Position);
        }

        override protected bool CanMoveTo(Coordinate newPos)
        {
            return true;
        }
    }

    class Cat : Piece {

        public Cat(Coordinate initialPos, playerName _owner) : base(initialPos, _owner) { }
        override public animal Value() { return animal.cat; }
    }

    class Dog : Piece {
        public Dog(Coordinate initialPos, playerName _owner) : base(initialPos, _owner) { }
        override public animal Value() { return animal.dog; }
    }

    class Wolf : Piece {
        public Wolf(Coordinate initialPos, playerName _owner) : base(initialPos, _owner) { }
        public override animal Value() { return animal.wolf; }
    }

    class Leopard : BigFeline {
        public Leopard(Coordinate initialPos, playerName _owner) : base(initialPos, _owner) { }
        public override animal Value() { return animal.leopard; }
    }

    class Tiger : BigFeline {
        public Tiger(Coordinate initialPos, playerName _owner) : base(initialPos, _owner) { }
        public override animal Value() { return animal.tiger; }
    }

    class Lion : BigFeline {
        public Lion(Coordinate initialPos, playerName _owner) : base(initialPos, _owner) { }
        public override animal Value() { return animal.lion; }
    }

    class Elephant : Piece {
        public Elephant(Coordinate initialPos, playerName _owner) : base(initialPos, _owner) { }
        public override animal Value() { return animal.elephant; }
    }


}

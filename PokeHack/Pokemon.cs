using System;
using PokeAPI;

// [...]


namespace PokeHack
{
	public class Pokemon	//Put everything in here
	{
		private PokeAPI.Pokemon Poke;
        private PokeAPI.PokemonSpecies Species;
        public Move[] MoveSet = new Move[4];
        public String Name = "missing";
		private int Level;
		private int HealthMax;
		public int HealthCurr;
		private int Attack;
		private int Defense;
		private int SpecialAttack;
		private int SpecialDefense;
        public int Speed;
		public int Accuracy = 100;
		public int Evasiveness = 100;
		private int Happiness;
		private int Weight;
		private Type Type1;
        private Type Type2 = Type.None;

        public Pokemon(int PokemonID, int level)
        {
            FetchPokemon(PokemonID);
            Level = level;
            Name = Poke.Name;

            PokemonStats[] Stats = Poke.Stats;

            HealthMax = (Stats[0].BaseValue * 2 * Level / 100) + 10 + Level;
            HealthCurr = HealthMax;

            Attack = (Stats[1].BaseValue * 2 * Level / 100) + 5;
            Defense = (Stats[2].BaseValue * 2 * Level / 100) + 5;
            SpecialAttack = (Stats[3].BaseValue * 2 * Level / 100) + 5;
            SpecialDefense = (Stats[4].BaseValue * 2 * Level / 100) + 5;

            // Happiness = p.Happiness;
            // Weight = p.Weight;

            PokemonTypeMap[] Types = Poke.Types;
            Type1 = StringToType(Types[0].Type.Name);
            if (Types.Length > 1)
            {
                Type2 = StringToType(Types[1].Type.Name);
            }

            //Randomly selects moves
            Random rand = new Random();
            for(int i = 0; i < 4; i++)
                MoveSet[i] = new Move(rand.Next(1, 621));
            

        }
	
        public async void FetchPokemon(int PokemonID)
        {
            Poke = await DataFetcher.GetApiObject<PokeAPI.Pokemon>(PokemonID);
            Species = await DataFetcher.GetApiObject<PokemonSpecies>(PokemonID);
        }

        public void TakeDamage(int damage) {
			HealthCurr -= damage;
		}
		public int MoveDamage(Move move, Pokemon defender) {
            int damage;
            if (String.Compare(move.DamageClass, "physical") == 0) 
                damage = (int)((((float)(2 * this.Level + 10)/250 * (this.Attack /	defender.Defense) + 2) * GetModifier(move.Type, defender.Type1, defender.Type2)));
            else
                damage = (int)((((float)(2 * this.Level + 10) / 250 * (this.SpecialAttack / defender.SpecialDefense) + 2) * GetModifier(move.Type, defender.Type1, defender.Type2)));
            return damage;
		}
		
		public static double GetModifier(Type attack, Type t1, Type t2)
	        {
	            double modifier = 1;
	            //Attack is row, Defense is column
	            double[,] resistances = new double[,] {
	            {  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1, .5,  0,  1,  1, .5,  1},
	            {  1, .5, .5,  1,  2,  2,  1,  1,  1,  1,  1,  2, .5,  1, .5,  1,  2,  1},
	            {  1,  2, .5,  1, .5,  1,  1,  1,  2,  1,  1,  1,  2,  1, .5,  1,  1,  1},
	            {  1,  1,  2, .5, .5,  1,  1,  1,  0,  2,  1,  1,  1,  1, .5,  1,  1,  1},
	            {  1, .5,  2,  1, .5,  1,  1, .5,  2, .5,  1, .5,  2,  1, .5,  1, .5,  1},
	            {  1, .5, .5,  1,  2, .5,  1,  1,  2,  2,  1,  1,  1,  1,  2,  1, .5,  1},
	            {  2,  1,  1,  1,  1,  2,  1, .5,  1, .5, .5, .5,  2,  0,  1,  2,  2, .5},
	            {  1,  1,  1,  1,  2,  1,  1, .5, .5,  1,  1,  1, .5, .5,  1,  1,  0,  2},
	            {  1,  2,  1,  2, .5,  1,  1,  2,  1,  0,  1, .5,  2,  1,  1,  1,  2,  1},
	            {  1,  1,  1, .5,  2,  1,  2,  1,  1,  1,  1,  2, .5,  1,  1,  1, .5,  1},
	            {  1,  1,  1,  1,  1,  1,  2,  2,  1,  1, .5,  1,  1,  1,  1,  0, .5,  1},
	            {  1, .5,  1,  1,  2,  1, .5, .5,  1, .5,  2,  1,  1, .5,  1,  2, .5, .5},
	            {  1,  2,  1,  1,  1,  2, .5,  1, .5,  2,  1,  2,  1,  1,  1,  1, .5,  1},
	            {  0,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  1,  1,  2,  1, .5,  1,  1},
	            {  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  1, .5,  0},
	            {  1,  1,  1,  1,  1,  1, .5,  1,  1,  1,  2,  1,  1,  2,  1, .5,  1,  1},
	            {  1, .5, .5, .5,  1,  2,  1,  1,  1,  1,  1,  1,  2,  1,  1,  1, .5,  1},
	            {  1, .5,  1,  1,  1,  1,  2, .5,  1,  1,  1,  1,  1,  1,  2,  2, .5,  1}};
	
	            modifier *= resistances[(int)attack, (int)t1];
	
	            if (t2 != Type.None)
	                modifier *= resistances[(int)attack, (int)t2];
	
	            return modifier;
	        }

        private Type StringToType(string typename)
        {
            switch (typename[0])
            {
                case 'b':
                    return Type.Bug;
                case 'd':
                    switch (typename[1])
                    {
                        case 'a':
                            return Type.Dark;
                        default:
                            return Type.Dragon;
                    }
                case 'e':
                    return Type.Electric;
                case 'f':
                    switch (typename[2])
                    {
                        case 'i':
                            return Type.Fairy;
                        case 'g':
                            return Type.Fighting;
                        case 'r':
                            return Type.Fire;
                        default:
                            return Type.Flying;
                    }
                case 'g':
                    switch (typename[4])
                    {
                        case 't':
                            return Type.Ghost;
                        case 's':
                            return Type.Grass;
                        default:
                            return Type.Ground;
                    }
                case 'i':
                    return Type.Ice;
                case 'n':
                    return Type.Normal;
                case 'p':
                    switch (typename[1])
                    {
                        case 'o':
                            return Type.Poison;
                        default:
                            return Type.Psychic;
                    }
                case 'r':
                    return Type.Rock;
                case 's':
                    return Type.Steel;
                default:
                    return Type.Water;
            }
        }
    }
}

﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PokeAPI;

namespace PokeHack
{
    class main
    {


        public static void Main()
        {
            Random rand = new Random();
            
            Pokemon p1 = new Pokemon(rand.Next(1, 721), 50);
            Pokemon p2 = new Pokemon(rand.Next(1, 721), 50);

            Console.WriteLine(p1.Name + " " + p1.Attack + " " + p1.Defense + " " + p1.HealthCurr + " " + p1.Speed);
            foreach (Move m in p1.MoveSet)
                Console.WriteLine(m.Name);

            Console.WriteLine(p2.Name);
            foreach (Move m in p2.MoveSet)
                Console.WriteLine(m.Name);
            Console.ReadLine();
            while (true)
            {
                if(p1.HealthCurr > 0 && p2.HealthCurr > 0)
                {
                    CombatRound(p1, p2);
                }
                if (p1.HealthCurr < 0)
                {
                    Console.WriteLine(p1.Name + " perished horribly");
                    break;
                }
                if (p2.HealthCurr < 0)
                {
                    Console.WriteLine(p2.Name + " perished horribly");
                    break;
                }
                  
                    
                

            }
            Console.ReadLine();
        }

        public static void CombatRound(Pokemon p1, Pokemon p2)
        {
            Move p1m = null, p2m = null;
            Random rand = new Random();

            p1m = PickMoveHighestPower(p1, p2);
            p2m = PickMoveHighestPower(p2, p1);
            if (p1.Speed > p2.Speed)
            {
                if (rand.Next(1, 100) < HitChance(p1m, p1, p2)) //Check for miss
                {
                    p2.TakeDamage(p1.MoveDamage(p1m, p2));
                    Console.WriteLine(p1.Name + " used " + p1m.Name);
                }
                else Console.WriteLine(p1.Name + " missed");
                if (p2.HealthCurr > 0) //Check for kill
                { 
                    if (rand.Next(1, 100) < HitChance(p2m, p2, p1))
                    {
                        p1.TakeDamage(p2.MoveDamage(p2m, p1));
                        Console.WriteLine(p2.Name + " used " + p2m.Name);
                    }
                    else Console.WriteLine(p2.Name + " missed");
                }
            }
            else
            {
                if (rand.Next(1, 100) < HitChance(p2m, p2, p1))
                {
                    p1.TakeDamage(p2.MoveDamage(p2m, p1));
                    Console.WriteLine(p2.Name + " used " + p2m.Name);
                }
                else Console.WriteLine(p2.Name + " missed");
                if (p1.HealthCurr > 0)
                {
                    if (rand.Next(1, 100) < HitChance(p1m, p1, p2))
                    {
                        p2.TakeDamage(p1.MoveDamage(p1m, p2));
                        Console.WriteLine(p1.Name + " used " + p1m.Name);
                    }
                    else Console.WriteLine(p1.Name + " missed");
                }
            }
        }
        //Picks move based on highest damage possible.
        //p1 is attacker, p2 is defender
        private static Move PickMoveHighestPower(Pokemon p1, Pokemon p2)
        {
            int maxpower = 0;
            Move p1m = null;
            foreach (Move m in p1.MoveSet)
                if (p1.MoveDamage(m, p2) > maxpower)
                {
                    p1m = m;
                    maxpower = p1.MoveDamage(m, p2);
                }
            return p1m;
        }

        private  static int HitChance(Move m, Pokemon att, Pokemon def)
        {
            return  m.Accuracy * att.Accuracy / def.Evasiveness;
        }

    }
}

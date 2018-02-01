using System;
using System.Collections.Generic;
using Stree;

namespace StreeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var hList = new List<Animal>
            {
                new Animal("Ape"),
                new Animal("Monkey"),
                new Animal("Hippo"),
                new Animal("Dog"),
                new Animal("Cat"),
                new Animal("Bird"),
                new Animal("Bear"),
                new Animal("Cow"),
                new Animal("Horse"),
            };


            var hStree = new Stree<Animal>();
            foreach (var animal in hList)
            {
                hStree.Add(animal.Name, animal);
            }

            var hAnimals = hStree.Find("C");
        }
    }

    class Animal
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public Animal(string name)
        {
            Name = name;
        }
    }
}

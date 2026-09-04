using PizzaApi.Models;

namespace PizzaApi.Services;

public class PizzaService
{
    public static List<Pizza> Pizzas { get; } = new()
    {
        new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false },
        new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true },
        new Pizza { Id = 3, Name = "Meat Lovers", IsGlutenFree = false },
        new Pizza { Id = 4, Name = "Hawaiian", IsGlutenFree = false }
    };

    private static int nextId = 5;

    public static List<Pizza> GetAll() => Pizzas;

    public static Pizza? Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);

    public static void Add(Pizza pizza)
    {
        pizza.Id = nextId++;
        Pizzas.Add(pizza);
    }

    public static void Delete(int id)
    {
        var pizza = Get(id);
        if (pizza is null)
        {
            return;
        }

        Pizzas.Remove(pizza);
    }

    public static void Update(Pizza pizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
        if (index == -1)
        {
            return;
        }

        Pizzas[index] = pizza;
    }
}

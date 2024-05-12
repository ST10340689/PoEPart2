using System;
using System.Collections.Generic;
using System.Linq;

class Recipe
{
    public string Name { get; set; }
    public int NumberOfIngredients { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public int NumberOfSteps { get; set; }
    public List<string> Steps { get; set; }
    public double TotalCalories { get; set; }

    public Recipe()
    {
        Ingredients = new List<Ingredient>();
        Steps = new List<string>();
    }
}

class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; }
    public double Calories { get; set; }
    public string FoodGroup { get; set; }
}

class Program
{
    static List<Recipe> recipes = new List<Recipe>();
    static Dictionary<string, double> originalQuantities = new Dictionary<string, double>();

    static Action<string> notifyExceedingCalories; // Delegate is used to notify user about calories exceeding 300

    static void AddRecipe()
    {
        Recipe recipe = new Recipe();

        Console.WriteLine("\nEnter details for the recipe:");
        Console.Write("Name: ");
        recipe.Name = Console.ReadLine();

        Console.Write("Enter the number of ingredients: "); //Depending on the number the user input this will repeat
        recipe.NumberOfIngredients = Convert.ToInt32(Console.ReadLine());

        double totalCalories = 0; // Variable to store total calories

        for (int i = 0; i < recipe.NumberOfIngredients; i++)
        {
            Ingredient ingredient = new Ingredient();

            Console.WriteLine($"\nEnter details for ingredient #{i + 1}:");

            Console.Write("Name: ");
            ingredient.Name = Console.ReadLine();

            Console.Write("Quantity: ");
            ingredient.Quantity = Convert.ToDouble(Console.ReadLine());

            Console.Write("Unit of measurement (tablespoon, teaspoon, cup): ");
            ingredient.Unit = Console.ReadLine();

            Console.Write("Calories: ");
            ingredient.Calories = Convert.ToDouble(Console.ReadLine());

            Console.Write("Food group: ");
            ingredient.FoodGroup = Console.ReadLine();

            recipe.Ingredients.Add(ingredient);
            originalQuantities[ingredient.Name] = ingredient.Quantity; // Store original quantity

            // Updates total calories
            totalCalories += ingredient.Calories * ingredient.Quantity;
        }

        recipe.TotalCalories = totalCalories; // Assign total calories to recipe

        // Checks if total calories exceed 300
        if (recipe.TotalCalories > 300 && notifyExceedingCalories != null)
        {
            notifyExceedingCalories($"Warning: Total calories for '{recipe.Name}' is exceeding 300");
        }

        Console.Write("\nEnter the number of steps: ");
        recipe.NumberOfSteps = Convert.ToInt32(Console.ReadLine());

        for (int i = 0; i < recipe.NumberOfSteps; i++)
        {
            Console.Write($"\nEnter description for step #{i + 1}: ");
            recipe.Steps.Add(Console.ReadLine());
        }

        recipes.Add(recipe);

        Console.WriteLine("Recipe added successfully!");
    }

    static void ResetRecipe()
    {
        foreach (var recipe in recipes)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                // Reset each ingredient's quantity to its original value
                ingredient.Quantity = originalQuantities[ingredient.Name];
            }
        }

        Console.WriteLine("Recipe quantities reset to original values successfully!");
    }

    static void ClearRecipe()
    {
        recipes.Clear();
        originalQuantities.Clear();
        Console.WriteLine("Recipe data cleared successfully!"); // Removes the recipe
    }

    static void DisplayRecipes()
    {
        Console.WriteLine("\nList of Recipes:");
        foreach (var recipe in recipes.OrderBy(r => r.Name))
        {
            Console.WriteLine(recipe.Name);
        }
    }

    static void DisplayRecipeDetails(string recipeName)
    {
        Recipe recipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
        if (recipe != null)
        {
            Console.WriteLine($"\nRecipe Details for '{recipe.Name}':");
            Console.WriteLine("\nIngredients:");
            foreach (var ingredient in recipe.Ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name}");
                Console.WriteLine($"   Calories: {ingredient.Calories}, Food Group: {ingredient.FoodGroup}");
            }
            Console.WriteLine($"\nTotal Calories: {recipe.TotalCalories}");

            Console.WriteLine("\nSteps:");
            for (int i = 0; i < recipe.NumberOfSteps; i++)
            {
                Console.WriteLine($"{i + 1}. {recipe.Steps[i]}");
            }
        }
        else
        {
            Console.WriteLine($"Recipe '{recipeName}' not found.");
        }
    }

    static void NotifyExceedingCalories(string message)
    {
        Console.WriteLine(message);
    }

    static void Main()
    {
        notifyExceedingCalories = NotifyExceedingCalories;

        int choice;
        do
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Add a recipe");
            Console.WriteLine("2. Display all recipes");
            Console.WriteLine("3. Display recipe details");
            Console.WriteLine("4. Reset recipe quantities");
            Console.WriteLine("5. Clear all recipes");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        AddRecipe();
                        break;
                    case 2:
                        DisplayRecipes();
                        break;
                    case 3:
                        Console.Write("Enter the name of the recipe: ");
                        string recipeName = Console.ReadLine();
                        DisplayRecipeDetails(recipeName);
                        break;
                    case 4:
                        ResetRecipe();
                        break;
                    case 5:
                        ClearRecipe();
                        break;
                    case 6:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        } while (choice != 6);
    }
}

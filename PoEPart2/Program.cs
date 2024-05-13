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

        Console.Write("Enter the number of ingredients: "); // Depending on the number the user input this will repeat
        recipe.NumberOfIngredients = Convert.ToInt32(Console.ReadLine());

        double totalCalories = 0; // Store total calories

        for (int i = 0; i < recipe.NumberOfIngredients; i++)
        {
            Ingredient ingredient = new Ingredient();

            Console.WriteLine($"\nEnter details for ingredient #{i + 1}:");

            Console.Write("Name: ");
            ingredient.Name = Console.ReadLine();

            Console.Write("Quantity: ");
            ingredient.Quantity = Convert.ToDouble(Console.ReadLine());

            Console.Write("Unit of measurement (grames, teaspoons, cups): ");
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
            notifyExceedingCalories($"Warning: Total calories for '{recipe.Name}' is exceeding 300"); // If true run this line
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
        Console.WriteLine("Recipe data removed"); // Removes the recipe
    }

    static void ScaleRecipe()
    {
        Console.WriteLine("Enter the name of the recipe you want to scale: ");
        string recipeName = Console.ReadLine();

        Recipe recipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
        if (recipe != null)
        {
            Console.WriteLine("Enter the scaling factor (0.5, 2, or 3): ");
            double scale = Convert.ToDouble(Console.ReadLine());

            if (scale == 0.5 || scale == 2 || scale == 3)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    ingredient.Quantity *= scale; // Scales ingredients with 0.5, 2 and 3 only
                }

                recipe.TotalCalories *= scale; // Updates as well total calories with the scale

                Console.WriteLine($"Recipe '{recipe.Name}' scaled by a factor of {scale} successfully!");
            }
            else
            {
                Console.WriteLine("Invalid scaling factor. Please enter 0.5, 2, or 3.");
            }
        }
        else
        {
            Console.WriteLine($"Recipe '{recipeName}' not found.");
        }
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
            Console.WriteLine($"\nRecipe Details for '{recipe.Name}':"); // Gets and displays recipe name
            Console.WriteLine("\nIngredients:"); // Gets and displays recipe ingredients
            foreach (var ingredient in recipe.Ingredients)
            {
                Console.ForegroundColor = ConsoleColor.Green; // Set ingredient name color to green
                Console.Write($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name}");
                Console.ResetColor(); 
                Console.WriteLine($"   Calories: {ingredient.Calories}, Food Group: {ingredient.FoodGroup}");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nTotal Calories: {recipe.TotalCalories}"); // Displays total calories of the recipe
            Console.ResetColor();

            Console.WriteLine("\nSteps:");
            for (int i = 0; i < recipe.NumberOfSteps; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta; // Sets step color
                Console.WriteLine($"{i + 1}. {recipe.Steps[i]}");
                Console.ResetColor(); 
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
                Console.WriteLine("6. Scale the recipe");
                Console.WriteLine("7. Exit");
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
                            ScaleRecipe();
                            break;
                        case 7:
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
            } while (choice != 7); //Omly exits when the user inputs 7
        }
    }

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EmailComposer
{
    class Program
    {
        /// <summary>
        /// The main entry point of the application
        /// </summary>
        /// <param name="args">Any command line arguments</param>
        static void Main(string[] args)
        {
            // Check if the template directory exists
            if (Directory.Exists(Globals.TEMPLATES_PATH))
            {
                // Run a user-controlled loop, so the user can process multiple templates without exiting
                bool userWishesToProceed = true;
                while (userWishesToProceed)
                {
                    string selectedTemplate = ChooseTemplate();

                    if (!string.IsNullOrEmpty(selectedTemplate))
                    {
                        ProcessTemplate(selectedTemplate);
                    }

                    // Ask if user wants to exit or choose another template
                    Console.WriteLine("Please enter 0 to exit or any other key to choose new template.");
                    string val = Console.ReadLine();
                    // Check if user input is a valid number
                    if (int.TryParse(val, out int userChoice))
                    {
                        if (userChoice == 0)
                            userWishesToProceed = false;
                    }
                }
            }
            else
            {
                // Inform the user and exit
                Console.WriteLine("No template exists! The application will now exit.");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Method which facilitates in picking a template from the available templates
        /// </summary>
        /// <returns>The chosen template's file path</returns>
        static string ChooseTemplate()
        {
            string selectedTemplate = string.Empty;

            try
            {
                // Check if files exists
                List<string> fileEntries = Directory.GetFiles(Globals.TEMPLATES_PATH).ToList();

                if (fileEntries.Count > 0)
                {
                    // Iterate over template files, display the list to user and prompt to choose one
                    int counter = 1;
                    Console.WriteLine($"{fileEntries.Count} templates found! Please select one to proceed:");

                    Dictionary<int, string> dictTemplates = new Dictionary<int, string>();
                    fileEntries.ForEach(fileName =>
                    {
                        dictTemplates.Add(counter, fileName);
                        Console.WriteLine($"{counter} {Path.GetFileName(fileName)}");
                        counter++;
                    });

                    // Asked user to choose a template, now take input
                    string val = Console.ReadLine();

                    // Check if user input is a valid number
                    if (int.TryParse(val, out int userChoice))
                    {
                        // Check if user input is a valid item from the list of templates
                        if (dictTemplates.TryGetValue(userChoice, out string fileName))
                        {
                            // All okay, return the template name to be processed and displayed
                            selectedTemplate = fileName;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                    }
                }
                else
                {
                    Console.WriteLine("No templates found!");
                }
            }
            catch (IOException ioEx)
            {
                // If required, this exception can be handled differently
                LogError(ioEx, nameof(ChooseTemplate));
            }
            catch (Exception ex)
            {
                // Generic handler for everything else
                LogError(ex, nameof(ChooseTemplate));
            }

            return selectedTemplate;
        }

        /// <summary>
        /// Reads the template file, parses it and displays the result
        /// </summary>
        /// <param name="FilePath">The template file's path to be read and parse</param>
        static void ProcessTemplate(string FilePath)
        {
            try
            {
                // Read all the template text
                string templateText = File.ReadAllText(FilePath);

                // Extract the mail text
                StringBuilder templateContent = new StringBuilder();
                templateContent.Append(templateText.Substring(0, templateText.IndexOf(Globals.PLACEHOLDERS)));

                // Extract all placeholders
                StringBuilder placeholders = new StringBuilder(templateText.Substring(templateText.IndexOf(Globals.PLACEHOLDERS)));
                placeholders.Replace(Globals.PLACEHOLDERS, string.Empty);

                Helpers helpers = new Helpers();
                Dictionary<string, string> dictPlaceholders = helpers.ExtractPlaceholders(placeholders.ToString());

                // Iterate over the placeholders list and ask user for input
                foreach (KeyValuePair<string, string> entry in dictPlaceholders)
                {
                    string placeholder = entry.Key;
                    string datatype = entry.Value;

                    Console.WriteLine($"Please enter a value for placeholder {placeholder}:");
                    string userInput = Console.ReadLine().Trim();

                    // Prompt till user provides a valid response
                    while (!helpers.ValidateInput(userInput, datatype))
                    {
                        Console.WriteLine($"Please enter a valid value for the {placeholder} to proceed.");
                        userInput = Console.ReadLine().Trim();
                    }

                    // Input is valid, replace placeholder in the text
                    templateContent.Replace(placeholder, userInput);
                }

                // Display the final mail to the user
                Console.WriteLine();
                Console.WriteLine("Here is the final mail:");
                Console.WriteLine("=====================================================================");
                Console.WriteLine(templateContent.ToString());
            }
            catch (IOException ioEx)
            {
                // If required, this exception can be handled differently
                LogError(ioEx, nameof(ProcessTemplate));
            }
            catch (Exception ex)
            {
                // Generic handler for everything else
                LogError(ex, nameof(ProcessTemplate));
            }
        }

        /// <summary>
        /// Method to log exceptions
        /// </summary>
        /// <param name="Ex">The exception to be logged</param>
        /// <param name="MethodName">The name of method where exception occured</param>
        static void LogError(Exception Ex, string MethodName)
        {
            // Output error to the console; any logging framework can be used to log the exception
            Console.WriteLine($"An error occured in the method {MethodName}");
            if (!string.IsNullOrEmpty(Ex.Message))
                Console.WriteLine($"More details are: {Ex.Message}");
        }
    }
}


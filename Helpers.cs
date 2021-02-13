using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EmailComposer
{
    class Helpers
    {
        /// <summary>
        /// Extracts the placeholders along with their corresponding datatypes
        /// </summary>
        /// <param name="Placeholders">The string containing all the parameters</param>
        /// <returns>A Dictionary containing all placeholders and their datatypes in key value pairs</returns>
        internal Dictionary<string, string> ExtractPlaceholders(string Placeholders)
        {
            // Dictionary for placeholders and datatypes
            Dictionary<string, string> placeholdersWithDataTypes = new Dictionary<string, string>();

            // Extract all placeholders alongwith their datatypes and add to the dictionary
            List<string> placeholdersList = Placeholders.ToString().Split(',').ToList();
            placeholdersList.ForEach(item =>
            {
                string[] tempArr = item.ToString().Trim().Split('|');
                placeholdersWithDataTypes.Add(tempArr[0], tempArr[1]);
            });

            return placeholdersWithDataTypes;
        }

        /// <summary>
        /// Validates the user input
        /// </summary>
        /// <param name="Input">The value to be validated</param>
        /// <param name="DataType">The datatype against which value must be validated</param>
        /// <returns>True if the input is of specified datatype, else false</returns>
        internal bool ValidateInput(string Input, string DataType)
        {
            bool inputIsValid = true;

            switch (DataType)
            {
                case "string":
                    if (string.IsNullOrEmpty(Input.Trim()))
                        inputIsValid = false;
                    break;
                case "datetime":
                    var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };
                    inputIsValid = DateTime.TryParseExact(Input, dateFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime scheduleDate);
                    break;
                case "number":
                    inputIsValid = int.TryParse(Input, out int result);
                    break;
            }

            return inputIsValid;
        }
    }
}

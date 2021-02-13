# EmailComposer

EmailComposer is a console application for composing emails based on pre-defined templates and user inputs.

## Usage
User can create any number of templates under the directory 'Templates'. 
The templates will have placeholders in the form of @@FIRST_NAME@@ inside the text. The same placeholders needs to be defined at the end of file, in a comma separated list after the keyword @@@PLACEHOLDERS@@@. 
All placeholders will also have their datatypes explicitly defined along side them, so that the input can be validated.
Valid datetime formats are "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy". New ones can be added in the Helper class.

## Way of working
The application will look for template files under the specified directory. If templates exists, it will display the list of the same and ask user to choose one to proceed with. 
After this, the application will prompt one after another for all placeholders' real values, validate the inputs and present the final text to the user.
From here on, the same can be extended to send out the final text in the form of email by calling an appropriate service.

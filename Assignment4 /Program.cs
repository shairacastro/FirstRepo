
using System;
using System.IO;
using Clients;

Client myClient = new();
List<Client> listOfClient = [];

LoadFileValuesToMemory(listOfClient);

bool loopAgain = true;
while (loopAgain)
{
    try
    {
        DisplayMainMenu();
        string mainMenuChoice = Prompt("\nEnter a Main Menu Choice: ").ToUpper();
        if (mainMenuChoice == "N")
            myClient = NewClient();
        if (mainMenuChoice == "S")
            ShowClientInfo(myClient);

        if (mainMenuChoice == "A")
            AddClientToList(myClient, listOfClient);
        if (mainMenuChoice == "F")
            myClient = FindClientInList(listOfClient);
        if (mainMenuChoice == "R")
            RemoveClientFromList(myClient, listOfClient);
        if (mainMenuChoice == "D")
            DisplayAllClientInList(listOfClient);
        if (mainMenuChoice == "Q")
        {
            SaveMemoryValuesToFile(listOfClient);
            loopAgain = false;
            throw new Exception("Bye, hope to see you again.");
        }
        if (mainMenuChoice == "E")
        {
            while (true)
            {
                DisplayEditMenu();
                string editMenuChoice = Prompt("\nEnter a Edit Menu Choice: ").ToUpper();
                if (editMenuChoice == "F")
                    GetFirstName(myClient);
                if (editMenuChoice == "L")
                    GetLastName(myClient);
                if (editMenuChoice == "W")
                    GetWeight(myClient);
                if (editMenuChoice == "H")
                    GetHeight(myClient);
                if (editMenuChoice == "R")
                    throw new Exception("Returning to Main Menu");
            }

        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{ex.Message}");
    }
}

void DisplayMainMenu()
{
    Console.WriteLine("-------------------------------------");
    Console.WriteLine("\n Main Menu");
    Console.WriteLine("============");
    Console.WriteLine("N) New Client PartA");
    Console.WriteLine("S) Show Client Info PartA");
    Console.WriteLine("E) Edit Client Info PartA");
    Console.WriteLine("A) Add Client To List PartB");
    Console.WriteLine("F) Find Client In List PartB");
    Console.WriteLine("R) Remove Client From List PartB");
    Console.WriteLine("D) Display all Client in List PartB");
    Console.WriteLine("Q) Quit");
}

void DisplayEditMenu()
{
    Console.WriteLine("Edit Menu");
    Console.WriteLine("=============");
    Console.WriteLine("F) First Name");
    Console.WriteLine("L) Last Name");
    Console.WriteLine("W) Weight");
    Console.WriteLine("H) Height");
    Console.WriteLine("R) Return to Main Menu");
}


string Prompt(string prompt)
{
    string myString = "";
    while (true)
    {
        try
        {
            Console.Write(prompt);
            myString = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(myString))
                throw new Exception($"Empty Input: Please enter something.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    return myString;
}

double PromptDoubleBetweenMinMax(String msg, double min, double max)
{
    double num = 0;
    while (true)
    {
        try
        {
            Console.Write($"{msg} between {min} and {max} inclusive: ");
            num = double.Parse(Console.ReadLine());
            if (num < min || num > max)
                throw new Exception($"Must be between {min:n2} and {max:n2}");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid: {ex.Message}");
        }
    }
    return num;
}

Client NewClient()
{
    Client myClient = new();
    GetFirstName(myClient);
    GetLastName(myClient);
    GetWeight(myClient);
    GetHeight(myClient);
    return myClient;
}


void GetFirstName(Client myClient)
{
    string firstName = Prompt("Enter Client's First Name: ");
    myClient.FirstName = firstName;
}

void GetLastName(Client myClient)
{
    string lastName = Prompt("Enter Client's Last Name: ");
    myClient.LastName = lastName;
}

void GetWeight(Client myClient)
{
    double weight = PromptDoubleBetweenMinMax("Enter Weight in pounds: ", 0, 200);
    myClient.Weight = weight;
}

void GetHeight(Client myClient)
{
    double height = PromptDoubleBetweenMinMax("Enter Height in inches: ", 12, 96);
    myClient.Height = height;
}


void AddClientToList(Client myClient, List<Client> listOfClient)
{

    listOfClient.Add(myClient);
    SaveMemoryValuesToFile(listOfClient);
}

Client FindClientInList(List<Client> listOfClient)
{
    if (listOfClient.Count == 0)
        throw new Exception("No clients in the list.");

    string search = Prompt("Search by First Name or Last Name: ");
    List<Client> filteredList = listOfClient.FindAll(c => c.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) || c.LastName.Contains(search, StringComparison.OrdinalIgnoreCase));

    if (filteredList.Count == 0)
    {
        Console.WriteLine("No clients found.");
        return null;
    }


    Console.WriteLine($"\n===== LIST OF CLIENTS =====");
    int index = 0;
    foreach (Client client in filteredList)
    {
        Console.WriteLine($"[{index++}] {client.LastName}, {client.FirstName} ");
    }


    return filteredList[0];
}



void RemoveClientFromList(Client clientToRemove, List<Client> listOfClients)
{
    int clientIndex = listOfClients.FindIndex(c =>
        c.FirstName.Equals(clientToRemove.FirstName, StringComparison.OrdinalIgnoreCase) &&
        c.LastName.Equals(clientToRemove.LastName, StringComparison.OrdinalIgnoreCase));

    if (clientIndex != -1)
    {
        Client removedClient = listOfClients[clientIndex];
        listOfClients.RemoveAt(clientIndex);
        Console.WriteLine($"Client {removedClient.FirstName} {removedClient.LastName} removed from the list successfully.");

        SaveMemoryValuesToFile(listOfClients);
    }
    else
    {
        Console.WriteLine("Client not found in the list.");
    }
}


void DisplayAllClientInList(List<Client> listOfClient)
{
    if (listOfClient.Count == 0)
    {
        Console.WriteLine("No clients in the list.");
        return;
    }

    Console.WriteLine("List of Clients:");
    Console.WriteLine("================");

    foreach (Client myClient in listOfClient)
    {
        ShowClientInfo(myClient);
        Console.WriteLine("----------------");
    }
}

void ShowClientInfo(Client myClient)
{
    Console.WriteLine("=== CLIENT INFO ===");
    Console.WriteLine($"Name: {myClient.FirstName} {myClient.LastName}");
    Console.WriteLine($"Weight: {myClient.Weight} lbs");
    Console.WriteLine($"Height: {myClient.Height} inches");
    Console.WriteLine($"\tBMI Score : {myClient.BmiScore:f2}");
    Console.WriteLine($"\tBMI Status : {myClient.BmiStatus}");
    Console.WriteLine();
}



void LoadFileValuesToMemory(List<Client> listOfClient)
{
    while (true)
    {
        try
        {
            string fileName = "clients.csv";
            string filePath = $"./data/{fileName}";

            if (!File.Exists(filePath))
                throw new Exception($"The file {fileName} does not exist.");

            string[] csvFileInput = File.ReadAllLines(filePath);
            foreach (string line in csvFileInput)
            {
                string[] clientInfo = line.Split(',');
                if (clientInfo.Length >= 10 )
                {
                    
                    Client myClient = new Client(clientInfo[0], clientInfo[1], double.Parse(clientInfo[2]), double.Parse(clientInfo[3]));
                    listOfClient.Add(myClient);
                }
                else
                {
                    Console.WriteLine($"Invalid data format: {line}");
                }
            }
            Console.WriteLine($"Load complete. {fileName} has {listOfClient.Count} data entries");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            
        }
    }
}


void SaveMemoryValuesToFile(List<Client> listOfClient)
{
    //string fileName = Prompt("Enter file name including .csv or .txt: ");
    string fileName = "clients.csv";
    string filePath = $"./data/{fileName}";
    string[] csvLines = new string[listOfClient.Count];
    for (int i = 0; i < listOfClient.Count; i++)
    {
        csvLines[i] = listOfClient[i].ToString();
    }
    File.WriteAllLines(filePath, csvLines);
    Console.WriteLine($"Save complete. {fileName} has {listOfClient.Count} entries.");
}
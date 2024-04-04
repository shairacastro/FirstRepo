
// TODO: declare a constant to represent the max size of the values
// and dates arrays. The arrays must be large enough to store
// values for an entire month.
int physicalSize = 31;
int logicalSize = 0;

// TODO: create a double array named 'values', use the max size constant you declared
// above to specify the physical size of the array.
double[] values = new double[physicalSize];

// TODO: create a string array named 'dates', use the max size constant you declared
// above to specify the physical size of the array.
string[] dates = new string[physicalSize];

bool goAgain = true;
while (goAgain)
{
  try
  {
    DisplayMainMenu();
    string mainMenuChoice = Prompt("\nEnter a Main Menu Choice: ").ToUpper();
    if (mainMenuChoice == "L")
      logicalSize = LoadFileValuesToMemory(dates, values);
    if (mainMenuChoice == "S")
      SaveMemoryValuesToFile(dates, values, logicalSize);
    if (mainMenuChoice == "D")
      DisplayMemoryValues(dates, values, logicalSize);
    if (mainMenuChoice == "A")
      logicalSize = AddMemoryValues(dates, values, logicalSize);
    if (mainMenuChoice == "E")
      EditMemoryValues(dates, values, logicalSize);
    if (mainMenuChoice == "Q")
    {
      goAgain = false;
      throw new Exception("Bye, hope to see you again.");
    }
    if (mainMenuChoice == "R")
    {
      while (true)
      {
        if (logicalSize == 0)
        {
          throw new Exception("No entries loaded. Please load a file into memory");
        }
        else
        {
          DisplayAnalysisMenu();
          string analysisMenuChoice = Prompt("\nEnter an Analysis Menu Choice: ").ToUpper();
          if (analysisMenuChoice == "A")
            FindAverageOfValuesInMemory(values, logicalSize);
          if (analysisMenuChoice == "H")
            FindHighestValueInMemory(values, logicalSize);
          if (analysisMenuChoice == "L")
            FindLowestValueInMemory(values, logicalSize);
          if (analysisMenuChoice == "G")
            GraphValuesInMemory(dates, values, logicalSize);
          if (analysisMenuChoice == "R")
            throw new Exception("Returning to Main Menu");
        }
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
  Console.WriteLine("\nMain Menu");
  Console.WriteLine("L) Load Values from File to Memory");
  Console.WriteLine("S) Save Values from Memory to File");
  Console.WriteLine("D) Display Values in Memory");
  Console.WriteLine("A) Add Value in Memory");
  Console.WriteLine("E) Edit Value in Memory");
  Console.WriteLine("R) Analysis Menu");
  Console.WriteLine("Q) Quit");
}

void DisplayAnalysisMenu()
{
  Console.WriteLine("\nAnalysis Menu");
  Console.WriteLine("A) Find Average of Values in Memory");
  Console.WriteLine("H) Find Highest Value in Memory");
  Console.WriteLine("L) Find Lowest Value in Memory");
  Console.WriteLine("G) Graph Values in Memory");
  Console.WriteLine("R) Return to Main Menu");
}

string Prompt(string prompt)
{
  string response = "";
  Console.Write(prompt);
  response = Console.ReadLine();
  return response;
}

string GetFileName()
{
  string fileName = "";
  do
  {
    fileName = Prompt("Enter file name including .csv or .txt: ");
  } while (string.IsNullOrWhiteSpace(fileName));
  return fileName;
}

int LoadFileValuesToMemory(string[] dates, double[] values)
{
  string fileName = GetFileName();
  int logicalSize = 0;
  string filePath = $"./data/{fileName}";
  if (!File.Exists(filePath))
    throw new Exception($"The file {fileName} does not exist.");
  string[] csvFileInput = File.ReadAllLines(filePath);
  for (int i = 0; i < csvFileInput.Length; i++)
  {
    Console.WriteLine($"lineIndex: {i}; line: {csvFileInput[i]}");
    string[] items = csvFileInput[i].Split(',');
    for (int j = 0; j < items.Length; j++)
    {
      Console.WriteLine($"itemIndex: {j}; item: {items[j]}");
    }
    if (i != 0)
    {
      dates[logicalSize] = items[0];
      values[logicalSize] = double.Parse(items[1]);
      logicalSize++;
    }
  }
  Console.WriteLine($"Load complete. {fileName} has {logicalSize} data entries");
  return logicalSize;
}

void DisplayMemoryValues(string[] dates, double[] values, int logicalSize)
{
  if (logicalSize == 0)
    throw new Exception($"No Entries loaded. Please load a file to memory or add a value in memory");
  Console.WriteLine($"\nCurrent Loaded Entries: {logicalSize}");
  Console.WriteLine($"   Date     Value");
  for (int i = 0; i < logicalSize; i++)
    Console.WriteLine($"{dates[i]}   {values[i]}");
}

double FindHighestValueInMemory(double[] values, int logicalSize)
{
  if (logicalSize == 0)
  {
    throw new Exception($"No entries loaded. Please load a file into memory or add values to memory");
  }
  double highestValue = values[0];
  for (int i = 1; i < logicalSize; i++)
  {
    if (values[i] > highestValue)
    {
      highestValue = values[i];
    }
  }
  Console.WriteLine($"Highest value in memory: {highestValue}");
  return highestValue;
}


double FindLowestValueInMemory(double[] values, int logicalSize)
{
  if (logicalSize == 0)
    throw new Exception($"No entries loaded. Please load a file into memory or add values to memory");
  double lowestValue = values[0];
  for (int i = 1; i < logicalSize; i++)
  {
    if (values[i] < lowestValue)
    {
      lowestValue = values[i];
    }
  }
  Console.WriteLine($"Lowest value in memory: {lowestValue}");
  return lowestValue;
}

void FindAverageOfValuesInMemory(double[] values, int logicalSize)
{
  if (logicalSize == 0)
    throw new Exception($"No entries loaded. Please load a file into memory or add values to memory");
  double sum = 0;
  foreach (double value in values)
  {
    sum += value;
  }
  double average = sum / logicalSize;
  Console.WriteLine($"Average value in memory: {average}");
}

void SaveMemoryValuesToFile(string[] dates, double[] values, int logicalSize)
{
  string fileName = GetFileName();
  try
  {
    using (StreamWriter writer = new StreamWriter(fileName))
    {
      writer.WriteLine("Date,Value");
      for (int i = 0; i < logicalSize; i++)
      {
        writer.WriteLine($"{dates[i]},{values[i]}");
      }
    }
    Console.WriteLine("Memory values saved to file successfully.");
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Error while saving memory values to file: {ex.Message}");
  }
}

int AddMemoryValues(string[] dates, double[] values, int logicalSize)
{
  Console.WriteLine("Enter a new value to add to memory:");
  double newValue;
  if (double.TryParse(Prompt("Value: "), out newValue))
  {
    string newDate = Prompt("Date (MM-DD-YYYY): ");
    if (!string.IsNullOrWhiteSpace(newDate))
    {
      dates[logicalSize] = newDate;
      values[logicalSize] = newValue;
      logicalSize++;
      Console.WriteLine("Value added to memory successfully.");
    }
    else
    {
      Console.WriteLine("Invalid date. Value not added to memory.");
    }
  }
  else
  {
    Console.WriteLine("Invalid value. Value not added to memory.");
  }
  return logicalSize;
}

void EditMemoryValues(string[] dates, double[] values, int logicalSize)
{
  if (logicalSize == 0)
  {
    Console.WriteLine("No entries loaded. Please load a file into memory or add values to memory.");
    return;
  }

  DisplayMemoryValues(dates, values, logicalSize);

  int index;
  string userInput;

  do
  {
    Console.Write("Enter the index of the value to edit: ");
    userInput = Console.ReadLine();
  } while (!int.TryParse(userInput, out index) || index < 0 || index >= logicalSize);

  do
  {
    Console.Write("Enter the new value: ");
    userInput = Console.ReadLine();
  } while (!double.TryParse(userInput, out values[index]));

  Console.WriteLine("Value edited successfully.");
}


void GraphValuesInMemory(string[] dates, double[] values, int logicalSize) {
    Console.WriteLine($"=== Sales of the month ===");

    Console.WriteLine($"Dollars");
    Array.Sort(dates, values, 0, logicalSize);

    int dollars = 100;
    string perLine = "";

    while(dollars >= 0) {
        Console.Write($"{dollars,4}|");

		bool anySalesFound = false;
        for(int i = 1; i <= physicalSize; i++) { 
            string[] salesDate = dates[0].Split('-');

            string formatDay = i.ToString("00");
            int salesIndex = Array.IndexOf(dates, $"{salesDate[0]}-{formatDay}-{salesDate[2]}"); 

            if(salesIndex != -1 ) {
              
              if (values[salesIndex] >= dollars && values[salesIndex] <= (dollars + 9))
              {
                  perLine += $" {values[salesIndex],1}";
				  anySalesFound = true;
              } else {
                  perLine += $"{' ',4}"; 
              }
            } 
			else
			 {
                  perLine += $"{' ',4}"; 
              }
        }
		if (!anySalesFound)
		{
			perLine = "";
		}
        Console.WriteLine($"{perLine}");
        perLine = "";
        dollars -= 10;
    }

    string line = "-----";
    string days = "";

    for(int i = 1; i <= physicalSize; i++) {
        string formatDay = i.ToString("00");
        line += "----";
        days += $"{formatDay,4}";
    }

    Console.WriteLine($"{line}");
    Console.Write($"Date|");
    Console.Write($"{days}");

    Console.WriteLine();  
}

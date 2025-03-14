﻿using CommandLine;
using NcMultiplicator;
using System.Globalization;

Options opt = Parser.Default.ParseArguments<Options>(args).Value;

if (opt.File == String.Empty)
{
    Console.WriteLine("Zadej osu (X/Y/Z/C):");
    string? axis = Console.ReadLine();
    if (axis != null) opt.Axis = axis;
}

if (opt.File == String.Empty)
{
    Console.WriteLine("Zadej název souboru:");
    string? file = Console.ReadLine();
    if (file != null) opt.File = file;
}

if (opt.Coefficient == String.Empty)
{
    Console.WriteLine("Zadej koeficient:");
    string? coeffStr = Console.ReadLine();
    if (coeffStr != null) opt.Coefficient = coeffStr;
}


Console.WriteLine("================================");
Console.WriteLine("Spuštěno s parametry:");
Console.WriteLine($"    Soubor = {opt.File}");
Console.WriteLine($"    Osa = {opt.Axis}");
Console.WriteLine($"    Koeficient = {opt.Coefficient}");
Console.WriteLine("================================");



if (!File.Exists(opt.File))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Zdrojový soubor nelze nalézt!");
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadLine();
    return Done(-1);
}

double coeff = 0;
if (!double.TryParse(opt.Coefficient, CultureInfo.InvariantCulture,out coeff))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Koeficient není číslo!");
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadLine();
    return Done(-1);
}

if(opt.Axis.Length != 1 || !(opt.Axis.ToLower() == "x" || opt.Axis.ToLower() == "y" || opt.Axis.ToLower() == "z" || opt.Axis.ToLower() == "c"))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Naplatné označení osy!");
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadLine();
    return Done(-1);
}

string newfilename = Path.GetFileNameWithoutExtension(opt.File);
newfilename += "_result" + Path.GetExtension(opt.File);

List<string> newLines = new List<string>(); 
Console.WriteLine("Zpracovávám vstupní data...");
int rowNum = 1;
try
{
    foreach (string line in File.ReadAllLines(opt.File))
    {
        newLines.Add(ProcessLine(line,coeff));
        rowNum++;
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Chyba na řádku [{rowNum}]:\n{ex.Message}");
    Console.ForegroundColor = ConsoleColor.White;

    return Done(-1);
}

Console.WriteLine("Zapisuji do výstupního souboru...");

File.WriteAllLines(newfilename, newLines);

Console.WriteLine("Hotovo");
return Done(0);

string ProcessLine(string originalLine,double coeff)
{
    string newLine = "";
    string[] fragments = originalLine.Split(' ');

    foreach (string fragment in fragments)
    {
        if (fragment.ToLower().StartsWith(opt.Axis.ToLower()))
        {
            newLine += " " + ProcessFragment(fragment, coeff);
        }
        else
        {
            newLine += " " + fragment;
        }
    }

    return newLine.Trim();
}

string ProcessFragment(string fragment,double coeff)
{
    char leadingChar = fragment[0];
    string numValue = fragment.Substring(1);
    double number = double.Parse(numValue,CultureInfo.InvariantCulture);
    double newNumber = number * coeff;

    return leadingChar + newNumber.ToString("F3", CultureInfo.InvariantCulture);
}

int Done(int code)
{
    Console.WriteLine("Stiskni ENTER pro ukončení aplikace");
    Console.ReadLine();
    return code;
}
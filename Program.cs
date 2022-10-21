using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Calculator
{
    internal class Program
    {
        public const string error = "Invalid Input";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, this is a calculator program, please input you calculation\n" +
                "Write it in this way: '5+6*5-4/5'\nAccepted operators include:\n+ for addition\n- for subtraction" +
                "\n* for multiplication\n/ for division\n" +
                "You can use parentheses if you wish, for example: '5*(6*5-4)/5'");
            string input=Console.ReadLine();


            char[] operators = { '*', '/', '-', '+' }; //In mathematical order

            string result = parentheses(input, operators);
            Console.WriteLine(result);
            while (doneCheck(result, operators) != 0)
            {
                result = Calculate(result, operators);
            }
            Console.WriteLine(result);

            //Creates a new function that does not incude the question, so that it can work as a recursive function.
            /*
            string result=Start(input);
            if(result.Contains(error)){
                Console.WriteLine(error+", try again!");
                Main(null);

            }
            else
            {
                Console.WriteLine("Result: " + result);
                while (true)
                {
                    Console.WriteLine("Do you want to perform another calculation? 1 for yes, 2 for no");
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Restarting program");
                            Main(null);
                            break;
                        case 2:
                            Console.WriteLine("Exiting program");
                            Environment.Exit(0);
                            break;
                    }
            
                }
            
            
            }
            */



        }




        private static string Start(string input)
        {
            char[] operators = { '*', '/', '-', '+' }; //In mathematical order
            if (checkViability(input) != "") return error;
            input = parentheses(input, operators);
            
            List<double> allValues = getAllValues(input, operators);
            List<string> allOperators = getAllOperators(input, allValues, operators);
            input = Negativevalues(input, allOperators, allValues, operators);

            while (true)
            {
                string value=Calculate(input, operators);
                if (doneCheck(input, operators) == 0)
                {
                    return ""+value;
                }
            } 
        }
        private static string Negativevalues(string input,List<string> allOperators, 
            List<double> allValues ,char[] operators)
        {
            string[] negativeNumberIndicator = { "*-", "/-", "--", "+-" };
            //Taken from: https://stackoverflow.com/a/38573531
            List<int> NegativeValueIndexer = new List<int>();
            if (input.StartsWith("-"))
            {
                NegativeValueIndexer.Add(0);
                input = input.Substring(1);
            }
            foreach (string s in negativeNumberIndicator)
            {
                if (input.Contains(s))
                {
                    for (int j = 0; j < count(input, s); j++)
                    {

                        string[] templist = input.Split(operators);
                        int index = 0;
                        foreach (string str in templist)
                        {
                            if (str == "")
                            {
                                NegativeValueIndexer.Add(index);
                            }
                            index++;

                        }
                    }

                    input.Replace(s, s[0].ToString());

                }
            }
            int i=0;
            List<double> tempValues = allValues;
            
            foreach(double value in tempValues)
            {
                double thisvalue =value;
                if (NegativeValueIndexer.Contains(i))
                {
                    thisvalue *= -1;
                    i++;

                }
                allValues[i] = thisvalue;

            }
            List<string> newOperators = getAllOperators(input, allValues, operators);
            allOperators = newOperators;

            return input;
        }
        private static string Calculate(string input,char[] operators)
        {
            List<double> allValues = getAllValues(input, operators);
            List<string> allOperators = getAllOperators(input, allValues,operators);
            //input = Negativevalues(input, allOperators,allValues,operators);
            //Console.WriteLine(allValues.Count);
            //Console.WriteLine(allOperators.Count);
            foreach (char c in operators)
            {   
                if (allOperators.Contains("" + c))
                {
                    int index = allOperators.IndexOf("" + c);
                    
                    double firstvalue = allValues[index-1];
                    double secondvalue = allValues[index];

                    switch (c)
                    {
                        case '*':
                            input = input.Replace("" + firstvalue + c + secondvalue, "" + (firstvalue * secondvalue));
                            break;
                        case '/':
                            input = input.Replace("" + firstvalue + c + secondvalue, "" + (firstvalue / secondvalue));
                            break;
                        case '+':
                            input = input.Replace("" + firstvalue + c + secondvalue, "" + (firstvalue + secondvalue));
                            break;
                        case '-':
                            input = input.Replace("" + firstvalue + c + secondvalue, "" + (firstvalue - secondvalue));
                            break;
                    }
                    return input;
                }
            }
            return "Invalid Input";

        }

        static string parentheses(string input, char[] operators)
        {
            Console.WriteLine("Loop: "+input);
            if (count(input, "(") == count(input, ")"))
            {
                if (input.Contains('('))
                {
                    int index1 = input.IndexOf('(');
                    int index2 = input.LastIndexOf(')');
                    string inside=input.Substring(index1+1, index2-index1-1);
                    string after=input.Substring(index2+1);
                    if (inside.Contains(')'))
                    {
                        if(inside.IndexOf(')') > inside.IndexOf('('))
                        {

                            Console.WriteLine("EarlyInside: " + inside);
                            
                            while (true)
                            {
                                if(count(inside, "(") == count(inside, ")"))
                                {
                                    List<int> RightParen = new List<int>();
                                    List<int> LeftParen = new List<int>();
                                    string TempInsideRight = inside;
                                    string TempInsideLeft = inside;

                                    for (int i=0; i<count(inside, ")"); i++)
                                    {
                                        RightParen.Add((inside.Length - TempInsideRight.Length) + 
                                            TempInsideRight.IndexOf('(') + 1);
                                        LeftParen.Add((inside.Length - TempInsideLeft.Length) + 
                                            TempInsideLeft.IndexOf(')') + 1);
                                        TempInsideRight = TempInsideRight.Substring(TempInsideRight.IndexOf('(')+1);
                                        TempInsideLeft = TempInsideLeft.Substring(TempInsideLeft.IndexOf(')')+1);

                                    }
                                    Console.WriteLine("RightParen");
                                    foreach (int i in RightParen)
                                    {
                                        Console.WriteLine(i);
                                    }
                                    Console.WriteLine("LeftParen");
                                    foreach (int i in LeftParen)
                                    {
                                        Console.WriteLine(i);
                                    }
                                }
                                
                                inside = inside.Substring(0, inside.LastIndexOf(')'));
                                Console.WriteLine("Inside: " + inside);
                            } 

                        }
                    }
                    
                    string[] allOtherValues = input.Split(new string[] { inside }, StringSplitOptions.None);
                    string before = allOtherValues[0];
                    after = allOtherValues[1];
                    char secondOperator = after[1];

                    before = before.Remove(before.Length - 1);
                    before = before.Remove(before.Length - 1);
                    after  = after.Substring(1);
                    after  = after.Substring(1);

                    Console.WriteLine("Before: "+before +" After: "+ after);




                    char firstOperator;
                    if (index1 > 0)
                    {
                        before = input.Substring(0, index1 - 1);
                        firstOperator = input[index1 - 1];

                    }
                    else
                    {
                        before = "";
                        firstOperator = '\0';
                    }

                   
                    bool running = true;
                    while (running)
                    {
                        bool insideContains = inside.Contains('(');
                        bool afterContains = after.Contains('(');
                        //Taken from: https://stackoverflow.com/a/8851333
                        string Contains = insideContains + "-" + afterContains;
                        switch (Contains)
                        {
                            case "False-False":
                                Console.WriteLine(Contains);
                                running = false;
                                break;
                            case "True-False":
                                Console.WriteLine(Contains);
                                inside = parentheses(inside, operators);
                                break;
                            case "False-True":
                                Console.WriteLine(Contains);
                                after = parentheses(after, operators);
                                break;
                            case "True-True":
                                Console.WriteLine(Contains);
                                inside = parentheses(inside, operators);
                                after = parentheses(after, operators);
                                break;
                            default:
                                Console.WriteLine("Faulty Program");
                                Environment.Exit(0);
                                break;
                        }
                    }
                    while (doneCheck(inside,operators) != 0)
                    {
                        inside= Calculate(inside, operators);
                    }
                    return before + firstOperator + inside + secondOperator + after;
                }
                return input;
            }
            else
            {
                return error;
            }
            
        }
        static List<double> getAllValues(string input, char[] operators)
        {
            string[] allValuesString;
            allValuesString = input.Split(operators);
            allValuesString = allValuesString.Except(new string[] { "" }).ToArray();
            List<double> allValues= new List<double>();
            foreach (string value in allValuesString)
            {
                allValues.Add(double.Parse(value));
            }
            return allValues;
        }
        static List<string> getAllOperators(string input,List<double> allValues, char[] operators)
        {
            string[] allOperators;
            string[] allValuseString = new string[allValues.Count];
            int i = 0;
            foreach(double d in allValues) 
            {
                i++;
                allValuseString[i-1] = "" + d;
                    
            } 
            allOperators = input.Split(allValuseString, StringSplitOptions.None);
            string[] tempAllOperators = allOperators;
            int j = 0;
            List<string> allOperatorsList = new List<string>();
            allOperatorsList.AddRange(allOperators);
            string[] allValuesTemp = input.Split(operators);
            foreach (string value in allValuesTemp)
            {
                j++;
                if(value=="" && j!=0 && j != allOperators.Length)
                {
                    allOperatorsList.RemoveAt(j);
                }
            }
            allOperators = allOperators.Except(new string[] { "" }).ToArray();

            
            return allOperatorsList;
        }
        static int count(string input, string looksFor)
        {
            string[] looksForList= new string[1];
            looksForList[0] = looksFor;
            //https://stackoverflow.com/a/55426470
            return input.Split(looksForList, StringSplitOptions.None).Length - 1;
        }
        static string checkViability(string input)
        {
            if (input.Contains(error) || input.Any(Char.IsLetter) || count(input, "(") != count(input, ")"))
            {
                return error;
            }
            else return "";
        }
        static int doneCheck(string input, char[] operators)
        {
            int winchecker = 0;
            foreach (char c in operators)
            {
                if (input.Contains(c))
                {
                    if (c == '-')
                    {
                        if (!(count(input, "-") == 1 && input.StartsWith("" + c)))
                        {
                            winchecker++;
                        }

                    }
                    else
                    {
                        winchecker++;
                    }
                }
            }
            return winchecker;

        }
        
        
    }
        
}
        
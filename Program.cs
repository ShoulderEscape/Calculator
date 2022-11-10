
namespace Calculator
{
    internal class Program
    {
        public const string invalid = "Invalid Input";
        public const string tryAgain = invalid + ", try again";
        static void Main(string[] args)
        {

            bool ProgramOn = true;
            List<string> results = new List<string>();
            Console.WriteLine("Hello, this is a calculator program, please input you calculation\n" +
                "Write it in this way: '5+6*5-4/5'\nAccepted operators include:\n+ for addition\n- for subtraction" +
                "\n* for multiplication\n/ for division\n" +
                "You can use parentheses if you wish, for example: '5*(6*5-4)/6'" + "\n");
            while (ProgramOn)
            {
                if (results.Count > 0)
                {
                    Console.WriteLine("You can write 'ans' to use the answer from your previous question in this calculation");
                }
                string start;
                string startInput;
                while (true)
                {
                    start = Console.ReadLine();
                    startInput = start;
                    if (start.ToLower().Contains("ans") && results.Count > 0)
                    {
                        if (results.Count > 0)
                        {
                            string[] values = results[results.Count - 1].Split('=');

                            startInput = start.Replace("ans", values[1]);
                        }

                    }
                    if (checkViability(startInput))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(tryAgain);
                    }

                }


                char[] operators = { '*', '/', '-', '+' }; //In mathematical order


                string result = parentheses(startInput);

                while (doneCheck(result) != 0)
                {
                    result = Calculate(result);
                }
                //Console.SetCursorPosition(0, Console.CursorTop-1);
                Console.WriteLine(start + " = " + result);
                results.Add(start + "=" + result);
                bool running = true;
                string choice;
                while (running)
                {
                    Console.WriteLine("Do you want to show all previous results?\n1 for yes, 2 for no");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            int i = 0;
                            foreach (string str in results)
                            {
                                Console.WriteLine(results[i]);
                                i++;
                            }
                            running = false;
                            break;
                        case "2":
                            running = false;
                            break;

                        default:
                            Console.WriteLine(tryAgain);
                            break;
                    }


                }
                running = true;
                while (running)
                {
                    Console.WriteLine("Do you want to do another calculation?\n1 for yes, 2 for no");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            running = false;
                            break;
                        case "2":
                            running = false;
                            ProgramOn = false;
                            break;

                        default:
                            Console.WriteLine(tryAgain);
                            break;
                    }
                }
                Console.Clear();


                List<List<string>> Negativevalues(string input)
                {

                    string[] ValuesArray = input.Split(operators);
                    List<double> Values = new List<double>();
                    bool nextIsNegative = false;
                    string[] theseOperators = input.Split(ValuesArray, StringSplitOptions.None);

                    foreach (string str in ValuesArray)
                    {
                        if (str == "")
                        {
                            nextIsNegative = true;
                        }
                        else if (nextIsNegative)
                        {
                            Values.Add(double.Parse(str) * -1);
                            nextIsNegative = false;
                        }
                        else
                        {
                            Values.Add(double.Parse(str));
                        }
                    }


                    List<string> newOperators = new List<string>();
                    foreach (string str in theseOperators)
                    {
                        if (str != "")
                        {
                            newOperators.Add("" + str[0]);
                        }

                    }

                    List<string> ValuesString = new List<string>();
                    foreach (double d in Values)
                    {
                        ValuesString.Add(d + "");
                    }
                    if (input.StartsWith("-"))
                    {
                        newOperators.RemoveAt(0);
                    }
                    return new List<List<string>> { ValuesString, newOperators };

                }
                string Calculate(string input)
                {
                    List<List<string>> Output = Negativevalues(input);
                    List<string> TheseValuesString = Output[0];
                    List<string> TheseOperators = Output[1];

                    List<double> TheseValues = new List<double>();
                    foreach (string str in TheseValuesString)
                    {
                        TheseValues.Add(double.Parse(str));
                    }


                    foreach (char c in operators)
                    {
                        if (TheseOperators.Contains("" + c))
                        {
                            int index = TheseOperators.IndexOf("" + c);
                            double firstvalue = TheseValues[index];
                            double secondvalue = TheseValues[index + 1];

                            switch (c)
                            {
                                case '*':
                                    input = input.Replace("" + firstvalue + c + secondvalue, "" + (firstvalue * secondvalue));
                                    break;
                                case '/':
                                    if (secondvalue == 0)
                                    {
                                        return "Undefined!";
                                    }
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
                    return invalid;

                }

                string parentheses(string input)
                {

                    if (count(input, "(") == count(input, ")"))
                    {
                        if (input.Contains('('))
                        {
                            int index1 = input.IndexOf('(');
                            int index2 = input.LastIndexOf(')');
                            string inside = input.Substring(index1 + 1, index2 - index1 - 1);

                            if (inside.Contains(')'))
                            {
                                int ParenIndex = 1;
                                for (int i = 0; i < inside.Length; i++)
                                {
                                    switch (inside[i])
                                    {
                                        case '(':
                                            ParenIndex++;
                                            break;
                                        case ')':
                                            ParenIndex--;

                                            if (ParenIndex == 0)
                                            {
                                                index2 = i;
                                                i += inside.Length;
                                            }
                                            break;
                                    }
                                }


                                Console.WriteLine(inside);
                                if (index2 != input.LastIndexOf(')'))
                                {
                                    inside = inside.Substring(0, index2);
                                }



                            }


                            string[] allOtherValues = input.Split(new string[] { inside }, 2, StringSplitOptions.None);
                            string before = "";
                            string after = "";
                            string secondOperator = "";

                            if (allOtherValues[0] != "(")
                            {

                                before = allOtherValues[0];
                                before = before.Remove(before.Length - 1);
                                before = before.Remove(before.Length - 1);

                            }
                            if (allOtherValues[1] != ")")
                            {
                                after = allOtherValues[1];
                                secondOperator = "" + after[1];

                                after = after.Substring(1);
                                after = after.Substring(1);


                            }








                            string firstOperator;
                            if (index1 > 0)
                            {
                                before = input.Substring(0, index1 - 1);
                                firstOperator = "" + input[index1 - 1];

                            }
                            else
                            {
                                before = "";
                                firstOperator = "";
                            }


                            running = true;
                            while (running)
                            {
                                bool insideContains = inside.Contains('(');
                                bool afterContains = after.Contains('(');
                                //Taken from: https://stackoverflow.com/a/8851333
                                string Contains = insideContains + "-" + afterContains;
                                switch (Contains)
                                {
                                    case "False-False":
                                        running = false;
                                        break;
                                    case "True-False":

                                        inside = parentheses(inside);
                                        break;
                                    case "False-True":

                                        after = parentheses(after);
                                        break;
                                    case "True-True":

                                        inside = parentheses(inside);
                                        after = parentheses(after);
                                        break;
                                    default:
                                        Console.WriteLine("Faulty Program");
                                        Environment.Exit(0);
                                        break;
                                }
                            }
                            while (doneCheck(inside) != 0)
                            {
                                inside = Calculate(inside);
                            }
                            return before + firstOperator + inside + secondOperator + after;
                        }
                        return input;
                    }
                    else
                    {
                        return invalid;
                    }

                }
                int count(string input, string looksFor)
                {
                    string[] looksForList = new string[1];
                    looksForList[0] = looksFor;
                    //From Here: https://stackoverflow.com/a/55426470
                    return input.Split(looksForList, StringSplitOptions.None).Length - 1;
                }
                bool checkViability(string input)
                {
                    if (input.Contains(invalid) || input.Any(Char.IsLetter) || count(input, "(") != count(input, ")"))
                    {
                        return false;
                    }
                    else return true;
                }
                int doneCheck(string input)
                {
                    int winchecker = 0;
                    foreach (char c in operators)
                    {
                        if (input.Contains(c))
                        {
                            if (c == '-')
                            {
                                if (!(count(input, "-") == 1 && input.StartsWith(c + "")))
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
    }
}
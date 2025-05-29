using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.Services
{
    public static class NumberToWords
    {
        private static readonly string[] Units = new string[]
        {
            "", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять"
        };

        private static readonly string[] Tens = new string[]
        {
            "", "десять", "двадцать", "тридцать", "сорок", "пятьдесят",
            "шестьдесят", "семьдесят", "восемьдесят", "девяносто"
        };

        private static readonly string[] Teens = new string[]
        {
            "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать",
            "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"
        };

        private static readonly string[] Hundreds = new string[]
        {
            "", "сто", "двести", "триста", "четыреста", "пятьсот",
            "шестьсот", "семьсот", "восемьсот", "девятьсот"
        };

        private static string ConvertGroup(int number, string[] units, string[] tens, string[] teens)
        {
            if (number == 0)
                return "";

            var result = "";

            // Сотни
            result += Hundreds[number / 100];

            int remainder = number % 100;

            // Десятки и единицы
            if (remainder < 10)
                result += " " + units[remainder];
            else if (remainder < 20)
                result += " " + teens[remainder - 10];
            else
                result += " " + tens[remainder / 10] + " " + units[remainder % 10];

            return result.Trim();
        }

        public static string Convert(string numericValue)
        {
            if (string.IsNullOrWhiteSpace(numericValue))
                return "";

            // Удаляем всё, кроме цифр
            string cleaned = new string(numericValue.Where(char.IsDigit).ToArray());

            if (!int.TryParse(cleaned, out int number))
                return "";

            if (number == 0)
                return "Ноль рублей";

            string result = "";

            int billions = number / 1_000_000_000;
            int millions = (number / 1_000_000) % 1_000;
            int thousands = (number / 1000) % 1000;
            int rest = number % 1000;

            result += ProcessGroup(billions, "миллиард", "миллиарда", "миллиардов");
            result += ProcessGroup(millions, "миллион", "миллиона", "миллионов");
            result += ProcessGroup(thousands, "тысяча", "тысячи", "тысяч");
            result += ProcessGroup(rest, "рубль", "рубля", "рублей");

            return char.ToUpper(result[0]) + result.Substring(1);
        }

        private static string ProcessGroup(int number, string one, string few, string many)
        {
            if (number == 0)
                return "";

            string words = ConvertGroup(number, Units, Tens, Teens);

            int lastDigit = number % 10;
            string suffix = GetSuffix(number, one, few, many);

            return $"{words} {suffix} ";
        }

        private static string GetSuffix(int number, string one, string few, string many)
        {
            int lastDigit = number % 10;
            int lastTwoDigits = number % 100;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
                return many;

            return lastDigit switch
            {
                1 => one,
                2 or 3 or 4 => few,
                _ => many
            };
        }
    }
}

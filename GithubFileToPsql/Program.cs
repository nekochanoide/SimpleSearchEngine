using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
                return;
            string[] lines = File.ReadAllLines(args[0]);
            string[] resultLines = new string[lines.Length - 1];
            resultLines[0] = "id,name,year";
            for (int i = 1, j = 1; i < lines.Length - 1; i++, j++)
            {
                if (i == 566 || i == 2088)
                {
                    j--;
                    continue;
                }
                resultLines[j] = Regex.Replace(lines[i], @" \(\d*\).*", new MatchEvaluator(Kek));
                if (!Regex.IsMatch(resultLines[j], @",\d{4}"))
                {
                    var previous = resultLines[j - 1];
                    resultLines[j] += previous.Substring(previous.Length - 5);
                }
            }

            var dir = Path.GetDirectoryName(args[0]);
            var name = Path.GetFileNameWithoutExtension(args[0]);
            var outPath = Path.Combine(dir, name + "_parsed.csv");
            File.WriteAllLines(outPath, resultLines);
        }

        static string Kek(Match match)
        {
            var raw = match.Value;
            return "," + Regex.Match(raw, @"\d+").ToString();
        }
    }
}

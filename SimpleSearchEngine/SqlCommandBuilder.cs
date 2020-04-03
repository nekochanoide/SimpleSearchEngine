using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSearchEngine
{
    class SqlCommandBuilder
    {
        public static string BuildByFts(string convertedToTsQuery)
        {
            var commandText = $@"
SELECT *
FROM movies
WHERE to_tsvector('english', CONCAT_WS(' ', name, year)) @@ to_tsquery('{convertedToTsQuery}');
";
            return commandText;
        }

        public static string BuildByIlike(IEnumerable<string> words)
        {
            var wordWithIlike = words.Select(x => $"AND name ILIKE '%{x}%'");
            var convertedForWhere = string.Concat(wordWithIlike).Substring(4);

            var commandText = $@"
SELECT *
FROM movies
WHERE {convertedForWhere};
";
            return commandText;
        }

        [Obsolete("Too hard to use", true)]
        public static string AddDate(string command, int date)
        {
            command = command.Replace(";", "");
            command = $"{command} AND\r\ndate={date};";
            return command;
        }
    }
}

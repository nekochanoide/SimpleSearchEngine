using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSearchEngine
{
    class MovieModel : PropertyChangedBase
    {
        private int id;
        private string name;
        private int year;

        public int Id { get => id; set => SetField(ref id, value); }
        public string Name { get => name; set => SetField(ref name, value); }
        public int Year { get => year; set => SetField(ref year, value); }
    }
}

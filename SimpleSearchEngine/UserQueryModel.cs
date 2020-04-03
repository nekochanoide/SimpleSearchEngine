using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSearchEngine
{
    class UserQueryModel : PropertyChangedBase
    {
        private string searchField;

        public string SearchField
        {
            get => searchField;
            set
            {
#if egg
                if (value.StartsWith("all hail ", StringComparison.CurrentCultureIgnoreCase))
                    CultFound?.Invoke(this, value.Substring(9));
#endif

                var tokens = value.Trim().Split(' ');
                Words = tokens;
                ConvertedToTsQuery = string.Join(" & ", tokens);

                SetField(ref searchField, value);
            }
        }

        public string ConvertedToTsQuery { get; private set; }

        public IEnumerable<string> Words { get; private set; }

#if egg
        public event EventHandler<string> CultFound;
#endif
    }
}

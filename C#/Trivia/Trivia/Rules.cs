using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia
{
    public class Rules
    {
        const int minPlayer = 2;
        const int maxPlayer = 6;
        const int nbQuestions = 50;
        const int nbPlaces = 12;
        const int nbCoinsToWin = 6;

        readonly List<string> questionCategories = new List<string>
        {
            "Pop",
            "Science",
            "Sports",
            "Rock"
        };

        public int MinPlayer { get => minPlayer; }
        public int MaxPlayer { get => maxPlayer; }
        public int NbQuestions { get => nbQuestions; }
        public int NbPlaces { get => nbPlaces; }
        public int NbCoinsToWin { get => nbCoinsToWin; }

        public List<string> QuestionCategories { get => questionCategories; }

        public bool DidPlayerWin(Player p)
        {
            return p.Purse == nbCoinsToWin;
        }
    }
}

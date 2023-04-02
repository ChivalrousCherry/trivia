namespace Trivia
{
    public class Game
    {
        Rules rules = new();
        private readonly List<Player> _players = new();

        LinkedList<Question> questions = new();
        private int _currentPlayer;
        private Player currentPlayer = new("");
        private bool _isGettingOutOfPenaltyBox;

        public Game()
        {
            rules.QuestionCategories.ForEach(delegate (string category)
            {
                questions.AddLast(new Question(category, rules.NbQuestions));
            });
        }

        public bool IsPlayable()
        {
            return (HowManyPlayers() >= rules.MinPlayer);
        }

        // on peut supprimer le type de retour bool qui n'est jamais utilisé
        public void Add(string playerName)
        {
            // dans l'exemple actuel on n'aura pas le cas
            // mais le fait de s'être libéré des tableaux a supprimé la limite de base des 6 joueurs
            // on la remet donc ici : si on a déjà le maxPlayer, on n'ajoute plus personne
            if(_players.Count >= rules.MaxPlayer)
            {
                return;
            }
            Player p = new Player(playerName);
            _players.Add(p);

            Console.WriteLine(p.Name + " was added");
            Console.WriteLine("They are player number " + _players.Count);
        }

        public int HowManyPlayers()
        {
            return _players.Count;
        }

        public void Roll(int roll)
        {
            GetCurrentPlayer(roll);

            if (currentPlayer.IsInPenaltyBox)
            {
                _isGettingOutOfPenaltyBox = CanPlayerGetOutOfPenaltyBox(roll);
                if (!_isGettingOutOfPenaltyBox)
                {
                    return;
                }
            }

            MoveCurrentPlayer(roll);

            AskQuestion();

        }

        private bool CanPlayerGetOutOfPenaltyBox(int roll)
        {
            bool canGetOut = roll % 2 != 0;
            string negation = canGetOut ? "" : "not ";
            Console.WriteLine(currentPlayer.Name + " is "
                + negation
                + "getting out of the penalty box");
            return canGetOut;
        }

        private void GetCurrentPlayer(int roll)
        {
            currentPlayer = _players[_currentPlayer];
            Console.WriteLine(currentPlayer.Name + " is the current player");
            Console.WriteLine("They have rolled a " + roll);
        }

        private void MoveCurrentPlayer(int roll)
        {
            // le % nbPlaces simule un tour de plateau : on revient à la première case
            currentPlayer.Place = (currentPlayer.Place + roll) % rules.NbPlaces;

            Console.WriteLine(currentPlayer.Name
                + "'s new location is "
                + currentPlayer.Place);
        }

        private void AskQuestion()
        {
            // calcule quelle est la catégorie de la case du joueur
            int place = currentPlayer.Place % rules.QuestionCategories.Count;
            Question currentQuestion = questions.ElementAt(place);
            Console.WriteLine("The category is " + currentQuestion.Type);
            Console.WriteLine(currentQuestion.AskQuestion());
        }

        public bool WasCorrectlyAnswered()
        {
            if (currentPlayer.IsInPenaltyBox && !_isGettingOutOfPenaltyBox)
            {
                SetNextPlayer();
                return true;
            }

            Console.WriteLine("Answer was correct!!!!");
            currentPlayer.Purse++;
            Console.WriteLine(currentPlayer.Name
                    + " now has "
                    + currentPlayer.Purse
                    + " Gold Coins.");

            var winner = rules.DidPlayerWin(currentPlayer);
            SetNextPlayer();

            return !winner;

        }

        private void SetNextPlayer()
        {
            // ++_currentPlayer pour augmenter la valeur avant le calcul
            // le modulo fonctionne comme pour le plateau : si on a dépassé le dernier joueur on revient au premier
            _currentPlayer = ++_currentPlayer % _players.Count;
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(currentPlayer.Name + " was sent to the penalty box");
            currentPlayer.IsInPenaltyBox = true;

            SetNextPlayer();
            return true;
        }

    }

}

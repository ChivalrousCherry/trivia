using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        readonly int minPlayer = 2;
        readonly int maxPlayer = 6;
        readonly int nbQuestions = 50;
        readonly int nbPlaces = 12;
        readonly int nbCoinsToWin = 6;
        private readonly List<Player> _players = new();

        List<string> questionCategories = new List<string>
        {
            "Pop",
            "Science",
            "Sports",
            "Rock"
        };
        LinkedList<Question> questions = new();
        private int _currentPlayer;
        private Player currentPlayer = new("");
        private bool _isGettingOutOfPenaltyBox;

        public Game()
        {
            questionCategories.ForEach(delegate (string category)
            {
                questions.AddLast(new Question(category, nbQuestions));
            });
        }

        public bool IsPlayable()
        {
            return (HowManyPlayers() >= minPlayer);
        }

        // on peut supprimer le type de retour bool qui n'est jamais utilisé
        public void Add(string playerName)
        {
            // dans l'exemple actuel on n'aura pas le cas
            // mais le fait de s'être libéré des tableaux a supprimé la limite de base des 6 joueurs
            // on la remet donc ici : si on a déjà le maxPlayer, on n'ajoute plus personne
            if(_players.Count >= maxPlayer)
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
            currentPlayer.Place = (currentPlayer.Place + roll) % nbPlaces;

            Console.WriteLine(currentPlayer.Name
                + "'s new location is "
                + currentPlayer.Place);
        }

        private void AskQuestion()
        {
            // calcule quelle est la catégorie de la case du joueur
            int place = currentPlayer.Place % questionCategories.Count;
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

            var winner = DidPlayerWin();
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


        private bool DidPlayerWin()
        {
            // la condition de victoire est bien d'avoir nbCoinsToWin pièces
            // il faut inverser l'ancienne logique pour être cohérent
            // car le GameRunner attend un "not a winner"
            return currentPlayer.Purse == nbCoinsToWin;
        }
    }

}

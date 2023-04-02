﻿using System;
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
        readonly int nbQuestionTypes = 4;
        private readonly List<string> _players = new();

        private readonly int[] _places;
        private readonly int[] _purses;

        private readonly bool[] _inPenaltyBox;

        const string popType = "Pop";
        private readonly LinkedList<string> _popQuestions = new();
        const string scienceType = "Science";
        private readonly LinkedList<string> _scienceQuestions = new();
        const string sportsType = "Sports";
        private readonly LinkedList<string> _sportsQuestions = new();
        const string rockType = "Rock";
        private readonly LinkedList<string> _rockQuestions = new();

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public Game()
        {
            _places = new int[maxPlayer];
            _purses = new int[maxPlayer];
            _inPenaltyBox = new bool[maxPlayer];

            for (var i = 0; i < nbQuestions; i++)
            {
                _popQuestions.AddLast(CreateQuestion(popType, i));
                _scienceQuestions.AddLast(CreateQuestion(scienceType, i));
                _sportsQuestions.AddLast(CreateQuestion(sportsType, i));
                _rockQuestions.AddLast(CreateQuestion(rockType, i));
            }
        }

        public string CreateQuestion(string type, int index)
        {
            return type + " Question " + index;
        }

        public bool IsPlayable()
        {
            return (HowManyPlayers() >= minPlayer);
        }

        // on peut supprimer le type de retour bool qui n'est jamais utilisé
        public void Add(string playerName)
        {
            _players.Add(playerName);
            _places[HowManyPlayers()] = 0;
            _purses[HowManyPlayers()] = 0;
            _inPenaltyBox[HowManyPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
        }

        public int HowManyPlayers()
        {
            return _players.Count;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(_players[_currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);
            bool canGetOut = roll % 2 != 0;

            if (_inPenaltyBox[_currentPlayer])
            {
                _isGettingOutOfPenaltyBox = roll % 2 != 0;
                string negation = _isGettingOutOfPenaltyBox ? "" : "not ";
                Console.WriteLine(_players[_currentPlayer] + " is " 
                    + negation 
                    + "getting out of the penalty box");
                if (!_isGettingOutOfPenaltyBox)
                {
                    return;
                }
            }

            MoveCurrentPlayer(roll);

            Console.WriteLine(_players[_currentPlayer]
                    + "'s new location is "
                    + _places[_currentPlayer]);
            Console.WriteLine("The category is " + CurrentCategory());
            AskQuestion();

        }

        private void MoveCurrentPlayer(int roll)
        {
            // le % nbPlaces simule un tour de plateau : on revient à la première case
            _places[_currentPlayer] = (_places[_currentPlayer] + roll) % nbPlaces;
        }

        private void AskQuestion()
        {
            string currentCategory = CurrentCategory();
            // TODO: créer de la gestion de question pour simplifier ici
            switch(currentCategory)
            {
                case popType:
                    AskQuestionOfType(_popQuestions);
                    break;
                case scienceType:
                    AskQuestionOfType(_scienceQuestions);
                    break;
                case sportsType:
                    AskQuestionOfType(_sportsQuestions);
                    break;
                case rockType:
                    AskQuestionOfType(_rockQuestions);
                    break;
            }
        }

        private void AskQuestionOfType(LinkedList<string> questions)
        {
            Console.WriteLine(questions.First());
            questions.RemoveFirst();
        }

        private string CurrentCategory()
        {
            int place = _places[_currentPlayer] % nbQuestionTypes;
            switch(place)
            {
                case 0:
                    return popType;
                case 1:
                    return scienceType;
                case 2:
                    return sportsType;
                default:
                    return rockType;

            }
        }

        public bool WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer] && !_isGettingOutOfPenaltyBox)
            {
                SetNextPlayer();
                return true;
            }

            Console.WriteLine("Answer was correct!!!!");
            _purses[_currentPlayer]++;
            Console.WriteLine(_players[_currentPlayer]
                    + " now has "
                    + _purses[_currentPlayer]
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
            Console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            SetNextPlayer();
            return true;
        }


        private bool DidPlayerWin()
        {
            // la condition de victoire est bien d'avoir nbCoinsToWin pièces
            // il faut inverser l'ancienne logique pour être cohérent
            // car le GameRunner attend un "not a winner"
            return _purses[_currentPlayer] == nbCoinsToWin;
        }
    }

}

namespace Trivia
{
    public class Question
    {
        private string type;
        private const string label = " Question ";
        private LinkedList<string> questions = new();

        public string Type { get => type; set => type = value; }

        public Question(string type, int number)
        {
            this.type = type;
            for(int i = 0; i < number; i++)
            {
                questions.AddLast(type + label + i);
            }
        }

        public string AskQuestion()
        {
            string question = questions.First();
            questions.RemoveFirst();
            return question;
        }

    }
}

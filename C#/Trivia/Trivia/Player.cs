namespace Trivia
{
    public class Player
    {
        private string name = "defaultPlayer";
        private int place = 0;
        private int purse = 0;
        private bool isInPenaltyBox = false;

        public string Name 
        { 
            get => name; 
            set 
            { 
                if(value != null)
                {
                    name = value;
                }
            } 
        }
        public int Purse { get => purse; set => purse = value; }
        public int Place { get => place; set => place = value; }
        public bool IsInPenaltyBox { get => isInPenaltyBox; set => isInPenaltyBox = value; }
        public Player(string name)
        {
            Name = name;
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using Packets;

namespace Server
{
    public enum GameTypes
    {
        Guess,
        Quizz,
      
    };


    public class Games
    {

        public List<string> cGameList;
      
        public Games()
        {
            cGameList = new List<string>();
            cGameList.Add("Guess the number");
            cGameList.Add("Quizz");
        }

        //and new items to the list eg from file
        public void addGame(List<string> newGames)
        {
            cGameList.AddRange(newGames);
        }
        public void addGame(string newGames)
        {
            cGameList.Add(newGames);
        }
        //get the list 
        public List<string> GetGameList() 
        {
            return cGameList;
        }

    }
   
   public class Lobby
    {
        public string cLobbyType { get; set; }
        
        List<int> cUserindex;
        public bool cIsGameEnd{ get; set; }
        TextGame cGame;
        public int cLobbyName { get; private set; }
       
        int cTurnCount;
        bool cIsHitNeeded=false;
        public Lobby(string lobbyType,int LobbyName,int userindex)
        {
            
            //set up
            cLobbyType = lobbyType;
            cLobbyName = LobbyName;
            cUserindex = new List<int>();
            cUserindex.Add(userindex);
            cIsGameEnd = false;
            switch (lobbyType.ToLower())
            {
                case ("quizz"):
                    cGame = new Quizz();
                    break;
                case ("guess the number"):
                    cGame = new GuessTheNumber();
                    break;
            }
            

          
        }

        public bool AddUser(int client)
        {
            bool isAdd=true;
            foreach(int c in cUserindex)
            {
                if (c == client)
                {
                    isAdd = false;
                }
            }

            if (isAdd)
            {
                cUserindex.Add(client);
                return true;
            }
            return false;
        }

        public void TakeOutUser(int user)
        {
            cUserindex.Remove(user);
        }
        public int NumberOfUser()
        {
            return cUserindex.Count();
        }
         public List<int> GetUserList()
        {
            return cUserindex;
        }
        public string GetQuestion()
        {
            if (cGame.getQuestionNumber() > 10)
            {
                cIsGameEnd = true;
            }
            return cGame.GetQuestion();
            
        }
        public bool CheckAnswer(string answer)
        {
            //count up to see if hint is needed
            cTurnCount++;
            if (cTurnCount == 10)
            {
                cIsHitNeeded = true;
                cTurnCount = 0;
            }
            else
            {
                cIsHitNeeded = false;
            }
            //if correct rest counter to hint
            bool isCorrect = cGame.CheckAnswers(answer);
            if (isCorrect)
            {
                cIsHitNeeded = false;
                cTurnCount = 0;

            }

            

            //retrun to say answer is correct ot not
            return isCorrect;
        }
       public bool IsHintNeed()
        {
            return cIsHitNeeded;
        }
        public string GetHint()
        {
            return cGame.GetHint();
        }
    }
    

    class TextGame
    {
        protected int cQuestionNumber;
        public string cServerResponce { get; protected set; }
        public TextGame()
        {
            cQuestionNumber = 0;
        }
        public virtual bool CheckAnswers(string answer)
        {
            return true;
        }
        public virtual string GetQuestion()
        {
            return "Question";
        }
        public virtual string GetHint()
        {
            return "Hint";
        }
        public int getQuestionNumber()
        {
            return cQuestionNumber;
        }
    }

    class GuessTheNumber:TextGame
    {
        private string cNumberToGuess;
        int cHintNo=0;
        public GuessTheNumber():base()
        {
            //get number
            cNumberToGuess = genNumber();
        }

        string genNumber()
        {
            //gen number
            return RandomNumber().ToString();
        }

        public override bool CheckAnswers(string number)
        {
            if (number == cNumberToGuess)
            {
                cHintNo = 0;
                cQuestionNumber++;
                return true;
            }
            else
            {
                return false;
            }
        }
        public override string GetQuestion()
        {
            return "Guess the Number between 1 and 100";
        }
        public override string GetHint()
        {    
            string Hint="";
            int a;
            Int32.TryParse(cNumberToGuess, out a);
            if (a >= 50)
            {
                switch (cHintNo)
                {
                    case 0:
                    Hint = "It is greater than or equal 50";
                        break;
                    case 1:
                        if (a >= 75)
                        {
                            Hint = "It is greater than or equal 75";
                        }
                        else if (a <= 75)
                        {
                            Hint = "It is greater than or equal 75";
                        }
                        break;
                    case 2:
                        if (a >= 90|| a<=100)
                        {
                            Hint = "It is between 90 and 100 ";
                        }
                        else if (a >= 80 || a <= 90)
                        {
                            Hint = "It is between 80 and 80 ";
                        }
                        else if (a >= 70 || a <= 80)
                        {
                            Hint = "It is between 70 and 90 ";
                        }
                        else if (a >= 60 || a <= 70)
                        {
                            Hint = "It is between 60 and 70 ";
                        }
                        else if (a >= 50 || a <= 60)
                        {
                            Hint = "It is between 50 and 60 ";
                        }
                        break;
                }
 
            }
            else if (a <= 50)
            {
                
                switch (cHintNo)
                {
                    case 0:
                        Hint = "It is less than or equal 50";
                        break;
                    case 1:
                        if (a >= 25)
                        {
                            Hint = "It is greater than or equal 25";
                        }
                        else if (a <= 25)
                        {
                            Hint = "It is greater than or equal 25";
                        }
                        break;
                    case 2:
                        if (a >= 40 || a <= 50)
                        {
                            Hint = "It is between 40 and 50 ";
                        }
                        else if (a >= 30 || a <= 40)
                        {
                            Hint = "It is between 30 and 40 ";
                        }
                        else if (a >= 20 || a <= 30)
                        {
                            Hint = "It is between 20 and 30 ";
                        }
                        else if (a >= 10 || a <= 20)
                        {
                            Hint = "It is between 10 and 20 ";
                        }
                        else if (a >= 0 || a <= 10)
                        {
                            Hint = "It is between 0 and 10 ";
                        }
                        break;
                }
            }

            cHintNo++;
            return Hint;
        }
        private int RandomNumber()
        {
            var rand = new Random();
            
           int randNum= rand.Next(1, 100);
            return randNum;

        }

    }


    class Quizz:TextGame
    {
       
        string cCurrentQuestion;
        string cCurrentAnswer;
        List<string> cQuestions = new List<string>();
        List<string> cAnswers = new List<string>();
       

        public Quizz():base()
        { 
            LoadQuestions();
            //set up
            cQuestionNumber = 0;
            Question();

        }
        

       

        private void LoadQuestions()
        {
            //q1
            cQuestions.Add("2+2?");
            cAnswers.Add("4");
            //q2
            cQuestions.Add("4+4?");
            cAnswers.Add("8");
            //q3
            cQuestions.Add("Stilton cheese originates from which country?");
            cAnswers.Add("England");
            //q4
            cQuestions.Add("What precious stone's name was derived from the Greek word meaning 'unconquerable'?");
            cAnswers.Add("Diamond");
            //q5
            cQuestions.Add("What is the maximum number of levels to play in Pac-Man?");
            cAnswers.Add("255");
            //q6
            cQuestions.Add("Who invented corn flakes?");
            cAnswers.Add("Dr.John Harvey Kellogg");
            //q7
            cQuestions.Add("On the game of Battleship, how many hits does it take to sink the submarine?");
            cAnswers.Add("3");
            //q8
            cQuestions.Add("What year was the UK an Olympic Games host for the first time?");
            cAnswers.Add("1908");
            //q9
            cQuestions.Add("Which country hosted the first Winter Olympic Games?");
            cAnswers.Add("France");
            //q10
            cQuestions.Add("Which river is often viewed as Russia's national river?");
            cAnswers.Add("Volga");
        }

        public override string GetQuestion()
        {
            Question();
            return cCurrentQuestion;
        }

        //questions
        void Question()
        {
            if (cQuestionNumber == cQuestions.Count())
            {
                cQuestionNumber = 0;
            }

            cCurrentQuestion = cQuestions[cQuestionNumber];
            cCurrentAnswer = cAnswers[cQuestionNumber];
            

        }
       public override bool CheckAnswers(string UserAnswers)
        {
            //wait for the amount of users to answer
           
                bool isCorrect = false;
          
                
                if (cCurrentAnswer.ToLower() == UserAnswers.ToLower())
                {
                        cQuestionNumber++;
                        base.cQuestionNumber = cQuestionNumber;
                        isCorrect = true;
                }
                
                if (isCorrect)
                {
                    
                    return true;
                }
                
            
            return false;
        }


        public override string GetHint()
        {
            string hint = "";

            switch (cQuestionNumber)
            {
                case 0:
                    hint = "it is a number below 10.";
                    break;
                case 1:
                    hint = "it is a number over 5 and below 10.";
                    break;
                case 2:
                    hint = "Its region of origin is: Derbyshire, Leicestershire, and Nottinghamshire.";
                    break;
                case 3:
                    hint = "Di_m_nd";
                    break;
                case 4:
                    hint = "It is FF in hexadecimal.";
                    break;
                case 5:
                    hint = "D_.Jo_n H_r_ey Ke_lo_g";
                    break;
                case 6:
                    hint = "3+7-8+1=?";
                    break;
                case 7:
                    hint = "it was between the 1900 and 1910.";
                    break;
                case 8:
                    hint = "It has part of the alps in it.";
                    break;
                case 9:
                    hint = "it has a length of 3,531 km and it flows into the Caspian Sea.";
                    break;
            }
           


            return hint;
        }


    }



}

using System;

namespace MyApp
{
    internal class Blackjack
    {
        public static class PublicVar  // maybe have a few classes named Player/Card/etc for public vals next time for better readability
        {
            // Counters for rounds and wins
            public static int roundCurrent = 1;
            public static int playerStarting, playerCurrent;
            public static int player0Wins, player1Wins, player2Wins;
            public static bool hiddenFirstCard = true;
            public static bool roundFinished = false;

            // Lists for building cards and counting score
            public static string[] playerName = { "Dealer", "Player 1", "Player 2" };
            public static string[] cardSuitSymbol = { "♠", "♥", "♣", "♦" };
            public static string[] cardSuitName = { "Pikes", "Hearts", "Clubs", "Diamonds" };
            public static string[] cardName = { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
            public static string[] cardRank = { "2 ", "3 ", "4 ", "5 ", "6 ", "7 ", "8 ", "9 ", "10", "J ", "Q ", "K ", "A " };
            public static int[] cardScore = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };

            // Lists and strings for storing cards
            public static string currentCardName = "";
            public static int[] playerScore = new int[3];  // Score of normal cards
            public static int[] playerAce = new int[3];    // Number of Aces
            public static int[] playerTotal = new int[3];  // Score + optimal Aces
            public static string[] cardPlayer = new string[14];  // 0-3 are cards of Dealer, 4-7 of Player1, 8-11 of Player2, 12-13 are Dealer's hidden card (hidden at 1,2, shown at 12, 13)
        }

        static void Main(string[] args)
        {
            bool gameRepeat = true;
            Random random = new Random();
            int currentCardID;


            Console.WriteLine(" Welcome to the game of Blackjack.\n In this game 2 players try to collect cards worth 21 points without going over.\n");
            Console.Write(" Player 1, write you name (leave empty for default): ");
            string nameChange = Console.ReadLine();
            if (nameChange != "")
            {
                PublicVar.playerName[1] = nameChange;
            }
            Console.Write("\n Player 2, write you name (leave empty for default): ");
            nameChange = Console.ReadLine();
            if (nameChange != "")
            {
                PublicVar.playerName[2] = nameChange;
            }

            while (gameRepeat == true) 
            {
                string answerAnotherCard;
                bool answerAnotherCardLooping = true, dealerStandsNow = true;
                PublicVar.roundFinished = false;
                PublicVar.playerCurrent = PublicVar.playerStarting;
                List<int> playerPlaying = [0, 1, 2];
                List<int> cardIDs = [000, 001, 002, 003, 004, 005, 006, 007, 008, 009, 010, 011, 012,   // First number is the suit: ♠ ♥ ♣ ♦
                                     100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112,   // 00-08 are numbers - 2 to 10 points
                                     200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212,   // 09-11 are Jack, Queen, King - 10 points
                                     300, 301, 302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312];  // 12 is Ace - 1 or 11 points


                RewriteInfoMain();
                Console.WriteLine(" {0} goes first.", PublicVar.playerName[PublicVar.playerStarting]);
                Console.Write(" Let's see the starting cards");
                Dots();
                for (int i = 0; i < 6; i++)  // Generates first 2 cards for each player
                {
                    currentCardID = cardIDs[random.Next(cardIDs.Count)];
                    cardIDs.Remove(currentCardID);
                    DrawCard(currentCardID);
                    CheckScore();
                    RewriteInfoMain();
                    if (PublicVar.playerCurrent < 2)
                    {
                        PublicVar.playerCurrent++;
                    }
                    else
                    {
                        PublicVar.playerCurrent = 0;
                    }
                    Thread.Sleep(1000);
                }


                // Taking a card
                while (answerAnotherCardLooping == true)
                {
                    if (PublicVar.playerCurrent == 0)  // Dealer's logic
                    { 
                        if (PublicVar.playerTotal[0] < 21 && 
                           (PublicVar.playerTotal[0] < PublicVar.playerTotal[1] || PublicVar.playerTotal[0] < PublicVar.playerTotal[2] || PublicVar.playerTotal[0] < 12 ))
                        {
                            answerAnotherCard = "yes";
                        }
                        else 
                        { 
                            answerAnotherCard = "no";
                        }
                    }
                    else  // Asking players for cards
                    {
                        Console.Write(" {0}, do you want to take another card? (yes/no): ", PublicVar.playerName[PublicVar.playerCurrent]);
                        answerAnotherCard = Console.ReadLine();
                    }
                    switch (answerAnotherCard) 
                    {
                        case "yes":
                            answerAnotherCard = "";
                            currentCardID = cardIDs[random.Next(cardIDs.Count)];
                            cardIDs.Remove(currentCardID);
                            DrawCard(currentCardID);
                            CheckScore();
                            RewriteInfoMain();
                            Console.Write(" {0}, your card is {1}", PublicVar.playerName[PublicVar.playerCurrent], PublicVar.currentCardName);
                            if (PublicVar.playerTotal[PublicVar.playerCurrent] > 21)  // Checks if player busted
                            {
                                Console.Write($"\n {PublicVar.playerName[PublicVar.playerCurrent]} busted!");
                                Dots();
                                RewriteInfoMain();
                                if (PublicVar.playerCurrent == 0)
                                {
                                    dealerStandsNow = false;
                                }
                                goto case "no";
                            }
                            else
                            {
                                Dots();
                                RewriteInfoMain();
                            }

                            if ((playerPlaying.IndexOf(PublicVar.playerCurrent) + 1) < playerPlaying.Count)
                            {
                                PublicVar.playerCurrent = playerPlaying[playerPlaying.IndexOf(PublicVar.playerCurrent) + 1];
                            }
                            else
                            {
                                PublicVar.playerCurrent = playerPlaying[0];
                            }
                            break;

                        case "no":
                            answerAnotherCard = "";
                            int playerCurrentIndex = playerPlaying.IndexOf(PublicVar.playerCurrent);
                            playerPlaying.Remove(PublicVar.playerCurrent);
                            if (playerCurrentIndex == 0 && dealerStandsNow == true)  // In case the dealer is the only one standing for 2 loops
                            {
                                Console.Write(" Dealer stands!");
                                Dots();
                                dealerStandsNow = false;
                            }

                            if (playerPlaying.Count == 0)
                            {
                                answerAnotherCardLooping = false;
                                PublicVar.roundFinished = true;
                            }
                            else if (playerCurrentIndex < playerPlaying.Count)
                            {
                                PublicVar.playerCurrent = playerPlaying[playerCurrentIndex];
                            }
                            else
                            {
                                PublicVar.playerCurrent = playerPlaying[0];
                            }
                            RewriteInfoMain();
                            break;

                        default:
                            RewriteInfoMain();
                            Console.WriteLine(" Answer unrecognized.");
                            break;
                    }
                }


                // Checking who won
                if (PublicVar.playerTotal[0] <= 21 && 
                   (PublicVar.playerTotal[0] > PublicVar.playerTotal[1] || PublicVar.playerTotal[1] > 21) && 
                   (PublicVar.playerTotal[0] > PublicVar.playerTotal[2] || PublicVar.playerTotal[2] > 21))
                {
                    PublicVar.player0Wins++; 
                    RewriteInfoMain();
                    Console.Write(" The house won!");
                    Dots();
                }
                else if ((PublicVar.playerTotal[1] <= 21 && PublicVar.playerTotal[2] <= 21) &&
                        ((PublicVar.playerTotal[1] > PublicVar.playerTotal[0] && PublicVar.playerTotal[2] > PublicVar.playerTotal[0]) || PublicVar.playerTotal[0] > 21))
                {
                    PublicVar.player1Wins++;
                    PublicVar.player2Wins++;
                    RewriteInfoMain();
                    Console.Write($" {PublicVar.playerName[1]} and {PublicVar.playerName[2]} won!");
                    Dots();
                }
                else if (PublicVar.playerTotal[1] <= 21 &&
                        (PublicVar.playerTotal[1] > PublicVar.playerTotal[0] || PublicVar.playerTotal[0] > 21))
                {
                    PublicVar.player1Wins++;
                    RewriteInfoMain();
                    Console.Write($" {PublicVar.playerName[1]} won!");
                    Dots();
                }
                else if (PublicVar.playerTotal[2] <= 21 &&
                        (PublicVar.playerTotal[2] > PublicVar.playerTotal[0] || PublicVar.playerTotal[0] > 21))
                {
                    PublicVar.player2Wins++;
                    RewriteInfoMain();
                    Console.Write($" {PublicVar.playerName[2]} won!");
                    Dots();
                }
                else
                {
                    Console.Write(" Tie!");
                    Dots();
                }


                // Another round?
                Console.Write("\n Do you want to play another round? (yes/no): ");
                string answerAnotherRound = Console.ReadLine();
                bool answerAnotherRoundLooping = true;
                while (answerAnotherRoundLooping == true)
                {
                    switch (answerAnotherRound)
                    {
                        case "yes":
                            answerAnotherRoundLooping = false;
                            // Increases number of round + starting player, empties cards and scores
                            PublicVar.roundCurrent++;
                            PublicVar.playerStarting = (PublicVar.playerStarting < 2) ? (PublicVar.playerStarting + 1) : 0;
                            PublicVar.playerScore[0] = PublicVar.playerScore[1] = PublicVar.playerScore[2] = PublicVar.playerAce[0] = PublicVar.playerAce[1] = PublicVar.playerAce[2] = 0;
                            PublicVar.cardPlayer[0] = PublicVar.cardPlayer[1] = PublicVar.cardPlayer[2] = PublicVar.cardPlayer[3] =
                            PublicVar.cardPlayer[4] = PublicVar.cardPlayer[5] = PublicVar.cardPlayer[6] = PublicVar.cardPlayer[7] =
                            PublicVar.cardPlayer[8] = PublicVar.cardPlayer[9] = PublicVar.cardPlayer[10] = PublicVar.cardPlayer[11] = PublicVar.cardPlayer[12] = PublicVar.cardPlayer[13] = "";
                            PublicVar.hiddenFirstCard = true;
                            break;
                        case "no":
                            answerAnotherRoundLooping = false;
                            gameRepeat = false;
                            Console.Write(" Remember: the house always wins!");
                            Dots();
                            Console.WriteLine();
                            break;
                        default:
                            RewriteInfoMain();
                            Console.Write(" Answer unrecognized. One more round? (yes/no): ");
                            answerAnotherRound = Console.ReadLine();
                            break;
                    }
                }
            }
        }

        static void RewriteInfoMain()
        {
            Console.Clear();
            Console.WriteLine(" Welcome to the game of Blackjack.\n In this game 2 players try to collect cards worth 21 points without going over.\n");
            Console.WriteLine(" Round {0}! \n Current wins: Dealer - {1}, {4} - {2}, {5} - {3}.\n",
                              PublicVar.roundCurrent, PublicVar.player0Wins, PublicVar.player1Wins, PublicVar.player2Wins, PublicVar.playerName[1], PublicVar.playerName[2]);
            if (PublicVar.roundFinished == false)  // Dealer's hidden card
            {
                Console.WriteLine(" {0}'s cards (the 1st card is hidden): \n {1} \n {2} \n {3} \n {4}", PublicVar.playerName[0], PublicVar.cardPlayer[0], PublicVar.cardPlayer[1],
                                                                                                        PublicVar.cardPlayer[2], PublicVar.cardPlayer[3]);
            }
            else
            {
                Console.WriteLine(" {0}'s cards: \n {1} \n {2} \n {3} \n {4}", PublicVar.playerName[0], PublicVar.cardPlayer[0], PublicVar.cardPlayer[12],
                                                                               PublicVar.cardPlayer[13], PublicVar.cardPlayer[3]);
            }
            Console.WriteLine(" {0}'s cards: \n {1} \n {2} \n {3} \n {4}", PublicVar.playerName[1], PublicVar.cardPlayer[4], PublicVar.cardPlayer[5],
                                                                           PublicVar.cardPlayer[6], PublicVar.cardPlayer[7]);
            Console.WriteLine(" {0}'s cards: \n {1} \n {2} \n {3} \n {4}", PublicVar.playerName[2], PublicVar.cardPlayer[8], PublicVar.cardPlayer[9],
                                                                           PublicVar.cardPlayer[10], PublicVar.cardPlayer[11]);  // Prints graphics of cards
        }

        static void DrawCard(int currentCardID)  // Generates cards
        {
            int cardSuit = Convert.ToInt32(currentCardID / 100);
            int cardRank = currentCardID;
            while (cardRank > 99)
            {
                cardRank -= 100;
            }
            PublicVar.currentCardName = PublicVar.cardName[cardRank] + " of " + PublicVar.cardSuitName[cardSuit];


            PublicVar.cardPlayer[PublicVar.playerCurrent * 4] += " ╔═══╗";  // Creates graphics of cards
            if (PublicVar.playerCurrent == 0)
            {
                if (PublicVar.hiddenFirstCard == true)
                {
                    PublicVar.cardPlayer[1] += " ║XXX║";
                    PublicVar.cardPlayer[12] += $" ║{PublicVar.cardRank[cardRank]}{PublicVar.cardSuitSymbol[cardSuit]}║";
                }
                else
                {
                    PublicVar.cardPlayer[1] += $" ║{PublicVar.cardRank[cardRank]}{PublicVar.cardSuitSymbol[cardSuit]}║";
                    PublicVar.cardPlayer[12] += $" ║{PublicVar.cardRank[cardRank]}{PublicVar.cardSuitSymbol[cardSuit]}║";
                }
            }
            else
            {
                PublicVar.cardPlayer[PublicVar.playerCurrent * 4 + 1] += $" ║{PublicVar.cardRank[cardRank]}{PublicVar.cardSuitSymbol[cardSuit]}║";
            }
            if (PublicVar.playerCurrent == 0)
            {
                if (PublicVar.hiddenFirstCard == true)
                {
                    PublicVar.cardPlayer[2] += " ║XXX║";
                    PublicVar.cardPlayer[13] += " ║   ║";
                    PublicVar.hiddenFirstCard = false;
                }
                else
                {

                    PublicVar.cardPlayer[2] += " ║   ║";
                    PublicVar.cardPlayer[13] += " ║   ║";
                }
            }
            else
            {
                PublicVar.cardPlayer[PublicVar.playerCurrent * 4 + 2] += " ║   ║";
            }
            PublicVar.cardPlayer[PublicVar.playerCurrent * 4 + 3] +=   " ╚═══╝";


            if (cardRank < 12)  // Adds score and aces
            {
                PublicVar.playerScore[PublicVar.playerCurrent] += PublicVar.cardScore[cardRank];
            }
            else
            {
                PublicVar.playerAce[PublicVar.playerCurrent]++;
            }
        }

        static void CheckScore()  // Checks total score
        {
            int maxi = PublicVar.playerAce[PublicVar.playerCurrent];
            for (int i = PublicVar.playerAce[PublicVar.playerCurrent]; i >= 0; i--)
            {
                PublicVar.playerTotal[PublicVar.playerCurrent] = PublicVar.playerScore[PublicVar.playerCurrent] + i * 11 + (maxi - i);
                if (PublicVar.playerTotal[PublicVar.playerCurrent] <= 21)
                {
                    break;
                }
            }
        }

        static void Dots()
        {
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
        }
    }
}
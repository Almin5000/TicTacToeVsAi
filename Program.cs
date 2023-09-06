using System;
using System.Linq;

namespace TicTacToe
{
    class Program
    {
        static Tuple<int, int> aiMove = null;
        static char[,] Board = new char[3, 3];
        static bool playerHasMoved = false;
        static int playerWins = 0;
        static int playerLosses = 0;
        static int draws = 0;
        static int hardAiWins = 0;
        static Random random = new Random();
        static bool isHardAI = true;

        public static void Main(string[] args)
        {

            while (true)
            {
                InitializeBoard();
                bool player1Turn = true;
                int row, col;

                while (true)
                {
                    PrintBoard();

                    if (IsGameOver())
                    {
                        char winner = GetWinner();
                        Console.WriteLine(GetGameOutcome(winner));
                        UpdateScore(winner);
                        DisplayScores();
                        Console.Write("PRESS 'R' TO PLAY AGAIN, OR 'Q' TO QUIT: ");
                        string input = Console.ReadLine();
                        if (input.ToLower() == "q")
                        {
                            // Exit the game if the player presses 'Q'
                            Environment.Exit(0);
                        }
                        else if (input.ToLower() != "r")
                        {
                            // If neither 'R' nor 'Q' is pressed, continue the game
                            continue;
                        }
                        InitializeBoard();
                        player1Turn = true;
                        break;
                    }

                    Console.WriteLine(player1Turn ? "Your turn (X)" : "Hard AI's turn (O)");

                    if (player1Turn)
                    {
                        Console.Write("Enter row (0-2): ");
                        if (!int.TryParse(Console.ReadLine(), out col) || col < 0 || col > 2)
                        {
                            Console.WriteLine("Invalid input. Please enter a number (0-2) for the row.");
                            Thread.Sleep(1500);
                            continue;
                        }
                        Console.Write("Enter column (0-2): ");
                        if (!int.TryParse(Console.ReadLine(), out row) || row < 0 || row > 2)
                        {
                            Console.WriteLine("Invalid input. Please enter a number (0-2) for the column.");
                            Thread.Sleep(1500);
                            continue;
                        }
                    }
                    else
                    {
                        aiMove = GetBestMove();
                        row = aiMove.Item1;
                        col = aiMove.Item2;
                    }

                    if (IsValidMove(row, col))
                    {
                        Board[row, col] = player1Turn ? 'X' : 'O';
                        player1Turn = !player1Turn;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Try again.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        static void InitializeBoard()
        {
            for (var i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Board[i, j] = ' ';
                }
            }
        }

        static void PrintBoard()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Tic Tac Toe against the Hard AI!");
            Console.WriteLine();

            Console.WriteLine("      0       1       2");
            for (int row = 0; row < 3; row++)
            {
                Console.WriteLine("  +-------+-------+-------+");
                Console.Write(row + " |");
                for (int col = 0; col < 3; col++)
                {
                    Console.Write(" " + " " + " " + Board[row, col] + "   |");
                }
                Console.WriteLine();
            }
            Console.WriteLine("  +-------+-------+-------+");
            Console.WriteLine();

            // Display AI move
            if (!IsGameOver() && aiMove != null)
            {
                Console.WriteLine(isHardAI ? "Hard AI chooses row " + aiMove.Item1 + ", column " + aiMove.Item2 : "Hard AI chooses row " + aiMove.Item1 + ", column " + aiMove.Item2);
            }

            // Add a separator line
            Console.WriteLine("-----");
        }

        static bool IsValidMove(int row, int col)
        {
            return row >= 0 && row < 3 && col >= 0 && col < 3 && Board[row, col] == ' ';
        }

        static bool IsGameOver()
        {
            return IsWin('X') || IsWin('O') || IsDraw();
        }

        static bool IsWin(char player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Board[i, 0] == player && Board[i, 1] == player && Board[i, 2] == player)
                    return true;
                if (Board[0, i] == player && Board[1, i] == player && Board[2, i] == player)
                    return true;
            }
            if (Board[0, 0] == player && Board[1, 1] == player && Board[2, 2] == player)
                return true;
            if (Board[0, 2] == player && Board[1, 1] == player && Board[2, 0] == player)
                return true;
            return false;
        }

        static bool IsDraw()
        {
            return !Board.Cast<char>().Any(cell => cell == ' ') && !IsWin('X') && !IsWin('O');
        }

        static Tuple<int, int> GetBestMove()
        {
            Console.WriteLine("AI Thinking...");
            Thread.Sleep(1500);

            Tuple<int, int> bestMove = null;
            int bestScore = int.MinValue;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (Board[row, col] == ' ')
                    {
                        Board[row, col] = 'O';
                        int score = Minimax(Board, 0, false);
                        Board[row, col] = ' ';

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = Tuple.Create(row, col);
                        }
                    }
                }
            }

            return bestMove;
            //turn GetRandomMove(); // Change to use the random move function for Easy AI
        }

        static int Minimax(char[,] board, int depth, bool isMaximizing)
        {
            if (IsWin('O'))
                return 1;
            if (IsWin('X'))
                return -1;
            if (IsDraw())
                return 0;

            int score;
            if (isMaximizing)
            {
                score = int.MinValue;
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (board[row, col] == ' ')
                        {
                            board[row, col] = 'O';
                            score = Math.Max(score, Minimax(board, depth + 1, false));
                            board[row, col] = ' ';
                        }
                    }
                }
            }
            else
            {
                score = int.MaxValue;
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (board[row, col] == ' ')
                        {
                            board[row, col] = 'X';
                            score = Math.Min(score, Minimax(board, depth + 1, true));
                            board[row, col] = ' ';
                        }
                    }
                }
            }
            return score;
        }

        static void UpdateScore(char winner)
        {
            if (winner == 'X')
                playerWins++;
            else if (winner == 'O')
                hardAiWins++;
            else
                draws++;
        }

        static void DisplayScores()
        {
            Console.WriteLine("Your Score: W-" + playerWins + "  L-" + hardAiWins + "  D-" + draws);
        }

        static Tuple<int, int> GetRandomMove()
        {
            int row, col;
            do
            {
                row = random.Next(0, 3);
                col = random.Next(0, 3);
            } while (!IsValidMove(row, col));
            return Tuple.Create(row, col);
        }

        //static void ResetScores()
        //{
        //    playerWins = 0;
        //    playerLosses = 0;
        //    draws = 0;
        //    hardAiWins = 0;
        //}

        static char GetWinner()
        {
            if (IsWin('X')) return 'X';
            if (IsWin('O')) return 'O';
            return 'D';
        }

        static string GetGameOutcome(char winner)
        {
            if (winner == 'X') return "You win!";
            if (winner == 'O') return "Hard AI wins!";
            return "It's a draw!";
        }
    }
}

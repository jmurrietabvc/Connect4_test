#nullable enable
using System;

public class GameController
{
    private GameBoard gameBoard;
    private Player currentPlayer;
    private Player player1;
    private Player player2;

    public GameController()
    {
        gameBoard = new GameBoard();
        player1 = new HumanPlayer("Player 1");
        player2 = new HumanPlayer("Player 2");
        currentPlayer = player1;
    }

    public void StartGame()
    {
        Console.WriteLine("Connect Four");
        Console.WriteLine();

        ChooseGameMode();

        bool gameover = false;

        while (!gameover)
        {
            currentPlayer.MakeMove(gameBoard);
            gameBoard.DisplayBoard();

            if (gameBoard.CheckWin(currentPlayer.GetToken()))
            {
                Console.WriteLine($"{currentPlayer.GetName()} wins!");
                gameover = true;
            }
            else if (gameBoard.IsBoardFull())
            {
                Console.WriteLine("It's a draw!");
                gameover = true;
            }
            else
            {
                currentPlayer = (currentPlayer == player1) ? player2 : player1;
            }
        }
    }

    private void ChooseGameMode()
    {
        Console.WriteLine("Choose game mode:");
        Console.WriteLine("1. Play against the computer");
        Console.WriteLine("2. Play with two human players");

        int choice;
        while (true)
        {
            Console.Write("Enter your choice (1 or 2): ");
            if (int.TryParse(Console.ReadLine(), out choice) && (choice == 1 || choice == 2))
                break;
            Console.WriteLine("Invalid input. Please enter 1 or 2.");
        }

        if (choice == 1)
        {
            player2 = new ComputerPlayer();
        }
        else
        {
            Console.WriteLine("Player 1, enter your name: ");
            string name1 = Console.ReadLine();
            player1 = new HumanPlayer(name1);

            Console.WriteLine("Player 2, enter your name: ");
            string name2 = Console.ReadLine();
            player2 = new HumanPlayer(name2);
        }
    }

    public abstract class Player
    {
        public abstract void MakeMove(GameBoard gameBoard);
        public abstract char GetToken();
        public abstract string GetName();
    }

    public class HumanPlayer : Player
    {
        private string name;

        public HumanPlayer(string playerName)
        {
            name = playerName;
        }

        public override void MakeMove(GameBoard gameBoard)
        {
            int column = -1;

            while (column < 0 || column >= GameBoard.Columns || gameBoard.IsColumnFull(column))
            {
                Console.Write($"{name}, enter the column number to place your token (0-6): ");
                int.TryParse(Console.ReadLine(), out column);
            }

            gameBoard.PlaceToken(column, GetToken());
        }

        public override char GetToken()
        {
            return 'X';
        }

        public override string GetName()
        {
            return name;
        }
    }

    public class ComputerPlayer : Player
    {
        private System.Random random;

        public ComputerPlayer()
        {
            random = new System.Random();
        }

        public override void MakeMove(GameBoard gameBoard)
        {
            int column;

            do
            {
                column = random.Next(0, GameBoard.Columns);
            }
            while (gameBoard.IsColumnFull(column));

            Console.WriteLine($"Computer chooses column {column}");
            gameBoard.PlaceToken(column, GetToken());
        }

        public override char GetToken()
        {
            return 'O';
        }

        public override string GetName()
        {
            return "Computer";
        }
    }

    public class GameBoard
    {
        private const int Rows = 6;
        public const int Columns = 7;
        private char[,] board;

        public GameBoard()
        {
            board = new char[Rows, Columns];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    board[row, col] = '-';
                }
            }
        }

        public void DisplayBoard()
        {
            Console.WriteLine("Connect Four");
            Console.WriteLine();

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    Console.Write(board[row, col] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public bool IsColumnFull(int column)
        {
            return board[0, column] != '-';
        }

        public bool IsBoardFull()
        {
            for (int col = 0; col < Columns; col++)
            {
                if (!IsColumnFull(col))
                    return false;
            }

            return true;
        }

        public void PlaceToken(int column, char token)
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (board[row, column] == '-')
                {
                    board[row, column] = token;
                    break;
                }
            }
        }

        public bool CheckWin(char token)
        {
            // Check horizontal
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (board[row, col] == token &&
                        board[row, col + 1] == token &&
                        board[row, col + 2] == token &&
                        board[row, col + 3] == token)
                    {
                        return true;
                    }
                }
            }

            // Check vertical
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (board[row, col] == token &&
                        board[row + 1, col] == token &&
                        board[row + 2, col] == token &&
                        board[row + 3, col] == token)
                    {
                        return true;
                    }
                }
            }

            // Check diagonal (down-right)
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (board[row, col] == token &&
                        board[row + 1, col + 1] == token &&
                        board[row + 2, col + 2] == token &&
                        board[row + 3, col + 3] == token)
                    {
                        return true;
                    }
                }
            }

            // Check diagonal (up-right)
            for (int row = 3; row < Rows; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (board[row, col] == token &&
                        board[row - 1, col + 1] == token &&
                        board[row - 2, col + 2] == token &&
                        board[row - 3, col + 3] == token)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class ConnectFourGame
    {
        public static void Main(string[] args)
        {
            GameController gameController = new GameController();
            gameController.StartGame();
        }
    }
}
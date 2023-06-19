using System;

public class GameController
{
    private GameBoard gameBoard;
    private Player currentPlayer;

    public GameController()
    {
        gameBoard = new GameBoard();
        currentPlayer = new HumanPlayer();
    }

    public void StartGame()
    {
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
                currentPlayer = (currentPlayer is HumanPlayer) ? new ComputerPlayer() : new HumanPlayer();
            }
        }
    }
}

public class GameBoard
{
    private const int Rows = 6;
    private const int Columns = 7;
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

public abstract class Player
{
    public abstract void MakeMove(GameBoard gameBoard);
    public abstract char GetToken();
    public abstract string GetName();
}

public class HumanPlayer : Player
{
    public override void MakeMove(GameBoard gameBoard)
    {
        int column = -1;

        while (column < 0 || column >= GameBoard.Columns || gameBoard.IsColumnFull(column))
        {
            Console.Write("Enter the column number to place your token (0-6): ");
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
        return "Human";
    }
}

public class ComputerPlayer : Player
{
    private Random random;

    public ComputerPlayer()
    {
        random = new Random();
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

public class ConnectFourGame
{
    public static void Main(string[] args)
    {
        GameController gameController = new GameController();
        gameController.StartGame();
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace DamkaForm
{
    // $G$ CSS-003 (-2) Bad readonly members variable name (should be in the form of r_PamelCase).
    public class Player
    {
        private String m_PlayerName = null;
        private readonly Piece.e_PieceType m_PlayerPiece;
        private int m_NumberOfPieces = 0;
        private int m_Points = 0;
        private readonly bool m_IsComputer;
        private List<Point[]> m_OptionalAnotherJumps = null;

        public Player(String i_PlayerName, Piece.e_PieceType i_PlayerPiece, bool i_IsComputer)
        {
            PlayerName = i_PlayerName;
            m_PlayerPiece = i_PlayerPiece;
            m_IsComputer = i_IsComputer;
        }
        // $G$ CSS-027 (-3) Spaces are not kept as required between methods.
        public String PlayerName
        {
            get { return m_PlayerName; }
            set { m_PlayerName = value; }
        }
        public Piece.e_PieceType PlayerPiece
        {
            get { return m_PlayerPiece; }
        }
        public List<Point[]> OptionalAnotherJumps
        {
            get { return m_OptionalAnotherJumps; }
            set { m_OptionalAnotherJumps = value; } // TODO : levade sheze nahon.
        }
        public bool IsComputer
        {
            get { return m_IsComputer; }
        }
        // $G$ DSN-002 (-5) You should not make UI calls from your logic classes. 
        // $G$ DSN-001 (0) This method should belong to the UI class
        public static String GetName(String i_FirstName = null)
        {
            bool isValid = false;
            String name = null;
            while (!isValid)
            {
                Console.WriteLine("Enter your name: ");
                name = Console.ReadLine();
                if (!isValidName(name))
                {
                    Console.WriteLine("Invalid input! Only letters (no spaces or any other characters) and must be up to 20 characters");
                }
                else if (isTheSameNameAsTheOtherPlayer(name, i_FirstName))
                {
                    Console.WriteLine("Invalid input! You must Choose a diffrent.");
                }
                else
                {
                    isValid = true;
                }
            }

            return name;
        }
        private static bool isTheSameNameAsTheOtherPlayer(String i_Name, String i_FirstName)
        {
            return i_Name == i_FirstName || i_Name == "Computer";
        }
        private static bool isValidName(String i_Name)
        {
            bool isValid = true;
            // $G$ CSS-027 (-1) Missing blank line.
            if (i_Name.Length > 20)
            {
                isValid = false;
            }
            else
            {
                if (!allCharectersIsLetters(i_Name))
                {
                    isValid = false;
                }
            }

            return isValid;
        }
        public void MakeMove(Board i_Board, out bool o_IsQuitInput,
            out Point o_From, out Point o_To, bool i_AnotherJump, List<Point[]> i_NextJumpMove)
        {
            if (m_IsComputer)
            {
                if (i_AnotherJump)
                {
                    o_From = i_NextJumpMove[0][0];
                    o_To = i_NextJumpMove[0][1];
                }
                else
                {
                    GenerateMove(out o_From, out o_To, i_Board);
                }
                o_IsQuitInput = false;
            }
            else
            {
                getMoveChoice(out o_From, out o_To, i_Board, out o_IsQuitInput, i_AnotherJump, i_NextJumpMove);
            }

            if (!o_IsQuitInput)
            {
                ExecuteMove(o_From, o_To, i_Board);
            }
            else
            {
                //o_IsJumpMove = false;
            }
        }
        private static bool allCharectersIsLetters(String i_Name)
        {
            bool result = true;
            // $G$ CSS-027 (-1) Missing blank line.
            for (int i = 0; i < i_Name.Length; i++)
            {
                if (!Char.IsLetter(i_Name[i]))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
        private void getMoveChoice(out Point o_From, out Point o_To, Board i_Board, out bool o_IsQuitInput, bool i_AnotherJump, List<Point[]> i_NextJump)
        {
            bool isValid = false;
            String choice;
            o_To = null;
            o_From = null;
            o_IsQuitInput = false;
            while (!isValid)
            {
                choice = Console.ReadLine();
                if (choice == "Q")
                {
                    o_IsQuitInput = true;
                    return;
                }
                else
                {
                    o_IsQuitInput = false;
                    if (i_AnotherJump)
                    {
                        completeTheJump(choice, out o_From, out o_To, i_NextJump, out o_IsQuitInput);
                        isValid = true;
                    }
                    else if (MustJump(out List<Point[]> optionalJumps, i_Board))
                    {
                        getChoiceOfJump(choice, out o_From, out o_To, optionalJumps, out o_IsQuitInput);
                        isValid = true;
                    }
                    else if (!isValidChoice(choice, i_Board))
                    {
                        Console.WriteLine("Invalid step! Please try again.");
                    }
                    else
                    {
                        isValid = true;
                        converteLettersToPoint(choice.Split('>'), ref o_From, ref o_To);
                    }
                }
            }

        }

        private void getChoiceOfJump(String i_choice, out Point o_From, out Point o_To, List<Point[]> i_optionalJumps, out bool o_IsQuitInput)
        {
            o_From = null;
            o_To = null;
            o_IsQuitInput = false;
            bool isValid = false;

            while (!isValid)
            {
                if (i_choice == "Q")
                {
                    o_IsQuitInput = true;
                    return;
                }
                for (int i = 0; i < i_optionalJumps.Count; i++)
                {
                    String optionalJumpString = Point.convertPointsToString(i_optionalJumps[i][0], i_optionalJumps[i][1]);
                    if (i_choice.Equals(optionalJumpString))
                    {
                        o_From = i_optionalJumps[i][0];
                        o_To = i_optionalJumps[i][1];
                        isValid = true;
                        break;
                    }
                }
                if (!isValid)
                {
                    Console.WriteLine("You must jump, please try again!");
                    i_choice = Console.ReadLine();
                }
            }
        }
        public bool IsChoiceInList(Point i_From, Point i_To, List<Point[]> i_ListOfPoints)
        {
            bool isValid = false;

            for (int i = 0; i < i_ListOfPoints.Count; i++)
            {
                if (i_From.IsEqualPoints(i_ListOfPoints[i][0]) && i_To.IsEqualPoints(i_ListOfPoints[i][1]))
                {
                    isValid = true;
                    break;
                }
            }

            return isValid;
        }
        public bool MustJump(out List<Point[]> o_OptionalJumpsRes, Board i_Board)
        {
            o_OptionalJumpsRes = new List<Point[]>();

            for (int i = 0; i < i_Board.Size; i++)
            {
                for (int j = 0; j < i_Board.Size; j++)
                {
                    if (isPlayerPiece(i_Board.GameBoard[i, j].PieceType))
                    {
                        Point jumpFrom = new Point(i, j);
                        CheckIfCanJumpAndMakeList(i_Board, jumpFrom, out List<Point[]> currentOptionalList);
                        if (currentOptionalList.Count > 0)
                        {
                            o_OptionalJumpsRes.AddRange(currentOptionalList);
                        }
                    }
                }
            }

            return (o_OptionalJumpsRes.Count > 0);
        }

        // $G$ CSS-013 (-2) Bad parameter name (nextJump should be in the form of i_PascalCase).
        // $G$ DSN-003 (-5) This method is too long. 
        private void completeTheJump(String i_choice, out Point o_From, out Point o_To, List<Point[]> nextJump, out bool o_IsQuitInput)
        {
            o_From = null;
            o_To = null;
            o_IsQuitInput = false;
            Point from1 = nextJump[0][0];
            Point to1 = nextJump[0][1];
            bool isValid = false;
            String move = Point.convertPointsToString(from1, to1);

            if (nextJump.Count == 1)
            {
                while (!isValid)
                {
                    if (i_choice == "Q")
                    {
                        o_IsQuitInput = true;
                        return;
                    }
                    if (i_choice.Equals(move))
                    {
                        o_From = from1;
                        o_To = to1;
                        isValid = true;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Invalid Step, please jump -> {0}", move));
                        i_choice = Console.ReadLine();
                    }
                }
            }
            else if (nextJump.Count == 2)
            {
                Point from2 = nextJump[1][0];
                Point to2 = nextJump[1][1];
                String move2 = Point.convertPointsToString(from2, to2);
                while (!isValid)
                {
                    if (i_choice.Equals(move))
                    {
                        isValid = true;
                        o_From = from1;
                        o_To = to1;
                    }
                    else if (i_choice.Equals(move2))
                    {
                        isValid = true;
                        o_From = from2;
                        o_To = to2;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Invalid Step, please jump -> {0} or -> {1}", move, move2));
                        i_choice = Console.ReadLine();
                    }
                }
            }
            else // count=3
            {
                Point from2 = nextJump[1][0];
                Point to2 = nextJump[1][1];
                String move2 = Point.convertPointsToString(from2, to2);
                Point from3 = nextJump[2][0];
                Point to3 = nextJump[2][1];
                String move3 = Point.convertPointsToString(from3, to3);
                while (!isValid)
                {
                    if (i_choice.Equals(move))
                    {
                        isValid = true;
                        o_From = from1;
                        o_To = to1;
                    }
                    else if (i_choice.Equals(move2))
                    {
                        isValid = true;
                        o_From = from2;
                        o_To = to2;
                    }
                    else if (i_choice.Equals(move3))
                    {
                        isValid = true;
                        o_From = from3;
                        o_To = to3;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Invalid Step, please jump -> {0} or -> {1} or -> {2}", move, move2, move3));
                        i_choice = Console.ReadLine();
                    }
                }
            }
        }
        private bool isValidChoice(String i_Choice, Board i_Board)
        {
            bool isValid = false;
            if (timesCharAtString(i_Choice, '>') == 1)
            {
                String[] moves = i_Choice.Split('>');
                if (isValidMove(moves, i_Board))
                {
                    isValid = true;
                }
            }

            return isValid;
        }
        private int timesCharAtString(String i_String, Char i_Char)
        {
            int res = 0;
            for (int i = 0; i < i_String.Length; i++)
            {
                if (i_Char == i_String[i])
                {
                    res++;
                }
            }

            return res;
        }
        private bool isValidMove(String[] i_Move, Board i_Board)
        {
            bool isValid = false;
            Point from = null, to = null;
            if (i_Move.Length == 2)
            {
                if (validLetters(i_Move[0][0], i_Move[0][1], i_Board.Size) && validLetters(i_Move[1][0], i_Move[1][1], i_Board.Size))
                {
                    converteLettersToPoint(i_Move, ref from, ref to);
                    if (IsLegalMove(from, to, i_Board))
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }
        private bool validLetters(Char i_FirstLetter, Char i_SecondLetter, int i_BoardSize)
        {
            return i_FirstLetter >= 'A' && i_FirstLetter <= 'A' + i_BoardSize - 1 && i_SecondLetter >= 'a' && i_SecondLetter <= 'a' + i_BoardSize - 1;
        }
        private void converteLettersToPoint(String[] i_Move, ref Point io_From, ref Point io_To)
        {
            io_From = new Point((i_Move[0][0] - 'A'), (i_Move[0][1] - 'a'));
            io_To = new Point((i_Move[1][0] - 'A'), (i_Move[1][1] - 'a'));
        }
        public bool IsLegalMove(Point i_From, Point i_To, Board i_Board)
        {
            bool isValid = false;
            // TODO : mabye delete TheMoveIsFromThePlayerSquare
            if ((isMoveDiagonally(i_From, i_To, i_Board) || IsJump(i_From, i_To, i_Board)))
            {
                isValid = true;
            }

            return isValid;
        }
        public bool TheMoveIsFromThePlayerSquare(Piece.e_PieceType i_PeiceType)
        {
            bool result = i_PeiceType == m_PlayerPiece;
            if (result == false)
            {
                if (m_PlayerPiece == Piece.e_PieceType.O && i_PeiceType == Piece.e_PieceType.U
                    || m_PlayerPiece == Piece.e_PieceType.X && i_PeiceType == Piece.e_PieceType.K)
                {
                    result = true;
                }
            }
            return result;
        }
        private bool isMoveDiagonally(Point i_From, Point i_To, Board i_Board)
        {
            bool isDiagonally = false;
            Piece.e_DirectionType direction = i_Board.GameBoard[i_From.X, i_From.Y].DirectionType;

            if (i_Board.GameBoard[i_To.X, i_To.Y].PieceType == Piece.e_PieceType.Empty)
            {
                if (direction == Piece.e_DirectionType.Up)
                {
                    if ((i_From.X - 1 == i_To.X && i_From.Y + 1 == i_To.Y) || (i_From.X - 1 == i_To.X && i_From.Y - 1 == i_To.Y))
                    {
                        isDiagonally = true;
                    }
                }
                else if (direction == Piece.e_DirectionType.Down)
                {
                    if ((i_From.X + 1 == i_To.X && i_From.Y - 1 == i_To.Y) || (i_From.X + 1 == i_To.X && i_From.Y + 1 == i_To.Y))
                    {
                        isDiagonally = true;
                    }
                }
                else if (direction == Piece.e_DirectionType.Both)
                {
                    if ((i_From.X - 1 == i_To.X && i_From.Y - 1 == i_To.Y) || (i_From.X + 1 == i_To.X && i_From.Y + 1 == i_To.Y) || (i_From.X + 1 == i_To.X && i_From.Y - 1 == i_To.Y) || (i_From.X - 1 == i_To.X && i_From.Y + 1 == i_To.Y))
                    {
                        isDiagonally = true;
                    }
                }
            }

            return isDiagonally;
        }
        // $G$ DSN-003 (-5) The code should be divided to methods. 
        public bool IsJump(Point i_From, Point i_To, Board i_Board)
        {
            bool makeJump = false;
            Piece.e_DirectionType direction = i_Board.GameBoard[i_From.X, i_From.Y].DirectionType;
            // $G$ CSS-027 (-1) Missing blank line.
            if (i_Board.GameBoard[i_To.X, i_To.Y].PieceType == Piece.e_PieceType.Empty)
            {
                if (direction == Piece.e_DirectionType.Up)
                {
                    if ((i_From.X - 2 == i_To.X && i_From.Y + 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X - 1, i_From.Y + 1].PieceType == Piece.e_PieceType.O ||
                        i_Board.GameBoard[i_From.X - 1, i_From.Y + 1].PieceType == Piece.e_PieceType.U))
                        ||
                       (i_From.X - 2 == i_To.X && i_From.Y - 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X - 1, i_From.Y - 1].PieceType == Piece.e_PieceType.O ||
                        i_Board.GameBoard[i_From.X - 1, i_From.Y - 1].PieceType == Piece.e_PieceType.U)))
                    {
                        makeJump = true;
                    }
                }
                else if (direction == Piece.e_DirectionType.Down)
                {
                    if ((i_From.X + 2 == i_To.X && i_From.Y - 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X + 1, i_From.Y - 1].PieceType == Piece.e_PieceType.X ||
                         i_Board.GameBoard[i_From.X + 1, i_From.Y - 1].PieceType == Piece.e_PieceType.K))
                         ||
                        (i_From.X + 2 == i_To.X && i_From.Y + 2 == i_To.Y &&
                         (i_Board.GameBoard[i_From.X + 1, i_From.Y + 1].PieceType == Piece.e_PieceType.X ||
                         i_Board.GameBoard[i_From.X + 1, i_From.Y + 1].PieceType == Piece.e_PieceType.K)))
                    {
                        makeJump = true;
                    }
                }
                else if (direction == Piece.e_DirectionType.Both && m_PlayerPiece == Piece.e_PieceType.O)
                {
                    if ((i_From.X - 2 == i_To.X && i_From.Y - 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X - 1, i_From.Y - 1].PieceType == Piece.e_PieceType.X ||
                         i_Board.GameBoard[i_From.X - 1, i_From.Y - 1].PieceType == Piece.e_PieceType.K))
                        ||
                        (i_From.X + 2 == i_To.X && i_From.Y + 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X + 1, i_From.Y + 1].PieceType == Piece.e_PieceType.X ||
                         i_Board.GameBoard[i_From.X + 1, i_From.Y + 1].PieceType == Piece.e_PieceType.K)) ||
                        (i_From.X + 2 == i_To.X && i_From.Y - 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X + 1, i_From.Y - 1].PieceType == Piece.e_PieceType.X ||
                         i_Board.GameBoard[i_From.X + 1, i_From.Y - 1].PieceType == Piece.e_PieceType.K)) ||
                        (i_From.X - 2 == i_To.X && i_From.Y + 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X - 1, i_From.Y + 1].PieceType == Piece.e_PieceType.X ||
                         i_Board.GameBoard[i_From.X - 1, i_From.Y + 1].PieceType == Piece.e_PieceType.K)))
                    {
                        makeJump = true;
                    }
                }
                else if (direction == Piece.e_DirectionType.Both && m_PlayerPiece == Piece.e_PieceType.X)
                {
                    if ((i_From.X - 2 == i_To.X && i_From.Y - 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X - 1, i_From.Y - 1].PieceType == Piece.e_PieceType.O ||
                         i_Board.GameBoard[i_From.X - 1, i_From.Y - 1].PieceType == Piece.e_PieceType.U))
                        ||
                        (i_From.X + 2 == i_To.X && i_From.Y + 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X + 1, i_From.Y + 1].PieceType == Piece.e_PieceType.O ||
                         i_Board.GameBoard[i_From.X + 1, i_From.Y + 1].PieceType == Piece.e_PieceType.U)) ||
                        (i_From.X + 2 == i_To.X && i_From.Y - 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X + 1, i_From.Y - 1].PieceType == Piece.e_PieceType.O ||
                         i_Board.GameBoard[i_From.X + 1, i_From.Y - 1].PieceType == Piece.e_PieceType.U)) ||
                        (i_From.X - 2 == i_To.X && i_From.Y + 2 == i_To.Y &&
                        (i_Board.GameBoard[i_From.X - 1, i_From.Y + 1].PieceType == Piece.e_PieceType.O ||
                         i_Board.GameBoard[i_From.X - 1, i_From.Y + 1].PieceType == Piece.e_PieceType.U)))
                    {
                        makeJump = true;
                    }
                }
            }

            return makeJump;
        }
        public void ExecuteMove(Point i_From, Point i_To, Board i_Board)
        {
            if (!IsJump(i_From, i_To, i_Board))
            {
                i_Board.GameBoard[i_To.X, i_To.Y].PieceType = i_Board.GameBoard[i_From.X, i_From.Y].PieceType;
                i_Board.GameBoard[i_To.X, i_To.Y].DirectionType = i_Board.GameBoard[i_From.X, i_From.Y].DirectionType;
                i_Board.GameBoard[i_From.X, i_From.Y].PieceType = Piece.e_PieceType.Empty;
                i_Board.GameBoard[i_From.X, i_From.Y].DirectionType = Piece.e_DirectionType.Empty;
            }
            else
            {
                if (fromDownToUpRight(i_From, i_To))
                {
                    i_Board.GameBoard[i_From.X - 1, i_From.Y + 1].PieceType = Piece.e_PieceType.Empty;
                    i_Board.GameBoard[i_From.X - 1, i_From.Y + 1].DirectionType = Piece.e_DirectionType.Empty;
                }
                else if (fromDownToUpLeft(i_From, i_To))
                {
                    i_Board.GameBoard[i_From.X - 1, i_From.Y - 1].PieceType = Piece.e_PieceType.Empty;
                    i_Board.GameBoard[i_From.X - 1, i_From.Y - 1].DirectionType = Piece.e_DirectionType.Empty;

                }
                else if (fromUpToDownLeft(i_From, i_To))
                {
                    i_Board.GameBoard[i_From.X + 1, i_From.Y - 1].PieceType = Piece.e_PieceType.Empty;
                    i_Board.GameBoard[i_From.X + 1, i_From.Y - 1].DirectionType = Piece.e_DirectionType.Empty;
                }
                else
                {
                    i_Board.GameBoard[i_From.X + 1, i_From.Y + 1].PieceType = Piece.e_PieceType.Empty;
                    i_Board.GameBoard[i_From.X + 1, i_From.Y + 1].DirectionType = Piece.e_DirectionType.Empty;

                }
                i_Board.GameBoard[i_To.X, i_To.Y].PieceType = i_Board.GameBoard[i_From.X, i_From.Y].PieceType;
                i_Board.GameBoard[i_To.X, i_To.Y].DirectionType = i_Board.GameBoard[i_From.X, i_From.Y].DirectionType;
                i_Board.GameBoard[i_From.X, i_From.Y].PieceType = Piece.e_PieceType.Empty;
                i_Board.GameBoard[i_From.X, i_From.Y].DirectionType = Piece.e_DirectionType.Empty;
            }
        }
        private bool fromDownToUpLeft(Point i_From, Point i_To)
        {
            return i_From.X - 2 == i_To.X && i_From.Y - 2 == i_To.Y;
        }
        private bool fromDownToUpRight(Point i_From, Point i_To)
        {
            return i_From.X - 2 == i_To.X && i_From.Y + 2 == i_To.Y;
        }
        private bool fromUpToDownLeft(Point i_From, Point i_To)
        {
            return i_From.X + 2 == i_To.X && i_From.Y - 2 == i_To.Y;
        }
        public int NumberOfPieces
        {
            get { return m_NumberOfPieces; }
            set { m_NumberOfPieces = value; }
        }

        public void GenerateMove(out Point o_From, out Point o_To, Board i_Board)
        {
            List<Point[]> possiblePositions = createPossiblePositionsList(i_Board);
            choseNextMove(possiblePositions, out o_From, out o_To, i_Board);
        }

        private List<Point[]> createPossiblePositionsList(Board i_Board)
        {

            List<Point[]> possiblePositions = new List<Point[]>();

            for (int i = 0; i < i_Board.Size; i++)
            {
                for (int j = 0; j < i_Board.Size; j++)
                {
                    Point currentFrom = new Point(i, j);
                    if (i_Board.GameBoard[i, j].PieceType == Piece.e_PieceType.O || i_Board.GameBoard[i, j].PieceType == Piece.e_PieceType.U)
                    {
                        checkAndAddPossiblePositions(i_Board, currentFrom, possiblePositions, i_Board.GameBoard[i, j].PieceType);
                    }
                }
            }

            return possiblePositions;
        }
        private void checkAndAddPossiblePositions(Board i_Board, Point i_CurrentFrom, List<Point[]> io_PossiblePositions, Piece.e_PieceType i_PieceType)
        {
            Point currentTo = new Point(i_CurrentFrom.X + 1, i_CurrentFrom.Y - 1);
            checkLegalAndAddToList(i_Board, i_CurrentFrom, currentTo, io_PossiblePositions);
            Point currentTo1 = new Point(i_CurrentFrom.X + 1, i_CurrentFrom.Y + 1);
            checkLegalAndAddToList(i_Board, i_CurrentFrom, currentTo1, io_PossiblePositions);
            Point currentTo2 = new Point(i_CurrentFrom.X + 2, i_CurrentFrom.Y - 2);
            checkLegalAndAddToList(i_Board, i_CurrentFrom, currentTo2, io_PossiblePositions);
            Point currentTo3 = new Point(i_CurrentFrom.X + 2, i_CurrentFrom.Y + 2);
            checkLegalAndAddToList(i_Board, i_CurrentFrom, currentTo3, io_PossiblePositions);

            if (i_PieceType == Piece.e_PieceType.U)
            {
                Point currentTo4 = new Point(i_CurrentFrom.X - 1, i_CurrentFrom.Y - 1);
                checkLegalAndAddToList(i_Board, i_CurrentFrom, currentTo4, io_PossiblePositions);
                Point currentTo5 = new Point(i_CurrentFrom.X - 1, i_CurrentFrom.Y + 1);
                checkLegalAndAddToList(i_Board, i_CurrentFrom, currentTo5, io_PossiblePositions);
                Point currentTo6 = new Point(i_CurrentFrom.X - 2, i_CurrentFrom.Y - 2);
                checkLegalAndAddToList(i_Board, i_CurrentFrom, currentTo6, io_PossiblePositions);
                Point currentTo7 = new Point(i_CurrentFrom.X - 2, i_CurrentFrom.Y + 2);
                checkLegalAndAddToList(i_Board, i_CurrentFrom, currentTo7, io_PossiblePositions);
            }

        }
        private void checkLegalAndAddToList(Board i_Board, Point i_CurrentFrom, Point i_CurrentTo, List<Point[]> io_PossiblePositions)
        {
            if (isLegalPoint(i_CurrentTo, i_Board))
            {
                if (IsLegalMove(i_CurrentFrom, i_CurrentTo, i_Board))
                {
                    io_PossiblePositions.Add(new Point[] { i_CurrentFrom, i_CurrentTo });
                }
            }
        }
        // $G$ NTT-007 (-5) There's no need to re-instantiate the Random instance each time it is used.
        private void choseNextMove(List<Point[]> i_possoblePositions, out Point o_From, out Point o_To, Board i_Board)
        {
            o_From = null;
            o_To = null;

            int lengthOfList = i_possoblePositions.Count;
            if (lengthOfList != 0)
            {
                //if jump choose it!
                foreach (Point[] points in i_possoblePositions)
                {
                    if (IsJump(points[0], points[1], i_Board))
                    {
                        o_From = points[0];
                        o_To = points[1];

                        break;
                    }
                }

                if (o_From == null && o_To == null)
                {
                    int randomIndex = new Random().Next(lengthOfList);
                    o_From = i_possoblePositions[randomIndex][0];
                    o_To = i_possoblePositions[randomIndex][1];
                }
            }
        }
        // $G$ CSS-999 (-3) bool template should be written like this - isXXX at the beginning.
        public bool NoMovesLeft(Board i_Board)
        {
            bool playerCantPlay = true;
            for (int i = 0; i < i_Board.Size; i++)
            {
                for (int j = 0; j < i_Board.Size; j++)
                {
                    if ((isPlayerPiece(i_Board.GameBoard[i, j].PieceType)) && !isStuckPiece(new Point(i, j), i_Board))
                    {
                        playerCantPlay = false;
                        break;
                    }
                }

                if (!playerCantPlay)
                {
                    break;
                }
            }

            return playerCantPlay;
        }
        private bool isPlayerPiece(Piece.e_PieceType i_TypePeice)
        {
            return (i_TypePeice == m_PlayerPiece) ||
                ((m_PlayerPiece == Piece.e_PieceType.O && i_TypePeice == Piece.e_PieceType.U) ||
                (m_PlayerPiece == Piece.e_PieceType.X && i_TypePeice == Piece.e_PieceType.K));
        }
        private bool isStuckPiece(Point i_From, Board i_Board)
        {
            bool isStuckPiece = true;
            if (isMoveDiagonallyWithValidation(i_From, new Point(i_From.X - 1, i_From.Y - 1), i_Board) ||
                isMoveDiagonallyWithValidation(i_From, new Point(i_From.X - 1, i_From.Y + 1), i_Board) ||
                isMoveDiagonallyWithValidation(i_From, new Point(i_From.X + 1, i_From.Y - 1), i_Board) ||
                isMoveDiagonallyWithValidation(i_From, new Point(i_From.X + 1, i_From.Y + 1), i_Board) ||
                isJumpWithValidation(i_From, new Point(i_From.X - 2, i_From.Y - 2), i_Board) ||
                isJumpWithValidation(i_From, new Point(i_From.X - 2, i_From.Y + 2), i_Board) ||
                isJumpWithValidation(i_From, new Point(i_From.X + 2, i_From.Y - 2), i_Board) ||
                isJumpWithValidation(i_From, new Point(i_From.X + 2, i_From.Y + 2), i_Board))
            {
                isStuckPiece = false;
            }

            return isStuckPiece;
        }
        private bool isLegalPoint(Point i_To, Board i_Board)
        {
            return i_To.X >= 0 && i_To.X < i_Board.Size && i_To.Y >= 0 && i_To.Y < i_Board.Size;
        }
        private bool isMoveDiagonallyWithValidation(Point i_From, Point i_To, Board i_Board)
        {
            bool result = false;
            if (isLegalPoint(i_To, i_Board))
            {
                result = isMoveDiagonally(i_From, i_To, i_Board);
            }

            return result;
        }
        private bool isJumpWithValidation(Point i_From, Point i_To, Board i_Board)
        {
            bool result = false;
            if (isLegalPoint(i_To, i_Board))
            {
                result = IsJump(i_From, i_To, i_Board);
            }

            return result;
        }
        // $G$ DSN-004 (-5) Code Duplication!
        public bool CheckIfCanJumpAndMakeList(Board i_Board, Point i_jumpFrom, out List<Point[]> io_nextJumpMove)
        {
            bool result = false;
            io_nextJumpMove = new List<Point[]>();
            if (m_PlayerPiece == Piece.e_PieceType.X)
            {

                if (isLegalPoint(new Point(i_jumpFrom.X - 2, i_jumpFrom.Y - 2), i_Board))
                {
                    if (IsJump(i_jumpFrom, new Point(i_jumpFrom.X - 2, i_jumpFrom.Y - 2), i_Board))
                    {
                        result = true;
                        io_nextJumpMove.Add(new Point[] { i_jumpFrom, new Point(i_jumpFrom.X - 2, i_jumpFrom.Y - 2) });
                    }
                }

                if (isLegalPoint(new Point(i_jumpFrom.X - 2, i_jumpFrom.Y + 2), i_Board))
                {
                    if (IsJump(i_jumpFrom, new Point(i_jumpFrom.X - 2, i_jumpFrom.Y + 2), i_Board))
                    {
                        result = true;
                        io_nextJumpMove.Add(new Point[] { i_jumpFrom, new Point(i_jumpFrom.X - 2, i_jumpFrom.Y + 2) });
                    }

                }

                if (i_Board.GameBoard[i_jumpFrom.X, i_jumpFrom.Y].PieceType == Piece.e_PieceType.K)
                {
                    if (isLegalPoint(new Point(i_jumpFrom.X + 2, i_jumpFrom.Y - 2), i_Board))
                    {
                        if (IsJump(i_jumpFrom, new Point(i_jumpFrom.X + 2, i_jumpFrom.Y - 2), i_Board))
                        {
                            result = true;
                            io_nextJumpMove.Add(new Point[] { i_jumpFrom, new Point(i_jumpFrom.X + 2, i_jumpFrom.Y - 2) });
                        }
                    }

                    if (isLegalPoint(new Point(i_jumpFrom.X + 2, i_jumpFrom.Y + 2), i_Board))
                    {
                        if (IsJump(i_jumpFrom, new Point(i_jumpFrom.X + 2, i_jumpFrom.Y + 2), i_Board))
                        {
                            result = true;
                            io_nextJumpMove.Add(new Point[] { i_jumpFrom, new Point(i_jumpFrom.X + 2, i_jumpFrom.Y + 2) });
                        }
                    }
                }

            }
            else //player is O
            {

                if (isLegalPoint(new Point(i_jumpFrom.X + 2, i_jumpFrom.Y - 2), i_Board))
                {
                    if (IsJump(i_jumpFrom, new Point(i_jumpFrom.X + 2, i_jumpFrom.Y - 2), i_Board))
                    {
                        result = true;
                        io_nextJumpMove.Add(new Point[] { i_jumpFrom, new Point(i_jumpFrom.X + 2, i_jumpFrom.Y - 2) });
                    }
                }
                if (isLegalPoint(new Point(i_jumpFrom.X + 2, i_jumpFrom.Y + 2), i_Board))
                {
                    if (IsJump(i_jumpFrom, new Point(i_jumpFrom.X + 2, i_jumpFrom.Y + 2), i_Board))
                    {
                        result = true;
                        io_nextJumpMove.Add(new Point[] { i_jumpFrom, new Point(i_jumpFrom.X + 2, i_jumpFrom.Y + 2) });
                    }

                }

                if (i_Board.GameBoard[i_jumpFrom.X, i_jumpFrom.Y].PieceType == Piece.e_PieceType.U)
                {
                    if (isLegalPoint(new Point(i_jumpFrom.X - 2, i_jumpFrom.Y - 2), i_Board))
                    {
                        if (IsJump(i_jumpFrom, new Point(i_jumpFrom.X - 2, i_jumpFrom.Y - 2), i_Board))
                        {
                            result = true;
                            io_nextJumpMove.Add(new Point[] { i_jumpFrom, new Point(i_jumpFrom.X - 2, i_jumpFrom.Y - 2) });
                        }
                    }
                    if (isLegalPoint(new Point(i_jumpFrom.X - 2, i_jumpFrom.Y + 2), i_Board))
                    {
                        if (IsJump(i_jumpFrom, new Point(i_jumpFrom.X - 2, i_jumpFrom.Y + 2), i_Board))
                        {
                            result = true;
                            io_nextJumpMove.Add(new Point[] { i_jumpFrom, new Point(i_jumpFrom.X - 2, i_jumpFrom.Y + 2) });
                        }
                    }
                }
            }

            return result;
        }
        public void addPoints(int i_Points)
        {
            m_Points += i_Points;
        }
        public int Points
        {
            get { return m_Points; }
        }
    }

}

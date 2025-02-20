﻿using System;
using System.Collections.Generic;

namespace DamkaGameLogic
{
    public class Player
    {
        private String m_PlayerName = null;
        private readonly Piece.e_PieceType m_PlayerPiece;
        private int m_NumberOfPieces = 0;
        private int m_Points = 0;
        private readonly bool r_IsComputer;
        private List<Point[]> m_OptionalAnotherJumps = null;

        public Player(String i_PlayerName, Piece.e_PieceType i_PlayerPiece, bool i_IsComputer)
        {
            PlayerName = i_PlayerName;
            m_PlayerPiece = i_PlayerPiece;
            r_IsComputer = i_IsComputer;
        }
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
            set { m_OptionalAnotherJumps = value; }
        }
        public bool IsComputer
        {
            get { return r_IsComputer; }
        }
        public int NumberOfPieces
        {
            get { return m_NumberOfPieces; }
            set { m_NumberOfPieces = value; }
        }
        public int Points
        {
            get { return m_Points; }
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
        public bool IsLegalMove(Point i_From, Point i_To, Board i_Board)
        {
            bool isValid = false;

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
        public bool IsJump(Point i_From, Point i_To, Board i_Board)
        {
            bool makeJump = false;
            Piece.e_DirectionType direction = i_Board.GameBoard[i_From.X, i_From.Y].DirectionType;

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
        public bool IsNoMovesLeft(Board i_Board)
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
        public void AddPoints(int i_Points)
        {
            m_Points += i_Points;
        }
        public void SelectNextMove(out  Point o_FromPoint, out Point o_ToPoint)
        {
            o_FromPoint = m_OptionalAnotherJumps[0][0];
            o_ToPoint = m_OptionalAnotherJumps[0][1];
        }
    }

}

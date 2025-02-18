using System;

namespace DamkaForm
{
    public class Game
    {
        private Board m_Board;
        private Player m_FirstPlayer = null;
        private Player m_SecondPlayer = null;
        private Player m_CurrentPlayer = null;
        private bool m_GameOver = false;
        private Player m_Winner = null;

        public Game(int i_BoardSize, String i_FirstPlayerName, String i_SecondPlayerName)
        {
            m_Board = new Board(i_BoardSize);
            m_FirstPlayer = new Player(i_FirstPlayerName, Piece.e_PieceType.X, false);
            m_SecondPlayer = new Player(i_SecondPlayerName, Piece.e_PieceType.O, isComputer(i_SecondPlayerName));
            m_CurrentPlayer = m_FirstPlayer;
        }
        public Board Board
        {
            get { return m_Board; }
            set { m_Board = value; }
        }
        public Player FirstPlayer
        {
            get { return m_FirstPlayer; }
        }
        public Player SecondPlayer
        {
            get { return m_SecondPlayer; }
        }
        public Player CurrentPlayer
        {
            get { return m_CurrentPlayer; }
            set { m_CurrentPlayer = value; }
        }
        public Player Winner
        {
            get { return m_Winner; }
            set { m_Winner = value; }
        }
        public void UpdatePlayerPoints()
        {
            if (m_Winner != null)
            {
                if (m_Winner == m_FirstPlayer)
                {
                    m_FirstPlayer.AddPoints(Math.Abs(calcPoints(m_FirstPlayer) - calcPoints(m_SecondPlayer)));
                }
                else
                {
                    m_SecondPlayer.AddPoints(Math.Abs(calcPoints(m_SecondPlayer) - calcPoints(m_FirstPlayer)));
                }
            }
        }
        private int calcPoints(Player i_Player)
        {
            int counterPoints = 0;
            for (int i = 0; i < m_Board.Size; i++)
            {
                for (int j = 0; j < m_Board.Size; j++)
                {
                    if (m_Board.GameBoard[i, j].PieceType == i_Player.PlayerPiece)
                    {
                        counterPoints++;
                    }
                    else if (m_Board.GameBoard[i, j].PieceType == Piece.e_PieceType.U && i_Player.PlayerPiece == Piece.e_PieceType.O)
                    {
                        counterPoints += 4;
                    }
                    else if (m_Board.GameBoard[i, j].PieceType == Piece.e_PieceType.K && i_Player.PlayerPiece == Piece.e_PieceType.X)
                    {
                        counterPoints += 4;
                    }

                }
            }

            return counterPoints;
        }
        public void ChangeTurn()
        {
            if (m_CurrentPlayer.PlayerName == m_FirstPlayer.PlayerName)
            {
                m_CurrentPlayer = m_SecondPlayer;
            }
            else
            {
                m_CurrentPlayer = m_FirstPlayer;
            }
        }
        public void SetTheNumberOfPicesToThePlayers()
        {
            if (m_Board.Size == 6)
            {
                m_FirstPlayer.NumberOfPieces = 6;
                m_SecondPlayer.NumberOfPieces = 6;
            }
            else if (m_Board.Size == 8)
            {
                m_FirstPlayer.NumberOfPieces = 12;
                m_SecondPlayer.NumberOfPieces = 12;
            }
            else
            {
                m_FirstPlayer.NumberOfPieces = 20;
                m_SecondPlayer.NumberOfPieces = 20;
            }
        }
        public bool GameOver
        {
            get { return m_GameOver; }
            set { m_GameOver = value; }
        }
        public void UpdateWinnerPlayer()
        {
            if (m_CurrentPlayer.PlayerName == m_FirstPlayer.PlayerName)
            {
                m_Winner = m_SecondPlayer;
            }
            else
            {
                m_Winner = m_FirstPlayer;
            }
        }
        public void CheckGameStatus()
        {
            if (m_FirstPlayer.NumberOfPieces == 0 || m_SecondPlayer.NumberOfPieces == 0)
            {
                GameOver = true;
                m_Winner = m_FirstPlayer.NumberOfPieces == 0 ? m_SecondPlayer : m_FirstPlayer;
            }
            else if (m_FirstPlayer.IsNoMovesLeft(m_Board) && m_FirstPlayer == m_CurrentPlayer)
            {
                GameOver = true;
                m_Winner = m_SecondPlayer;
            }
            else if (m_SecondPlayer.IsNoMovesLeft(m_Board) && m_SecondPlayer == m_CurrentPlayer)
            {
                GameOver = true;
                m_Winner = m_FirstPlayer;
            }
            else if (m_FirstPlayer.IsNoMovesLeft(m_Board) && m_SecondPlayer.IsNoMovesLeft(m_Board))
            {
                GameOver = true;
            }
        }
        private bool isComputer(String i_PlayerName)
        {
            return i_PlayerName == "Computer";
        }
    }
}

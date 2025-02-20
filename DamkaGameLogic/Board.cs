namespace DamkaGameLogic
{
    public class Board
    {
        private readonly int m_Size;
        private readonly Piece[,] m_GameBoard;

        public Board(int i_Size)
        {
            m_Size = i_Size;
            m_GameBoard = new Piece[m_Size, m_Size];
            initializeBoard(Piece.e_PieceType.X, Piece.e_PieceType.O);
        }
        public Piece[,] GameBoard
        {
            get { return m_GameBoard; }
        }
        public int Size
        {
            get { return m_Size;}
        }
        private void initializeBoard(Piece.e_PieceType i_FirstPiece, Piece.e_PieceType i_SecondPiece)
        {
            makeEmptyBoard();
            for (int i = m_Size - 1; i > m_Size - (m_Size / 2); i--)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if ((i + j + 1) % 2 == 0)
                        m_GameBoard[i, j] = new Piece(i_FirstPiece, Piece.e_DirectionType.Up);
                }
            }
            for (int i = 0; i < (m_Size / 2) - 1; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if ((i + j + 1) % 2 == 0)
                    {
                        m_GameBoard[i, j] = new Piece(i_SecondPiece, Piece.e_DirectionType.Down);
                    }
                }
            }
        }
        private void makeEmptyBoard()
        {
            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    m_GameBoard[i, j] = new Piece(Piece.e_PieceType.Empty, Piece.e_DirectionType.Empty);
                }
            }
        }
        public void UpdateKingCase(Piece.e_PieceType i_PlayerPiece)
        {
            if (i_PlayerPiece == Piece.e_PieceType.X)
            {
                for (int i = 0; i < m_Size; i++)
                {
                    if (m_GameBoard[0, i].PieceType == Piece.e_PieceType.X)
                    {
                        m_GameBoard[0, i].PieceType = Piece.e_PieceType.K;
                        m_GameBoard[0, i].DirectionType = Piece.e_DirectionType.Both;
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_Size; i++)
                {
                    if (m_GameBoard[m_Size - 1, i].PieceType == Piece.e_PieceType.O)
                    {
                        m_GameBoard[m_Size - 1, i].PieceType = Piece.e_PieceType.U;
                        m_GameBoard[m_Size - 1, i].DirectionType = Piece.e_DirectionType.Both;
                    }
                }
            }
        }
    }
}

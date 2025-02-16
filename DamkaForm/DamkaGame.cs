using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DamkaForm
{
    public partial class DamkaGame : Form
    {
        private Game m_CurrentGame;
        private Button[,] boardButtons;
        private int m_CellSize = 30;
        private Button m_From = null;
        private Button m_To = null;
        public DamkaGame(Game i_CurrentGame)
        {
            m_CurrentGame = i_CurrentGame;
            InitializeComponent();
            this.labelPlayer1.Text = m_CurrentGame.FirstPlayer.PlayerName + ":";
            this.labelPlayer1.BackColor = Color.LightBlue;
            this.labelPlayer2.Text = m_CurrentGame.SecondPlayer.PlayerName + ":";
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            int boardSize = m_CurrentGame.Board.Size;
            boardButtons = new Button[boardSize, boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Button button = new Button();
                    button.Size = new Size(m_CellSize, m_CellSize);
                    button.Location = new System.Drawing.Point(50 + j * m_CellSize, 50 + i * m_CellSize);
                    if (m_CurrentGame.Board.GameBoard[i, j].PieceType != Piece.e_PieceType.Empty)
                    {
                        button.Text = m_CurrentGame.Board.GameBoard[i, j].PieceType.ToString();
                    }
                    button.Tag = new System.Drawing.Point(i, j);
                    button.Click += OnCellClick;

                    if ((i + j) % 2 == 0)
                    {
                        button.BackColor = Color.Gray;
                        button.Enabled = false;
                    }
                    else
                    {
                        button.BackColor = Color.White;
                    }

                    boardButtons[i, j] = button;
                    this.Controls.Add(button);
                }
            }
        }
        private void OnCellClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            System.Drawing.Point clickedPoint = (System.Drawing.Point)clickedButton.Tag;
            List<Point[]> o_OptionalJumpsRes;
            bool isJumpMove = false;
            bool invalidChoice = false;

            if (clickedButton.BackColor == Color.LightBlue)
            {
                m_From = null;
                clickedButton.BackColor = Color.White;
            }
            else if (m_From == null)
            {
                if (isCurrentPlayerPiece(clickedPoint))
                {
                    m_From = clickedButton;
                    clickedButton.BackColor = Color.LightBlue;
                }
            }
            else if (m_To == null)
            {
                m_To = clickedButton;

                System.Drawing.Point fromTagPoint = (System.Drawing.Point)m_From.Tag;
                Point fromPoint = new Point(fromTagPoint.X, fromTagPoint.Y);

                System.Drawing.Point toTagPoint = (System.Drawing.Point)m_To.Tag;
                Point toPoint = new Point(toTagPoint.X, toTagPoint.Y);

                if (m_CurrentGame.CurrentPlayer.IsLegalMove(fromPoint, toPoint, m_CurrentGame.Board))
                {
                    if (!isOptimalAnotherJumpsEmpty())
                    {
                        isJumpMove = true;
                        if (!m_CurrentGame.CurrentPlayer.IsChoiceInList(fromPoint, toPoint, m_CurrentGame.CurrentPlayer.OptionalAnotherJumps))
                        {
                            // TODO: This to lines get out to funtion, very similar to the next if.
                            MessageBox.Show("You have to keep jumping!");
                            invalidChoice = true;
                        }
                    }
                    else if (m_CurrentGame.CurrentPlayer.MustJump(out o_OptionalJumpsRes, m_CurrentGame.Board))
                    {
                        isJumpMove = true;
                        if (!m_CurrentGame.CurrentPlayer.IsChoiceInList(fromPoint, toPoint, o_OptionalJumpsRes))
                        {
                            // TODO: This to lines get out to funtion, very similar to the next if.
                            MessageBox.Show("You must jump!");
                            invalidChoice = true;
                        }
                    }
                    else
                    {
                        m_CurrentGame.CurrentPlayer.OptionalAnotherJumps = null;
                    }

                    if (invalidChoice)
                    {
                        m_From.BackColor = Color.White;
                        m_From = null;
                        m_To = null;
                        return;
                    }

                    m_CurrentGame.CurrentPlayer.ExecuteMove(fromPoint, toPoint, m_CurrentGame.Board);
                    // make Move
                    //updateBoard
                    // update the player list.
                    m_CurrentGame.Board.UpdateKingCase(m_CurrentGame.CurrentPlayer.PlayerPiece);
                    updateBoard();
                    if (isJumpMove)
                    {
                        List<Point[]> tempOptionalAnotherJumps;
                        m_CurrentGame.CurrentPlayer.CheckIfCanJumpAndMakeList(m_CurrentGame.Board, toPoint, out tempOptionalAnotherJumps);
                        m_CurrentGame.CurrentPlayer.OptionalAnotherJumps = tempOptionalAnotherJumps;
                    }
                    if (isOptimalAnotherJumpsEmpty())
                    {
                        m_CurrentGame.ChangeTurn();
                        updateLabelBackColor();
                    }
                    // Keep the turn if I can eat or change

                    // Update king case

                    // Check if someone won.
                    m_CurrentGame.CheckGameStatus();
                    showMessageBoxIfTheGameIsOver();

                    // Update score (labels)

                    // Switch color
                }
                else
                {
                    MessageBox.Show("The move is illegal, please try again!");
                }
                m_From.BackColor = Color.White;
                m_From = null;
                m_To = null;
            }
        }
        private void updateBoard()
        {
            int boardSize = m_CurrentGame.Board.Size;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (m_CurrentGame.Board.GameBoard[i, j].PieceType == Piece.e_PieceType.Empty)
                    {
                        boardButtons[i, j].Text = "";
                    }
                    else
                    {
                        boardButtons[i, j].Text = m_CurrentGame.Board.GameBoard[i, j].PieceType.ToString();
                    }
                }
            }
        }
        private bool isCurrentPlayerPiece(System.Drawing.Point i_ClickedPoint)
        {
            return m_CurrentGame.CurrentPlayer.TheMoveIsFromThePlayerSquare(m_CurrentGame.Board.GameBoard[i_ClickedPoint.X, i_ClickedPoint.Y].PieceType);
        }

        private bool isOptimalAnotherJumpsEmpty()
        {
            return m_CurrentGame.CurrentPlayer.OptionalAnotherJumps == null || m_CurrentGame.CurrentPlayer.OptionalAnotherJumps.Count == 0;
        }
        private void updateLabelBackColor()
        {
            if (m_CurrentGame.CurrentPlayer == m_CurrentGame.FirstPlayer)
            {
                labelPlayer1.BackColor = Color.LightBlue;
                labelPlayer2.BackColor = Color.Empty;
            }
            else
            {
                labelPlayer2.BackColor = Color.LightBlue;
                labelPlayer1.BackColor = Color.Empty;
            }
        }
        private void showMessageBoxIfTheGameIsOver()
        {
            String messageToDisplay;

            if (m_CurrentGame.GameOver)
            {
                if (m_CurrentGame.Winner != null)
                {
                    messageToDisplay = $"{m_CurrentGame.Winner.PlayerName} Won!\nAnother Round?"; // TODO : /n
                }
                else
                {
                    messageToDisplay = "Tie!\nAnother Round?"; // TODO : /n
                }

                DialogResult result = MessageBox.Show(messageToDisplay, "Damka",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    prepareAnotherGame();
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }
            }
        }
        private void prepareAnotherGame()
        {
            updateScore();
            m_CurrentGame.SetTheNumberOfPicesToThePlayers();
            m_CurrentGame.Board = new Board(m_CurrentGame.Board.Size);
            for (int i = 0; i < m_CurrentGame.Board.Size; i++)
            {
                for (int j = 0; j < m_CurrentGame.Board.Size; j++)
                {
                    if (m_CurrentGame.Board.GameBoard[i, j].PieceType != Piece.e_PieceType.Empty)
                    {
                        boardButtons[i,j].Text = m_CurrentGame.Board.GameBoard[i, j].PieceType.ToString();
                    }
                    else
                    {
                        boardButtons[i, j].Text = "";
                    }
                }
            }
            m_CurrentGame.CurrentPlayer = m_CurrentGame.FirstPlayer;
            labelPlayer1.BackColor = Color.LightBlue;
            labelPlayer2.BackColor = Color.Empty;
            m_CurrentGame.Winner = null;
            m_CurrentGame.GameOver = false;
        }
        private void updateScore()
        {
            m_CurrentGame.UpdatePlayerPoints();
            Player1Score.Text = m_CurrentGame.FirstPlayer.Points.ToString();
            Player2Score.Text = m_CurrentGame.SecondPlayer.Points.ToString();
        }
    }
}

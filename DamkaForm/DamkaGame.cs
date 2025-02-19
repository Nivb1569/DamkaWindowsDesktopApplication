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
        private bool m_isExiting = false;


        public DamkaGame(Game i_CurrentGame)
        {
            m_CurrentGame = i_CurrentGame;
            InitializeComponent();
            initializeLabels();
            initializeBoard();
            fitFormSize();
        }

        private void initializeLabels()
        {
            this.labelPlayer1.Text = m_CurrentGame.FirstPlayer.PlayerName + ":";
            this.labelPlayer1.BackColor = Color.LightBlue;
            this.labelPlayer2.Text = m_CurrentGame.SecondPlayer.PlayerName + ":";
        }
        private void initializeBoard()
        {
            int boardSize = m_CurrentGame.Board.Size;
            boardButtons = new Button[boardSize, boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Button button = new Button();
                    button.Size = new Size(m_CellSize, m_CellSize);
                    button.Location = new System.Drawing.Point(j * m_CellSize, i * m_CellSize);
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
                    boardPanel.Controls.Add(button);
                }
            }

            boardPanel.Size = new Size(boardSize * m_CellSize, boardSize * m_CellSize);
        }
        private void OnCellClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            System.Drawing.Point clickedPoint = (System.Drawing.Point)clickedButton.Tag;

            if (m_CurrentGame.CurrentPlayer.IsComputer)
            {
                return;
            }

            if (isButtonHasBeenSelected(clickedButton))
            {
                resetSelection(clickedButton);
            }
            else if (m_From == null)
            {
                selectPiece(clickedButton, clickedPoint);
            }
            else if (m_To == null)
            {
                m_To = clickedButton;
                if (handelMove())
                {
                    m_From.BackColor = Color.White;
                    m_From = null;
                    m_To = null;
                }
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
            return m_CurrentGame.CurrentPlayer.TheMoveIsFromThePlayerSquare(m_CurrentGame.Board
                .GameBoard[i_ClickedPoint.X, i_ClickedPoint.Y].PieceType);
        }
        private bool isOptimalAnotherJumpsEmpty()
        {
            return m_CurrentGame.CurrentPlayer.OptionalAnotherJumps == null ||
                   m_CurrentGame.CurrentPlayer.OptionalAnotherJumps.Count == 0;
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
                    m_isExiting = true;
                    this.Close();
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
                        boardButtons[i, j].Text = m_CurrentGame.Board.GameBoard[i, j].PieceType.ToString();
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
        private void fitFormSize()
        {
            int boardSizePixel = m_CurrentGame.Board.Size * m_CellSize;
            int extraSpace = 100;
            this.ClientSize = new Size(boardSizePixel + extraSpace, boardSizePixel + extraSpace);

            boardPanel.Location = new System.Drawing.Point(
                (this.ClientSize.Width - boardPanel.Width) / 2,
                (this.ClientSize.Height - boardPanel.Height) / 2);

            flowLayoutPanelLabels.Location = new System.Drawing.Point(
                boardPanel.Left + (boardPanel.Width - flowLayoutPanelLabels.Width) / 2,
                boardPanel.Top - flowLayoutPanelLabels.Height - 10
            );
        }
        private void labelPlayer2_Click(object sender, EventArgs e)
        {
            if (m_CurrentGame.CurrentPlayer.IsComputer && labelPlayer2.BackColor == Color.LightBlue)
            {
                playComputerTurn();
            }
        }
        private void playComputerTurn()
        {
            Point fromPoint, ToPoint;
            bool isJump = false;

            if (!isOptimalAnotherJumpsEmpty())
            {
                selectNextMove(out fromPoint, out ToPoint);
            }
            else
            {
                m_CurrentGame.CurrentPlayer.GenerateMove(out fromPoint, out ToPoint, m_CurrentGame.Board);
            }
            isJump = m_CurrentGame.CurrentPlayer.IsJump(fromPoint, ToPoint, m_CurrentGame.Board);
            PerformPlayerMove(fromPoint, ToPoint);
            if (isJump)
            {
                makeListIfAnotherJump(ToPoint);
            }
            else
            {
                m_CurrentGame.CurrentPlayer.OptionalAnotherJumps = null;
            }

            if (isOptimalAnotherJumpsEmpty())
            {
                m_CurrentGame.ChangeTurn();
                updateLabelBackColor();
            }

            m_CurrentGame.CheckGameStatus();
            showMessageBoxIfTheGameIsOver();
        }
        private void selectNextMove(out Point o_From, out Point o_To)
        { // TODO : to logic.
            o_From = m_CurrentGame.CurrentPlayer.OptionalAnotherJumps[0][0];
            o_To = m_CurrentGame.CurrentPlayer.OptionalAnotherJumps[0][1];
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!m_isExiting)
            {
                DialogResult result = MessageBox.Show(
                    "This round has finished. Do you wish to quit and start a new round?",
                    "Finish Round",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        m_CurrentGame.UpdateWinnerPlayer();
                        prepareAnotherGame();
                        e.Cancel = true;
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
            base.OnFormClosing(e);
        }
        private bool isButtonHasBeenSelected(Button i_ClicketButton)
        {
            return i_ClicketButton.BackColor == Color.LightBlue;
        }
        private void resetSelection(Button i_ClicketButton)
        {
            m_From = null;
            i_ClicketButton.BackColor = Color.White;
        }
        private void selectPiece(Button i_ClicketButton, System.Drawing.Point i_ClickedPoint)
        {
            if (isCurrentPlayerPiece(i_ClickedPoint))
            {
                m_From = i_ClicketButton;
                i_ClicketButton.BackColor = Color.LightBlue;
            }
        }
        private bool handelMove()
        {
            bool returnRes = true;
            System.Drawing.Point fromTagPoint = (System.Drawing.Point)m_From.Tag;
            Point fromPoint = new Point(fromTagPoint.X, fromTagPoint.Y);

            System.Drawing.Point toTagPoint = (System.Drawing.Point)m_To.Tag;
            Point toPoint = new Point(toTagPoint.X, toTagPoint.Y);

            if (m_CurrentGame.CurrentPlayer.IsLegalMove(fromPoint, toPoint, m_CurrentGame.Board))
            {
                returnRes = validateJump(fromPoint, toPoint);
            }
            else
            {
                MessageBox.Show("The move is illegal, please try again!");
            }

            return returnRes;
        }
        private bool validateJump(Point i_FromPoint, Point i_ToPoint)
        {
            bool returnRes = true;
            bool isJumpMove = false;
            bool invalidChoice = false;
            List<Point[]> o_OptionalJumpsRes;

            if (!isOptimalAnotherJumpsEmpty())
            {
                handelSelection(i_FromPoint, i_ToPoint, ref invalidChoice, ref isJumpMove, m_CurrentGame.CurrentPlayer.OptionalAnotherJumps, "You have to keep jumping!");
            }
            else if (m_CurrentGame.CurrentPlayer.MustJump(out o_OptionalJumpsRes, m_CurrentGame.Board))
            {
                handelSelection(i_FromPoint, i_ToPoint, ref invalidChoice, ref isJumpMove, o_OptionalJumpsRes, "You must jump!");
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
                returnRes = false;
            }
            else
            {
                PerformPlayerMove(i_FromPoint, i_ToPoint);
                if (isJumpMove)
                {
                    makeListIfAnotherJump(i_ToPoint);
                }
                if (isOptimalAnotherJumpsEmpty())
                {
                    m_CurrentGame.ChangeTurn();
                    updateLabelBackColor();
                }
                m_CurrentGame.CheckGameStatus();
                showMessageBoxIfTheGameIsOver();
            }
            return returnRes;
        }
        private void handelSelection(Point i_FromPoint, Point i_ToPoint, ref bool io_InvalidChoice, ref bool i_IsJumpMove, List<Point[]> i_ListPoints, String i_Message)
        {
            i_IsJumpMove = true;

            if (!m_CurrentGame.CurrentPlayer.IsChoiceInList(i_FromPoint, i_ToPoint, i_ListPoints))
            {
                MessageBox.Show(i_Message);
                io_InvalidChoice = true;
            }
        }
        private void PerformPlayerMove(Point i_FromPoint, Point i_ToPoint)
        {
            m_CurrentGame.CurrentPlayer.ExecuteMove(i_FromPoint, i_ToPoint, m_CurrentGame.Board);
            m_CurrentGame.Board.UpdateKingCase(m_CurrentGame.CurrentPlayer.PlayerPiece);
            updateBoard();
        }
        public void makeListIfAnotherJump(Point i_ToPoint)
        {
            List<Point[]> tempOptionalAnotherJumps;

            m_CurrentGame.CurrentPlayer.CheckIfCanJumpAndMakeList(m_CurrentGame.Board, i_ToPoint,
                out tempOptionalAnotherJumps);
            m_CurrentGame.CurrentPlayer.OptionalAnotherJumps = tempOptionalAnotherJumps;
        }
    }
}

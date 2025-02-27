﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DamkaGameLogic;
using SDPoint = System.Drawing.Point;
using DLPoint = DamkaGameLogic.Point; 


namespace DamkaForm
{
    public partial class DamkaGame : Form
    {
        private Game m_CurrentGame;
        private Button[,] boardButtons;
        private int m_CellSize = 50;
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
            labelPlayer1.Text = m_CurrentGame.FirstPlayer.PlayerName + ":";
            labelPlayer1.BackColor = Color.LightBlue;
            labelPlayer2.Text = m_CurrentGame.SecondPlayer.PlayerName + ":";
            labelPlayer1.Font = new Font("Arial", 16, FontStyle.Bold);
            labelPlayer2.Font = new Font("Arial", 16, FontStyle.Bold);
            Player1Score.Font = new Font("Arial", 16, FontStyle.Bold);
            Player2Score.Font = new Font("Arial", 16, FontStyle.Bold);

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
                    button.Location = new SDPoint(j * m_CellSize, i * m_CellSize);
                    addIcons(button, i, j);
                    button.Tag = new SDPoint(i, j);
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
        private void addIcons(Button i_Button, int i, int j)
        {
            if (m_CurrentGame.Board.GameBoard[i, j].PieceType == Piece.e_PieceType.X)
            {
                i_Button.Image = Properties.Resources.black;
                i_Button.Image = new Bitmap(Properties.Resources.black, new Size(m_CellSize - 10, m_CellSize - 10));
            }
            else if (m_CurrentGame.Board.GameBoard[i, j].PieceType == Piece.e_PieceType.O)
            {
                i_Button.Image = Properties.Resources.red;
                i_Button.Image = new Bitmap(Properties.Resources.red, new Size(m_CellSize - 10, m_CellSize - 10));
            }
            else
            {
                i_Button.Image = null; 
            }
            i_Button.ImageAlign = ContentAlignment.MiddleCenter;

        }
        private void OnCellClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            SDPoint clickedPoint = (SDPoint)clickedButton.Tag;

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
                    if (m_CurrentGame.Board.GameBoard[i, j].PieceType == Piece.e_PieceType.X)
                    {
                        boardButtons[i, j].Image = Properties.Resources.black;
                        boardButtons[i, j].Image = new Bitmap(Properties.Resources.black, new Size(m_CellSize - 10, m_CellSize - 10));
                    }
                    else if (m_CurrentGame.Board.GameBoard[i, j].PieceType == Piece.e_PieceType.O)
                    {
                        boardButtons[i, j].Image = Properties.Resources.red;
                        boardButtons[i, j].Image = new Bitmap(Properties.Resources.red, new Size(m_CellSize - 10, m_CellSize - 10));
                    }
                    else if (m_CurrentGame.Board.GameBoard[i, j].PieceType == Piece.e_PieceType.K)
                    {
                        boardButtons[i, j].Image = Properties.Resources.blackKing;
                        boardButtons[i, j].Image = new Bitmap(Properties.Resources.blackKing, new Size(m_CellSize - 10, m_CellSize - 10));
                    }
                    else if (m_CurrentGame.Board.GameBoard[i, j].PieceType == Piece.e_PieceType.U)
                    {
                        boardButtons[i, j].Image = Properties.Resources.redKing;
                        boardButtons[i, j].Image = new Bitmap(Properties.Resources.redKing, new Size(m_CellSize - 10, m_CellSize - 10));
                    }
                    else
                    {
                        boardButtons[i, j].Image = null;
                    }
                }
            }
        }
        private bool isCurrentPlayerPiece(SDPoint i_ClickedPoint)
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
                    messageToDisplay = $"{m_CurrentGame.Winner.PlayerName} Won!{Environment.NewLine}Another Round?"; 
                }
                else
                {
                    messageToDisplay = $"Tie!{Environment.NewLine}Another Round?";
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
                    addIcons(boardButtons[i, j], i, j);
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
            int extraSpace = 150;
            this.ClientSize = new Size(boardSizePixel + extraSpace, boardSizePixel + extraSpace);

            boardPanel.Location = new SDPoint(
                (this.ClientSize.Width - boardPanel.Width) / 2,
                (this.ClientSize.Height - boardPanel.Height) / 2);

            flowLayoutPanelLabels.Location = new SDPoint(
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
            DLPoint fromPoint, ToPoint;
            bool isJump;

            if (!isOptimalAnotherJumpsEmpty())
            {
                m_CurrentGame.CurrentPlayer.SelectNextMove(out fromPoint, out ToPoint);
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
        private void selectPiece(Button i_ClicketButton, SDPoint i_ClickedPoint)
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
            SDPoint fromTagPoint = (SDPoint)m_From.Tag;
            DLPoint fromPoint = new DLPoint(fromTagPoint.X, fromTagPoint.Y);
            SDPoint toTagPoint = (SDPoint)m_To.Tag;
            DLPoint toPoint = new DLPoint(toTagPoint.X, toTagPoint.Y);

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
        private bool validateJump(DLPoint i_FromPoint, DLPoint i_ToPoint)
        {
            bool returnRes = true;
            bool isJumpMove = false;
            bool invalidChoice = false;
            List<DLPoint[]> o_OptionalJumpsRes;

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
        private void handelSelection(DLPoint i_FromPoint, DLPoint i_ToPoint, ref bool io_InvalidChoice, ref bool i_IsJumpMove, List<DLPoint[]> i_ListPoints, String i_Message)
        {
            i_IsJumpMove = true;

            if (!m_CurrentGame.CurrentPlayer.IsChoiceInList(i_FromPoint, i_ToPoint, i_ListPoints))
            {
                MessageBox.Show(i_Message);
                io_InvalidChoice = true;
            }
        }
        private void PerformPlayerMove(DLPoint i_FromPoint, DLPoint i_ToPoint)
        {
            m_CurrentGame.CurrentPlayer.ExecuteMove(i_FromPoint, i_ToPoint, m_CurrentGame.Board);
            m_CurrentGame.Board.UpdateKingCase(m_CurrentGame.CurrentPlayer.PlayerPiece);
            updateBoard();
        }
        private void makeListIfAnotherJump(DLPoint i_ToPoint)
        {
            List<DLPoint[]> tempOptionalAnotherJumps;

            m_CurrentGame.CurrentPlayer.CheckIfCanJumpAndMakeList(m_CurrentGame.Board, i_ToPoint,
                out tempOptionalAnotherJumps);
            m_CurrentGame.CurrentPlayer.OptionalAnotherJumps = tempOptionalAnotherJumps;
        }
    }
}

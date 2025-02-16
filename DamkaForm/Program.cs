using System;
using System.Windows.Forms;

namespace DamkaForm
{
    class Program
    {
        public static void Main()
        {
            FormGameSettings formGameSettings = new FormGameSettings();

            if (formGameSettings.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Game game = new Game(formGameSettings.BoardSize, formGameSettings.FirstPlayer, formGameSettings.SecondPlayer);
            DamkaGame damkaGame = new DamkaGame(game);
            game.SetTheNumberOfPicesToThePlayers();
            damkaGame.ShowDialog();
        }
    }
}
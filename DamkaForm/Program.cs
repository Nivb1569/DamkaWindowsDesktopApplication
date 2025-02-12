using System;
using System.Windows.Forms;

namespace DamkaForm
{
    class Program
    {
        public static void Main()
        {
            FormGameSettings formGameSettings = new FormGameSettings();
            formGameSettings.ShowDialog();
            Game game = new Game(formGameSettings.BoardSize, formGameSettings.FirstPlayer, formGameSettings.SecondPlayer);
            DamkaGame damkaGame = new DamkaGame(game);
            damkaGame.ShowDialog();
        }
    }
}
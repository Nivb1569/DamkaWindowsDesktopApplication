using System.Windows.Forms;
using DamkaGameLogic;

namespace DamkaForm
{
    public static class GameManager
    {
        public static void StartGame()
        {
            FormGameSettings formGameSettings = new FormGameSettings();

            if (formGameSettings.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Game game = new Game(formGameSettings.BoardSize, formGameSettings.FirstPlayer, formGameSettings.SecondPlayer);
            game.SetTheNumberOfPicesToThePlayers();
            DamkaGame damkaGame = new DamkaGame(game);
            damkaGame.ShowDialog();
        }
    }
}

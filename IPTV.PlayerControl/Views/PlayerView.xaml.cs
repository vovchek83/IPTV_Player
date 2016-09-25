using System.ComponentModel.Composition;
using System.Windows.Controls;
using IPTV.PlayerControl.ViewModels;

namespace IPTV.PlayerControl.Views
{

    /// <summary>
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    [Export]
    public partial class PlayerView : UserControl
    {
        //[Import]
        //public PlayerViewModel PlayerViewModel { get; set; }

        public PlayerView()
        {
            InitializeComponent();
         //   DataContext = PlayerViewModel;
        }
    }
}

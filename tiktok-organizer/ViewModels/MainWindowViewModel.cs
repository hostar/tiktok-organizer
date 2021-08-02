using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using tiktok_organizer.Models;

namespace tiktok_organizer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string profileName = "";
        public string ProfileName
        {
            get => profileName;
            set => this.RaiseAndSetIfChanged(ref profileName, value);
        }

        private ObservableCollection<Video> videoList = new ObservableCollection<Video>();
        public ObservableCollection<Video> VideoList
        {
            get => videoList;
            set => this.RaiseAndSetIfChanged(ref videoList, value);
        }
    }
}

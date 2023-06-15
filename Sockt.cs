using CommunityToolkit.Mvvm.ComponentModel;

namespace ModbusTCPApp.Models
{
    public class Sockt : ObservableRecipient
    {
        public int Number
        {
            get
            {
                return _number;
            }

            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        private int _number;
    }
}
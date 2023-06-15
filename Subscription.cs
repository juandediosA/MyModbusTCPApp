using CommunityToolkit.Mvvm.ComponentModel;

namespace ModbusTCPApp.Models
{
    public class Subscription : ObservableRecipient
    {
        public int Address { get; set; }

        public string Result
        {
            get
            {
                return _result;
            }

            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public int Server { get; set; }

        public SubsType Type { get; set; }

        private string _result;
    }
}
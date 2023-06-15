using CommunityToolkit.Mvvm.ComponentModel;

namespace ModbusTCPApp.Models
{
    public class Values : ObservableRecipient
    {
        public int Address { get; set; }

        public string Value { get; set; }
    }
}
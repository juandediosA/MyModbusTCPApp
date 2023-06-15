using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomEasyModbus.Models;
using ModbusTCPApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace ModbusTCPApp
{
    internal class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            AddCoilCommand = new RelayCommand(AddCoilFunction);
            AddHRCommand = new RelayCommand(AddHRFunction);

            // Write
            WriteCoilCommand = new RelayCommand(WriteCoilFunction);
            WriteHRCommand = new RelayCommand(WriteHRFunction);
            RefreshListCommand = new RelayCommand(RefreshListFunction);
            WriteCoilListCommand = new RelayCommand(WriteCoilListFunction);
            RefreshHRListCommand = new RelayCommand(RefreshHRListFunction);
            WriteHRListCommand = new RelayCommand(WriteHRListFunction);

            ConnectCommand = new RelayCommand(ConnectFunction);
            DisconnectCommand = new RelayCommand(DisconnectFunction);

            CleanLogCommand = new RelayCommand(CleanLogFunction);
            ClearDataCommand = new RelayCommand(ClearDataFunction);

            TimerReadIntervalCommand = new RelayCommand(TimerReadIntervalFunction);
            TimerScanIntervalCommand = new RelayCommand(TimerScanIntervalFunction);

            RemoveItemCommand = new RelayCommand(RemoveItemFunction);
            SetReadTimer();
            SetScanTimer();
        }

        public RelayCommand AddCoilCommand { get; private set; }

        public string AddCoilText { get; set; } = string.Empty;

        public RelayCommand AddHRCommand { get; private set; }

        public string AddHRText { get; set; } = string.Empty;

        public RelayCommand CleanLogCommand { get; private set; }

        public RelayCommand ClearDataCommand { get; private set; }

        public RelayCommand ConnectCommand { get; private set; }

        public RelayCommand DisconnectCommand { get; private set; }

        public DateTime FechaLocal { get; set; }

        public string IPText { get; set; } = string.Empty;

        public string LogText { get; set; } = string.Empty;

        public string NumberCoilText { get; set; } = "1";

        public string NumberCoilWText { get; set; }

        public string NumberHRText { get; set; } = "1";

        public string NumberHRWText { get; set; }

        public string pattern { get; set; } = "yyyy/MM/dd HH:mm:ss";

        public string ReadDataText { get; set; } = string.Empty;

        public RelayCommand RefreshHRListCommand { get; private set; }

        public RelayCommand RefreshListCommand { get; private set; }

        public RelayCommand RemoveItemCommand { get; private set; }

        public string ResultCoilText { get; set; } = string.Empty;

        public string ResultHRText { get; set; } = string.Empty;

        public Subscription SelectedItemDataGrid { get; set; }

        public string SelectedServer { get; set; }

        public ObservableCollection<Sockt> Sockets { get; private set; } = new ObservableCollection<Sockt>();

        public string StartAddressWHRText { get; set; }

        public string StartAddressWText { get; set; }

        public ObservableCollection<Subscription> Subscriptions { get; private set; } = new ObservableCollection<Subscription>();

        public RelayCommand TimerReadIntervalCommand { get; private set; }

        public RelayCommand TimerScanIntervalCommand { get; private set; }

        public string TimerReadIntervalText { get; set; } = string.Empty;

        public string TimerScanIntervalText { get; set; } = string.Empty;

        public string ValueCoilText { get; set; } = string.Empty;

        public string ValueHRText { get; set; } = string.Empty;

        public ObservableCollection<Values> ValuesHRList { get; private set; } = new ObservableCollection<Values>();

        public ObservableCollection<Values> ValuesList { get; private set; } = new ObservableCollection<Values>();

        public RelayCommand WriteCoilCommand { get; private set; }

        public RelayCommand WriteCoilListCommand { get; private set; }

        public string WriteCoilText { get; set; } = string.Empty;

        public RelayCommand WriteHRCommand { get; private set; }

        public RelayCommand WriteHRListCommand { get; private set; }

        public string WriteHRText { get; set; } = string.Empty;

        public void AddCoilFunction()
        {
            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            State state = CustomEasyModbus.Modbus.CheckCoilDirection(AddCoilText);
            if (state == State.Correct)
            {
                if (SelectedServer != null)
                {
                    int number = int.Parse(NumberCoilText);
                    string[] values = new string[number];
                    values = CustomEasyModbus.Modbus.ReadCoils(AddCoilText, number);
                    for (int i = 0; i < number; i++)
                    {
                        Subscription element = new Subscription();
                        element.Server = int.Parse(SelectedServer);
                        element.Type = SubsType.Coil;
                        element.Address = int.Parse(AddCoilText) + i;
                        element.Result = values[i];
                        AddElement(element);
                    }

                    LogText = fechaLocal + " Coil added: " + AddCoilText + "\r\n" + LogText;
                }
                else
                {
                    LogText = fechaLocal + " Select a server.\r\n" + LogText;
                }
            }
            else if (state == State.NotValid)
            {
                LogText = fechaLocal + "Error. Not valid Coil.\r\n" + LogText;
            }
            else if (state == State.NotNumeric)
            {
                LogText = fechaLocal + "Error. Not numeric Coil.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
        }

        public void AddElement(Subscription element)
        {
            mut.WaitOne();
            Subscriptions.Add(element);
            mut.ReleaseMutex();
        }

        public void AddHRFunction()
        {
            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            State state = CustomEasyModbus.Modbus.CheckHRDirection(AddHRText);
            if (state == State.Correct)
            {
                if (SelectedServer != null)
                {
                    int number = int.Parse(NumberHRText);
                    string[] values = new string[number];
                    values = CustomEasyModbus.Modbus.ReadHRs(AddHRText, number);
                    for (int i = 0; i < number; i++)
                    {
                        Subscription element = new Subscription();
                        element.Server = int.Parse(SelectedServer);
                        element.Type = SubsType.HR;
                        element.Address = int.Parse(AddHRText) + i;
                        element.Result = values[i];
                        AddElement(element);
                    }

                    LogText = fechaLocal + " HR added: " + AddHRText + "\r\n" + LogText;
                }
                else
                {
                    LogText = fechaLocal + " Select a server.\r\n" + LogText;
                }
            }
            else if (state == State.NotValid)
            {
                LogText = fechaLocal + "Error. Not valid HR.\r\n" + LogText;
            }
            else if (state == State.NotNumeric)
            {
                LogText = fechaLocal + "Error. Not numeric HR.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
        }

        public void AddSocket(Sockt socket)
        {
            mutSocket.WaitOne();
            Sockets.Add(socket);
            mutSocket.ReleaseMutex();
        }

        public void CleanLogFunction()
        {
            LogText = string.Empty;
            OnPropertyChanged(nameof(LogText));
        }

        public void ClearDataFunction()
        {
            mut.WaitOne();

            Subscriptions.Clear();

            mut.ReleaseMutex();

            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            LogText = fechaLocal + " Data cleared.\r\n" + LogText;

            OnPropertyChanged(nameof(LogText));
        }

        // Connections
        public void ConnectFunction()
        {
            bool ipCorrect;
            ipCorrect = CustomEasyModbus.Modbus.IPSetup(IPText);
            if (ipCorrect)
            {
                bool repeated = false;
                int skt = 0;
                int valueIP = int.Parse(IPText);
                while (repeated == false && skt < Sockets.Count)
                {
                    if (valueIP == Sockets[skt].Number)
                    {
                        repeated = true;
                    }

                    skt += 1;
                }

                if (repeated == false)
                {
                    Connection connection = CustomEasyModbus.Modbus.Connect();
                    if (connection == Connection.Connected)
                    {
                        Sockt s = new Sockt();
                        s.Number = valueIP;
                        AddSocket(s);
                        LogText = "Connected to server " + IPText + ".\r\n" + LogText;

                        if (Sockets.Count == 1)
                        {
                            // Timers
                            read_timer.Start();
                            scan_timer.Start();
                        }
                    }
                    else if (connection == Connection.Failure)
                    {
                        LogText = "Failure to connect.\r\n" + LogText;
                    }
                    else if (connection == Connection.Unreachable)
                    {
                        LogText = "Unreachable server.\r\n" + LogText;
                    }
                }
                else
                {
                    LogText = "Already connected to server.\r\n" + LogText;
                }
            }
            else
            {
                LogText = "Set IP address correctly.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
        }

        public void DisconnectFunction()
        {
            if (Sockets.Count > 0)
            {
                if (SelectedServer != null)
                {
                    bool disconnected = CustomEasyModbus.Modbus.Disconnect();

                    if (disconnected == false)
                    {
                        LogText = "Disconnected from server " + SelectedServer + ".\r\n" + LogText;

                        Sockt s = new Sockt();
                        s.Number = int.Parse(SelectedServer);
                        RemoveSocket(s);

                        if (Sockets.Count == 0)
                        {
                            // Timers
                            read_timer.Stop();
                            scan_timer.Stop();
                        }
                    }
                    else
                    {
                        LogText = "Impossible to disconnect.\r\n" + LogText;
                    }
                }
                else
                {
                    LogText = "Select a server.\r\n" + LogText;
                }
            }
            else
            {
                LogText = "Not connected to any server.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
        }

        public void ModifyElement()
        {
            mut.WaitOne();
            foreach (Subscription s in _subscriptions)
            {
                string[] values = new string[1];
                if (s.Type == SubsType.Coil)
                {
                    values = CustomEasyModbus.Modbus.ReadCoils(s.Address.ToString(), 1);
                    s.Result = values[0];
                }
                else if (s.Type == SubsType.HR)
                {
                    values = CustomEasyModbus.Modbus.ReadHRs(s.Address.ToString(), 1);
                    s.Result = values[0];
                }
            }

            mut.ReleaseMutex();
        }

        public int ObtainValue()
        {
            return 1;
        }

        public void RefreshElement()
        {
            mut.WaitOne();
            foreach (Subscription s in Subscriptions)
            {
                string[] values = new string[1];
                if (s.Type == SubsType.Coil)
                {
                    values = CustomEasyModbus.Modbus.ReadCoils(s.Address.ToString(), 1);
                    s.Result = values[0];
                }
                else if (s.Type == SubsType.HR)
                {
                    values = CustomEasyModbus.Modbus.ReadHRs(s.Address.ToString(), 1);
                    s.Result = values[0];
                }
            }

            mut.ReleaseMutex();
        }

        public void RefreshHRListFunction()
        {
            ValuesHRList.Clear();
            bool correct = CustomEasyModbus.Modbus.CheckWriteParameters(StartAddressWHRText, NumberHRWText);
            if (correct)
            {
                int numReg = int.Parse(NumberHRWText);
                int startAdd = int.Parse(StartAddressWHRText);
                for (int i = startAdd; i < numReg + startAdd; i++)
                {
                    Values element = new Values();
                    element.Address = i;
                    element.Value = string.Empty;
                    ValuesHRList.Add(element);
                }
            }
            else
            {
                string fechaLocal;
                FechaLocal = DateTime.Now;
                fechaLocal = FechaLocal.ToString();

                LogText = fechaLocal + " Set address and number correctly.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
            OnPropertyChanged(nameof(ValuesList));
        }

        public void RefreshListFunction()
        {
            ValuesList.Clear();
            bool correct = CustomEasyModbus.Modbus.CheckWriteParameters(StartAddressWText, NumberCoilWText);
            if (correct)
            {
                int numReg = int.Parse(NumberCoilWText);
                int startAdd = int.Parse(StartAddressWText);
                for (int i = startAdd; i < numReg + startAdd; i++)
                {
                    Values element = new Values();
                    element.Address = i;
                    element.Value = string.Empty;
                    ValuesList.Add(element);
                }
            }
            else
            {
                string fechaLocal;
                FechaLocal = DateTime.Now;
                fechaLocal = FechaLocal.ToString();

                LogText = fechaLocal + " Set address and number correctly.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
            OnPropertyChanged(nameof(ValuesList));
        }

        public void RemoveItemFunction()
        {
            Subscriptions.Remove(SelectedItemDataGrid);

            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            LogText = fechaLocal + " Element removed.\r\n" + LogText;

            // OnPropertyChanged(nameof(Subscriptions));
            OnPropertyChanged(nameof(LogText));
        }

        public void RemoveSocket(Sockt socket)
        {
            mutSocket.WaitOne();
            for (int l = 0; l < Sockets.Count; l++)
            {
                if (Sockets[l].Number == socket.Number)
                {
                    Sockets.Remove(Sockets[l]);
                }
            }

            mutSocket.ReleaseMutex();
            OnPropertyChanged(nameof(Sockets));
        }

        public void TimerReadIntervalFunction()
        {
            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            bool valid = CustomEasyModbus.Modbus.CheckTimerInterval(TimerReadIntervalText);
            if (valid)
            {
                read_timer.Stop();
                read_timer.Interval = double.Parse(TimerReadIntervalText);
                read_timer.Start();

                LogText = fechaLocal + " Read interval changed to " + TimerReadIntervalText + "ms.\r\n" + LogText;
            }
            else
            {
                LogText = fechaLocal + " Invalid time interval.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
        }

        public void TimerScanIntervalFunction()
        {
            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            bool valid = CustomEasyModbus.Modbus.CheckTimerInterval(TimerScanIntervalText);
            if (valid)
            {
                scan_timer.Stop();
                scan_timer.Interval = double.Parse(TimerScanIntervalText);
                scan_timer.Start();

                LogText = fechaLocal + " Scan interval changed to " + TimerScanIntervalText + "ms.\r\n" + LogText;
            }
            else
            {
                LogText = fechaLocal + " Invalid time interval.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
        }

        public void WriteCoilFunction()
        {
            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            State state = CustomEasyModbus.Modbus.CheckCoilDirection(WriteCoilText);
            bool data = CustomEasyModbus.Modbus.CheckCoilData(ValueCoilText);

            if (state == State.Correct)
            {
                if (data == true)
                {
                    bool written = CustomEasyModbus.Modbus.WriteCoil(WriteCoilText, ValueCoilText);
                    if (written)
                    {
                        LogText = fechaLocal + " Written : " + ValueCoilText + " in addr. " + AddCoilText + "\r\n" + LogText;
                        ResultCoilText = "OK";
                    }
                    else
                    {
                        LogText = fechaLocal + " Not connected to server.\r\n" + LogText;
                        ResultCoilText = "Er.";
                    }
                }
                else
                {
                    LogText = fechaLocal + " Invalid data.\r\n" + LogText;
                }
            }
            else if (state == State.NotValid)
            {
                LogText = fechaLocal + "Error. Not valid Coil.\r\n" + LogText;
                ResultCoilText = "Er.";
            }
            else if (state == State.NotNumeric)
            {
                LogText = fechaLocal + "Error. Not numeric Coil.\r\n" + LogText;
                ResultCoilText = "Er.";
            }

            OnPropertyChanged(nameof(LogText));
            OnPropertyChanged(nameof(ResultCoilText));
        }

        public void WriteCoilListFunction()
        {
            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            bool correct = CustomEasyModbus.Modbus.CheckWriteParameters(StartAddressWText, NumberCoilWText);
            if (correct)
            {
                bool data;
                int i = 0;
                int numElements = int.Parse(NumberCoilWText);
                string[] values = new string[numElements];
                if (ValuesList.Count > 0)
                {
                    do
                    {
                        data = CustomEasyModbus.Modbus.CheckCoilData(ValuesList[i].Value);
                        values[i] = ValuesList[i].Value;
                        i++;
                    }
                    while (data == true && i < numElements);

                    if (data == true)
                    {
                        bool written = CustomEasyModbus.Modbus.WriteMultipleCoil(StartAddressWText, values);
                        if (written)
                        {
                            LogText = fechaLocal + " Written several Coils. " + ".\r\n" + LogText;
                            ResultCoilText = "OK";
                        }
                        else
                        {
                            LogText = fechaLocal + " Not connected to server.\r\n" + LogText;
                            ResultCoilText = "Er.";
                        }
                    }
                    else
                    {
                        LogText = fechaLocal + " Invalid numbers.\r\n" + LogText;
                    }
                }
                else
                {
                    LogText = fechaLocal + " Refresh the list.\r\n" + LogText;
                }
            }
            else
            {
                LogText = fechaLocal + " Set address and number correctly.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
            OnPropertyChanged(nameof(ResultCoilText));
        }

        public void WriteHRFunction()
        {
            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            State state = CustomEasyModbus.Modbus.CheckHRDirection(WriteHRText);
            bool data = CustomEasyModbus.Modbus.CheckHRData(ValueHRText);

            if (state == State.Correct)
            {
                if (data == true)
                {
                    bool written = CustomEasyModbus.Modbus.WriteSR(WriteHRText, ValueHRText);
                    if (written)
                    {
                        LogText = fechaLocal + " Written : " + ValueHRText + " in addr. " + AddHRText + "\r\n" + LogText;
                        ResultHRText = "OK";
                    }
                    else
                    {
                        LogText = fechaLocal + " Not connected to server.\r\n" + LogText;
                        ResultHRText = "Er.";
                    }
                }
                else
                {
                    LogText = fechaLocal + " Invalid data.\r\n" + LogText;
                    ResultHRText = "Er.";
                }
            }
            else if (state == State.NotValid)
            {
                LogText = fechaLocal + "Error. Not valid HR.\r\n" + LogText;
                ResultHRText = "Er.";
            }
            else if (state == State.NotNumeric)
            {
                LogText = fechaLocal + "Error. Not numeric HR.\r\n" + LogText;
                ResultHRText = "Er.";
            }

            OnPropertyChanged(nameof(LogText));
            OnPropertyChanged(nameof(ResultHRText));
        }

        public void WriteHRListFunction()
        {
            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();
            bool correct = CustomEasyModbus.Modbus.CheckWriteParameters(StartAddressWHRText, NumberHRWText);
            if (correct)
            {
                bool data;
                int i = 0;
                int numElements = int.Parse(NumberHRWText);
                string[] values = new string[numElements];
                if (ValuesHRList.Count > 0)
                {
                    do
                    {
                        data = CustomEasyModbus.Modbus.CheckHRData(ValuesHRList[i].Value);
                        values[i] = ValuesHRList[i].Value;
                        i++;
                    }
                    while (data == true && i < numElements);

                    if (data == true)
                    {
                        bool written = CustomEasyModbus.Modbus.WriteMultipleHR(StartAddressWHRText, values);
                        if (written)
                        {
                            LogText = fechaLocal + " Written several HRs. " + ".\r\n" + LogText;
                            ResultHRText = "OK";
                        }
                        else
                        {
                            LogText = fechaLocal + " Not connected to server.\r\n" + LogText;
                            ResultHRText = "Er.";
                        }
                    }
                    else
                    {
                        LogText = fechaLocal + " Invalid number in address " + ValuesHRList[i].Address + ".\r\n" + LogText;
                    }
                }
                else
                {
                    LogText = fechaLocal + " Refresh the list.\r\n" + LogText;
                }
            }
            else
            {
                LogText = fechaLocal + " Set address and number of HR correctly.\r\n" + LogText;
            }

            OnPropertyChanged(nameof(LogText));
            OnPropertyChanged(nameof(ResultHRText));
        }

        private ObservableCollection<Subscription> _subscriptions = new ObservableCollection<Subscription>();

        private Mutex mut = new Mutex();

        private Mutex mutSocket = new Mutex();

        // Timers
        private System.Timers.Timer read_timer;

        private System.Timers.Timer scan_timer;

        private void OnTimedEventRead(Object source, System.Timers.ElapsedEventArgs e)
        {
            string fechaLocal;
            FechaLocal = DateTime.Now;
            fechaLocal = FechaLocal.ToString();

            LogText = fechaLocal + " Data up to date.\r\n" + LogText;

            RefreshElement();

            OnPropertyChanged(nameof(LogText));
        }

        private void OnTimedEventScan(Object source, System.Timers.ElapsedEventArgs e)
        {
            ModifyElement();
        }

        private void SetReadTimer()
        {
            // Create a timer with a two second interval.
            read_timer = new System.Timers.Timer(5000);

            // Hook up the Elapsed event for the timer.
            read_timer.Elapsed += OnTimedEventRead;
            read_timer.AutoReset = true;
            read_timer.Enabled = true;
            read_timer.Stop();
        }

        private void SetScanTimer()
        {
            // Create a timer with tiny interval
            scan_timer = new System.Timers.Timer(2000);    //Preguntar tiempo apropiado

            // Hook up the Elapsed event for the timer.
            scan_timer.Elapsed += OnTimedEventScan;
            scan_timer.AutoReset = true;
            scan_timer.Enabled = true;
            scan_timer.Stop();
        }
    }
}
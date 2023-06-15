using CustomEasyModbus.Models;
using EasyModbus;
using System.Net;

namespace CustomEasyModbus
{
    public class Modbus
    {
        public static bool CheckCoilData(string value)
        {
            int i_valor;
            bool b_valor = false;

            b_valor = int.TryParse(value, out i_valor);
            if ((b_valor == true) && (i_valor >= 0) && (i_valor <= 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static State CheckCoilDirection(string addr)
        {
            int i = 0;

            // Validación dirección introducida.
            bool b_result = int.TryParse(addr, out i);
            if (b_result == true)
            {
                // Dirección numérica. Rangos válidos
                if ((i >= 1 && i <= 6800) || (i >= 8192 && i <= 8211) || (i >= 8256 && i <= 8319))
                {
                    // Dirección correcta
                    return State.Correct;
                }
                else
                {
                    return State.NotValid;
                }
            }
            else
            {
                return State.NotNumeric;
            }
        }

        public static bool CheckHRData(string value)
        {
            int i_valor;
            bool b_valor = false;

            b_valor = int.TryParse(value, out i_valor);
            if ((b_valor == true) && (i_valor >= 0) && (i_valor <= 65535))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static State CheckHRDirection(string addr)
        {
            int i = 0;

            // Validación dirección introducida.
            bool b_result = int.TryParse(addr, out i);
            if (b_result == true)
            {
                // Dirección numérica. Rangos válidos
                if ((i >= 1 && i <= 6800) || (i >= 8192 && i <= 8211) || (i >= 8256 && i <= 8319))
                {
                    // Dirección correcta
                    return State.Correct;
                }
                else
                {
                    return State.NotValid;
                }
            }
            else
            {
                return State.NotNumeric;
            }
        }

        public static bool CheckTimerInterval(string value)
        {
            int i = 0;

            bool b_result = int.TryParse(value, out i);
            if (b_result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckWriteParameters(string startAddress, string numberCoils)
        {
            int i = 0;

            // Validación dirección introducida.
            bool b_result = int.TryParse(startAddress, out i);
            if (b_result == true)
            {
                if (i >= 1 && i <= 6800)
                {
                    // Validate number of coils
                    int j = 0;

                    bool n_result = int.TryParse(numberCoils, out j);
                    if (n_result == true)
                    {
                        if (j >= 1 && j <= 6800)
                        {
                            // Todo correcto
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Connect and disconnect
        public static Connection Connect()
        {
            try
            {
                Client.Connect();

                if (Client.Connected == true)
                {
                    // Cliente CONECTADO al servidor Modbus/TCP.
                    return Connection.Connected;
                }
                else
                {
                    // Cliente NO CONECTADO.
                    return Connection.Failure;
                }
            }
            catch
            {
                return Connection.Unreachable;
            }
        }

        public static bool Disconnect()
        {
            Client.Disconnect();

            return Client.Connected;
        }

        public static bool IPSetup(string ipAddress)
        {
            int i_length = 0;
            string str_cadena;
            i_length = ipAddress.Length;
            str_cadena = ipAddress.Substring(0, i_length);
            if (CheckIP(str_cadena))
            {
                // Formato de IP correcto.
                Client.IPAddress = ipAddress;

                return true;
            }
            else
            {
                // Error. Formato de IP Incorrecto.
                return false;
            }
        }

        public static string[] ReadCoils(string addr, int number)
        {
            string[] str_value = new string[number];
            if (Client.Connected == true)
            {
                bool[] value;

                value = Client.ReadCoils(int.Parse(addr) - 1, number);
                for (int i = 0; i < number; i++)
                {
                    str_value[i] = value[i].ToString();
                }

                return str_value;
            }
            else
            {
                return str_value;
            }
        }

        public static string[] ReadHRs(string addr, int number)
        {
            string[] str_value = new string[number];
            if (Client.Connected == true)
            {
                int[] value;

                value = Client.ReadHoldingRegisters(int.Parse(addr) - 1, number);
                for (int i = 0; i < number; i++)
                {
                    str_value[i] = value[i].ToString();
                }

                return str_value;
            }
            else
            {
                return str_value;
            }
        }

        public static bool WriteCoil(string addr, string value)
        {
            if (Client.Connected)
            {
                bool value_bool;
                value_bool = value == "1" ? true : false;
                Client.WriteSingleCoil(int.Parse(addr) - 1, value_bool);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool WriteMultipleCoil(string startingAddress, string[] values)
        {
            if (Client.Connected)
            {
                bool[] values_bool = new bool[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    values_bool[i] = values[i] == "1" ? true : false;
                }

                Client.WriteMultipleCoils(int.Parse(startingAddress) - 1, values_bool);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool WriteMultipleHR(string startingAddress, string[] values)
        {
            if (Client.Connected)
            {
                int[] values_int = new int[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    values_int[i] = int.Parse(values[i]);
                }

                Client.WriteMultipleRegisters(int.Parse(startingAddress) - 1, values_int);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool WriteSR(string addr, string value)
        {
            if (Client.Connected)
            {
                Client.WriteSingleRegister(int.Parse(addr) - 1, int.Parse(value));
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string pattern = "yyyy/MM/dd HH:mm:ss";

        private static ModbusClient Client { get; set; } = new ModbusClient();

        public static bool CheckIP(string sIP)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(sIP);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
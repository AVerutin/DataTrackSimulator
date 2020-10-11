using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MtsConnect;
using NLog;
using SignalGenerator.Data;

namespace SignalGenerator
{
    class Program
    {
        private static IConfigurationRoot _config;
        private static Data.MtsConnect _mtsConnection;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static List<MtsSignal> _signalsList;

        static void Main(string[] args)
        {
            ConnectToMts();
            // Установлено подключение к службе MTS Service
            Console.WriteLine("Установлено подключение к службе MTS Service");
            StartSimulate();

            Console.ReadKey(true);
        }
        
        /// <summary>
        /// Подключение к службе MTS Service
        /// </summary>
        /// <returns>Результат подключения к службе MTS Service</returns>
        private async static void ConnectToMts()
        {
            List<ushort> signals = AddMtsSignals();
            
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            string mtsIp = _config.GetSection("Mts:Ip").Value;
            int mtsPort = Int32.Parse(_config.GetSection("Mts:Port").Value);
            int mtsTimeout = Int32.Parse(_config.GetSection("Mts:Timeout").Value);
            int mtsReconnect = Int32.Parse(_config.GetSection("Mts:ReconnectTimeout").Value);

            _mtsConnection = new Data.MtsConnect("Signal-Generator", mtsIp, mtsPort, mtsTimeout, mtsReconnect);
            _mtsConnection.Connect();
            
            await _mtsConnection.Subscribe(signals, NewData);
        }

        /// <summary>
        /// Добавление сигналов MTS Service службы в список
        /// </summary>
        private static List<ushort> AddMtsSignals()
        {
            _signalsList = new List<MtsSignal>();
            List<ushort> res = new List<ushort>();
            
            // Список сигналов для управления загрузкой весовых бункеров
            _signalsList.Add(new MtsSignal(4000, "Загрузка материала в загрузочный бункер 1", 0));
            _signalsList.Add(new MtsSignal(4001, "Загрузка материала в загрузочный бункер 2", 0));
            _signalsList.Add(new MtsSignal(4002, "Выгрузка материала из загрузочного бункера 1", 0));
            _signalsList.Add(new MtsSignal(4003, "Выгрузка материала из загрузочного бункера 2", 0));
            _signalsList.Add(new MtsSignal(4004, "Датчик наличия материала 1 на конвейере 1", 0));
            _signalsList.Add(new MtsSignal(4005, "Датчик наличия материала 2 на конвейере 1", 0));
            _signalsList.Add(new MtsSignal(4006, "Датчик наличия материала 3 на конвейере 1", 0));
            _signalsList.Add(new MtsSignal(4007, "Датчик наличия материала 4 на конвейере 1", 0));
            _signalsList.Add(new MtsSignal(4008, "Датчик наличия материала 5 на конвейере 1", 0));
            _signalsList.Add(new MtsSignal(4009, "Датчик наличия материала 1 на конвейере 2", 0));
            _signalsList.Add(new MtsSignal(4010, "Датчик наличия материала 2 на конвейере 2", 0));
            _signalsList.Add(new MtsSignal(4011, "Датчик наличия материала 3 на конвейере 2", 0));
            _signalsList.Add(new MtsSignal(4012, "Датчик наличия материала 4 на конвейере 2", 0));
            _signalsList.Add(new MtsSignal(4013, "Датчик наличия материала 5 на конвейере 2", 0));
            _signalsList.Add(new MtsSignal(4014, "Датчик наличия материала 1 на конвейере 3", 0));
            _signalsList.Add(new MtsSignal(4015, "Датчик наличия материала 2 на конвейере 3", 0));
            _signalsList.Add(new MtsSignal(4016, "Датчик наличия материала 3 на конвейере 3", 0));
            _signalsList.Add(new MtsSignal(4017, "Датчик наличия материала 4 на конвейере 3", 0));
            _signalsList.Add(new MtsSignal(4018, "Датчик наличия материала 5 на конвейере 3", 0));
            _signalsList.Add(new MtsSignal(4019, "Загрузка материала в силос 1", 0));
            _signalsList.Add(new MtsSignal(4020, "Загрузка материала в силос 2", 0));
            _signalsList.Add(new MtsSignal(4021, "Загрузка материала в силос 3", 0));
            _signalsList.Add(new MtsSignal(4022, "Загрузка материала в силос 4", 0));
            _signalsList.Add(new MtsSignal(4023, "Загрузка материала в силос 5", 0));
            _signalsList.Add(new MtsSignal(4024, "Загрузка материала в силос 6", 0));
            _signalsList.Add(new MtsSignal(4025, "Загрузка материала в силос 7", 0));
            _signalsList.Add(new MtsSignal(4026, "Загрузка материала в силос 8", 0));
            _signalsList.Add(new MtsSignal(4027, "Выгрузка материала из силоса 1", 0));
            _signalsList.Add(new MtsSignal(4028, "Выгрузка материала из силоса 2", 0));
            _signalsList.Add(new MtsSignal(4029, "Выгрузка материала из силоса 3", 0));
            _signalsList.Add(new MtsSignal(4030, "Выгрузка материала из силоса 4", 0));
            _signalsList.Add(new MtsSignal(4031, "Выгрузка материала из силоса 5", 0));
            _signalsList.Add(new MtsSignal(4032, "Выгрузка материала из силоса 6", 0));
            _signalsList.Add(new MtsSignal(4033, "Выгрузка материала из силоса 7", 0));
            _signalsList.Add(new MtsSignal(4034, "Выгрузка материала из силоса 8", 0));

            foreach (MtsSignal item in _signalsList)
            {
                res.Add(item.Number);
            }

            return res;
        }

        private static void NewData(SubscriptionStateEventArgs e)
        {
            SignalsState diff = e.Diff.Signals;
            if (diff != null)
            {
                foreach (var item in diff)
                {
                    CheckSignal(item.Key, item.Value);
                }
            }
        }

        private static async void CheckSignal(ushort signal, double value)
        {
            Console.WriteLine($"[{signal}] = {value}");
            // switch (signal)
            // {
            //     case 4001: break;
            // }
        }
        
        private static async void StartSimulate()
        {
            // Начало симуляции сигналов
            // Сигнал 1 - Загрузка материала в первый загрузочный бункер
            
            // Сигнал 2 - Загрузка материала во второй загрузочный бункер
            // Сигнал 3 - Выбор цели: Силос 2 
            // Сигнал 4 - Выгрузка материала из первого весового бункера
            // Сигнал 5 - Наличие материала на 1 датчике первого конвейера
            // Сигнал  6 - .........
        }

    }
}
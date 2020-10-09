using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using NLog;
using SignalGenerator.Data;

namespace SignalGenerator
{
    class Program
    {
        private static IConfigurationRoot _config;
        private static Data.MtsConnect _mtsConnection;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static List<MtsSignal> _signals;
        
        static void Main(string[] args)
        {
            if (!ConnectToMts())
            {
                Logger.Error("Ошибка при подключении к службе MTS Service!");
                Console.WriteLine("Ошибка при подключении к службе MTS Service!");
            }
            else
            {
                // Установлено подключение к службе MTS Service
                Console.WriteLine("Установлено подключение к службе MTS Service");
                AddMtsSignals();
                Console.ReadKey(true);
            }
        }
        
        /// <summary>
        /// Подключение к службе MTS Service
        /// </summary>
        /// <returns>Результат подключения к службе MTS Service</returns>
        private static bool ConnectToMts()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            string mtsIp = _config.GetSection("Mts:Ip").Value;
            int mtsPort = Int32.Parse(_config.GetSection("Mts:Port").Value);
            int mtsTimeout = Int32.Parse(_config.GetSection("Mts:Timeout").Value);
            int mtsReconnect = Int32.Parse(_config.GetSection("Mts:ReconnectTimeout").Value);

            _mtsConnection = new Data.MtsConnect("Signal-Generator", mtsIp, mtsPort, mtsTimeout, mtsReconnect);
            return _mtsConnection.Connect();
        }

        /// <summary>
        /// Добавление сигналов MTS Service службы в список
        /// </summary>
        private static void AddMtsSignals()
        {
            _signals = new List<MtsSignal>();
            
            // Список сигналов для управления загрузкой весовых бункеров
            _signals.Add(new MtsSignal(4000, "Загрузка материала в загрузочный бункер 1", 0));
            _signals.Add(new MtsSignal(4001, "Загрузка материала в загрузочный бункер 2", 0));
            _signals.Add(new MtsSignal(4002, "Выгрузка материала из загрузочного бункера 1", 0));
            _signals.Add(new MtsSignal(4003, "Выгрузка материала из загрузочного бункера 2", 0));
            _signals.Add(new MtsSignal(4004, "Датчик наличия материала 1 на конвейере 1", 0));
            _signals.Add(new MtsSignal(4005, "Датчик наличия материала 2 на конвейере 1", 0));
            _signals.Add(new MtsSignal(4006, "Датчик наличия материала 3 на конвейере 1", 0));
            _signals.Add(new MtsSignal(4007, "Датчик наличия материала 4 на конвейере 1", 0));
            _signals.Add(new MtsSignal(4008, "Датчик наличия материала 5 на конвейере 1", 0));
            _signals.Add(new MtsSignal(4009, "Датчик наличия материала 1 на конвейере 2", 0));
            _signals.Add(new MtsSignal(4010, "Датчик наличия материала 2 на конвейере 2", 0));
            _signals.Add(new MtsSignal(4011, "Датчик наличия материала 3 на конвейере 2", 0));
            _signals.Add(new MtsSignal(4012, "Датчик наличия материала 4 на конвейере 2", 0));
            _signals.Add(new MtsSignal(4013, "Датчик наличия материала 5 на конвейере 2", 0));
            _signals.Add(new MtsSignal(4014, "Датчик наличия материала 1 на конвейере 3", 0));
            _signals.Add(new MtsSignal(4015, "Датчик наличия материала 2 на конвейере 3", 0));
            _signals.Add(new MtsSignal(4016, "Датчик наличия материала 3 на конвейере 3", 0));
            _signals.Add(new MtsSignal(4017, "Датчик наличия материала 4 на конвейере 3", 0));
            _signals.Add(new MtsSignal(4018, "Датчик наличия материала 5 на конвейере 3", 0));
            _signals.Add(new MtsSignal(4019, "Загрузка материала в силос 1", 0));
            _signals.Add(new MtsSignal(4020, "Загрузка материала в силос 2", 0));
            _signals.Add(new MtsSignal(4021, "Загрузка материала в силос 3", 0));
            _signals.Add(new MtsSignal(4022, "Загрузка материала в силос 4", 0));
            _signals.Add(new MtsSignal(4023, "Загрузка материала в силос 5", 0));
            _signals.Add(new MtsSignal(4024, "Загрузка материала в силос 6", 0));
            _signals.Add(new MtsSignal(4025, "Загрузка материала в силос 7", 0));
            _signals.Add(new MtsSignal(4026, "Загрузка материала в силос 8", 0));
            _signals.Add(new MtsSignal(4027, "Выгрузка материала из силоса 1", 0));
            _signals.Add(new MtsSignal(4028, "Выгрузка материала из силоса 2", 0));
            _signals.Add(new MtsSignal(4029, "Выгрузка материала из силоса 3", 0));
            _signals.Add(new MtsSignal(4030, "Выгрузка материала из силоса 4", 0));
            _signals.Add(new MtsSignal(4031, "Выгрузка материала из силоса 5", 0));
            _signals.Add(new MtsSignal(4032, "Выгрузка материала из силоса 6", 0));
            _signals.Add(new MtsSignal(4033, "Выгрузка материала из силоса 7", 0));
            _signals.Add(new MtsSignal(4034, "Выгрузка материала из силоса 8", 0));
        }
    }
}
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigReader;
using MtsConnect;
using MtsDb.Config;
using NLog;
using SignalGenerator.Data;

namespace SignalGenerator
{
    class Program
    {
        private static Reader _reader;
        private static string _cfgMill;
        private static List<string> _objectsJson;
        
        private static IConfigurationRoot _config;
        private static SubscriptionConfig _cfg;
        private static Task<MtsTcpConnection> _connection;
        private static Subscription _subscription;
        private static bool _connected;
        private static string _mtsIp;
        private static int _mtsPort;
        private static int _mtsTimeout;
        private static int _mtsReconnect;
        private static IngotsState _ingots;
        
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
        private static void ConnectToMts()
        {
            List<ushort> signals = AddMtsSignals();
            _objectsJson = new List<string>();
            
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _mtsIp = _config.GetSection("Mts:Ip").Value;
            _mtsPort = Int32.Parse(_config.GetSection("Mts:Port").Value);
            _mtsTimeout = Int32.Parse(_config.GetSection("Mts:Timeout").Value);
            _mtsReconnect = Int32.Parse(_config.GetSection("Mts:ReconnectTimeout").Value);
            _cfgMill = _config.GetSection("RollingMillConfigPath").Value;
            
            // _reader = new Reader(_cfgMill);
            _reader = new Reader(_mtsIp, _mtsPort);
            try
            {
                _reader.ReadAsync().Wait();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка чтения конфигурационного файла [{ex.Message}]");
                Console.WriteLine($"Ошибка чтения конфигурационного файла [{ex.Message}]");
            }

            if (_reader.GetObjects<Tu>().Any<Tu>((Func<Tu, bool>) (tu => tu.Aggregate == null)))
            {
                LogManager.GetCurrentClassLogger().Error("Не для всех ТУ задан Агрегат.");
                throw new Exception("Не для всех ТУ задан Агрегат.");
            }
            
            foreach (var obj in _reader.Objects)
            {
                _objectsJson.Add(JsonConvert.SerializeObject(obj));
            }

            TryConnect();
            
            _subscription.AddSignals(signals).Wait();
            // _subscription.RemoveSignals(signals).Wait();
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
            _signalsList.Add(new MtsSignal(25101, "Скорость рольганга №1", 1));
            _signalsList.Add(new MtsSignal(4303, "Скорость рольганга №1", 0));

            foreach (MtsSignal item in _signalsList)
            {
                res.Add(item.Number);
            }

            return res;
        }
        
        /// <summary>
        /// Обработка разрыва соединения со службой MTS Service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnError(object sender, SubscriptionErrorEventArgs e)
        {
            // Вызов функции обратного вызова и попытка переподключения при возникновении ошибки от сигнала
            Logger.Error($"Ошибка подключения: {e.Message}");
            TryReconnect();
        }
        
        /// <summary>
        /// Попытка переподключения к службе MTS Service
        /// </summary>
        private static void TryReconnect()
        {
            // Попытка переподключения если прошло указанное в настройках время
            Task.Delay(TimeSpan.FromMilliseconds(_mtsReconnect));

            // Сбрасываем предыдущую неудачную попытку подключения
            ResetConnection();

            // Пытаемся подключиться заново
            TryConnect();
        }
        
        /// <summary>
        /// Попытка подключения к службе MTS Service
        /// </summary>
        private static void TryConnect()
        {
            if (!_connected)
            {
                try
                {
                    // Пытаемся создать подключение к MTSSercice
                    _connection = MtsTcpConnection.CreateAsync(_mtsIp, _mtsPort);
                    _connection.Wait();
                    _connected = true;

                    // Поключение создано, устанавливаем подписки на изменение значений сигналов
                    
                    _cfg = new SubscriptionConfig()
                        .Identity("Signal-Generator")
                        .Timeout(TimeSpan.FromMilliseconds(_mtsTimeout));
                    _subscription = _connection.Result.CreateNewSubscription(_cfg);
                    _subscription.OnError += OnError;
                    _subscription.NewData += OnNewData;
                }
                catch (Exception e)
                {
                    // Если не удалось подключиться
                    _connected = false;
                    Console.WriteLine(e.Message);
                    Logger.Error($"Ошибка при подключении к сервису MTSService: {e.Message}");

                    // Пытаемся переподключиться через указаное в настройках время
                    TryReconnect();
                }
            }
        }

        /// <summary>
        /// Сброс неудавшегося поключения
        /// </summary>
        private static void ResetConnection()
        {
            // Попытка сбросить предыдущее неудачное соединение
            _connection?.Dispose();
            if (_subscription == null)
                return;

            // Удаление предыдущих подписок на сигналы
            _subscription.OnError -= OnError;
            _subscription.NewData -= OnNewData;
        }

        
        /// <summary>
        /// Обработка события поступления новых значений сигналов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnNewData(object sender, SubscriptionStateEventArgs e)
        {
            SignalsState diff = e.Diff.Signals;
            _ingots = e.State.Ingots;
            
            if(_ingots!=null && _ingots.Any())
            {
                var ingot = _ingots.Select(i => i.ToString());
                Console.WriteLine("Ingots:\n" + ingot.Aggregate("", (a, b) => a + "\n" + b));
                
                foreach (Ingot item in _ingots)
                {
                    // string s = item.Parameters.Values.Select(p => $"[{p.Id}={p.Value}]")
                    //     .Aggregate("", (a, b) => string.IsNullOrEmpty(a)? b: $"{a}, {b}");
                    foreach (IngotParam param in item.Parameters.Values)
                    {
                        if(param.Id == 1)
                        {
                            // Console.WriteLine($"{param.Id} = {param.Value}");
                            try
                            {
                                MaterialsList json =
                                    JsonConvert.DeserializeObject<MaterialsList>(param.Value.ToString());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }            
            
            if (diff != null)
            {
                Console.WriteLine($"{e.State.TrackTime:O} / {e.State.TimeStamp:O}");
                foreach (var item in diff)
                {
                    CheckSignal(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// Обработчик вновь поступившего значения сигнала
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="value"></param>
        private static async void CheckSignal(ushort signal, double value)
        {
            Console.WriteLine($"[{signal}] = {value:F4}");
            // switch (signal)
            // {
            //     case 4001: break;
            // }
        }
        
        /// <summary>
        /// Начало симуляции процесса работы тракта сыпучих
        /// </summary>
        private static void StartSimulate()
        {
            Random random = new Random();
            
            List<Chemical> chemicals = new List<Chemical>();
            List<Material> materialList = new List<Material>();
            MaterialsList materials;
            Material material;
            
            // Материал 1
            chemicals.Add(new Chemical(1, "Железо", "Fe", 0.75));
            chemicals.Add(new Chemical(2, "Хром", "Cr", 0.05));
            chemicals.Add(new Chemical(3, "Ваннадий", "Vn", 0.0015));
            chemicals.Add(new Chemical(4, "Марганец", "Mn", 0.025));
            material = new Material(1, "FeSiMn", chemicals);
            materialList.Add(material);
            
            // Материал 2
            chemicals = new List<Chemical>();
            chemicals.Add(new Chemical(1, "Кремний", "Si", 25.15));
            chemicals.Add(new Chemical(2, "Кальций", "Ca", 65.85));
            material = new Material(2, "SiCa", chemicals);
            materialList.Add(material);
            
            // Материал 3
            material = new Material(3, "CaO", new List<Chemical>());
            materialList.Add(material);

            materials = new MaterialsList(materialList);

            ConsoleKeyInfo key;
            double sig4001;
            double sig4002;
            double sig4003;
            double sig4004;
            int sig25101 = 1;

            do
            {
                key = Console.ReadKey(true);
                switch (key.KeyChar)
                {
                    case 'g':
                    case 'G':
                        sig4001 = random.NextDouble() * 100;
                        sig4002 = random.NextDouble() * 100;
                        sig4003 = random.NextDouble() * 100;
                        sig4004 = random.NextDouble() * 100;
                        sig25101 = sig25101 == 1 ? 0 : 1;
                        _subscription.SetSignal(4303, sig25101);
                        // _subscription.CreateBatch()
                        //     .SetSignal(4001, sig4001)
                        //     .SetSignal(4002, sig4002)
                        //     .SetSignal(4003, sig4003)
                        //     .SetSignal(4004, sig4004)
                        //     .Execute().Wait();
                        break;
                    case 'r':
                    case 'R':
                        Console.Write("Номер нити: ");
                        string threadStr = Console.ReadLine();
                        ushort threadNum = 0;
                        if (threadStr.Trim() != "")
                        {
                            try
                            {
                                threadNum = ushort.Parse(threadStr);
                            }
                            catch
                            {
                                threadNum = 0;
                            }
                        }
                        _subscription.AddAoI(minX: 0, maxX: 100, thread: threadNum).Wait();
                        break;
                    case 'a':
                    case 'A':
                        _subscription.AddIngot(36, 38, 0, 0.5, 5, level3Id: 0, baseId: 0, rollingId: 0)
                            .Wait();
                        break;
                    case 'd':
                    case 'D':
                        if (_ingots != null && _ingots.Any())
                        {
                            _subscription.DeleteIngot(_ingots.First().Id).Wait();
                        }

                        break;
                    case 'p':
                    case 'P':
                        if (_ingots != null && _ingots.Any())
                        {
                            string param = JsonConvert.SerializeObject(materials, Formatting.None);
                            _subscription.ModifyIngot(_ingots.First().Id, parameters: new[]
                            {
                                new ValueTuple<ushort, object>(1, param)
                            });
                        }

                        break;
                    case 'm':
                    case 'M':
                        // Ручное управление сигналами
                        ushort num = 0;
                        double val = 0;
                        Console.Write("Номер сигнала: ");
                        string sigNum = Console.ReadLine();
                        Console.Write("Значение: ");
                        string sigVal = Console.ReadLine();
                        if (sigNum.Trim() != "" && sigVal.Trim() != "")
                        {
                            try
                            {
                                num = ushort.Parse(sigNum);
                                val = double.Parse(sigVal);
                            }
                            catch
                            {
                                num = 0;
                                val = 0;
                            }
                        }

                        if (val > 0 && num > 0)
                        {
                            _subscription.SetSignal(num, val).Wait();
                        }
                        break;
                }
            } while (key.KeyChar != 'x');
            
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
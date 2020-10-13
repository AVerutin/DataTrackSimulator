// Decompiled with JetBrains decompiler
// Type: MtsDb.Config.Connection
// Assembly: MtsDb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A81A53C2-7DA3-4BAB-BE2A-32F2E8382C06
// Assembly location: D:\MTS\Project\MTSDb\MtsDb.exe

using ConfigReader;

namespace SignalGenerator.Config
{
  [Section(new string[] {"Подключение"})]
  public class Connection
  {
    [Param(new string[] {"Логин"})]
    public string Login;
    [Param(new string[] {"Пароль"})]
    public string Password;
    [Param(new string[] {"БД"})]
    public string Db;
    [Param(new string[] {"ConnectionString", "СтрокаСоединения"})]
    public string ConnectionString;
  }
}

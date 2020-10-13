// Decompiled with JetBrains decompiler
// Type: MtsDb.Config.Signal
// Assembly: MtsDb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A81A53C2-7DA3-4BAB-BE2A-32F2E8382C06
// Assembly location: D:\MTS\Project\MTSDb\MtsDb.exe

using ConfigReader;

namespace SignalGenerator.Config
{
  [Section(new string[] {"Сигнал"})]
  public class Signal
  {
    [Param(new string[] {"Идентификатор"})]
    public ushort Id;
    [Param(new string[] {"Имя"})]
    public string Name;
    [Param(new string[] {"ФильтрБД"})]
    public string Filter;
    public string TableName;
  }
}

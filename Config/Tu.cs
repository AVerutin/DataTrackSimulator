// Decompiled with JetBrains decompiler
// Type: MtsDb.Config.Tu
// Assembly: MtsDb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A81A53C2-7DA3-4BAB-BE2A-32F2E8382C06
// Assembly location: D:\MTS\Project\MTSDb\MtsDb.exe

using ConfigReader;

namespace SignalGenerator.Config
{
  [Section(new string[] {"Техузел"})]
  public class Tu
  {
    [Param(new string[] {"Идентификатор"})]
    public int Id;
    [Param(new string[] {"Имя"})]
    public string Name;
    [Param(new string[] {"КоординатаНачала"})]
    public double XStart;
    [Param(new string[] {"КоординатаЗавершения"})]
    public double XEnd;
    [Param(new string[] {"НомерНити"})]
    public ushort Thread;
    [Param(new string[] {"ПризнакТУ"})]
    public string TuFlag;
    [Param(new string[] {"Агрегат"})]
    public int AggregateId;

    [Ref("AggregateId", "Id")]
    public Aggregate Aggregate { get; set; }
  }
}

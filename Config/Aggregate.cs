﻿// Decompiled with JetBrains decompiler
// Type: MtsDb.Config.Aggregate
// Assembly: MtsDb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A81A53C2-7DA3-4BAB-BE2A-32F2E8382C06
// Assembly location: D:\MTS\Project\MTSDb\MtsDb.exe

using ConfigReader;

namespace MtsDb.Config
{
  [Section(new string[] {"Агрегат"})]
  public class Aggregate
  {
    [Param(new string[] {"Идентификатор"})]
    public int Id;
    [Param(new string[] {"Имя"})]
    public string Name;
    [Param(new string[] {"Префикс"})]
    public int Prefix;
    [Param(new string[] {"ЧастотаЗаписиСигналов"})]
    public int MaxTimeoutSignals;
  }
}
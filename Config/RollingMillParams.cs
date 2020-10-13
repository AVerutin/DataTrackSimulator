// Decompiled with JetBrains decompiler
// Type: MtsDb.Config.RollingMillParams
// Assembly: MtsDb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A81A53C2-7DA3-4BAB-BE2A-32F2E8382C06
// Assembly location: D:\MTS\Project\MTSDb\MtsDb.exe

using ConfigReader;

namespace SignalGenerator.Config
{
  [Section(new string[] {"ОбщиеПараметрыСтана", "CommonParameters"})]
  internal class RollingMillParams
  {
    [Param(new string[] {"IPАдресСервераВизуализацииСлежения"})]
    public string Ip;
    [Param(new string[] {"ПортСервераВизуализацииСлежения"})]
    public int Port;
  }
}

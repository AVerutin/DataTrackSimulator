// Decompiled with JetBrains decompiler
// Type: MtsDb.Config.TcpServer
// Assembly: MtsDb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A81A53C2-7DA3-4BAB-BE2A-32F2E8382C06
// Assembly location: D:\MTS\Project\MTSDb\MtsDb.exe

using ConfigReader;

namespace SignalGenerator.Config
{
  [Section(new string[] {"TcpServer"})]
  public class TcpServer
  {
    [Param(new string[] {"Port"})]
    public int Port;
  }
}

// Decompiled with JetBrains decompiler
// Type: MtsDb.Config.Subscriptions
// Assembly: MtsDb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A81A53C2-7DA3-4BAB-BE2A-32F2E8382C06
// Assembly location: D:\MTS\Project\MTSDb\MtsDb.exe

using ConfigReader;

namespace MtsDb.Config
{
  [Section(new string[] {"Подписки"})]
  public class Subscriptions
  {
    [Param(new string[] {"Потоки"})]
    public int Threads;
    [SubSection]
    public TcpServer[] TcpServers;
  }
}

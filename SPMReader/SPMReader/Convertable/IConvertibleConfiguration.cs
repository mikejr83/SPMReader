using System;

namespace SPMReader
{
  public interface IConvertibleConfiguration
  {
    string FrameRate{ get; }

    string TrimMode { get; }

    string TrimType { get; }
  }
}


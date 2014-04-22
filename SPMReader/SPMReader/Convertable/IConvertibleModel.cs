using System;

namespace SPMReader.Convertable
{
  public interface IConvertibleModel
  {
    IConvertibleModelInformation StandardizedModelInformation{ get; }

    IConvertibleTrainer StandardizedTrainer { get; }

    IConvertibleConfiguration StandardizedConfiguration{ get; }

    IConvertibleFlightMode StandardizedFlightMode{ get; }
  }
}


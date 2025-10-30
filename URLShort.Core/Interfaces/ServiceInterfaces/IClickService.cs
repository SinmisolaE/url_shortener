using System;

namespace URLShort.Core.Interfaces.ServiceInterfaces;

public interface IClickService
{

    Task RecordClick(string shortCode);

}

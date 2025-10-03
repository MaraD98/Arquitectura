using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

// El namespace debe coincidir con el de la clase base IntegrationEvent (Core.Application)
namespace Core.Application
{
    // [NUEVA INTERFAZ] Esta es la interfaz que faltaba y causaba el error CS0246
    public interface IIntegrationEvent
    {
        Guid Id { get; }
        DateTime CreateDateUtc { get; }
        string EventType { get; }
        string Subject { get; }
        object Data { get; }
    }
}
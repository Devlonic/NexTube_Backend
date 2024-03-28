using MediatR;

namespace NexTube.Application.Common.Interfaces {
    public interface IEventPublisher<EventDataType> {
        Task SendEvent(EventDataType data);
    }
}

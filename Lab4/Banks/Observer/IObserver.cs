using Banks.Messages;

namespace Banks.Observer;

public interface IObserver
{
    void Update(IMessage message);
}
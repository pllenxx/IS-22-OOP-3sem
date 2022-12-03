using Banks.Messages;

namespace Banks.Observer;

public interface IObservable
{
    void AddObserver(IObserver account);
    void RemoveObserver(IObserver account);
    void NotifyObservers(IMessage message);
}
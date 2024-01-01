namespace Moyou.Aspects.Memento;

public interface IOriginator
{
    public void RestoreMemento(IMemento memento);
    public IMemento CreateMemento();
}
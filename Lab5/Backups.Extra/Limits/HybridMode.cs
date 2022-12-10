using Backups.Extra.Tools;

namespace Backups.Extra.Remover;

public class HybridMode
{
    public HybridMode(bool type)
    {
        if (type)
        {
            All = true;
            Any = false;
        }
        else
        {
            All = false;
            Any = true;
        }
    }

    public bool All { get; }
    public bool Any { get; }
}
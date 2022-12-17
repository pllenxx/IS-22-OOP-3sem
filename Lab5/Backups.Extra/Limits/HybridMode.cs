using Backups.Extra.Tools;

namespace Backups.Extra.Remover;

public class HybridMode
{
    public HybridMode(bool isAllLimitsOn)
    {
        if (isAllLimitsOn)
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
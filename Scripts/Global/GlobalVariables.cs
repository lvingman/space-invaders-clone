using System.Threading.Tasks;
using Godot;

namespace SpaceInvadersClone.Scripts.Global;

public class GlobalVariables
{
    private static GlobalVariables _instance;
    private static readonly object _lock = new object();

    // Instance variables
    public int Lives { get; set; }
    public int Score { get; set; }
    public int Round { get; set; }

    // Private constructor to prevent instantiation outside of this class.
    private GlobalVariables()
    {
        // Initialize variables
        Lives = 3;
        Score = 0;
        Round = 1;
    }

    // Public accessor for the singleton instance
    public static GlobalVariables Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new GlobalVariables();
                }
                return _instance;
            }
        }
    }
}
public partial class GlobalFunctions : Node
{
    private static GlobalFunctions instance;

    public static GlobalFunctions Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GlobalFunctions();
                // Initialize if needed
            }
            return instance;
        }
    }   

    public async Task Wait(float seconds, Node contextNode)
    {
        var timer = contextNode.GetTree().CreateTimer(seconds);
        await ToSignal(timer, "timeout");
    }
}
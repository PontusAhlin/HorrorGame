public class AchievementZero
{

    private int index = 0;
    private string title = "First death";
    private string description = "You died for the first time";

    public bool isAchieved;

    // Start is called before the first frame update
    void Start()
    {
        Storage.GetAchievement(0);
    }

    // =============================== GETTERS ===============================
    public string GetTitle()
    {
        return title;
    }
    public string GetDescription()
    {
        return description;
    }

}


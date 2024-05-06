public interface AchievementInterface
{

    private int index;
    private string title;
    private string description;
    private bool isAchieved;
    private int progress;
    private int maxProgress;

    // Start is called before the first frame update
    void Start()
    {
        // Get status of achievement
        isAchieved =  Storage.GetAchievement(0);
        progress = Storage.GetAchievementProgress(0);
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
    public bool GetIsAchieved()
    {
        return isAchieved;
    }
    public int GetProgress()
    {
        return progress;
    }

    // =============================== SETTERS ===============================
    public void SetIsArchived(bool value)
    {
        isAchieved = value;
    }

}


using System.Collections.Generic;

public enum QuestStatus
{
    NOTACCEPTED,

    GIVEN,

    COMPLETE
}

[System.Serializable]
public class Quest
{
    #region Public Properties

    public string Description
    {
        get;
        set;
    }

    public bool EvaluateObjectives
    {
        get
        {
            int i = 0;

            foreach (Objective obj in Objectives)
            {
                if (obj.Evaluate && !obj.Complete)
                {
                    i++;
                    QuestManager.Instance.UnSubscribeToEvent(obj);
                    obj.Complete = true;
                }

                if (obj.Complete)

                    if (i == Objectives.Count)
                    {
                        Status = QuestStatus.COMPLETE;
                        return true;
                    }
            }

            return false;
        }
    }

    public List<Objective> Objectives
    {
        get;
        set;
    }

    public string QuestGiverName
    {
        get;
        set;
    }

    public QuestMB QuestMB
    {
        get;
        set;
    }

    public QuestStatus Status
    {
        get;
        set;
    }

    public string Title
    {
        get;
        set;
    }

    #endregion Public Properties
}
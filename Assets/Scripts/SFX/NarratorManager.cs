using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorManager : MonoBehaviour
{
    public AudioSource audioSource;
    //public SubtitleManager subtitleManager;

    private static NarratorManager _get;
    public static NarratorManager get
    {
        get
        {
            if (_get == null)
            {
                _get = GameObject.Instantiate(Resources.Load<NarratorManager>("NarratorManager")).GetComponent<NarratorManager>();
            }
            return _get;
        }
    }

    [SerializeField]
    public Dictionary<string, float> TriggersHappened;

    public NarratorEvent current_playing_event;

    public List<NarratorEvent> QueuedNarratorEvents;
    public List<NarratorEvent> PendingNarratorEvents;
    public List<NarratorEvent> InterruptedNarratorEvents;
    public List<NarratorEvent> DoneNarratorEvents;

    // Start is called before the first frame update
    void Start()
    {
        _get = this;
        //subtitleManager = SubtitleManager.get;
        for (int i = 0; i < PendingNarratorEvents.Count; i++)
        {
            PendingNarratorEvents[i].CleanUp();
        }
    }

    public void Initialize()
    {
        Debug.Log("Narrator Manager Initialized");
        TriggersHappened = new Dictionary<string, float>();
    }

    public void TriggerHappened(string TriggerId)
    {
        if (!TriggersHappened.ContainsKey(TriggerId))
        {
            TriggersHappened[TriggerId] = Time.time;
            for (int i = PendingNarratorEvents.Count - 1; i >= 0; i--)
            {
                var p = PendingNarratorEvents[i];
                if (string.Equals(p.TriggeredById, TriggerId, StringComparison.CurrentCultureIgnoreCase))
                {
                    PendingNarratorEvents.RemoveAt(i);
                    QueuedNarratorEvents.Add(p);
                }
            }
        }
    }

    private float last_time_checked_queued_narrator = 0.0f;
    public void Update()
    {
        if (last_time_checked_queued_narrator + 0.25 < Time.time)
        {
            last_time_checked_queued_narrator = Time.time;
            CheckQueue();
        }
    }

    public void CheckQueue()
    {
        if (current_playing_event != null && !current_playing_event.Interruptable)
        {
            return;
        }

        for (int i = 0; i < QueuedNarratorEvents.Count; i++)
        {
            var when_was_triggered = TriggersHappened[QueuedNarratorEvents[i].TriggeredById];
            if (when_was_triggered + QueuedNarratorEvents[i].TriggeredDelay < Time.time)
            {
                StartNarrator(QueuedNarratorEvents[i]);
            }
        }

        if (current_playing_event != null && !current_playing_event.IsRunning)
        {
            ActuallyStartNarrator();
        }
    }

    public void StartNarrator(NarratorEvent new_event)
    {
        if (current_playing_event != null)
        {
            Debug.Log($"Interrupting narrator {current_playing_event.NarratorEventID}");
            InterruptedNarratorEvents.Add(current_playing_event);
            current_playing_event.InterruptEvent(audioSource);
        }

        if (QueuedNarratorEvents.Contains(new_event))
        {
            QueuedNarratorEvents.Remove(new_event);
        }
        current_playing_event = new_event;
        ActuallyStartNarrator();
    }

    public void ActuallyStartNarrator()
    {
        //PLAY SOUND DO STUFF
        Debug.Log($"Started playing narrator {current_playing_event.NarratorEventID}");
        current_playing_event.StartEvent(audioSource);
        StartCoroutine(WaitAndEndNarratorDebug(current_playing_event));
    }

    private IEnumerator PlayNarratorEvent(NarratorEvent narratorEvent)
    {
        while (narratorEvent.IsRunning && narratorEvent.PlayNextClip(audioSource))
        {
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        narratorEvent.IsRunning = false;
        DoneNarratorEvents.Add(narratorEvent);
        current_playing_event = null;
        Debug.Log($"Finished playing narrator {narratorEvent.NarratorEventID}");

        if (InterruptedNarratorEvents.Count > 0)
        {
            StartNarrator(InterruptedNarratorEvents[0]);
            InterruptedNarratorEvents.RemoveAt(0);
        }
    }

    public IEnumerator WaitAndEndNarratorDebug(NarratorEvent doing_event)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        if (current_playing_event == doing_event)
        {
            DoneNarratorEvents.Add(doing_event);
            current_playing_event = null;
            Debug.Log($"Finished playing narrator {doing_event.NarratorEventID}");
            for (int i = 0; i < InterruptedNarratorEvents.Count; i++)
            {
                StartNarrator(InterruptedNarratorEvents[i]);
                InterruptedNarratorEvents.RemoveAt(0);
                break;
            }
        }
    }

}